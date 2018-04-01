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
        
        public override ObservableMemberBinding<T> ApplyParameters(INotifyExpression<T> newTarget, IDictionary<string, object> parameters, IDictionary<INotifiable, INotifiable> trace)
        {
            return new ObservableListInitializer<T, TElement>(newTarget, Value.ApplyParameters(parameters, trace), AddAction, RemoveAction);
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            IValueChangedNotificationResult<T> targetChange = null;
            IValueChangedNotificationResult<TElement> valueChange = null;

            if (sources.Count == 1)
            {
                if (sources[0].Source == Target)
                    targetChange = sources[0] as IValueChangedNotificationResult<T>;
                else
                    valueChange = sources[0] as IValueChangedNotificationResult<TElement>;
            }
            else if(sources.Count == 2)
            {
                if (sources[0].Source == Target)
                {
                    targetChange = sources[0] as IValueChangedNotificationResult<T>;
                    valueChange = sources[1] as IValueChangedNotificationResult<TElement>;
                }
                else
                {
                    targetChange = sources[1] as IValueChangedNotificationResult<T>;
                    valueChange = sources[0] as IValueChangedNotificationResult<TElement>;
                }
            }
            
            if (targetChange != null && valueChange != null)
            {
                RemoveAction(targetChange.OldValue, valueChange.OldValue);
                AddAction(targetChange.NewValue, valueChange.NewValue);
                return new ValueChangedNotificationResult<T>(this, targetChange.OldValue, targetChange.NewValue);
            }
            else if (targetChange != null)
            {
                RemoveAction(targetChange.OldValue, Value.Value);
                AddAction(targetChange.NewValue, Value.Value);
                return new ValueChangedNotificationResult<T>(this, targetChange.OldValue, targetChange.NewValue);
            }
            else if (valueChange != null)
            {
                RemoveAction(Target.Value, valueChange.OldValue);
                AddAction(Target.Value, valueChange.NewValue);
                return new ValueChangedNotificationResult<T>(this, Target.Value, Target.Value);
            }
            else
            {
                return UnchangedNotificationResult.Instance;
            }
        }

        protected override void OnAttach()
        {
            AddAction(Target.Value, Value.Value);
        }

        protected override void OnDetach()
        {
            RemoveAction(Target.Value, Value.Value);
        }
    }

}
