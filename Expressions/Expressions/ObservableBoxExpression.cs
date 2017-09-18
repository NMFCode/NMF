using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    internal class ObservableBoxExpression<T> : NotifyExpression<object>
    {
        protected INotifyExpression<T> Inner { get; private set; }

        public ObservableBoxExpression(INotifyExpression<T> inner)
        {
            if (inner == null) throw new ArgumentNullException("inner");

            Inner = inner;
        }

        public override bool IsParameterFree
        {
            get
            {
                return Inner.IsParameterFree;
            }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Inner;
            }
        }

        public override INotifyExpression<object> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableBoxExpression<T>(Inner.ApplyParameters(parameters));
        }

        protected override object GetValue()
        {
            return Inner.Value;
        }
    }

    internal class ObservableReversableBoxExpression<T> : ObservableBoxExpression<T>, INotifyReversableExpression<object>
    {
        public ObservableReversableBoxExpression(INotifyReversableExpression<T> inner) : base(inner)
        {
        }

        public bool IsReversable
        {
            get
            {
                var innerReversable = Inner as INotifyReversableExpression<T>;
                return innerReversable != null && innerReversable.IsReversable;
            }
        }

        object INotifyReversableValue<object>.Value
        {
            get
            {
                return Inner.Value;
            }
            set
            {
                var innerReversable = (INotifyReversableExpression<T>)Inner;
                innerReversable.Value = (T)value;
            }
        }
    }
}
