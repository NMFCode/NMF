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
			: base(Array.CreateInstance(typeof(T), expressions.Count) as T[])
		{
            if (expressions == null) throw new ArgumentNullException("expressions");

            Expressions = new ReadOnlyCollection<INotifyExpression<T>>(expressions);

            foreach (var arg in expressions)
            {
                arg.ValueChanged += ArgumentChanged;
            }
		}

        public override bool CanBeConstant
        {
            get
            {
                return Expressions.All(ex => ex.IsConstant);
            }
        }

        protected override T[] GetValue()
        {
			return Value;
        }

        protected override void DetachCore()
        {
            foreach (var arg in Expressions)
            {
                arg.Detach();
            }
        }

        protected override void AttachCore()
        {
            for (int i = 0; i < Expressions.Count; i++)
            {
                var expression = Expressions[i];
                expression.Attach();
                Array array = (Array)Value;
                array.SetValue(expression.Value, i);
            }
        }

        private void ArgumentChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
			for (int i = 0; i < Expressions.Count; i++)
			{
				if (Expressions[i] == sender)
				{
					((Array)Value).SetValue(Expressions[i].Value, i);
				}
			}
            OnValueChanged(Value, Value);
        }

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.NewArrayInit;
            }
        }

        public override bool IsParameterFree
        {
            get { return Expressions.All(e => e.IsParameterFree); }
        }

        public override INotifyExpression<T[]> ApplyParameters(IDictionary<string, object> parameters)
        {
            return new ObservableArrayInitializationExpression<T>(Expressions.Select(e => e.ApplyParameters(parameters)));
        }
    }
}
