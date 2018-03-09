﻿using System;
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

        protected override INotifyExpression<T> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableConditionalExpression<T>(Test.ApplyParameters(parameters, trace), True.ApplyParameters(parameters, trace), False.ApplyParameters(parameters, trace));
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Any(c => c.Source == Test))
            {
                if (Test.Value)
                {
                    True.Successors.Set(this);
                    False.Successors.Unset(this);
                }
                else
                {
                    True.Successors.Unset(this);
                    False.Successors.Set(this);
                }
            }

            return base.Notify(sources);
        }
    }
}
