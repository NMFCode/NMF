using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Glsp.Processing
{
    internal abstract class ActionElement
    {
        private List<Func<string, bool>> _filters = new List<Func<string, bool>>();

        public void AddFilter(Func<string, bool> filter)
        {
            if (filter != null)
            {
                _filters.Add(filter);
            }
        }

        public bool ShowInContext(string context)
        {
            return _filters.All(filter => filter(context));
        }
    }
}
