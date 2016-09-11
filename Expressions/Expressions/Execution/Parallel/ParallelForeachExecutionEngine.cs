using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    public class ParallelForeachExecutionEngine : ParallelExecutionEngine
    {
        protected override void Schedule(HashSet<INotifiable> nodes)
        {
            Parallel.ForEach(nodes, node => NotifyNode(node, 1, true));
        }
    }
}
