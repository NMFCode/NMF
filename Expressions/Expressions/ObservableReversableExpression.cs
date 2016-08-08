using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            get
            {
                return base.Value;
            }
            set
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
