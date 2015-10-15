using System;

namespace NMF.Expressions
{
    public sealed class ConstantValue<T> : INotifyValue<T>
    {
        private T value;

        public ConstantValue(T value)
        {
            this.value = value;
        }

        public T Value
        {
            get { return value; }
        }

        event EventHandler<ValueChangedEventArgs> INotifyValue<T>.ValueChanged { add { } remove { } }

        void INotifyValue<T>.Detach() { }

        void INotifyValue<T>.Attach() { }

        bool INotifyValue<T>.IsAttached
        {
            get { return true; }
        }
    }
}
