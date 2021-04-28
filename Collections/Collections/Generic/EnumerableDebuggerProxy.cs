using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NMF.Collections.Generic
{
    public class EnumerableDebuggerProxy<T>
    {
        private readonly IEnumerable<T> items;

        public EnumerableDebuggerProxy(IEnumerable<T> items)
        {
            this.items = items;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items { get { return items.ToArray(); } }
    }
}
