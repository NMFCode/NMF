using System;
using NMF.Expressions;
using NMF.Transformations;

namespace NMF.Synchronizations
{
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
                if ((bool)e.NewValue) return;

                //// Instantiation Path of the current computation is wrong

                //var context = Computation.SynchronizationContext;

                //// First collect all affected computations
                //var allComputations = context.Trace.Trace(Computation.Input).OfType<ComputationBase>();
                //var rearranged = InstantiationHelper.RearrangeComputations(allComputations, Computation);
                //var firstSaved = rearranged.Previous;

                //var instantiationPath = context.Transformation.ComputeInstantiatingTransformationRulePath(firstSaved.Value);
                
                //var newComputations = new List<Computation>();
                //var input = Computation.CreateInputArray();

                //while (instantiationPath.Count > 0)
                //{
                //    newComputations.Add(instantiationPath.Pop().CreateComputation(input, null));
                //}
                


                //throw new NotImplementedException();
            }

            public void Dispose()
            {
                InstantiationMonitor.ValueChanged -= InstantiationChanged;
                InstantiationMonitor.Dispose();
            }
        }
    }
}
