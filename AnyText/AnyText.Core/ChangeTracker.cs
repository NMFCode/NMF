using NMF.AnyText.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    internal class ChangeTracker
    {
        private readonly List<TextEdit> _edits = new List<TextEdit>();
        private readonly List<string[]> _oldTexts = new List<string[]>();
        private readonly ItemEqualityComparer<string> _contentComparer = new ItemEqualityComparer<string>();

        public IList<TextEdit> CurrentEdits => _edits.AsReadOnly();

        public void AddEdit(TextEdit edit, string[] input)
        {
            var editIndex = 0;
            var adjacentEdit = GetNextEdit(ref editIndex, edit.Start);
            if (adjacentEdit == null)
            {
                if (editIndex > 0 && _edits[editIndex - 1].EndAfterEdit == edit.Start)
                {
                    var prev = _edits[editIndex - 1];
                    var newUpdate = MergeTexts(prev.NewText, edit.NewText);
                    var newOld = MergeTexts(_oldTexts[editIndex - 1], GetOldText(edit, input));
                    _edits[editIndex - 1] = new TextEdit(prev.Start, edit.End, newUpdate);
                    _oldTexts[editIndex - 1] = newOld;
                }
                else
                {
                    _edits.Add(edit);
                    _oldTexts.Add(GetOldText(edit, input));
                }
            }
            else if (adjacentEdit.Start >= edit.Start)
            {
                AddEditWithAdjacentAfterStart(edit, input, editIndex, adjacentEdit);
            }
            else // adj.EndAfterEdit >= Start but adj.Start < Start => overlap
            {
                if (edit.End <= adjacentEdit.EndAfterEdit)
                {
                    var relativeStart = edit.Start - adjacentEdit.Start;
                    var relativeEnd = edit.End - adjacentEdit.Start;
                    var innerEdit = new TextEdit(new ParsePosition(relativeStart.Line, relativeStart.Col), new ParsePosition(relativeEnd.Line, relativeEnd.Col), edit.NewText);
                    var updatedText = innerEdit.Apply(adjacentEdit.NewText);
                    if (_contentComparer.Equals(updatedText, _oldTexts[editIndex]))
                    {
                        _edits.RemoveAt(editIndex);
                        _oldTexts.RemoveAt(editIndex);
                    }
                    else
                    {
                        _edits[editIndex] = new TextEdit(adjacentEdit.Start, adjacentEdit.End, updatedText);
                    }
                }
                else
                {
                    var startPosDeltaInAdjacent = edit.Start - adjacentEdit.Start;
                    var startPosInAdjacent = new ParsePosition(startPosDeltaInAdjacent.Line, startPosDeltaInAdjacent.Col);
                    var insertion = new TextEdit(startPosInAdjacent,
                            new ParsePosition(adjacentEdit.NewText.Length - 1, adjacentEdit.NewText[adjacentEdit.NewText.Length - 1].Length),
                            edit.NewText);
                    var updatedText = insertion.Apply(adjacentEdit.NewText);
                    var overlapEnd = UpdateEndPosition(edit.End, adjacentEdit.EndAfterEdit, edit.Start);
                    _edits[editIndex] = new TextEdit(adjacentEdit.Start, overlapEnd, updatedText);
                    // TODO: update oldText
                }
            }
        }

        private string[] MergeTexts(string[] text1, string[] text2)
        {
            var t1Len = Math.Max(text1.Length, 1);
            var t2Len = Math.Max(text2.Length, 1);
            var ret = new string[t1Len + t2Len - 1];
            Array.Copy(text1, ret, text1.Length);
            if (text2.Length > 0)
            {
                ret[t1Len - 1] += text2[0];
            }
            Array.Copy(text2, 1, ret, t1Len, t2Len - 1);
            return ret;
        }

        private ParsePosition UpdateEndPosition(ParsePosition end, ParsePosition innerEditEnd, ParsePosition innerEditEndAfterEdit)
        {
            if (innerEditEndAfterEdit.Line < end.Line)
            {
                return new ParsePosition(end.Line + innerEditEndAfterEdit.Line - innerEditEnd.Line, end.Col);
            }
            return end + (innerEditEndAfterEdit - innerEditEnd);
        }

        private void AddEditWithAdjacentAfterStart(TextEdit edit, string[] input, int editIndex, TextEdit adjacentEdit)
        {
            var oldText = _oldTexts[editIndex];
            var firstStart = adjacentEdit.Start;
            var lastEnd = adjacentEdit.End;
            var lastEndAfterEdit = adjacentEdit.EndAfterEdit;
            while (adjacentEdit.EndAfterEdit <= edit.End)
            {
                _edits.RemoveAt(editIndex);
                _oldTexts.RemoveAt(editIndex);
                lastEnd = adjacentEdit.End;
                lastEndAfterEdit = adjacentEdit.EndAfterEdit;
                if (editIndex < _edits.Count)
                {
                    adjacentEdit = _edits[editIndex];
                    if (adjacentEdit.Start <= edit.End)
                    {
                        continue;
                    }
                }
                else
                {
                    adjacentEdit = null;
                }
                break;
            }
            if (adjacentEdit == null || adjacentEdit.EndAfterEdit < edit.End)
            {
                var lengthTillStart = firstStart - edit.Start;
                var lengthFromEnd = edit.End - lastEndAfterEdit;
                if (lengthTillStart != default || lengthFromEnd != default)
                {
                    if (lastEnd == lastEndAfterEdit)
                    {
                        InsertEdit(editIndex, edit, input);
                    }
                    else
                    {
                        InsertEdit(editIndex, new TextEdit(edit.Start, UpdateEndPosition(edit.End, lastEndAfterEdit, lastEnd), edit.NewText), input);
                    }
                }
                else if (!_contentComparer.Equals(oldText, edit.NewText))
                {
                    _edits.Insert(editIndex, new TextEdit(edit.Start, edit.EndAfterEdit, edit.NewText));
                    _oldTexts.Insert(editIndex, edit.NewText);
                }
            }
            else
            {
                InsertEdit(editIndex, edit, input);
            }
        }

        private void InsertEdit(int editIndex, TextEdit edit, string[] input)
        {
            _edits.Insert(editIndex, edit);
            _oldTexts.Insert(editIndex, GetOldText(edit, input));
        }

        private string[] GetOldText(TextEdit edit, string[] input)
        {
            var ret = new string[edit.End.Line - edit.Start.Line + 1];
            if (edit.Start.Line == edit.End.Line)
            {
                if (edit.Start.Line < input.Length)
                {
                    ret[0] = input[edit.Start.Line].Substring(edit.Start.Col, edit.End.Col - edit.Start.Col);
                }
                else
                {
                    ret[0] = string.Empty;
                }
            }
            else
            {
                var startLine = edit.Start.Line;
                var endLine = edit.End.Line;
                ret[0] = input[startLine].Substring(edit.Start.Col);
                var index = 1;
                while (index + startLine < endLine)
                {
                    if (startLine + index < input.Length)
                    {
                        ret[index] = input[startLine + index];
                    }
                    else
                    {
                        ret[index] = string.Empty;
                    }
                    index++;
                }
                if (endLine < input.Length)
                {
                    ret[index] = input[endLine].Substring(0, edit.End.Col);
                }
                else
                {
                    ret[index] = string.Empty;
                }
            }
            return ret;
        }

        public List<RuleApplicationListMigrationEntry> CalculateListMigrations(List<RuleApplication> old, List<RuleApplication> migrateTo, ParseContext context)
        {
            var result = new List<RuleApplicationListMigrationEntry>();
            int index = 0;
            int suffix = CalculateCommonSuffix(old, migrateTo);
            int len = old.Count - suffix;
            int mLen = migrateTo.Count - suffix;
            int indexOffset = 0;
            int editIndex = 0;
            while (index + indexOffset < len)
            {
                var current = old[index + indexOffset];
                if (index < mLen && migrateTo[index] == current)
                {
                    index++;
                    continue;
                }
                if (index >= mLen)
                {
                    result.Add(new RuleApplicationListMigrationEntry(index, RuleApplicationListMigrationType.Remove));
                    indexOffset++;
                    continue;
                }
                var target = migrateTo[index];
                if (target.IsActive)
                {
                    var oldIndex = old.IndexOf(target, index + indexOffset);
                    if (oldIndex != -1)
                    {
                        while (index + indexOffset < oldIndex)
                        {
                            result.Add(new RuleApplicationListMigrationEntry(index, RuleApplicationListMigrationType.Remove));
                            indexOffset++;
                        }
                        continue;
                    }
                }
                var edit = GetNextEdit(ref editIndex, target.CurrentPosition);
                if (edit == null)
                {
                    result.Add(new RuleApplicationListMigrationEntry(index, RuleApplicationListMigrationType.Migrate));
                    index++;
                }
                else
                {
                    if (index== 0 && editIndex > 0)
                    {
                        var lastEdit = _edits[editIndex - 1];
                        RemoveObsoleted(old, context, result, len, ref indexOffset, lastEdit, index);
                        if (index + indexOffset < len)
                        {
                            current = old[index + indexOffset];
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (MigrateApplicationsBeforeEditReachesEnd(old, migrateTo, result, ref index, len, mLen, indexOffset, ref current, ref target, edit))
                    {
                        continue;
                    }
                    var migrateIndex = -1;
                    if (!IsInsertion(target, context, edit) && !IsObsoleted(current, context, edit))
                    {
                        migrateIndex = result.Count;
                        result.Add(new RuleApplicationListMigrationEntry(index, RuleApplicationListMigrationType.Migrate));
                        index++;
                    }
                    RemoveObsoleted(old, context, result, len, ref indexOffset, edit, index);
                    InsertInsertions(migrateTo, context, result, ref index, ref indexOffset, mLen, edit);

                    var endsWithInsertion = index < mLen && edit.EndAfterEdit > migrateTo[index].CurrentPosition;
                    var endsWithRemove = index + indexOffset < len && IsEndOfRemove(old[index + indexOffset], edit, context);

                    if (endsWithInsertion && !endsWithRemove)
                    {
                        InsertOrModifyMigrateToInsert(result, index, ref indexOffset, migrateIndex);
                        index++;
                    }
                    else if (endsWithRemove && !endsWithInsertion)
                    {
                        RemoveOrModifyMigrateToRemove(result, index, migrateIndex);
                        indexOffset++;
                    }
                    else if (endsWithInsertion && endsWithRemove)
                    {
                        result.Add(new RuleApplicationListMigrationEntry(index, RuleApplicationListMigrationType.Migrate));
                        index++;
                    }
                    editIndex++;
                }
            }
            while (index < mLen)
            {
                var nextInner = migrateTo[index];
                result.Add(new RuleApplicationListMigrationEntry(index, RuleApplicationListMigrationType.Insert));
                index++;
            }
            return result;
        }

        private static void RemoveOrModifyMigrateToRemove(List<RuleApplicationListMigrationEntry> result, int index, int migrateIndex)
        {
            if (migrateIndex != -1)
            {
                result[migrateIndex] = new RuleApplicationListMigrationEntry(result[migrateIndex].Index, RuleApplicationListMigrationType.Remove);
                for (int i = migrateIndex + 1; i < result.Count; i++)
                {
                    var en = result[i];
                    result[i] = new RuleApplicationListMigrationEntry(en.Index - 1, en.Type);
                }
                result.Add(new RuleApplicationListMigrationEntry(index - 1, RuleApplicationListMigrationType.Migrate));
            }
            else
            {
                result.Add(new RuleApplicationListMigrationEntry(index, RuleApplicationListMigrationType.Remove));
            }
        }

        private static void InsertOrModifyMigrateToInsert(List<RuleApplicationListMigrationEntry> result, int index, ref int indexOffset, int migrateIndex)
        {
            if (migrateIndex != -1)
            {
                result[migrateIndex] = new RuleApplicationListMigrationEntry(result[migrateIndex].Index, RuleApplicationListMigrationType.Insert);
                result.Add(new RuleApplicationListMigrationEntry(index, RuleApplicationListMigrationType.Migrate));
                indexOffset--;
            }
            else
            {
                result.Add(new RuleApplicationListMigrationEntry(index, RuleApplicationListMigrationType.Insert));
            }
        }

        private static bool MigrateApplicationsBeforeEditReachesEnd(List<RuleApplication> old, List<RuleApplication> migrateTo, List<RuleApplicationListMigrationEntry> result, ref int index, int len, int mLen, int indexOffset, ref RuleApplication current, ref RuleApplication target, TextEdit edit)
        {
            var earlyQuit = false;
            while (current.CurrentPosition + current.Length <= edit.Start && target.CurrentPosition + target.Length <= edit.Start)
            {
                result.Add(new RuleApplicationListMigrationEntry(index, RuleApplicationListMigrationType.Migrate));
                index++;
                if (index + indexOffset < len && index < mLen)
                {
                    current = old[index + indexOffset];
                    target = migrateTo[index];
                }
                else
                {
                    earlyQuit = true;
                    break;
                }
            }

            return earlyQuit;
        }

        private static bool IsEndOfRemove(RuleApplication ruleApplication, TextEdit edit, ParseContext context)
        {
            if (context.Matcher.IsFaithfulPosition(ruleApplication))
            {
                return edit.EndAfterEdit > ruleApplication.CurrentPosition;
            }
            else
            {
                return edit.End > ruleApplication.CurrentPosition;
            }
        }

        private void InsertInsertions(List<RuleApplication> migrateTo, ParseContext context, List<RuleApplicationListMigrationEntry> result, ref int index, ref int indexOffset, int mLen, TextEdit edit)
        {
            if (edit.NewText.Length > 1 || (edit.NewText.Length == 1 && edit.NewText[0].Length > 0))
            {
                while (index < mLen && IsInsertion(migrateTo[index], context, edit))
                {
                    result.Add(new RuleApplicationListMigrationEntry(index, RuleApplicationListMigrationType.Insert));
                    index++;
                    indexOffset--;
                }
            }
        }

        private void RemoveObsoleted(List<RuleApplication> old, ParseContext context, List<RuleApplicationListMigrationEntry> result, int len, ref int indexOffset, TextEdit edit, int oldIndex)
        {
            if (edit.End > edit.Start)
            {
                while (oldIndex + indexOffset < len && IsObsoleted(old[oldIndex + indexOffset], context, edit))
                {
                    result.Add(new RuleApplicationListMigrationEntry(oldIndex, RuleApplicationListMigrationType.Remove));
                    indexOffset++;
                }
            }
        }

        private TextEdit GetNextEdit(ref int editIndex, ParsePosition position)
        {
            while (editIndex < _edits.Count)
            {
                var edit = _edits[editIndex];
                if (edit.EndAfterEdit >= position)
                {
                    return edit;
                }
                editIndex++;
            }
            return null;
        }

        private int CalculateCommonSuffix(List<RuleApplication> old, List<RuleApplication> migrateTo)
        {
            var suffixLength = 0;
            var min = Math.Min(old.Count, migrateTo.Count);
            var len = old.Count - 1;
            var mLen = migrateTo.Count - 1;
            while (suffixLength < min && old[len - suffixLength] == migrateTo[mLen - suffixLength])
            {
                suffixLength++;
            }

            return suffixLength;
        }

        private bool IsInsertion(RuleApplication ruleApplication, ParseContext context, TextEdit edit)
        {
            if (ruleApplication.CurrentPosition >= edit.Start &&
                ruleApplication.CurrentPosition + ruleApplication.Length <= edit.EndAfterEdit)
            {
                return true;
            }
            return false;
        }

        private bool IsObsoleted(RuleApplication ruleApplication, ParseContext context, TextEdit byEdit)
        {
            if (context.Matcher.IsObsoleted(ruleApplication))
            {
                return true;
            }
            return ruleApplication.CurrentPosition >= byEdit.Start &&
                ruleApplication.CurrentPosition + ruleApplication.Length <= byEdit.EndAfterEdit;
        }

        public void Reset()
        {
            _edits.Clear();
            _oldTexts.Clear();
        }
    }
}
