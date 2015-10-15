using NMF.Transformations.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Transformations.Parallel
{
    public class ParallelComputationContext : ComputationContext
    {
        public int transformationRequirements;
        ConcurrentQueue<Action> computations;

        public ParallelComputationContext(ITransformationContext context) : base(context) { }

        public override void MarkRequire(Computation other, bool isRequired)
        {
            base.MarkRequire(other, isRequired);
            if (isRequired)
            {
                Interlocked.Increment(ref transformationRequirements);
                other.Computed += DecreaseTransformationRequirements;
            }
        }

        private void DecreaseTransformationRequirements(object sender, EventArgs e)
        {
            if (Interlocked.Decrement(ref transformationRequirements) == 0)
            {
                lock (this)
                {
                    var compsLocal = Interlocked.Exchange(ref computations, null);
                    if (compsLocal != null)
                    {
                        Action item;
                        while (compsLocal.TryDequeue(out item))
                        {
                            Task.Factory.StartNew(item);
                        }
                    }
                }
            }
        }

        internal void RunTransform(Action transformationAction)
        {
            if (transformationAction == null) throw new ArgumentNullException("transformationAction");
            if (transformationRequirements == 0)
            {
                Task.Factory.StartNew(transformationAction);
            }
            else
            {
                lock (this)
                {
                    if (transformationRequirements == 0)
                    {
                        Task.Factory.StartNew(transformationAction);
                    }
                    else
                    {
                        ConcurrentQueue<Action> current = computations;
                        while (current == null)
                        {
                            var newList = new ConcurrentQueue<Action>();
                            current = Interlocked.CompareExchange(ref computations, newList, null);
                        }
                        current.Enqueue(transformationAction);
                    }
                }
            }
        }
    }
}
