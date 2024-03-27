using System.Collections.Generic;
using NMF.Transformations.Core;

namespace NMF.Synchronizations
{
    internal static class InstantiationHelper
    {
        public static LinkedListNode<Computation> RearrangeComputations(IEnumerable<Computation> computations, Computation origin)
        {
            var linked = new LinkedList<Computation>();
            var originNode = new LinkedListNode<Computation>(origin);
            linked.AddFirst(originNode);
            var changed = true;
            while (changed)
            {
                changed = false;
                foreach (var c in computations)
                {
                    if (c.TransformationRule.BaseRule == linked.Last.Value.TransformationRule)
                    {
                        linked.AddLast(c);
                        changed = true;
                    }
                    else if (linked.First.Value.TransformationRule.BaseRule == c.TransformationRule)
                    {
                        linked.AddFirst(c);
                        changed = true;
                    }
                }
            }
            return originNode;
        }
    }
}
