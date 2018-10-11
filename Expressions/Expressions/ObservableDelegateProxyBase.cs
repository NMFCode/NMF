using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal abstract class ObservableDelegateProxyBase<TReturn> : NotifyExpression<TReturn>
    {
        public INotifyValue<TReturn> Inner { get; private set; }
        private bool argChangesTriggerReeval = false;
        public bool InnerListensToArgChanges { get => !argChangesTriggerReeval; }

        protected abstract INotifyExpression<Delegate> GetTarget();
        
        private void Update()
        {
            var method = GetTarget().Value?.Method;
            if (method != null)
            {
                if (Inner != null)
                {
                    Inner.Successors.Unset(this);
                    if (Inner is IDisposable disposable)
                    {
                        disposable.Dispose();
                    }
                }
                Inner = CreateCall(method, out argChangesTriggerReeval);
            }
        }

        private bool ShouldUpdate(IList<INotificationResult> changes)
        {
            for (int i = 0; i < changes.Count; i++)
            {
                var change = changes[i];
                if (change.Source == GetTarget()) return true;
                if (change.Source != Inner && argChangesTriggerReeval) return true;
            }
            return false;
        }

        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (ShouldUpdate(sources)) Update();
            return base.Notify(sources);
        }

        protected override void OnAttach()
        {
            Update();
        }

        protected abstract INotifyValue<TReturn> CreateCall(MethodInfo method, out bool argChangesTriggerReeval);

        protected override TReturn GetValue()
        {
            return Inner.Value;
        }

        public override ExpressionType NodeType => ExpressionType.Invoke;

        public override string ToString()
        {
            return "[DelegateProxy]";
        }
    }
}
