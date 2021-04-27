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
            : this(inner, expression.Bindings.Select(m => binder.VisitMemberBinding(m, inner))) { }

        public ObservableMemberInit(INotifyExpression<T> innerExpression, IEnumerable<ObservableMemberBinding<T>> memberBindings)
        {
            if (innerExpression == null) throw new ArgumentNullException("innerExpression");
            if (memberBindings == null) throw new ArgumentNullException("memberBindings");

            InnerExpression = innerExpression;

            if(memberBindings is not List<ObservableMemberBinding<T>> list)
            {
                list = memberBindings.ToList();
            }
            MemberBindings = new ReadOnlyCollection<ObservableMemberBinding<T>>(list);
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

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return InnerExpression;
                foreach (var memberBinding in MemberBindings)
                    yield return memberBinding;
            }
        }

        protected override T GetValue()
        {
            return InnerExpression.Value;
        }

        protected override INotifyExpression<T> ApplyParametersCore(IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            var inner = InnerExpression.ApplyParameters(parameters, trace);
            return new ObservableMemberInit<T>(inner, MemberBindings.Select(b => b.ApplyParameters(inner, parameters, trace)));
        }
    }
}
