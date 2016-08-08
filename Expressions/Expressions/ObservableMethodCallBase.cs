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
        private T targetValue;
        
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
            targetValue = Target.Value;
            AttachCollectionChangeListener();
            RenewFunction();
        }

        protected override void OnDetach()
        {
            DetachCollectionChangeListener();
            targetValue = default(T);
        }

        public override bool Notify(IEnumerable<INotifiable> sources)
        {
            if (sources.Contains(Target))
            {
                DetachCollectionChangeListener();
                targetValue = Target.Value;
                AttachCollectionChangeListener();
                RenewFunction();
            }

            var oldValue = Value;
            bool result = base.Notify(sources);

            if (result)
            {
                var disposable = oldValue as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            return result;
        }

        private void AttachCollectionChangeListener()
        {
            var newTarget = targetValue as INotifyCollectionChanged;
            if (newTarget != null)
                ExecutionEngine.Current.AddChangeListener(this, newTarget);
        }

        private void DetachCollectionChangeListener()
        {
            var oldTarget = targetValue as INotifyCollectionChanged;
            if (oldTarget != null)
                ExecutionEngine.Current.RemoveChangeListener(this, oldTarget);
        }
    }
}
