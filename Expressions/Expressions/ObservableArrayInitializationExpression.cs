using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableArrayInitializationExpression<T> : NotifyExpression<T[]>
    {
        public ReadOnlyCollection<INotifyExpression<T>> Expressions { get; private set; }

        public ObservableArrayInitializationExpression(NewArrayExpression node, ObservableExpressionBinder binder)
            : this(node.Expressions.Select(e => binder.VisitObservable<T>(e))) { }

        public ObservableArrayInitializationExpression(IEnumerable<INotifyExpression<T>> expressions)
            : this(expressions as List<INotifyExpression<T>> ?? expressions.ToList()) { }

        private ObservableArrayInitializationExpression(List<INotifyExpression<T>> expressions)
            : base(new T[expressions.Count])
        {
            if (expressions == null) throw new ArgumentNullException("expressions");

            Expressions = new ReadOnlyCollection<INotifyExpression<T>>(expressions);
        }

        public override bool CanBeConstant
        {
            get { return Expressions.All(ex => ex.IsConstant); }
        }

        public override ExpressionType NodeType { get { return ExpressionType.NewArrayInit; } }

        public override bool IsParameterFree
        {
            get { return Expressions.All(e => e.IsParameterFree); }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                foreach (var expression in Expressions)
                    yield return expression;
            }
        }

        protected override T[] GetValue()
        {
            return Value;
        }

        protected override INotifyExpression<T[]> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableArrayInitializationExpression<T>(Expressions.Select(e => e.ApplyParameters(parameters, trace)));
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (sources.Count > 0)
            {
                foreach (IValueChangedNotificationResult<T> change in sources)
                {
                    var index = Expressions.IndexOf((INotifyExpression<T>)change.Source);
                    Value[index] = change.NewValue;
                }
                OnValueChanged(Value, Value);
                return new ValueChangedNotificationResult<T[]>(this, Value, Value);
            }
            return UnchangedNotificationResult.Instance;
        }

        protected override void OnAttach()
        {
            for (int i = 0; i < Expressions.Count; i++)
            {
                Value[i] = Expressions[i].Value;
            }
        }

        protected override void OnDetach()
        {
            Array.Clear(Value, 0, Value.Length);
        }
    }
}
