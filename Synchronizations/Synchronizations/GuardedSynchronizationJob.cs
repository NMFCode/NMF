using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Expressions;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes the base class for a synchronization job that is filtered by a guard
    /// </summary>
    /// <typeparam name="TLeft">The LHS type of the guard</typeparam>
    /// <typeparam name="TRight">The RHS type of the guard</typeparam>
    public abstract class GuardedSynchronizationJob<TLeft, TRight> : ISynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        private ISynchronizationJob<TLeft, TRight> Inner { get; set; }

        /// <summary>
        /// Creates a new guarded synchronization job
        /// </summary>
        /// <param name="inner">The inner synchronization job</param>
        public GuardedSynchronizationJob(ISynchronizationJob<TLeft, TRight> inner)
        {
            if (inner == null) throw new ArgumentNullException("inner");

            Inner = inner;
        }

        /// <inheritdoc />
        public bool IsEarly
        {
            get
            {
                return Inner.IsEarly;
            }
        }

        /// <inheritdoc />
        public IDisposable Perform(SynchronizationComputation<TLeft, TRight> computation, SynchronizationDirection direction, ISynchronizationContext context)
        {
            return new Dependency(Inner, computation, direction, CreateTracker(computation));
        }

        /// <summary>
        /// Creates a tracker for the given computation
        /// </summary>
        /// <param name="computation">The computation that shall be tracked</param>
        /// <returns></returns>
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

    /// <summary>
    /// Denotes a class that represents a synchronization job guarded at both LHS and RHS
    /// </summary>
    /// <typeparam name="TLeft">The LHS type</typeparam>
    /// <typeparam name="TRight">The RHS type</typeparam>
    public class BothGuardedSynchronizationJob<TLeft, TRight> : GuardedSynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        private ObservingFunc<TLeft, TRight, bool> Guard { get; set; }

        /// <summary>
        /// Creates a new guarded synchronization job
        /// </summary>
        /// <param name="inner">The inner synchronization job</param>
        /// <param name="guard">The actual guard</param>
        public BothGuardedSynchronizationJob(ISynchronizationJob<TLeft, TRight> inner, ObservingFunc<TLeft, TRight, bool> guard)
            : base(inner)
        {
            if (guard == null) throw new ArgumentNullException("guard");

            Guard = guard;
        }

        /// <inheritdoc />
        protected override INotifyValue<bool> CreateTracker(SynchronizationComputation<TLeft, TRight> computation)
        {
            var tracker = Guard.Observe(computation.Input, computation.Opposite.Input);
            tracker.Successors.SetDummy();
            return tracker;
        }
    }

    /// <summary>
    /// Denotes a synchronization job that is guarded at the RHS
    /// </summary>
    /// <typeparam name="TLeft">The LHS type</typeparam>
    /// <typeparam name="TRight">The RHS type</typeparam>
    public class RightGuardedSynchronizationJob<TLeft, TRight> : GuardedSynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        private ObservingFunc<TRight, bool> Guard { get; set; }

        /// <summary>
        /// Creates a new synchronization job guarded at the RHS
        /// </summary>
        /// <param name="inner"></param>
        /// <param name="guard"></param>
        public RightGuardedSynchronizationJob(ISynchronizationJob<TLeft, TRight> inner, ObservingFunc<TRight, bool> guard)
            : base(inner)
        {
            if (guard == null) throw new ArgumentNullException("guard");

            Guard = guard;
        }

        /// <inheritdoc />
        protected override INotifyValue<bool> CreateTracker(SynchronizationComputation<TLeft, TRight> computation)
        {
            var tracker = Guard.Observe(computation.Opposite.Input);
            tracker.Successors.SetDummy();
            return tracker;
        }
    }

    /// <summary>
    /// Denotes a synchronization job guarded at the LHS
    /// </summary>
    /// <typeparam name="TLeft">The LHS type</typeparam>
    /// <typeparam name="TRight">The RHS type</typeparam>
    public class LeftGuardedSynchronizationJob<TLeft, TRight> : GuardedSynchronizationJob<TLeft, TRight>
        where TLeft : class
        where TRight : class
    {
        private ObservingFunc<TLeft, bool> Guard { get; set; }

        /// <summary>
        /// Creates a new synchronization job guarded at the LHS
        /// </summary>
        /// <param name="inner">The inner synchronization job</param>
        /// <param name="guard">The guard function</param>
        public LeftGuardedSynchronizationJob(ISynchronizationJob<TLeft, TRight> inner, ObservingFunc<TLeft, bool> guard)
            : base(inner)
        {
            if (guard == null) throw new ArgumentNullException("guard");

            Guard = guard;
        }

        /// <inheritdoc />
        protected override INotifyValue<bool> CreateTracker(SynchronizationComputation<TLeft, TRight> computation)
        {
            var tracker = Guard.Observe(computation.Input);
            tracker.Successors.SetDummy();
            return tracker;
        }
    }
}
