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

        protected override INotifyExpression<object> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableBoxExpression<T>(Inner.ApplyParameters(parameters, trace));
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
                return Inner is INotifyReversableExpression<T> innerReversable && innerReversable.IsReversable;
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
