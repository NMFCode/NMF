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
                            result.Add(new RuleApplicationListMigrationEntry(index + indexOffset, RuleApplicationListMigrationType.Remove));
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
            while (editIndex < _edits.Length)
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

        internal void Reset() => SetEdits(Array.Empty<TextEdit>());
    }
}
