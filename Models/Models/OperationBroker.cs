using NMF.Models.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    public class OperationBroker
    {
        private static readonly OperationBroker instance = new OperationBroker();

        private OperationBroker() { }

        public static OperationBroker Instance
        {
            get
            {
                return instance;
            }
        }

        private readonly Dictionary<object, object> broker = new Dictionary<object, object>();

        public TDelegate GetRegisteredDelegate<TDelegate>(IOperation op)
            where TDelegate : class
        {
            if (broker.TryGetValue(op, out  object del))
            {
                return (TDelegate)del;
            }
            else
            {
                foreach (var item in broker.Keys.OfType<Lazy<IOperation>>())
                {
                    if (item.Value == op)
                    {
                        var d = broker[item];
                        broker.Add(item.Value, d);
                        return (TDelegate)d;
                    }
                }
                return null;
            }
        }

        public TDelegate GetRegisteredDelegate<TDelegate>(Lazy<IOperation> op)
            where TDelegate : class
        {
            if (broker.TryGetValue(op, out object del))
            {
                return (TDelegate)del;
            }
            else
            {
                return GetRegisteredDelegate<TDelegate>(op.Value);
            }
        }

        public void RegisterDelegate(IOperation op, object @delegate)
        {
            if (broker.ContainsKey(op))
            {
                broker[op] = @delegate;
            }
            else
            {
                broker.Add(op, @delegate);
            }
        }

        public void RegisterDelegate(Lazy<IOperation> op, object @delegate)
        {
            if (broker.ContainsKey(op))
            {
                broker[op] = @delegate;
            }
            else
            {
                broker.Add(op, @delegate);
            }
            if (op.IsValueCreated)
            {
                RegisterDelegate(op.Value, @delegate);
            }
        }
    }
}
