using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public void Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            switch (context.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    return;
                case ChangePropagationMode.OneWay:
                    if (direction != SynchronizationDirection.LeftToRight && direction != SynchronizationDirection.LeftToRightForced && direction != SynchronizationDirection.LeftWins)
                    {
                        return;
                    }
                    break;
                case ChangePropagationMode.TwoWay:
                    break;
                default:
                    throw new InvalidOperationException("The change propagation mode is invalid.");
            }

            var instantiationMonitor = Predicate.Observe(computation.Input);
            var monitor = new Monitor(instantiationMonitor, computation);
            monitor.StartMonitoring();
        }

        private class Monitor
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

        public void Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            switch (context.ChangePropagation)
            {
                case ChangePropagationMode.None:
                    return;
                case ChangePropagationMode.OneWay:
                    if (direction != SynchronizationDirection.RightToLeft && direction != SynchronizationDirection.RightToLeftForced && direction != SynchronizationDirection.RightWins)
                    {
                        return;
                    }
                    break;
                case ChangePropagationMode.TwoWay:
                    break;
                default:
                    throw new InvalidOperationException("The change propagation mode is invalid.");
            }

            var instantiationMonitor = Predicate.Observe(computation.Opposite.Input);
            var monitor = new Monitor(instantiationMonitor, computation);
            monitor.StartMonitoring();
        }

        private class Monitor
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
        }
    }
}
