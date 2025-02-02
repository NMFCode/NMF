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

        private MemoLine GetLine(int line)
        {
            while (_memoTable.Count <= line)
            {
                _memoTable.Add(new MemoLine());
            }
            return _memoTable[line];
        }

        /// <summary>
        /// Gets all rule applications at the given position that only span the current line
        /// </summary>
        /// <param name="position">the position at which rule applications are searched</param>
        /// <param name="includeFailed">true, if the result should include failed rule applications</param>
        /// <returns>a collection of rule applications</returns>
        public IEnumerable<RuleApplication> GetRuleApplicationsAt(ParsePosition position, bool includeFailed = false)
        {
            if (_memoTable.Count <= position.Line)
            {
                yield break;
            }
            var line = _memoTable[position.Line];
            foreach (var col in line.Columns)
            {
                if (col.Key > position.Col)
                {
                    yield break;
                }
                foreach (var ruleApplication in col.Value.Where(r => r.ExaminedTo.Line == 0))
                {
                    if (ruleApplication.IsPositive)
                    {
                        if (col.Key + ruleApplication.Length.Col >= position.Col)
                        {
                            yield return ruleApplication;
                        }
                    }
                    else if (includeFailed && col.Key + ruleApplication.ExaminedTo.Col >= position.Col)
                    {
                        yield return ruleApplication;
                    }
                }
            }
        }

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
                return col.Where(ra => !ra.IsPositive);
            }
            return Enumerable.Empty<RuleApplication>();
        }

        private static IEnumerable<RuleApplication> GetCol(SortedDictionary<int, List<RuleApplication>> line, int col)
        {
            if (line.TryGetValue(col, out var ruleApplications))
            {
                return ruleApplications;
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
                var ruleApplicationList = GetOrCreateColumnsCore(line, ruleApplications, col);
                if (rule.IsLeftRecursive)
                {
                    var cycleDetector = new RecursiveRuleApplication(rule, position, default, new ParsePositionDelta(1, 0));
                    var index = ruleApplicationList.Count;
                    ruleApplicationList.Add(cycleDetector);
                    ruleApplication = rule.Match(context, ref position);
                    if (cycleDetector.Continuations.Count > 0)
                    {
                        var backup = new List<RuleApplication>();
                        var cont = true;
                        while (cont)
                        {
                            backup.AddRange(ruleApplicationList);
                            ruleApplicationList.Clear();
                            ruleApplicationList.Add(ruleApplication);
                            cont = ResolveContinuations(cycleDetector.Continuations, context, ref position, ref ruleApplication);
                        }
                        ruleApplicationList.AddRange(backup);
                    }
                    if (ruleApplicationList.Count > index && ruleApplicationList[index] == cycleDetector)
                    {
                        ruleApplicationList[index] = ruleApplication;
                    }
                    else
                    {
                        ruleApplicationList.Remove(cycleDetector);
                        ruleApplicationList.Add(ruleApplication);
                    }
                    line.MaxExaminedLength = ParsePositionDelta.Larger(line.MaxExaminedLength, PrependColOffset(ruleApplication.ExaminedTo, col));
                }
                else
                {
                    ruleApplication = rule.Match(context, ref position);
                    line.MaxExaminedLength = ParsePositionDelta.Larger(line.MaxExaminedLength, PrependColOffset(ruleApplication.ExaminedTo, col));
                    ruleApplicationList.Add(ruleApplication);
                }
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

        private static List<RuleApplication> GetOrCreateColumnsCore(MemoLine line, IEnumerable<RuleApplication> ruleApplications, int col)
        {
            if (!(ruleApplications is List<RuleApplication> ruleApplicationList))
            {
                // need to check again because another call could have created the column
                ruleApplicationList = GetCol(line.Columns, col) as List<RuleApplication>;
                if (ruleApplicationList == null)
                {
                    ruleApplicationList = new List<RuleApplication>();
                    line.Columns.Add(col, ruleApplicationList);
                }
            }

            return ruleApplicationList;
        }

        private static bool ResolveContinuations(List<RecursiveContinuation> continuations, ParseContext context, ref ParsePosition position, ref RuleApplication ruleApplication)
        {
            foreach (var continuation in continuations)
            {
                var newRuleApplication = continuation.ResolveRecursion(ruleApplication, context, ref position);
                if (newRuleApplication != ruleApplication)
                {
                    ruleApplication = newRuleApplication;
                    return true;
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
            return new FailedRuleApplication(context.RootRule, position, new ParsePositionDelta(position.Line, position.Col), position, "Unexpected content");
        }

        /// <summary>
        /// Removes any memoization based on the given text edit and updates the memo table
        /// </summary>
        /// <param name="edit">The change in the input text</param>
        public void Apply(TextEdit edit)
        {
            for (int i = 0; i <= edit.Start.Line; i++)
            {
                var line = GetLine(i);

                if (new ParsePosition(i, 0) + line.MaxExaminedLength < edit.Start)
                {
                    continue;
                }

                var maxReach = default(ParsePositionDelta);
                
                foreach (var col in line.Columns)
                {
                    var pos = new ParsePosition(i, col.Key);

                    for (int j = col.Value.Count - 1; j >= 0; j--)
                    {
                        var ruleApplication = col.Value[j];
                        if (pos + ruleApplication.ExaminedTo < edit.Start)
                        {
                            maxReach = ParsePositionDelta.Larger(maxReach, PrependColOffset(ruleApplication.ExaminedTo, col.Key));
                        }
                        else
                        {
                            col.Value.RemoveAt(j);
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
                    _memoTable.Insert(edit.Start.Line, new MemoLine());
                }
            }
            else if (linesDelta < 0)
            {
                for (int i = 0; i > linesDelta; i--)
                {
                    _memoTable.RemoveAt(edit.Start.Line);
                }
            }
        }
        private void MoveOverWhitespaceAndComments(ParseContext context, ref ParsePosition position)
        {
            MoveOverWhitespace(context, ref position);
            var lastPosition = position;
            RuleApplication comment = null;
            do
            {
                foreach (var commentRule in _commentRules)
                {
                    comment = commentRule.Match(context, ref position);
                    if (comment != null)
                    {
                        var line = GetLine(lastPosition.Line);
                        var col = GetOrCreateColumnsCore(line, GetCol(line.Columns, lastPosition.Col), lastPosition.Col);
                        col.Add(comment);
                        MoveOverWhitespace(context, ref position);
                        lastPosition = position;
                        break;
                    }
                }
            } while (comment != null);
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

        private sealed class MemoLine
        {
            public SortedDictionary<int, List<RuleApplication>> Columns { get; } = new SortedDictionary<int, List<RuleApplication>>();
            
            public ParsePositionDelta MaxExaminedLength { get; set; }
        }
    }
}
