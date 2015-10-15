using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Transformations.Core
{
    /// <summary>
    /// This class is used to represent dependencies on more than one child object
    /// </summary>
    [DebuggerDisplay("{Representation}")]
    public class MultipleDependency : Dependency
    {
        internal Func<Computation, IEnumerable<object[]>> Selector { get; set; }
        internal Action<object, IEnumerable> Persistor { get; set; }

        internal string Representation
        {
            get
            {
                if (ExecuteBefore)
                {
                    return "Require many " + DependencyTransformation.ToString();
                }
                else
                {
                    return "Call many " + DependencyTransformation.ToString();
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

            if (Filter != null && !Filter(computation)) return;
            var context = computation.TransformationContext;
            if (Selector != null)
            {
                if (computation.IsDelayed)
                {
                    var delay = computation.OutputDelay;
                    if (!NeedOutput)
                    {
                        HandleDelayedComputation(computation, context, delay);
                    }
                }
                else
                {
                    HandleNonDelayedComputation(computation, context);
                }
            }
        }

        private void HandleNonDelayedComputation(Computation computation, ITransformationContext context)
        {
            var output = computation.Output;
            var inCollection = Selector(computation);
            if (inCollection != null)
            {
                if (Persistor != null)
                {
                    Type listType = (typeof(List<>)).MakeGenericType(DependencyTransformation.OutputType);
                    IList list = System.Activator.CreateInstance(listType) as IList;

                    MultipleResultAwaitingPersistor delayPersistor = new MultipleResultAwaitingPersistor()
                    {
                        List = list,
                        Persistor = Persistor,
                        Target = output
                    };
                    bool needDependencyPersistor = false;
                    GeneralTransformationRule dependent = DependencyTransformation;
                    if (context.IsThreadSafe)
                    {
                        Parallel.ForEach(inCollection, dependencyInput =>
                        {
                            if (HandleNonDelayedPersistedDependencyInput(computation, context, list, delayPersistor, dependent, dependencyInput))
                            {
                                needDependencyPersistor = true;
                            }
                        });
                    }
                    else
                    {
                        foreach (var dependencyInput in inCollection)
                        {
                            if (HandleNonDelayedPersistedDependencyInput(computation, context, list, delayPersistor, dependent, dependencyInput))
                            {
                                needDependencyPersistor = true;
                            }
                        }
                    }
                    if (!needDependencyPersistor) Persistor(output, list);
                }
                else
                {
                    GeneralTransformationRule dependent = DependencyTransformation;
                    if (context.IsThreadSafe)
                    {
                        Parallel.ForEach(inCollection, dependencyInput =>
                        {
                            var comp2 = context.CallTransformation(dependent, dependencyInput);
                            computation.MarkRequireInternal(comp2, ExecuteBefore, this);
                        });
                    }
                    else
                    {
                        foreach (var dependencyInput in inCollection)
                        {
                            var comp2 = context.CallTransformation(dependent, dependencyInput);
                            computation.MarkRequireInternal(comp2, ExecuteBefore, this);
                        }
                    }
                }
            }
        }

        private bool HandleNonDelayedPersistedDependencyInput(Computation computation, ITransformationContext context, IList list, MultipleResultAwaitingPersistor delayPersistor, GeneralTransformationRule dependent, object[] dependencyInput)
        {
            var needDelayedPersistor = false;
            var comp2 = context.CallTransformation(dependent, dependencyInput);
            if (!comp2.IsDelayed)
            {
                if (comp2.Output != null)
                {
                    var lockList = context.IsThreadSafe;
                    if (lockList)
                    {
                        lock (list)
                        {
                            list.Add(comp2.Output);
                        }
                    }
                    else
                    {
                        list.Add(comp2.Output);
                    }
                }
            }
            else
            {
                needDelayedPersistor = true;
                delayPersistor.WaitFor(comp2);
            }

            computation.MarkRequireInternal(comp2, ExecuteBefore, this);
            return needDelayedPersistor;
        }

        private void HandleDelayedComputation(Computation computation, ITransformationContext context, OutputDelay delay)
        {
            var inCollection = Selector(computation);
            if (inCollection != null)
            {
                if (Persistor != null)
                {
                    Type listType = (typeof(List<>)).MakeGenericType(DependencyTransformation.OutputType);
                    IList list = System.Activator.CreateInstance(listType) as IList;

                    MultipleResultAwaitingPersistor delayPersistor = new MultipleResultAwaitingPersistor()
                    {
                        List = list,
                        Persistor = Persistor
                    };
                    if (context.IsThreadSafe)
                    {
                        Parallel.ForEach(inCollection, dependencyInput =>
                        {
                            HandleDelayedDependencyInput(computation, context, list, delayPersistor, dependencyInput);
                        });
                    }
                    else
                    {
                        foreach (var dependencyInput in inCollection)
                        {
                            HandleDelayedDependencyInput(computation, context, list, delayPersistor, dependencyInput);
                        }
                    }
                    delay.Persistors.Add(delayPersistor);
                }
                else
                {
                    if (context.IsThreadSafe)
                    {
                        Parallel.ForEach(inCollection, dependencyInput =>
                        {
                            GeneralTransformationRule dependent = DependencyTransformation;
                            var comp2 = context.CallTransformation(dependent, dependencyInput);

                            computation.MarkRequireInternal(comp2, ExecuteBefore, this);
                        });
                    }
                    else
                    {
                        foreach (var dependencyInput in inCollection)
                        {
                            GeneralTransformationRule dependent = DependencyTransformation;
                            var comp2 = context.CallTransformation(dependent, dependencyInput);

                            computation.MarkRequireInternal(comp2, ExecuteBefore, this);
                        }
                    }
                }
            }
        }

        private void HandleDelayedDependencyInput(Computation computation, ITransformationContext context, IList list, MultipleResultAwaitingPersistor delayPersistor, object[] dependencyInput)
        {
            GeneralTransformationRule dependent = DependencyTransformation;
            var comp2 = context.CallTransformation(dependent, dependencyInput);
            if (!comp2.IsDelayed)
            {
                if (comp2.Output != null)
                {
                    if (context.IsThreadSafe)
                    {
                        lock (list)
                        {
                            list.Add(comp2.Output);
                        }
                    }
                    else
                    {
                        list.Add(comp2.Output);
                    }
                }
            }
            else
            {
                computation.DelayOutputAtLeast(comp2.Context.MinOutputDelayLevel);
                delayPersistor.WaitFor(comp2);
            }

            if (ExecuteBefore)
            {
                computation.DelayTransformationAtLeast(comp2.Context.MinTransformDelayLevel);
            }
            else
            {
                comp2.DelayTransformationAtLeast(computation.Context.MinTransformDelayLevel);
            }
        }

        private void CallComputation(object sender, EventArgs e)
        {
            Computation c = sender as Computation;
            HandleDependency(c);
            c.OutputInitialized -= CallComputation;
        }
    }

    
}
