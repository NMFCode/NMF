using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableConditionalExpression<T> : NotifyExpression<T>
    {
        public override string ToString()
        {
            return string.Format("({0} ? {1} : {2})", Test.ToString(), True.ToString(), False.ToString()) + "{" + (Value != null ? Value.ToString() : "(null)") + "}";
        }

        public ObservableConditionalExpression(ConditionalExpression expression, ObservableExpressionBinder binder)
            : this(binder.VisitObservable<bool>(expression.Test), binder.VisitObservable<T>(expression.IfTrue), binder.VisitObservable<T>(expression.IfFalse)) { }

        public ObservableConditionalExpression(INotifyExpression<bool> test, INotifyExpression<T> truePart, INotifyExpression<T> falsePart)
        {
            if (test == null) throw new ArgumentNullException("test");
            if (truePart == null) throw new ArgumentNullException("truePart");
            if (falsePart == null) throw new ArgumentNullException("falsePart");

            Test = test;
            True = truePart;
            False = falsePart;

            Test.ValueChanged += TestChanged;
            True.ValueChanged += TruePartChanged;
            False.ValueChanged += FalsePartChanged;
        }

        private void FalsePartChanged(object sender, EventArgs e)
        {
            if (!IsAttached) return;
            if (!(bool)Test.Value) Refresh();
        }

        private void TruePartChanged(object sender, EventArgs e)
        {
            if (!IsAttached) return;
            if ((bool)Test.Value) Refresh();
        }

        private void TestChanged(object sender, EventArgs e)
        {
            if (!IsAttached) return;
            if (Test.Value)
            {
                True.Attach();
                False.Detach();
            }
            else
            {
                True.Detach();
                False.Attach();
            }
            Refresh();
        }

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Conditional;
            }
        }

        public override bool CanBeConstant
        {
            get
            {
                return (Test.IsConstant && ((bool)Test.Value ? True.IsConstant : False.IsConstant))
                    || (True.IsConstant && False.IsConstant && object.Equals(True.Value, False.Value)); 
            }
        }

        public override INotifyExpression<T> Reduce()
        {
            if (Test.CanBeConstant)
            {
                if ((bool)Test.Value)
                {
                    return True.Reduce();
                }
                else
                {
                    return False.Reduce();
                }
            }
            else
            {
                if (True.CanBeConstant && False.CanBeConstant && object.Equals(True.Value, False.Value))
                {
                    return new ObservableConstant<T>(True.Value);
                }
                else
                {
                    return this;
                }
            }
        }

        private Type TypeCore { get; set; }

        public INotifyExpression<bool> Test { get; set; }

        public INotifyExpression<T> True { get; set; }

        public INotifyExpression<T> False { get; set; }

        protected override T GetValue()
        {
            if (Test.Value)
            {
                return True.Value;
            }
            else
            {
                return False.Value;
            }
        }

        protected override void DetachCore()
        {
            var currentTest = Test.Value;
            Test.Detach();
            if (currentTest)
            {
                True.Detach();
            }
            else
            {
                False.Detach();
            }
        }

        protected override void AttachCore()
        {
            Test.Attach();
            if (Test.Value)
            {
                True.Attach();
            }
            else
            {
                False.Attach();
            }
        }

        public override bool IsParameterFree
        {
            get { return Test.IsParameterFree && True.IsParameterFree && False.IsParameterFree; }
        }

        public override INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableConditionalExpression<T>(Test.ApplyParameters(parameters), True.ApplyParameters(parameters), False.ApplyParameters(parameters));
        }
    }
}
