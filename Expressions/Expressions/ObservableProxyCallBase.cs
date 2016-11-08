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
        protected void ArgumentChanged(object sender, ValueChangedEventArgs e)
        {
            if (!IsAttached) return;
            RenewProxy();
            Refresh();
        }

        public override string ToString()
        {
            return "proxy for " + Proxy.ToString();
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
                Proxy.ValueChanged -= ProxyValueChanged;
                IDisposable disposable = Proxy as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            Proxy = CreateProxy();
            Proxy.Attach();
            Proxy.ValueChanged += ProxyValueChanged;
        }

        private void ProxyValueChanged(object sender, ValueChangedEventArgs e)
        {
            Refresh();
        }

        protected abstract INotifyValue<TResult> CreateProxy();

        protected override void DetachCore()
        {
            if (Proxy != null)
            {
                Proxy.Detach();
                IDisposable disposable = Proxy as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
                Proxy = null;
            }
        }

        protected override void AttachCore()
        {
            if (Proxy != null)
            {
                Proxy.Attach();
            }
            else
            {
                RenewProxy();
            }
        }

        public override ExpressionType NodeType
        {
            get
            {
                return ExpressionType.Extension;
            }
        }
    }
}
