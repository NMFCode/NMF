using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableMemberInit<T> : NotifyExpression<T>
    {
        public INotifyExpression<T> InnerExpression { get; private set; }

        public ReadOnlyCollection<ObservableMemberBinding<T>> MemberBindings { get; private set; }

        public ObservableMemberInit(MemberInitExpression expression, ObservableExpressionBinder binder)
            : this(expression, binder, binder.VisitObservable<T>(expression.NewExpression)) { }

        private ObservableMemberInit(MemberInitExpression expression, ObservableExpressionBinder binder, INotifyExpression<T> inner)
            : this(inner, expression.Bindings.Select(m => binder.VisitMemberBinding<T>(m, inner))) { }

        public ObservableMemberInit(INotifyExpression<T> innerExpression, IEnumerable<ObservableMemberBinding<T>> memberBindings)
        {
            if (innerExpression == null) throw new ArgumentNullException("innerExpression");
            if (memberBindings == null) throw new ArgumentNullException("memberBindings");

            InnerExpression = innerExpression;
            innerExpression.ValueChanged += InnerValueChanged;
            var list = memberBindings as List<ObservableMemberBinding<T>>;
            if (list == null)
            {
                list = memberBindings.ToList();
            }
            MemberBindings = new ReadOnlyCollection<ObservableMemberBinding<T>>(list);
        }

        private void InnerValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
            Refresh();
        }

        protected override T GetValue()
        {
            return InnerExpression.Value;
        }

        protected override void DetachCore()
        {
            InnerExpression.Detach();
            foreach (var binding in MemberBindings)
            {
                binding.Detach();
            }
        }

        protected override void AttachCore()
        {
            InnerExpression.Attach();
            foreach (var binding in MemberBindings)
            {
                binding.Attach();
            }
        }

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.MemberInit;
            }
        }

        public override bool IsParameterFree
        {
            get { return InnerExpression.IsParameterFree && MemberBindings.All(m => m.IsParameterFree); }
        }

        public override INotifyExpression<T> ApplyParameters(IDictionary<string, object> parameters)
        {
            var inner = InnerExpression.ApplyParameters(parameters);
            return new ObservableMemberInit<T>(inner, MemberBindings.Select(b => b.ApplyParameters(inner, parameters)));
        }
    }
}
