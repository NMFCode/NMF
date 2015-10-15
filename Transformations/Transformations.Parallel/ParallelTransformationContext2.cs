using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.Transformations.Parallel
{
    public class ParallelTransformationContext2 : TransformationContext
    {
        public ParallelTransformationContext2(Transformation transformation) : base(transformation) { }

        protected override ComputationContext CreateComputationContext(object[] input, GeneralTransformationRule rule)
        {
            return new ParallelComputationContext(this);
        }

        protected override void ExecuteLevel(IList<Computation> computationsOfLevel)
        {
            using (var countEvent = new CountdownEvent(1))
            {
                foreach (var item in computationsOfLevel)
                {
                    var cc = item.Context as ParallelComputationContext;
                    if (cc != null)
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
