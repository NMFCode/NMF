namespace NMF.Expressions
{
    internal abstract class ObservableReversableExpression<T> : NotifyExpression<T>, INotifyReversableExpression<T>
    {
        public abstract void SetValue(T value);

        public virtual bool IsReversable
        {
            get { return true; }
        }

        public new T Value
        {
#pragma warning disable S4275 // Getters and setters should access the expected fields
            get
            {
                return base.Value;
            }
            set
#pragma warning restore S4275 // Getters and setters should access the expected fields
            {
                SetValue(value);
            }
        }

        public new object ValueObject
        {
            get
            {
                return Value;
            }
        }
    }
}
