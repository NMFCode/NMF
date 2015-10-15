using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace NMF.Collections.Generic
{
    public class EnumerableDebuggerProxy<T>
    {
        private IEnumerable<T> items;

        public EnumerableDebuggerProxy(IEnumerable<T> items)
        {
            this.items = items;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays"), DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items { get { return items.ToArray(); } }
    }
}
