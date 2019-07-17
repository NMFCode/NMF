using NMF.Expressions;
using System;

namespace NMF.Synchronizations
{
    internal class GuardedSynchronization<TLeft, TRight> : IDisposable
        where TLeft : class
        where TRight : class
    {
        public SynchronizationComputation<TLeft, TRight> Computation { get; set; }
        public Func<SynchronizationComputation<TLeft, TRight>, IDisposable> Func { get; set; }
        public IDisposable Current { get; set; }
        public INotifyValue<bool> Guard { get; set; }

        public GuardedSynchronization(SynchronizationComputation<TLeft, TRight> computation, Func<SynchronizationComputation<TLeft, TRight>, IDisposable> func, INotifyValue<bool> guard)
        {
            Computation = computation;
            Func = func;
            Guard = guard;

            if (guard.Value)
            {
                Current = func(computation);
                if (Current != null)
                {
                    Computation.Dependencies.Add(Current);
                }
            }

            Guard.ValueChanged += Guard_ValueChanged;
        }

        private void Guard_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (Current != null)
            {
                Computation.Dependencies.Remove(Current);
                Current.Dispose();
                Current = null;
            }
            if (Guard.Value)
            {
                Current = Func(Computation);
                if (Current != null)
                {
                    Computation.Dependencies.Add(Current);
                }
            }
        }

        public void Dispose()
        {
            Guard.ValueChanged -= Guard_ValueChanged;
            Guard.Dispose();
        }
    }
}
