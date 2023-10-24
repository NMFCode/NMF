using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Graph
{
    public class GGraph : GElement
    {
        private readonly Dictionary<string, GElement> _elementsById = new Dictionary<string, GElement>();

        public GElement Resolve(string id)
        {
            if (_elementsById.TryGetValue(id, out var element))
            {
                return element;
            }
            throw new InvalidOperationException("element could not be found");
        }
    }
}
