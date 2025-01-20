using System;
using System.Collections.Generic;
using System.Linq;
using NMF.Expressions;

namespace NMF.Models.Dynamic
{
    internal class ReversibleConstant<T> : NotifyExpression<T>, INotifyReversableExpression<T>
    {
        public ReversibleConstant(T value) : base(value)
        {
        }

        public override bool IsParameterFree => true;

        public override IEnumerable<INotifiable> Dependencies => Enumerable.Empty<INotifiable>();

        public bool IsReversable => false;

        T INotifyReversableValue<T>.Value { get => Value; set => throw new NotSupportedException(); }

        protected override INotifyExpression<T> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return this;
        }

        protected override T GetValue()
        {
            return Value;
        }
    }
}
