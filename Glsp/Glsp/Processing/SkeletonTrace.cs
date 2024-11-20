using NMF.Glsp.Graph;
using System.Collections.Concurrent;

namespace NMF.Glsp.Processing
{
    internal class SkeletonTrace : ISkeletonTrace
    {
        private readonly ConcurrentDictionary<(object, object), GElement> _trace = new ConcurrentDictionary<(object, object), GElement>();

        public GElement RemoveElement(object element, object skeleton)
        {
            if (_trace.TryRemove((element, skeleton), out var gElement))
            {
                return gElement;
            }
            return null;
        }

        public GElement ResolveElement(object element, object skeleton)
        {
            if (_trace.TryGetValue((element, skeleton), out var gelement))
            {
                return gelement;
            }
            return null;
        }

        public void Trace(object element, GElement gElement)
        {
            _trace.TryAdd((element, gElement.Skeleton), gElement);
        }
    }
}
