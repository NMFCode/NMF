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
        private T targetValue;
        private TElement valueValue;

        public INotifyExpression<T> Target { get; private set; }

        public INotifyExpression<TElement> Value { get; private set; }

        public Action<T, TElement> AddAction { get; private set; }
        public Action<T, TElement> RemoveAction { get; private set; }

        public override bool IsParameterFree
        {
            get { return Value.IsParameterFree; }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                yield return Target;
                yield return Value;
            }
        }

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
        
        public override ObservableMemberBinding<T> ApplyParameters(INotifyExpression<T> newTarget, IDictionary<string, object> parameters)
        {
            return new ObservableListInitializer<T, TElement>(newTarget, Value.ApplyParameters(parameters), AddAction, RemoveAction);
        }

        public override bool Notify(IEnumerable<INotifiable> sources)
        {
            var newTargetValue = Target.Value;
            var newValueValue = Value.Value;

            if (!newTargetValue.Equals(targetValue))
            {
                RemoveAction(targetValue, valueValue);
                AddAction(newTargetValue, newValueValue);
                targetValue = newTargetValue;
            }
            else if (!newValueValue.Equals(valueValue))
            {
                RemoveAction(targetValue, valueValue);
                AddAction(targetValue, newValueValue);
                valueValue = newValueValue;
            }

            return true;
        }

        protected override void OnAttach()
        {
            targetValue = Target.Value;
            valueValue = Value.Value;
            AddAction(targetValue, valueValue);
        }

        protected override void OnDetach()
        {
            RemoveAction(targetValue, valueValue);
        }
    }

}
