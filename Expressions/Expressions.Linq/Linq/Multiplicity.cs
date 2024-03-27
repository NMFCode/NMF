namespace NMF.Expressions.Linq
{
    internal struct Multiplicity<TItem>
    {
        public Multiplicity(TItem item, int count)
            : this()
        {
            Item = item;
            Count = count;
        }

        public int Count { get; private set; }

        public TItem Item { get; private set; }
    }
}
