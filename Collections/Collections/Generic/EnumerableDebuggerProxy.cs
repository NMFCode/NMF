using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NMF.Collections.Generic
{
    /// <summary>
    /// A class that emulates any collection as an array for debugging support
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumerableDebuggerProxy<T>
    {
        private readonly IEnumerable<T> items;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="items">The base collection</param>
        public EnumerableDebuggerProxy(IEnumerable<T> items)
        {
            this.items = items;
        }

        /// <summary>
        /// Gets the items as an array
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
#pragma warning disable S2365 // Properties should not make collection or array copies
        public T[] Items { get { return items.ToArray(); } }
#pragma warning restore S2365 // Properties should not make collection or array copies
    }
}
