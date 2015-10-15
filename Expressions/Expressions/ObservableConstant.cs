using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableConstant<T> : NotifyExpression<T>
    {
        public ObservableConstant(T value) : base(value) { }

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Constant;
            }
        }

        public override bool CanReduce
        {
            get
            {
                return false;
            }
        }

        public override INotifyExpression<T> Reduce()
        {
            return this;
        }

        public override bool CanBeConstant
        {
            get
            {
                return true;
            }
        }

        public override bool IsConstant
        {
            get
            {
                return true;
            }
        }

        protected override T GetValue()
        {
            return Value;
        }

        public override void Refresh() { }

        protected override void DetachCore() { }
        protected override void AttachCore() { }

        public override bool IsParameterFree
        {
            get { return true; }
        }

        public override INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            return this;
        }
    }
}
