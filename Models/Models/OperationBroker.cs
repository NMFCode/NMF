using NMF.Models.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    /// <summary>
    /// Denotes a broker for model operations
    /// </summary>
    public class OperationBroker
    {
        private static readonly OperationBroker instance = new OperationBroker();

        private OperationBroker() { }

        /// <summary>
        /// Gets the singleton operation broker instance
        /// </summary>
        public static OperationBroker Instance
        {
            get
            {
                return instance;
            }
        }

        private readonly Dictionary<object, object> broker = new Dictionary<object, object>();

        /// <summary>
        /// Gets the registered delegate for the given operation
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate</typeparam>
        /// <param name="op">the operation for which a delegate is requested</param>
        /// <returns>The registered delegate or null, if no delegate can be found</returns>
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

        /// <summary>
        /// Gets the registered delegate for the given operation
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate</typeparam>
        /// <param name="op">the operation for which a delegate is requested</param>
        /// <returns>The registered delegate or null, if no delegate can be found</returns>
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

        /// <summary>
        /// Registers the provided delegate for the given operation
        /// </summary>
        /// <param name="op">the operation</param>
        /// <param name="delegate">the delegate</param>
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

        /// <summary>
        /// Registers the provided delegate for the given operation
        /// </summary>
        /// <param name="op">the operation</param>
        /// <param name="delegate">the delegate</param>
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
