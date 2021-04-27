using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NMF.Expressions
{
    internal abstract class ObservableProxyCallBase<TResult> : ObservableReversableExpression<TResult>
    {
        public INotifyValue<TResult> Proxy { get; private set; }
        
        public override ExpressionType NodeType { get { return ExpressionType.Extension; } }

        public override bool IsReversable
        {
            get
            {
                if (Proxy == null) return true;
                return Proxy is INotifyReversableValue<TResult> reversable && reversable.IsReversable;
            }
        }

        public override bool IsParameterFree
        {
            get
            {
                return false;
            }
        }

        public override IEnumerable<INotifiable> Dependencies
        {
            get
            {
                if (Proxy != null)
                    yield return Proxy;
            }
        }

        protected override TResult GetValue()
        {
            return Proxy.Value;
        }

        public override void SetValue(TResult value)
        {
            var reversable = Proxy as INotifyReversableValue<TResult>;
            reversable.Value = value;
        }

        public void RenewProxy()
        {
            if (Proxy != null)
            {
                Proxy.Dispose();
            }
            Proxy = CreateProxy();
            Proxy.Successors.Set(this);
        }

        public override string ToString()
        {
            return $"[ProxyInvoke {ProxyMethodName}]";
        }

        protected abstract string ProxyMethodName { get; }

        protected abstract INotifyValue<TResult> CreateProxy();

        protected virtual bool RequireRenewProxy(IList<INotificationResult> changes)
        {
            return changes.Count > 1 || changes[0].Source != Proxy;
        }
        
        public override INotificationResult Notify(IList<INotificationResult> sources)
        {
            if (RequireRenewProxy(sources))
                RenewProxy();
            return base.Notify(sources);
        }
    }
}
