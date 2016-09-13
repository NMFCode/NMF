using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Expressions;
using NMF.Transformations;
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

    internal class LeftInstantiationMonitorJob<TLeft, TRight> : ISynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        public ObservingFunc<TLeft, bool> Predicate { get; private set; }

        public LeftInstantiationMonitorJob(ObservingFunc<TLeft, bool> instantiationPredicate)
        {
            Predicate = instantiationPredicate;
        }

        public bool IsEarly
        {
            get
            {
                return true;
            }
        }

        public IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            switch (context.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    return null;
                case ChangePropagationMode.OneWay:
                    if (direction != SynchronizationDirection.LeftToRight && direction != SynchronizationDirection.LeftToRightForced && direction != SynchronizationDirection.LeftWins)
                    {
                        return null;
                    }
                    break;
                case ChangePropagationMode.TwoWay:
                    break;
                default:
                    throw new InvalidOperationException("The change propagation mode is invalid.");
            }

            var instantiationMonitor = Predicate.Observe(computation.Input);
            instantiationMonitor.Successors.SetDummy();
            var monitor = new Monitor(instantiationMonitor, computation);
            monitor.StartMonitoring();
            return monitor;
        }

        private class Monitor : IDisposable
        {
            public INotifyValue<bool> InstantiationMonitor { get; private set; }
            public SynchronizationComputation<TLeft, TRight> Computation { get; private set; }

            public Monitor(INotifyValue<bool> instantiationMonitor, SynchronizationComputation<TLeft, TRight> computation)
            {
                InstantiationMonitor = instantiationMonitor;
                Computation = computation;
            }

            public void StartMonitoring()
            {
                InstantiationMonitor.ValueChanged += InstantiationChanged;
            }

            private void InstantiationChanged(object sender, ValueChangedEventArgs e)
            {
                if ((bool)e.NewValue) throw new InvalidOperationException();

                // Instantiation Path of the current computation is wrong

                var context = Computation.SynchronizationContext;

                // First collect all affected computations
                var allComputations = context.Trace.Trace(Computation.Input).OfType<ComputationBase>();
                var rearranged = InstantiationHelper.RearrangeComputations(allComputations, Computation);
                var firstSaved = rearranged.Previous;

                var instantiationPath = context.Transformation.ComputeInstantiatingTransformationRulePath(firstSaved.Value);
                
                var newComputations = new List<Computation>();
                var input = Computation.CreateInputArray();

                while (instantiationPath.Count > 0)
                {
                    newComputations.Add(instantiationPath.Pop().CreateComputation(input, null));
                }
                


                throw new NotImplementedException();
            }

            public void Dispose()
            {
                InstantiationMonitor.ValueChanged -= InstantiationChanged;
                InstantiationMonitor.Dispose();
            }
        }
    }

    internal class RightInstantiationMonitorJob<TLeft, TRight> : ISynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        public ObservingFunc<TRight, bool> Predicate { get; private set; }

        public RightInstantiationMonitorJob(ObservingFunc<TRight, bool> instantiationPredicate)
        {
            Predicate = instantiationPredicate;
        }

        public bool IsEarly
        {
            get
            {
                return true;
            }
        }

        public IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            switch (context.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    return null;
                case ChangePropagationMode.OneWay:
                    if (!direction.IsRightToLeft())
                    {
                        return null;
                    }
                    break;
                case ChangePropagationMode.TwoWay:
                    break;
                default:
                    throw new InvalidOperationException("The change propagation mode is invalid.");
            }

            var instantiationMonitor = Predicate.Observe(computation.Opposite.Input);
            instantiationMonitor.Successors.SetDummy();
            var monitor = new Monitor(instantiationMonitor, computation);
            monitor.StartMonitoring();
            return monitor;
        }

        private class Monitor : IDisposable
        {
            public INotifyValue<bool> InstantiationMonitor { get; private set; }
            public SynchronizationComputation<TLeft, TRight> Computation { get; private set; }

            public Monitor(INotifyValue<bool> instantiationMonitor, SynchronizationComputation<TLeft, TRight> computation)
            {
                InstantiationMonitor = instantiationMonitor;
                Computation = computation;
            }

            public void StartMonitoring()
            {
                InstantiationMonitor.ValueChanged += InstantiationChanged;
            }

            private void InstantiationChanged(object sender, ValueChangedEventArgs e)
            {
                if ((bool)e.NewValue) throw new InvalidOperationException();

                // Instantiation Path of the current computation is wrong
                throw new NotImplementedException();
            }

            public void Dispose()
            {
                InstantiationMonitor.ValueChanged -= InstantiationChanged;
                InstantiationMonitor.Dispose();
            }
        }
    }
}
