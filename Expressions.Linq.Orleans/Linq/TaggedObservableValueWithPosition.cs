namespace NMF.Expressions.Linq.Orleans
{
    internal class TaggedObservableValueWithPosition<T, TTag> : TaggedObservableValue<T, TTag>
    {
        public int Offset { get; private set; }

        public TaggedObservableValueWithPosition(INotifyExpression<T> expression, TTag tag, int offset) : base(expression, tag)
        {
            Offset = offset;
        }
    }
}