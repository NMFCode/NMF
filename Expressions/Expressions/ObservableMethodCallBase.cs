using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal abstract class ObservableMethodBase<T, TDelegate, TResult> : NotifyExpression<TResult>
        where TDelegate : class
    {
        private readonly CollectionChangeListener<object> listener;

        public ObservableMethodBase(INotifyExpression<T> target, MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException("method");
            if (target == null) throw new ArgumentNullException("target");

            Target = target;
            Method = method;
            listener = new CollectionChangeListener<object>(this);
        }

        public MethodInfo Method { get; private set; }

        public TDelegate Function { get; private set; }

        public override ExpressionType NodeType { get { return ExpressionType.Invoke; } }

        public override string ToString()
        {
            return $"[Invoke {Method.Name}]";
        }

        public override bool CanBeConstant
        {
            get
            {
                return Target.CanBeConstant && !(Target.Value is INotifyCollectionChanged);
            }
        }

        public INotifyExpression<T> Target { get; private set; }

        private void RenewFunction()
        {
            Function = ReflectionHelper.CreateDelegate(typeof(TDelegate), Target.Value, Method) as TDelegate;
        }
        
        protected override void OnAttach()
        {
            AttachCollectionChangeListener(Target.Value);
            RenewFunction();
        }

        protected override void OnDetach()
        {
            listener.Unsubscribe();
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            IValueChangedNotificationResult<T> targetChange = null;
            foreach (var change in sources)
            {
                if (change.Source == Target)
                {
                    targetChange = change as IValueChangedNotificationResult<T>;
                    break;
                }
            }

            if (targetChange != null)
            {
                listener.Unsubscribe();
                AttachCollectionChangeListener(targetChange.NewValue);
                RenewFunction();
            }

            var oldValue = Value;
            var result = base.Notify(sources);

            if (result.Changed)
            {
                if(oldValue is IDisposable disposable)
                    disposable.Dispose();
            }

            return result;
        }

        private void AttachCollectionChangeListener(object target)
        {
            if(target is INotifyCollectionChanged newTarget)
                listener.Subscribe( newTarget );
        }
    }
}
