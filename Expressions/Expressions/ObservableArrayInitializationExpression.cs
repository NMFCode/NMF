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

            Expressions = expressions.AsReadOnly();
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

        public override INotifyExpression<T[]> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableArrayInitializationExpression<T>(Expressions.Select(e => e.ApplyParameters(parameters)));
        }

        public override bool Notify(IEnumerable<INotifiable> sources)
        {
            for (int i = 0; i < Expressions.Count; i++)
            {
                if (sources.Contains(Expressions[i]))
                {
                    Value[i] = Expressions[i].Value;
                }
            }
            return true;
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
