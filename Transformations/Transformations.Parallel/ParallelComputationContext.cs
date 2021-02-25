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
    /// <summary>
    /// Denotes a context of a computation in a parallel transformation
    /// </summary>
    public class ParallelComputationContext : ComputationContext
    {
        private int transformationRequirements;
        ConcurrentQueue<Action> computations;

        /// <summary>
        /// Creates a new parallel execution context
        /// </summary>
        /// <param name="context"></param>
        public ParallelComputationContext(ITransformationContext context) : base(context) { }

        /// <inheritdoc />
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
