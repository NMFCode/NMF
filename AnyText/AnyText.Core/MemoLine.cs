using System.Collections.Generic;
using System.Diagnostics;

namespace NMF.AnyText
{
    [DebuggerDisplay("Line {LineNo} (spans {MaxExaminedLength})")]
    internal sealed class MemoLine
    {
        public MemoLine() { }

        public SortedDictionary<int, MemoColumn> Columns { get; } = new SortedDictionary<int, MemoColumn>();

        public ParsePositionDelta MaxExaminedLength { get; set; }

        public int LineNo { get; set; }

        public MemoColumn GetOrCreateColumn(int col)
        {
            if (!Columns.TryGetValue(col, out var column))
            {
                column = new MemoColumn(col, this);
                Columns.Add(col, column);
            }

            return column;
        }
    }
}
