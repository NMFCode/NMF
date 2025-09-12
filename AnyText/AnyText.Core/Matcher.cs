using Microsoft.Win32.SafeHandles;
using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    if (memoCol.Value.Applications.Values.Any(a => a.IsPositive && a.Rule.IsLiteral && !a.Rule.IsComment))
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

                    foreach (var ruleApplication in col.Value.Applications.Values)
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
                return col.Applications.Values.Where(ra => !ra.IsPositive);
            }
            return Enumerable.Empty<RuleApplication>();
        }

        private static Dictionary<Rule, RuleApplication> GetCol(SortedDictionary<int, MemoColumn> line, int col)
        {
            if (line.TryGetValue(col, out var ruleApplications))
            {
                return ruleApplications.Applications;
            }
            return null;
        }

        internal RuleApplication MatchCore(Rule rule, RecursionContext recursionContext, ParseContext context, ref ParsePosition position)
        {
            var processor = MatchOrCreateMatchProcessor(rule, context, ref recursionContext, ref position);
            if (processor.IsMatch)
            {
                return processor.Match;
            }
            RuleApplication ruleApplication = null;
            var processorStack = new Stack<MatchProcessor>();
            processorStack.Push(processor.MatchProcessor);
            while (processorStack.Count > 0)
            {
                var current = processorStack.Pop();
                var next = current.NextMatchProcessor(context, ruleApplication, ref position, ref recursionContext);
                if (next.IsMatchProcessor)
                {
                    processorStack.Push(current);
                    processorStack.Push(next.MatchProcessor);
                    ruleApplication = null;
                }
                else
                {
                    ruleApplication = next.Match;
                }
            }
            return ruleApplication;
        }

        internal MatchOrMatchProcessor MatchOrCreateMatchProcessor(Rule rule, ParseContext context, ref RecursionContext recursionContext, ref ParsePosition position)
        {
            var line = GetLine(position.Line);
            var ruleApplications = GetCol(line.Columns, position.Col);

            if (ruleApplications == null || !ruleApplications.TryGetValue(rule, out var ruleApplication))
            {
                var col = position.Col;
                var column = line.GetOrCreateColumn(col);
                if (rule.IsLeftRecursive)
                {
                    RecursionContext oldRecursion = recursionContext;
                    RecursionContext createdRecursion = null;
                    if (recursionContext == null || recursionContext.Position != position)
                    {
                        recursionContext = new RecursionContext(position);
                        createdRecursion = recursionContext;
                    }
                    recursionContext.RuleStack.Push(rule);
                    var cycleDetector = new FailedRuleApplication(rule, new ParsePositionDelta(1, 0), "Recursive");
                    cycleDetector.AddToColumn(column);
                    var processor = rule.NextMatchProcessor(context, recursionContext, ref position);
                    if (processor.IsMatchProcessor)
                    {
                        if (createdRecursion != null)
                        {
                            return new MatchOrMatchProcessor(new RecursiveProcessorWrapper(processor.MatchProcessor, column, createdRecursion, oldRecursion));
                        }
                        return new MatchOrMatchProcessor(new StandardProcessorWrapper(processor.MatchProcessor, column));
                    } 
                    else
                    {
                        ruleApplication = processor.Match;
                    }
                    if (createdRecursion != null)
                    {
                        createdRecursion.StopAddingContinuations();
                        ExtendContinuations(createdRecursion, column, context, ref position, ref ruleApplication);
                    }
                    ruleApplication.AddToColumn(column);
                    line.MaxExaminedLength = ParsePositionDelta.Larger(line.MaxExaminedLength, PrependColOffset(ruleApplication.ExaminedTo, col));
                }
                else
                {
                    var processor = rule.NextMatchProcessor(context, recursionContext, ref position);
                    if (processor.IsMatchProcessor)
                    {
                        return new MatchOrMatchProcessor(new StandardProcessorWrapper(processor.MatchProcessor, column));
                    }
                    ruleApplication = processor.Match;
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
            return new MatchOrMatchProcessor(ruleApplication);
        }

        private sealed class StandardProcessorWrapper : MatchProcessor
        {
            public StandardProcessorWrapper(MatchProcessor inner, MemoColumn column)
            {
                _inner = inner;
                _column = column;
            }

            private readonly MatchProcessor _inner;
            private readonly MemoColumn _column;

            public override Rule Rule => _inner.Rule;

            public override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RuleApplication ruleApplication, ref ParsePosition position, ref RecursionContext recursionContext)
            {
                var next = _inner.NextMatchProcessor(context, ruleApplication, ref position, ref recursionContext);
                if (next.IsMatch)
                {
                    ruleApplication = next.Match;
                    var line = _column.Line;
                    line.MaxExaminedLength = ParsePositionDelta.Larger(line.MaxExaminedLength, PrependColOffset(ruleApplication.ExaminedTo, _column.Column));
                    ruleApplication.AddToColumn(_column);
                    ruleApplication.Comments = _column.Comments;
                    if (ruleApplication.Rule.TrailingWhitespaces)
                    {
                        context.Matcher.MoveOverWhitespaceAndComments(context, ref position);
                    }
                }
                return next;
            }
        }

        private sealed class RecursiveProcessorWrapper : MatchProcessor
        {
            public RecursiveProcessorWrapper(MatchProcessor inner, MemoColumn column, RecursionContext recursionContext, RecursionContext oldRecursionContext)
            {
                _inner = inner;
                _column = column;
                _recursionContext = recursionContext;
                _oldRecursionContext = oldRecursionContext;
            }

            private readonly MatchProcessor _inner;
            private readonly MemoColumn _column;
            private readonly RecursionContext _recursionContext;
            private readonly RecursionContext _oldRecursionContext;

            public override Rule Rule => _inner.Rule;

            public override MatchOrMatchProcessor NextMatchProcessor(ParseContext context, RuleApplication ruleApplication, ref ParsePosition position, ref RecursionContext recursionContext)
            {
                var next = _inner.NextMatchProcessor(context, ruleApplication, ref position, ref recursionContext);
                if (next.IsMatch)
                {
                    ruleApplication = next.Match;
                    _recursionContext.StopAddingContinuations();
                    ExtendContinuations(_recursionContext, _column, context, ref position, ref ruleApplication);
                    var line = _column.Line;
                    line.MaxExaminedLength = ParsePositionDelta.Larger(line.MaxExaminedLength, PrependColOffset(ruleApplication.ExaminedTo, _column.Column));
                    ruleApplication.AddToColumn(_column);
                    ruleApplication.Comments = _column.Comments;
                    if (ruleApplication.Rule.TrailingWhitespaces)
                    {
                        context.Matcher.MoveOverWhitespaceAndComments(context, ref position);
                    }
                    recursionContext = _oldRecursionContext;
                    return new MatchOrMatchProcessor(ruleApplication);
                }
                return next;
            }
        }

        private static void ExtendContinuations(RecursionContext recursionContext, MemoColumn column, ParseContext context, ref ParsePosition position, ref RuleApplication ruleApplication)
        {
            var headRule = ruleApplication.Rule;
            var cont = true;
            while (cont)
            {
                ruleApplication.AddToColumn(column);
                cont = TryExtendContinuations(recursionContext, context, column.Applications, ref position, ref ruleApplication);
            }
            if (ruleApplication.Rule != headRule)
            {
                var headPosition = new ParsePosition(column.Line.LineNo, column.Column);
                ruleApplication = headRule.Match(context, recursionContext, ref headPosition);
                position = headPosition;
            }
        }

        private static bool TryExtendContinuations(RecursionContext recursionContext, ParseContext context, Dictionary<Rule, RuleApplication> column, ref ParsePosition position, ref RuleApplication ruleApplication)
        {
            var currentPosition = position;
            foreach (var continuation in recursionContext.Continuations)
            {
                InvalidateRules(column, ruleApplication, continuation);
                var newRuleApplication = continuation.ResolveRecursion(ruleApplication, context, recursionContext, ref position);
                if (newRuleApplication != ruleApplication && position > currentPosition)
                {
                    ruleApplication = newRuleApplication;
                    InvalidateRules(column, ruleApplication, continuation);
                    return true;
                }
                else
                {
                    position = currentPosition;
                }
            }
            return false;
        }

        private static void InvalidateRules(Dictionary<Rule, RuleApplication> column, RuleApplication ruleApplication, RecursiveContinuation continuation)
        {
            foreach (var rule in continuation.AffectedRules)
            {
                if (column.TryGetValue(rule, out var currentApp) && currentApp != ruleApplication)
                {
                    column.Remove(rule);
                }
            }
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
            var match = MatchCore(context.RootRule, null, context, ref position);
            while (position.Line < context.Input.Length)
            {
                var recovery = match.Recover(match, context, out position);
                if (recovery == match || recovery.Length == match.Length)
                {
                    break;
                }
                match = recovery;
            }
            if (!match.IsPositive || position.Line == context.Input.Length)
            {
                return match;
            }
            var unexpected = new FailedParseApplication(context.RootRule, match, new ParsePositionDelta(position.Line, position.Col), "Unexpected content");
            unexpected.SetColumn(GetLine(position.Line).GetOrCreateColumn(position.Col));
            return unexpected;
        }

        /// <summary>
        /// Removes any memoization based on the given text edit and updates the memo table
        /// </summary>
        /// <param name="edit">The change in the input text</param>
        public void Apply(TextEdit edit)
        {
            var refreshLineIndices = false;
            for (int i = 0; i <= edit.End.Line && i < _memoTable.Count; i++)
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

                    foreach (var entry in col.Value.Applications.ToArray())
                    {
                        var ruleApplication = entry.Value;
                        if (pos + ruleApplication.ExaminedTo < edit.Start)
                        {
                            maxReach = ParsePositionDelta.Larger(maxReach, PrependColOffset(ruleApplication.ExaminedTo, col.Key));
                        }
                        else
                        {
                            col.Value.Applications.Remove(entry.Key);
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
                foreach (var ruleApplication in line.Columns[cols[j]].Applications.Values)
                {
                    if (ruleApplication.Rule.IsComment)
                    {
                        RemoveComment(ruleApplication, lineIndex, cols[j]);
                    }
                }
                line.Columns.Remove(cols[j]);
            }
        }

        internal void MoveOverWhitespaceAndComments(ParseContext context, ref ParsePosition position)
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
                        foreach (var app in column.Applications.Values)
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
            if (line.Columns.TryGetValue(position.Col, out var column) && column.Applications.Values.FirstOrDefault() is var firstMatch && firstMatch != null && firstMatch.Rule.IsComment)
            {
                position += firstMatch.Length;
                MoveOverWhitespace(context, ref position);
            }
            foreach (var commentRule in _commentRules)
            {
                var comment = commentRule.Match(context, null, ref position);
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
