using System.Collections.Generic;

namespace NMF.AnyText
{
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
