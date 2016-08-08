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
                var reversable = Proxy as INotifyReversableValue<TResult>;
                return reversable != null && reversable.IsReversable;
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

        public INotifyValue<TResult> Proxy { get; private set; }

        protected override TResult GetValue()
        {
            return Proxy.Value;
        }

        public override void SetValue(TResult value)
        {
            var reversable = Proxy as INotifyReversableValue<TResult>;
            reversable.Value = value;
        }

        public override bool IsReversable
        {
            get
            {
                if (Proxy == null) return true;
                var reversable = Proxy as INotifyReversableValue<TResult>;
                return reversable != null && reversable.IsReversable;
            }
        }

        public void RenewProxy()
        {
            if (Proxy != null)
            {
                Proxy.Successors.Remove(this);
                IDisposable disposable = Proxy as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
                Proxy = null;
            }
            Proxy = CreateProxy();
            Proxy.Successors.Add(this);
        }

        public override string ToString()
        {
            return "proxy for " + Proxy.ToString();
        }

        protected abstract INotifyValue<TResult> CreateProxy();
        
        public override bool Notify(IEnumerable<INotifiable> sources)
        {
            RenewProxy();
            return base.Notify(sources);
        }

        protected override void OnAttach()
        {
            Proxy = CreateProxy();
        }
    }
}
