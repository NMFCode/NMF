using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Expressions;

namespace NMF.Synchronizations
{
    public abstract class GuardedSynchronizationJob<TLeft, TRight> : ISynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        public ISynchronizationJob<TLeft, TRight> Inner { get; private set; }

        public GuardedSynchronizationJob(ISynchronizationJob<TLeft, TRight> inner)
        {
            if (inner == null) throw new ArgumentNullException("inner");

            Inner = inner;
        }

        public bool IsEarly
        {
            get
            {
                return Inner.IsEarly;
            }
        }

        public IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            return new Dependency(Inner, computation, direction, CreateTracker(computation));
        }

        protected abstract INotifyValue<bool> CreateTracker(SynchronizationComputation<TLeft, TRight> computation);

        private class Dependency : IDisposable
        {
            public ISynchronizationJob<TLeft, TRight> Inner { get; private set; }
            public SynchronizationComputation<TLeft, TRight> Computation { get; private set; }
            public SynchronizationDirection Direction { get; private set; }
            public IDisposable Current { get; private set; }
            public INotifyValue<bool> Tracker { get; private set; }

            public Dependency(ISynchronizationJob<TLeft, TRight> inner, SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, INotifyValue<bool> tracker)
            {
                Inner = inner;
                Computation = computation;
                Direction = direction;
                Tracker = tracker;

                if (tracker.Value)
                {
                    Current = Inner.Perform(Computation, Direction, Computation.SynchronizationContext);
                }

                Tracker.ValueChanged += Tracker_ValueChanged;
            }

            private void Tracker_ValueChanged(object sender, ValueChangedEventArgs e)
            {
                if (Current != null)
                {
                    Computation.Dependencies.Remove(Current);
                    Current.Dispose();
                    Current = null;
                }
                if (Tracker.Value)
                {
                    Current = Inner.Perform(Computation, Direction, Computation.SynchronizationContext);
                }
            }

            public void Dispose()
            {
                if (Current != null)
                {
                    Current.Dispose();
                }
                Tracker.ValueChanged -= Tracker_ValueChanged;
                Tracker.Dispose();
            }
        }
    }

    public class BothGuardedSynchronizationJob<TLeft, TRight> : GuardedSynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        public ObservingFunc<TLeft, TRight, bool> Guard { get; private set; }

        public BothGuardedSynchronizationJob(ISynchronizationJob<TLeft, TRight> inner, ObservingFunc<TLeft, TRight, bool> guard)
            : base(inner)
        {
            if (guard == null) throw new ArgumentNullException("guard");

            Guard = guard;
        }

        protected override INotifyValue<bool> CreateTracker(SynchronizationComputation<TLeft, TRight> computation)
        {
            return Guard.Observe(computation.Input, computation.Opposite.Input);
        }
    }

    public class RightGuardedSynchronizationJob<TLeft, TRight> : GuardedSynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        public ObservingFunc<TRight, bool> Guard { get; private set; }

        public RightGuardedSynchronizationJob(ISynchronizationJob<TLeft, TRight> inner, ObservingFunc<TRight, bool> guard)
            : base(inner)
        {
            if (guard == null) throw new ArgumentNullException("guard");

            Guard = guard;
        }

        protected override INotifyValue<bool> CreateTracker(SynchronizationComputation<TLeft, TRight> computation)
        {
            return Guard.Observe(computation.Opposite.Input);
        }
    }

    public class LeftGuardedSynchronizationJob<TLeft, TRight> : GuardedSynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        public ObservingFunc<TLeft, bool> Guard { get; private set; }

        public LeftGuardedSynchronizationJob(ISynchronizationJob<TLeft, TRight> inner, ObservingFunc<TLeft, bool> guard)
            : base(inner)
        {
            if (guard == null) throw new ArgumentNullException("guard");

            Guard = guard;
        }

        protected override INotifyValue<bool> CreateTracker(SynchronizationComputation<TLeft, TRight> computation)
        {
            return Guard.Observe(computation.Input);
        }
    }
}
