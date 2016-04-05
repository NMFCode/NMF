using NMF.Expressions;
using NMF.Transformations.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Transformations.Linq
{
    /// <summary>
    /// Represents the usage of a transformation rule with one input argument in a relational pattern
    /// </summary>
    /// <typeparam name="TIn">The type of the transformation rule input argument</typeparam>
    /// <typeparam name="TOut">The transformation rule output type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class TransformationRuleSource<TIn, TOut> : ITransformationRuleDependency, INotifyEnumerable<TransformationComputationWrapper<TIn, TOut>>
        where TIn : class
        where TOut : class
    {
        /// <summary>
        /// Creates a new TransformationRuleSource instance for the given transformation rule in the given context
        /// </summary>
        /// <param name="rule">The transformation rule that should be used as source</param>
        /// <param name="context">The context in which the computations should be used by the current instance</param>
        public TransformationRuleSource(TransformationRuleBase<TIn, TOut> rule, ITransformationContext context)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (context == null) throw new ArgumentNullException("context");

            TransformationRule = rule;
            Context = context;

            rule.Dependencies.Add(this);
            foreach (var c in context.Trace.TraceAllIn(rule).OfType<Computation>())
            {
                items.Add(new TransformationComputationWrapper<TIn, TOut>(c));
            }
        }

        private List<TransformationComputationWrapper<TIn, TOut>> items = new List<TransformationComputationWrapper<TIn, TOut>>();


        /// <summary>
        /// Gets the transformation rule this transformation rule source is responsible for
        /// </summary>
        public TransformationRuleBase<TIn, TOut> TransformationRule { get; private set; }

        /// <summary>
        /// Gets the context in which the transformation rule source is active
        /// </summary>
        public ITransformationContext Context { get; private set; }

        /// <summary>
        /// Gets or sets the filter to be used to filter computations
        /// </summary>
        public Func<TransformationComputationWrapper<TIn, TOut>, bool> Filter { get; set; }

        /// <summary>
        /// Receives a new computation from the computation rule
        /// </summary>
        /// <param name="computation">The computation that is received</param>
        public void HandleDependency(Computation computation)
        {
            if (computation != null && computation.TransformationContext == Context)
            {
                var c = new TransformationComputationWrapper<TIn, TOut>(computation);
                if (Filter != null && !Filter(c)) return;
                if (CollectionChanged != null)
                {
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, c));
                }
                items.Add(c);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the computation object should be forwarded before or after the dependencies are resolved
        /// </summary>
        public bool ExecuteBefore
        {
            get;
            set;
        }

        /// <summary>
        /// Clears dependencies
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Clears dependencies
        /// </summary>
        /// <param name="disposing">A value indicating whether Dispose was called</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TransformationRule.Dependencies.Remove(this);
            }
        }

        /// <summary>
        /// Adds a null instance to the received computations
        /// </summary>
        internal void AddNullItem()
        {
            items.Add(new TransformationComputationWrapper<TIn, TOut>());
        }

        /// <summary>
        /// Gets an enumerator that enumerates the collected computations so far
        /// </summary>
        /// <returns>An enumerator</returns>
        public IEnumerator<TransformationComputationWrapper<TIn, TOut>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        void INotifyEnumerable.Attach()
        {
        }

        void INotifyEnumerable.Detach()
        {
        }

        bool INotifyEnumerable.IsAttached
        {
            get { return true; }
        }

        /// <summary>
        /// Gets fired when new elements appear in the trace
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }

    /// <summary>
    /// Represents the usage of a transformation rule with two input arguments in a relational pattern
    /// </summary>
    /// <typeparam name="TIn1">The type of the first transformation rule input argument</typeparam>
    /// <typeparam name="TIn2">The type of the second transformation rule input argument</typeparam>
    /// <typeparam name="TOut">The transformation rule output type</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class TransformationRuleSource<TIn1, TIn2, TOut> : ITransformationRuleDependency, INotifyEnumerable<TransformationComputationWrapper<TIn1, TIn2, TOut>>
        where TIn1 : class
        where TIn2 : class
        where TOut : class
    {
        /// <summary>
        /// Creates a new TransformationRuleSource instance for the given transformation rule in the given context
        /// </summary>
        /// <param name="rule">The transformation rule that should be used as source</param>
        /// <param name="context">The context in which the computations should be used by the current instance</param>
        public TransformationRuleSource(TransformationRuleBase<TIn1, TIn2, TOut> rule, ITransformationContext context)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (context == null) throw new ArgumentNullException("context");

            TransformationRule = rule;
            Context = context;

            rule.Dependencies.Add(this);
            foreach (var c in context.Trace.TraceAllIn(rule).OfType<Computation>())
            {
                items.Add(new TransformationComputationWrapper<TIn1, TIn2, TOut>(c));
            }
        }

        private List<TransformationComputationWrapper<TIn1, TIn2, TOut>> items = new List<TransformationComputationWrapper<TIn1, TIn2, TOut>>();


        /// <summary>
        /// Gets the transformation rule this transformation rule source is responsible for
        /// </summary>
        public TransformationRuleBase<TIn1, TIn2, TOut> TransformationRule { get; private set; }

        /// <summary>
        /// Gets the context in which the transformation rule source is active
        /// </summary>
        public ITransformationContext Context { get; private set; }

        /// <summary>
        /// Gets or sets the filter to be used to filter computations
        /// </summary>
        public Func<TransformationComputationWrapper<TIn1, TIn2, TOut>, bool> Filter { get; set; }

        /// <summary>
        /// Receives a new computation from the computation rule
        /// </summary>
        /// <param name="computation">The computation that is received</param>
        public void HandleDependency(Computation computation)
        {
            if (computation != null && computation.TransformationContext == Context)
            {
                var c = new TransformationComputationWrapper<TIn1, TIn2, TOut>(computation);
                if (Filter != null && !Filter(c)) return;
                if (CollectionChanged != null)
                {
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, c));
                }
                items.Add(c);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the computation object should be forwarded before or after the dependencies are resolved
        /// </summary>
        public bool ExecuteBefore
        {
            get;
            set;
        }

        /// <summary>
        /// Clears dependencies
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Clears dependencies
        /// </summary>
        /// <param name="disposing">A value indicating whether Dispose was called</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TransformationRule.Dependencies.Remove(this);
            }
        }

        /// <summary>
        /// Adds a null instance to the received computations
        /// </summary>
        internal void AddNullItem()
        {
            items.Add(new TransformationComputationWrapper<TIn1, TIn2, TOut>());
        }

        /// <summary>
        /// Gets an enumerator that enumerates the collected computations so far
        /// </summary>
        /// <returns>An enumerator</returns>
        public IEnumerator<TransformationComputationWrapper<TIn1, TIn2, TOut>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        void INotifyEnumerable.Attach()
        {
        }

        void INotifyEnumerable.Detach()
        {
        }

        bool INotifyEnumerable.IsAttached
        {
            get { return true; }
        }

        /// <summary>
        /// Gets fired when new elements appear in the trace
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }

    /// <summary>
    /// Represents the usage of a transformation rule with one input argument in a relational pattern
    /// </summary>
    /// <typeparam name="TIn">The type of the transformation rule input argument</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class InPlaceTransformationRuleSource<TIn> : ITransformationRuleDependency, INotifyEnumerable<InPlaceComputationWrapper<TIn>>
        where TIn : class
    {
        /// <summary>
        /// Creates a new TransformationRuleSource instance for the given transformation rule in the given context
        /// </summary>
        /// <param name="rule">The transformation rule that should be used as source</param>
        /// <param name="context">The context in which the computations should be used by the current instance</param>
        public InPlaceTransformationRuleSource(InPlaceTransformationRuleBase<TIn> rule, ITransformationContext context)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (context == null) throw new ArgumentNullException("context");

            TransformationRule = rule;
            Context = context;

            rule.Dependencies.Add(this);
            foreach (var c in context.Trace.TraceAllIn(rule).OfType<Computation>())
            {
                items.Add(new InPlaceComputationWrapper<TIn>(c));
            }
        }

        private List<InPlaceComputationWrapper<TIn>> items = new List<InPlaceComputationWrapper<TIn>>();


        /// <summary>
        /// Gets the transformation rule this transformation rule source is responsible for
        /// </summary>
        public InPlaceTransformationRuleBase<TIn> TransformationRule { get; private set; }

        /// <summary>
        /// Gets the context in which the transformation rule source is active
        /// </summary>
        public ITransformationContext Context { get; private set; }

        /// <summary>
        /// Gets or sets the filter to be used to filter computations
        /// </summary>
        public Func<InPlaceComputationWrapper<TIn>, bool> Filter { get; set; }

        /// <summary>
        /// Receives a new computation from the computation rule
        /// </summary>
        /// <param name="computation">The computation that is received</param>
        public void HandleDependency(Computation computation)
        {
            if (computation != null && computation.TransformationContext == Context)
            {
                var c = new InPlaceComputationWrapper<TIn>(computation);
                if (Filter != null && !Filter(c)) return;
                if (CollectionChanged != null)
                {
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, c));
                }
                items.Add(c);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the computation object should be forwarded before or after the dependencies are resolved
        /// </summary>
        public bool ExecuteBefore
        {
            get;
            set;
        }

        /// <summary>
        /// Clears dependencies
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Clears dependencies
        /// </summary>
        /// <param name="disposing">A value indicating whether Dispose was called</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TransformationRule.Dependencies.Remove(this);
            }
        }

        /// <summary>
        /// Adds a null instance to the received computations
        /// </summary>
        internal void AddNullItem()
        {
            items.Add(new InPlaceComputationWrapper<TIn>());
        }

        /// <summary>
        /// Gets an enumerator that enumerates the collected computations so far
        /// </summary>
        /// <returns>An enumerator</returns>
        public IEnumerator<InPlaceComputationWrapper<TIn>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        void INotifyEnumerable.Attach()
        {
        }

        void INotifyEnumerable.Detach()
        {
        }

        bool INotifyEnumerable.IsAttached
        {
            get { return true; }
        }

        /// <summary>
        /// Gets fired when new elements appear in the trace
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }

    /// <summary>
    /// Represents the usage of a transformation rule with two input arguments in a relational pattern
    /// </summary>
    /// <typeparam name="TIn1">The type of the first transformation rule input argument</typeparam>
    /// <typeparam name="TIn2">The type of the second transformation rule input argument</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    public class InPlaceTransformationRuleSource<TIn1, TIn2> : ITransformationRuleDependency, INotifyEnumerable<InPlaceComputationWrapper<TIn1, TIn2>>
        where TIn1 : class
        where TIn2 : class
    {
        /// <summary>
        /// Creates a new TransformationRuleSource instance for the given transformation rule in the given context
        /// </summary>
        /// <param name="rule">The transformation rule that should be used as source</param>
        /// <param name="context">The context in which the computations should be used by the current instance</param>
        public InPlaceTransformationRuleSource(InPlaceTransformationRuleBase<TIn1, TIn2> rule, ITransformationContext context)
        {
            if (rule == null) throw new ArgumentNullException("rule");
            if (context == null) throw new ArgumentNullException("context");

            TransformationRule = rule;
            Context = context;

            rule.Dependencies.Add(this);
            foreach (var c in context.Trace.TraceAllIn(rule).OfType<Computation>())
            {
                items.Add(new InPlaceComputationWrapper<TIn1, TIn2>(c));
            }
        }

        private List<InPlaceComputationWrapper<TIn1, TIn2>> items = new List<InPlaceComputationWrapper<TIn1, TIn2>>();


        /// <summary>
        /// Gets the transformation rule this transformation rule source is responsible for
        /// </summary>
        public InPlaceTransformationRuleBase<TIn1, TIn2> TransformationRule { get; private set; }

        /// <summary>
        /// Gets the context in which the transformation rule source is active
        /// </summary>
        public ITransformationContext Context { get; private set; }

        /// <summary>
        /// Gets or sets the filter to be used to filter computations
        /// </summary>
        public Func<InPlaceComputationWrapper<TIn1, TIn2>, bool> Filter { get; set; }

        /// <summary>
        /// Receives a new computation from the computation rule
        /// </summary>
        /// <param name="computation">The computation that is received</param>
        public void HandleDependency(Computation computation)
        {
            if (computation != null && computation.TransformationContext == Context)
            {
                var c = new InPlaceComputationWrapper<TIn1, TIn2>(computation);
                if (Filter != null && !Filter(c)) return;
                if (CollectionChanged != null)
                {
                    CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, c));
                }
                items.Add(c);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the computation object should be forwarded before or after the dependencies are resolved
        /// </summary>
        public bool ExecuteBefore
        {
            get;
            set;
        }

        /// <summary>
        /// Clears dependencies
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Clears dependencies
        /// </summary>
        /// <param name="disposing">A value indicating whether Dispose was called</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                TransformationRule.Dependencies.Remove(this);
            }
        }

        /// <summary>
        /// Adds a null instance to the received computations
        /// </summary>
        internal void AddNullItem()
        {
            items.Add(new InPlaceComputationWrapper<TIn1, TIn2>());
        }

        /// <summary>
        /// Gets an enumerator that enumerates the collected computations so far
        /// </summary>
        /// <returns>An enumerator</returns>
        public IEnumerator<InPlaceComputationWrapper<TIn1, TIn2>> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        void INotifyEnumerable.Attach()
        {
        }

        void INotifyEnumerable.Detach()
        {
        }

        bool INotifyEnumerable.IsAttached
        {
            get { return true; }
        }

        /// <summary>
        /// Gets fired when new elements appear in the trace
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
