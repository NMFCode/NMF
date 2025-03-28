﻿using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a class that matches text using an incremental packrat parser
    /// </summary>
    public class Matcher
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="comments">the comment rules</param>
        public Matcher(params CommentRule[] comments)
        {
            _commentRules = comments ?? Array.Empty<CommentRule>();
        }

        private readonly CommentRule[] _commentRules;
        private readonly List<MemoLine> _memoTable = new List<MemoLine>();
        private List<RuleApplication> _trailingComments;

        private MemoLine GetLine(int line)
        {
            while (_memoTable.Count <= line)
            {
                _memoTable.Add(new MemoLine() { LineNo = _memoTable.Count });
            }
            return _memoTable[line];
        }

        /// <summary>
        /// Resets the memoization table
        /// </summary>
        public void Reset()
        {
            _memoTable.Clear();
            _trailingComments = null;
        }

        /// <summary>
        /// Gets the position of the next token, starting from the given position
        /// </summary>
        /// <param name="position">the position where to look for the next token</param>
        /// <returns>the position of the next token position</returns>
        public ParsePosition NextTokenPosition(ParsePosition position)
        {
            var line = position.Line;
            var col = position.Col + 1;
            while (line < _memoTable.Count)
            {
                var memoLine = _memoTable[line];
                foreach (var memoCol in memoLine.Columns)
                {
                    if (memoCol.Key < col)
                    {
                        continue;
                    }
                    if (memoCol.Value.Applications.Any(a => a.IsPositive && a.Rule.IsLiteral && !a.Rule.IsComment))
                    {
                        return new ParsePosition(line, memoCol.Key);
                    }
                }
                line++;
                col = 0;
            }
            return new ParsePosition(line, col);
        }

        /// <summary>
        /// Determines whether the input between the given position and the target position only consists of white space
        /// </summary>
        /// <param name="position">the start position</param>
        /// <param name="targetPosition">the target position</param>
        /// <returns>true, if there is only white spaces between the given position and the target position, otherwise false</returns>
        public bool IsWhiteSpaceTo(ParsePosition position, ParsePosition targetPosition)
        {
            for (int currentLine = position.Line; currentLine < _memoTable.Count && currentLine < targetPosition.Line; currentLine++)
            {
                var line = _memoTable[currentLine];

                foreach (var col in line.Columns)
                {
                    int currentCol = currentLine == position.Line ? position.Col : 0;

                    if (col.Key < currentCol)
                    {
                        continue;
                    }
                    else if (currentLine == targetPosition.Line && col.Key >= targetPosition.Col)
                    {
                        break;
                    }

                    foreach (var ruleApplication in col.Value.Applications)
                    {
                        if (ruleApplication.IsPositive && !ruleApplication.Rule.IsComment)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Gets a collection of comments found after the last rule application
        /// </summary>
        public IEnumerable<RuleApplication> TrailingComments => _trailingComments ?? Enumerable.Empty<RuleApplication>();

        /// <summary>
        /// Gets a collection of failed rule applications exactly at the given position 
        /// </summary>
        /// <param name="position">the position</param>
        /// <returns>a collection of rule applications</returns>
        public IEnumerable<RuleApplication> GetErrorsExactlyAt(ParsePosition position)
        {
            if (_memoTable.Count <= position.Line)
            {
                return Enumerable.Empty<RuleApplication>();
            }
            var line = _memoTable[position.Line];
            if (line.Columns.TryGetValue(position.Col, out var col))
            {
                return col.Applications.Where(ra => !ra.IsPositive);
            }
            return Enumerable.Empty<RuleApplication>();
        }

        private static IEnumerable<RuleApplication> GetCol(SortedDictionary<int, MemoColumn> line, int col)
        {
            if (line.TryGetValue(col, out var ruleApplications))
            {
                return ruleApplications.Applications;
            }
            return Enumerable.Empty<RuleApplication>();
        }

        internal RuleApplication MatchCore(Rule rule, ParseContext context, ref ParsePosition position)
        {
            var line = GetLine(position.Line);
            var ruleApplications = GetCol(line.Columns, position.Col);

            var ruleApplication = ruleApplications.FirstOrDefault(r => r.Rule == rule);
            if (ruleApplication == null)
            {
                var col = position.Col;
                var column = line.GetOrCreateColumn(col);
                if (rule.IsLeftRecursive)
                {
                    var cycleDetector = new RecursiveRuleApplication(rule, position, default, new ParsePositionDelta(1, 0));
                    cycleDetector.AddToColumn(column);
                    ruleApplication = rule.Match(context, ref position);
                    if (cycleDetector.Continuations.Count > 0)
                    {
                        ExtendContinuations(cycleDetector.Continuations, column, context, ref position, ref ruleApplication);
                    }
                    column.Applications.Remove(cycleDetector);
                    ruleApplication.AddToColumn(column);
                    line.MaxExaminedLength = ParsePositionDelta.Larger(line.MaxExaminedLength, PrependColOffset(ruleApplication.ExaminedTo, col));
                }
                else
                {
                    ruleApplication = rule.Match(context, ref position);
                    line.MaxExaminedLength = ParsePositionDelta.Larger(line.MaxExaminedLength, PrependColOffset(ruleApplication.ExaminedTo, col));
                    ruleApplication.AddToColumn(column);
                }
                ruleApplication.Comments = column.Comments;
            }
            else
            {
                position += ruleApplication.Length;
            }
            if (rule.TrailingWhitespaces)
            {
                MoveOverWhitespaceAndComments(context, ref position);
            }
            return ruleApplication;
        }

        private static void ExtendContinuations(List<RecursiveContinuation> continuations, MemoColumn column, ParseContext context, ref ParsePosition position, ref RuleApplication ruleApplication)
        {
            var backup = new List<RuleApplication>();
            var cont = true;
            while (cont)
            {
                backup.AddRange(column.Applications);
                column.Applications.Clear();
                ruleApplication.AddToColumn(column);
                cont = TryExtendContinuations(continuations, context, ref position, ref ruleApplication);
            }
            column.Applications.AddRange(backup);
        }

        private static bool TryExtendContinuations(List<RecursiveContinuation> continuations, ParseContext context, ref ParsePosition position, ref RuleApplication ruleApplication)
        {
            var currentPosition = position;
            foreach (var continuation in continuations)
            {
                var newRuleApplication = continuation.ResolveRecursion(ruleApplication, context, ref position);
                if (newRuleApplication != ruleApplication)
                {
                    ruleApplication = newRuleApplication;
                    return position > currentPosition;
                }
            }
            return false;
        }

        private static ParsePositionDelta PrependColOffset(ParsePositionDelta examinedTo, int col)
        {
            if (examinedTo.Line == 0)
            {
                return new ParsePositionDelta(0, examinedTo.Col + col);
            }
            return examinedTo;
        }

        /// <summary>
        /// Matches the provided rule with the given parse context
        /// </summary>
        /// <param name="context">The context in which the text is parsed, including the current input</param>
        /// <returns>A rule application for the entire text</returns>
        public RuleApplication Match(ParseContext context)
        {
            var position = new ParsePosition(0, 0);
            MoveOverWhitespaceAndComments(context, ref position);
            var match = MatchCore(context.RootRule, context, ref position);
            if (!match.IsPositive || position.Line == context.Input.Length)
            {
                return match;
            }
            return new UnexpectedContentApplication(context.RootRule, match, position, new ParsePositionDelta(position.Line, position.Col), "Unexpected content");
        }

        /// <summary>
        /// Removes any memoization based on the given text edit and updates the memo table
        /// </summary>
        /// <param name="edit">The change in the input text</param>
        public void Apply(TextEdit edit)
        {
            var refreshLineIndices = false;
            for (int i = 0; i <= edit.Start.Line; i++)
            {
                var line = GetLine(i);

                if (new ParsePosition(i, 0) + line.MaxExaminedLength < edit.Start)
                {
                    continue;
                }

                if (i == edit.Start.Line)
                {
                    RemoveColsAfterStart(edit, line, i);
                }

                var maxReach = default(ParsePositionDelta);
                
                foreach (var col in line.Columns)
                {
                    var pos = new ParsePosition(i, col.Key);

                    for (int j = col.Value.Applications.Count - 1; j >= 0; j--)
                    {
                        var ruleApplication = col.Value.Applications[j];
                        if (pos + ruleApplication.ExaminedTo < edit.Start)
                        {
                            maxReach = ParsePositionDelta.Larger(maxReach, PrependColOffset(ruleApplication.ExaminedTo, col.Key));
                        }
                        else
                        {
                            col.Value.Applications.RemoveAt(j);
                            if (ruleApplication.Rule.IsComment)
                            {
                                RemoveComment(ruleApplication, i, col.Key);
                            }
                        }
                    }
                }

                line.MaxExaminedLength = maxReach;
            }
            var linesDelta = edit.NewText.Length - (edit.End.Line - edit.Start.Line + 1);
            if (linesDelta > 0)
            {
                for (int i = 0; i < linesDelta; i++)
                {
                    _memoTable.Insert(edit.Start.Line, new MemoLine() { LineNo = edit.Start.Line });
                    refreshLineIndices = true;
                }
            }
            else if (linesDelta < 0)
            {
                var deleteOffset = edit.Start.Col > 0 ? 1 : 0;
                for (int i = 0; i > linesDelta; i--)
                {
                    _memoTable.RemoveAt(edit.Start.Line + deleteOffset);
                }
                refreshLineIndices = true;
            }
            if (refreshLineIndices)
            {
                for (int i = edit.Start.Line; i < _memoTable.Count; i++)
                {
                    _memoTable[i].LineNo = i;
                }
            }
        }

        private void RemoveComment(RuleApplication ruleApplication, int line, int col)
        {
            while (line < _memoTable.Count)
            {
                var ln = _memoTable[line];
                foreach (var column in ln.Columns)
                {
                    if (column.Key >= col && column.Value != null && column.Value.Comments != null && column.Value.Comments.Remove(ruleApplication))
                    {
                        return;
                    }
                }
                line++;
                col = -1;
            }
        }

        private void RemoveColsAfterStart(TextEdit edit, MemoLine line, int lineIndex)
        {
            var cols = line.Columns.Keys.ToArray();
            for (int j = cols.Length - 1; j >= 0 && cols[j] >= edit.Start.Col; j--)
            {
                foreach (var ruleApplication in line.Columns[cols[j]].Applications)
                {
                    if (ruleApplication.Rule.IsComment)
                    {
                        RemoveComment(ruleApplication, lineIndex, cols[j]);
                    }
                }
                line.Columns.Remove(cols[j]);
            }
        }

        private void MoveOverWhitespaceAndComments(ParseContext context, ref ParsePosition position)
        {
            MoveOverWhitespace(context, ref position);
            var lastPosition = position;
            List<RuleApplication> comments = null;
            RuleApplication comment;
            do
            {
                comment = MatchComment(context, ref position);
                if (comment != null)
                {
                    var line = GetLine(lastPosition.Line);
                    comment.AddToColumn(line.GetOrCreateColumn(lastPosition.Col));
                    line.MaxExaminedLength = ParsePositionDelta.Larger(line.MaxExaminedLength, PrependColOffset(comment.ExaminedTo, lastPosition.Col));
                    MoveOverWhitespace(context, ref position);
                    lastPosition = position;
                    comments ??= new List<RuleApplication>();
                    comments.Add(comment);
                }
            } while (comment != null);
            if (comments != null)
            {
                if (position.Line < context.Input.Length)
                {
                    var line = GetLine(position.Line);
                    var column = line.GetOrCreateColumn(position.Col);
                    if (column.Comments == null)
                    {
                        column.Comments = comments;
                        foreach (var app in column.Applications)
                        {
                            app.Comments = comments;
                        }
                    }
                    else
                    {
                        column.Comments.AddRange(comments);
                    }
                }
                else
                {
                    if (_trailingComments == null)
                    {
                        _trailingComments = comments;
                    }
                    else
                    {
                        _trailingComments.AddRange(comments);
                    }
                }
            }
        }

        private RuleApplication MatchComment(ParseContext context, ref ParsePosition position)
        {
            var line = GetLine(position.Line);
            if (line.Columns.TryGetValue(position.Col, out var column) && column.Applications.FirstOrDefault() is var firstMatch && firstMatch != null && firstMatch.Rule.IsComment)
            {
                position += firstMatch.Length;
                MoveOverWhitespace(context, ref position);
            }
            foreach (var commentRule in _commentRules)
            {
                var comment = commentRule.Match(context, ref position);
                if (comment != null)
                {
                    return comment;
                }
            }
            return null;
        }

        private static void MoveOverWhitespace(ParseContext context, ref ParsePosition position)
        {
            var lineNo = position.Line;
            var col = position.Col;
            while (lineNo < context.Input.Length)
            {
                var line = context.Input[lineNo];

                while (col < line.Length)
                {
                    if (!char.IsWhiteSpace(line[col]))
                    {
                        position = new ParsePosition(lineNo, col);
                        return;
                    }
                    col++;
                }
                lineNo++;
                col = 0;
            }
            position = new ParsePosition(lineNo, 0);
        }
    }
}
