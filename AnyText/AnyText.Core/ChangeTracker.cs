using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    internal class ChangeTracker
    {
        private TextEdit[] _edits = Array.Empty<TextEdit>();

        public void SetEdits(IEnumerable<TextEdit> edits)
        {
            if (edits.TryGetNonEnumeratedCount(out var count) && count <= 1)
            {
                _edits = edits as TextEdit[] ?? edits.ToArray();
            }
            else
            {
                _edits = edits.OrderBy(e => e.Start).ToArray();
            }
        }

        public bool IsInsertion(RuleApplication ruleApplication, ParseContext context)
        {
            // TODO: implement binary search
            for (int i = 0; i < _edits.Length; i++)
            {
                var edit = _edits[i];
                if (ruleApplication.CurrentPosition >= edit.Start &&
                    ruleApplication.CurrentPosition + ruleApplication.Length <= context.Matcher.NextTokenPosition(edit.Start + GetLength(edit)))
                {
                    return true;
                }
            }
            return false;
        }

        private ParsePositionDelta GetLength(TextEdit textEdit)
        {
            if (textEdit.NewText.Length == 0)
            {
                return default;
            }
            var lastLineLength = textEdit.NewText[textEdit.NewText.Length - 1].Length;
            return new ParsePositionDelta(textEdit.NewText.Length - 1, lastLineLength);
        }

        public bool IsObsoleted(RuleApplication ruleApplication, ParseContext context)
        {
            if (context.Matcher.IsObsoleted(ruleApplication))
            {
                return true;
            }
            for (int i = 0; i < _edits.Length; i++)
            {
                var edit = _edits[i];
                var pos = ruleApplication.CurrentPosition;
                if (pos >= edit.Start)
                {
                    pos = CompensateMatcherInsertions(pos, edit);
                    if (pos + ruleApplication.Length <= context.Matcher.NextTokenPosition(edit.End))
                    {
                        return true;
                    }
                }
                else
                {
                    // edits are sorted, further edits cannot contain rule application
                    return false;
                }
            }
            return false;
        }

        private static ParsePosition CompensateMatcherInsertions(ParsePosition pos, TextEdit edit)
        {
            var linesDelta = edit.NewText.Length - (edit.End.Line - edit.Start.Line + 1);
            pos = new ParsePosition(pos.Line - linesDelta, pos.Col);
            return pos;
        }

        internal void Reset() => SetEdits(Array.Empty<TextEdit>());
    }
}
