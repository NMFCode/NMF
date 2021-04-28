using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Transformations.Parallel
{
    /// <summary>
    /// An alternative implementation of a parallel transformation context
    /// </summary>
    public class ParallelTransformationContext2 : TransformationContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="transformation"></param>
        public ParallelTransformationContext2(Transformation transformation) : base(transformation) { }

        /// <inheritdoc />
        protected override ComputationContext CreateComputationContext(object[] input, GeneralTransformationRule rule)
        {
            return new ParallelComputationContext(this);
        }

        /// <inheritdoc />
        protected override void ExecuteLevel(IList<Computation> computationsOfLevel)
        {
            using (var countEvent = new CountdownEvent(1))
            {
                foreach (var item in computationsOfLevel)
                {
                    if (item.Context is ParallelComputationContext cc)
                    {
                        countEvent.AddCount();
                        cc.RunTransform(() =>
                        {
                            item.Transform();
                            countEvent.Signal();
                        });
                    }
                    else
                    {
                        countEvent.Signal();
                        countEvent.Wait();
                        item.Transform();
                        countEvent.Reset();
                    }
                    OnComputationCompleted(new ComputationEventArgs(item));
                }
                countEvent.Signal();
                countEvent.Wait();
            }
        }
    }
}
