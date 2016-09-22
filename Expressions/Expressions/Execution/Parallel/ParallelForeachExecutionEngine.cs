using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Expressions
{
    public class ParallelForeachExecutionEngine : ParallelExecutionEngine
    {
        protected override void Schedule(List<INotifiable> nodes, Action<INotifiable> action)
        {
            var part = Partitioner.Create(nodes);
            Parallel.ForEach(part, action);
        }
    }
}
