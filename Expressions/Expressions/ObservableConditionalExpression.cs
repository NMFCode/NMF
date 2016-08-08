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
        }

        public override ExpressionType NodeType { get { return ExpressionType.Conditional; } }

        public override bool CanBeConstant
        {
            get
            {
                return (Test.IsConstant && (Test.Value ? True.IsConstant : False.IsConstant))
                    || (True.IsConstant && False.IsConstant && object.Equals(True.Value, False.Value)); 
            }
        }

        public INotifyExpression<bool> Test { get; set; }

        public INotifyExpression<T> True { get; set; }

        public INotifyExpression<T> False { get; set; }

        public override bool IsParameterFree
        {
            get { return Test.IsParameterFree && True.IsParameterFree && False.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Test;
                yield return Test.Value ? True : False;
            }
        }

        public override INotifyExpression<T> Reduce()
        {
            if (Test.CanBeConstant)
            {
                if (Test.Value)
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

        public override INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableConditionalExpression<T>(Test.ApplyParameters(parameters), True.ApplyParameters(parameters), False.ApplyParameters(parameters));
        }

        public override bool Notify(IEnumerable<INotifiable> sources)
        {
            if (sources.Contains(Test))
            {
                if (Test.Value)
                {
                    True.Successors.Add(this);
                    False.Successors.Remove(this);
                }
                else
                {
                    True.Successors.Remove(this);
                    False.Successors.Add(this);
                }
            }

            return base.Notify(sources);
        }
    }
}
