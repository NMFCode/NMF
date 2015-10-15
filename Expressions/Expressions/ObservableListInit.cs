using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal class ObservableListInit<T> : ObservableMemberInit<T>
    {
        public ObservableListInit(ListInitExpression expression, ObservableExpressionBinder binder)
            : this(expression, binder, binder.VisitObservable<T>(expression.NewExpression)) { }

        private ObservableListInit(ListInitExpression expression, ObservableExpressionBinder binder, INotifyExpression<T> inner)
            : base(inner, expression.Initializers.Select(e => binder.VisitElementInit<T>(e, inner))) { }
    }

	internal class ObservableListInitializer<T, TElement> : ObservableMemberBinding<T>
	{
		public INotifyExpression<T> Target { get; private set; }

		public INotifyExpression<TElement> Value { get; private set; }

        public Action<T, TElement> AddAction { get; private set; }
        public Action<T, TElement> RemoveAction { get; private set; }

        public ObservableListInitializer(ElementInit expression, ObservableExpressionBinder binder, INotifyExpression<T> target)
            : this(target, binder.VisitObservable<TElement>(expression.Arguments[0]), expression.AddMethod) { }

		public ObservableListInitializer(INotifyExpression<T> target, INotifyExpression<TElement> value, MethodInfo addMethod)
		{
            if (target == null) throw new ArgumentNullException("target");
            if (value == null) throw new ArgumentNullException("value");
            if (addMethod == null) throw new ArgumentNullException("addMethod");

			Target = target;
			Value = value;

            AddAction = ReflectionHelper.CreateDelegate<Action<T, TElement>>(addMethod);
            var removeMethod = ReflectionHelper.GetRemoveMethod(typeof(T), typeof(TElement));
            if (removeMethod == null)
            {
                throw new InvalidOperationException("Could not find appropriate Remove method for " + addMethod.Name);
            }
            if (removeMethod.ReturnType == typeof(void))
            {
                RemoveAction = ReflectionHelper.CreateDelegate<Action<T, TElement>>(removeMethod);
            }
            else if (removeMethod.ReturnType == typeof(bool))
            {
                var tempAction = ReflectionHelper.CreateDelegate<Func<T, TElement, bool>>(removeMethod);
                RemoveAction = (o, i) => tempAction(o, i);
            }
            else
            {
                throw new NotSupportedException();
            }
		}

        private ObservableListInitializer(INotifyExpression<T> target, INotifyExpression<TElement> value, Action<T, TElement> addMethod, Action<T, TElement> removeMethod)
        {
            Target = target;
            Value = value;

            AddAction = addMethod;
            RemoveAction = removeMethod;
        }

		private void AddToList(bool removeOld, object old)
		{
            if (removeOld)
            {
                RemoveAction.Invoke(Target.Value, (TElement)old);
            }
            AddAction.Invoke(Target.Value, Value.Value);
		}

		private void TargetChanged(object sender, ValueChangedEventArgs e)
        {
            if (!Target.IsAttached) return;
            RemoveAction.Invoke((T)e.OldValue, Value.Value);
			AddToList(false, null);
		}

		private void ValueChanged(object sender, ValueChangedEventArgs e)
		{
            if (!Target.IsAttached) return;
			AddToList(true, e.OldValue);
		}

		public override void Attach()
		{
			Target.Attach();
            Value.Attach();

            Target.ValueChanged += TargetChanged;
            Value.ValueChanged += ValueChanged;

            AddToList(false, null);
		}

		public override void Detach()
		{
			Target.Detach();
            Value.Detach();

            Target.ValueChanged -= TargetChanged;
            Value.ValueChanged -= ValueChanged;

            RemoveAction.Invoke(Target.Value, Value.Value);
		}

        public override bool IsParameterFree
        {
            get { return Value.IsParameterFree; }
        }

        public override ObservableMemberBinding<T> ApplyParameters(INotifyExpression<T> newTarget, IDictionary<string, object> parameters)
        {
            return new ObservableListInitializer<T, TElement>(newTarget, Value.ApplyParameters(parameters), AddAction, RemoveAction);
        }
    }

}
