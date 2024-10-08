﻿using NMF.Transformations.Core;
using System;
using System.Collections.Concurrent;
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
#pragma warning disable S2551 // Shared resources should not be used for locking
                lock (this)
#pragma warning restore S2551 // Shared resources should not be used for locking
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
            if (transformationAction == null) throw new ArgumentNullException(nameof(transformationAction));
            if (transformationRequirements == 0)
            {
                Task.Factory.StartNew(transformationAction);
            }
            else
            {
#pragma warning disable S2551 // Shared resources should not be used for locking
                lock (this)
#pragma warning restore S2551 // Shared resources should not be used for locking
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
