using NMF.Expressions.Execution;
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
        public ObservableMethodBase(INotifyExpression<T> target, MethodInfo method)
        {
            if (method == null) throw new ArgumentNullException("method");
            if (target == null) throw new ArgumentNullException("target");

            Target = target;
            Method = method;
        }

        public MethodInfo Method { get; private set; }

        public TDelegate Function { get; private set; }

        public override ExpressionType NodeType { get { return ExpressionType.Invoke; } }

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
            DetachCollectionChangeListener(Target.Value);
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            var targetChange = sources.FirstOrDefault(c => c.Source == Target) as ValueChangedNotificationResult<T>;
            if (targetChange != null)
            {
                DetachCollectionChangeListener(targetChange.OldValue);
                AttachCollectionChangeListener(targetChange.NewValue);
                RenewFunction();
            }

            var oldValue = Value;
            var result = base.Notify(sources);

            if (result.Changed)
            {
                var disposable = oldValue as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            return result;
        }

        private void AttachCollectionChangeListener(object target)
        {
            var newTarget = target as INotifyCollectionChanged;
            if (newTarget != null)
                ExecutionEngine.Current.AddChangeListener(this, newTarget);
        }

        private void DetachCollectionChangeListener(object target)
        {
            var oldTarget = target as INotifyCollectionChanged;
            if (oldTarget != null)
                ExecutionEngine.Current.RemoveChangeListener(this, oldTarget);
        }
    }
}
