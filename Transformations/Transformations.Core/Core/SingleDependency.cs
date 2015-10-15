using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This class represents a single require- or call-after- dependency
    /// </summary>
    [DebuggerDisplay("{Representation}")]
    public class SingleDependency : Dependency
    {
        internal Func<Computation, object[]> Selector { get; set; }
        internal Action<object, object> Persistor { get; set; }

        internal string Representation
        {
            get
            {
                if (ExecuteBefore)
                {
                    return "Require " + DependencyTransformation.ToString();
                }
                else
                {
                    return "Call " + DependencyTransformation.ToString();
                }
            }
        }

        /// <summary>
        /// Calls the transformation dependency for the given computation
        /// </summary>
        /// <param name="computation">The computation that this dependency is to be called</param>
        public override void HandleDependency(Computation computation)
        {
            if (computation == null) throw new ArgumentNullException("computation");

            if (computation.IsDelayed && NeedOutput)
            {
                computation.OutputInitialized += CallComputation;
                return;
            }

            if (Filter == null || Filter(computation))
            {
                var context = computation.TransformationContext;
                if (Selector != null)
                {
                    if (computation.IsDelayed)
                    {
                        //case delayed
                        var delay = computation.OutputDelay;
                        if (!NeedOutput)
                        {
                            object[] dependencyInput = Selector(computation);
                            if (dependencyInput != null)
                            {
                                GeneralTransformationRule dependent = DependencyTransformation;
                                var comp2 = context.CallTransformation(dependent, dependencyInput);
                                if (Persistor != null)
                                {
                                    if (!comp2.IsDelayed)
                                    {
                                        delay.Persistors.Add(new SingleItemPersistor()
                                        {
                                            Persistor = Persistor,
                                            Output = comp2.Output
                                        });
                                    }
                                    else
                                    {
                                        computation.DelayOutputAtLeast(comp2.Context.MinOutputDelayLevel);
                                        var delayPersistor = new SingleResultAwaitingPersistor(Persistor);
                                        delayPersistor.WaitFor(comp2);
                                        delay.Persistors.Add(delayPersistor);
                                    }
                                }
                                else // persistor is null
                                {
                                    if (ExecuteBefore)
                                    {
                                        if (comp2.IsDelayed)
                                        {
                                            computation.DelayOutputAtLeast(comp2.Context.MinOutputDelayLevel);
                                        }
                                    }
                                }

                                computation.MarkRequireInternal(comp2, ExecuteBefore, this);
                            }
                        }
                    }
                    else // not delayed
                    {
                        // case output is already created
                        var output = computation.Output;
                        object[] dependencyInput = Selector(computation);
                        if (dependencyInput != null)
                        {
                            GeneralTransformationRule dependent = DependencyTransformation;
                            var comp2 = context.CallTransformation(dependent, dependencyInput);
                            if (Persistor != null)
                            {
                                if (!comp2.IsDelayed)
                                {
                                    Persistor(output, comp2.Output);
                                }
                                else
                                {
                                    var delay = new SingleResultAwaitingPersistor(Persistor, computation.Output);
                                    delay.WaitFor(comp2);
                                }
                            }

                            computation.MarkRequireInternal(comp2, ExecuteBefore, this);
                        }
                    }
                }
            }
        }

        void CallComputation(object sender, EventArgs e)
        {
            Computation c = sender as Computation;
            HandleDependency(c);
            c.OutputInitialized -= CallComputation;
        }
    }

    

}
