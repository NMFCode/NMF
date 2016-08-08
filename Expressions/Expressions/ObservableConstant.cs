using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableConstant<T> : NotifyExpression<T>
    {
        public override string ToString()
        {
            return Value != null ? Value.ToString() : "(null)";
        }

        public ObservableConstant(T value) : base(value) { }

        public override ExpressionType NodeType { get { return ExpressionType.Constant; } }

        public override bool CanReduce { get { return false; } }

        public override bool CanBeConstant { get { return true; } }

        public override bool IsConstant { get { return true; } }

        public override bool IsParameterFree { get { return true; } }

        public override IEnumerable<INotifiable> Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        public override INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            return this;
        }

        protected override T GetValue()
        {
            return Value;
        }

        public override INotifyExpression<T> Reduce()
        {
            return this;
        }

        public override bool Notify(IEnumerable<INotifiable> sources)
        {
            throw new InvalidOperationException("A constant cannot have a dependency and therefore cannot be notified of a dependency change.");
        }
    }
}
