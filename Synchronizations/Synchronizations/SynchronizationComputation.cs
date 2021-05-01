using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions;
using System.Collections;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes a computation in a synchronization
    /// </summary>
    /// <typeparam name="TIn"></typeparam>
    /// <typeparam name="TOut"></typeparam>
    public abstract class SynchronizationComputation<TIn, TOut> : Computation, INotifyValue<TOut>, IOutputAccept<TOut>, ISuccessorList
    {
        private TIn input;

        /// <summary>
        /// Gets raised when the output of this synchronization changed
        /// </summary>
        public event EventHandler<ValueChangedEventArgs> OutputChanged;

        /// <summary>
        /// Indicates whether candidate search can be skipped because the context was newly established
        /// </summary>
        public bool OmitCandidateSearch
        {
            get;
            protected set;
        }

        /// <summary>
        /// Fires the event that the output has changed
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnOutputChanged(ValueChangedEventArgs e)
        {
            OutputChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Gets the input of this correspondence
        /// </summary>
        public TIn Input
        {
            get
            {
                return input;
            }
            set
            {
                if (!EqualityComparer<TIn>.Default.Equals( input, value))
                {
                    var changeEvent = new ValueChangedEventArgs(input, value);
                    if (input != null)
                    {
                        TransformationContext.Trace.RevokeEntry(this);
                    }
                    var disposal = Dependencies;
                    foreach (var item in disposal)
                    {
                        item.Dispose();
                    }
                    disposal.Clear();
                    input = value;
                    if (input != null)
                    {
                        TransformationContext.Trace.PublishEntry(this);
                    }
                    Opposite.OnOutputChanged(changeEvent);
                }
            }
        }

        /// <summary>
        /// Gets the opposite correspondence
        /// </summary>
        public SynchronizationComputation<TOut, TIn> Opposite { get; private set; }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rule">The transformation rule for which this correspondence was created</param>
        /// <param name="reverseRule">The reverse transformation rule</param>
        /// <param name="context">The context in which the correspondence is created</param>
        /// <param name="input">The input element</param>
        public SynchronizationComputation(TransformationRuleBase<TIn, TOut> rule, TransformationRuleBase<TOut, TIn> reverseRule, IComputationContext context, TIn input)
            : base(rule, context)
        {
            Opposite = new OppositeComputation(this, reverseRule);
            Input = input;
        }

        private SynchronizationComputation(TransformationRuleBase<TIn, TOut> rule, SynchronizationComputation<TOut, TIn> opposite)
            : base(rule, opposite.Context)
        {
            Opposite = opposite;
        }

        event EventHandler<ValueChangedEventArgs> INotifyValue<TOut>.ValueChanged
        {
            add
            {
                OutputChanged += value;
            }

            remove
            {
                OutputChanged -= value;
            }
        }

        /// <inheritdoc />
        public override object GetInput(int index)
        {
            if (index == 0)
            {
                return Input;
            }
            else
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }

        /// <summary>
        /// Performs the given action when the corresponding element of this computation was found
        /// </summary>
        /// <param name="toPerform">The action to perform</param>
        public void DoWhenOutputIsAvailable(Action<TIn, TOut> toPerform)
        {
            if (toPerform == null) return;
            if (!IsDelayed)
            {
                toPerform(Input, Opposite.Input);
            }
            else
            {
                OutputInitialized += (o, e) => toPerform(Input, Opposite.Input);
            }
        }

        /// <inheritdoc />
        public void AcceptNewOutput(TOut value)
        {
            Opposite.Input = value;
        }

        /// <inheritdoc />
        protected sealed override object OutputCore
        {
            get
            {
                return Opposite.Input;
            }
            set
            {
                Opposite.Input = (TOut)value;
            }
        }

        /// <summary>
        /// The context in which the correspondence has been established
        /// </summary>
        public ISynchronizationContext SynchronizationContext
        {
            get
            {
                return TransformationContext as ISynchronizationContext;
            }
        }

        /// <summary>
        /// Gets whether this correspondence was the original one
        /// </summary>
        public virtual bool IsOriginalComputation
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the synchronization rule of this correspondence
        /// </summary>
        public SynchronizationRuleBase SynchronizationRule
        {
            get
            {
                return ((ISynchronizationTransformationRule)TransformationRule).SynchronizationRule;
            }
        }

        TOut INotifyValue<TOut>.Value
        {
            get
            {
                return Opposite.Input;
            }
        }

        /// <summary>
        /// Gets the dependencies for this correspondence
        /// </summary>
        public virtual ICollection<IDisposable> Dependencies
        {
            get
            {
                return Opposite.Dependencies;
            }
        }


        /// <inheritdoc />
        public ISuccessorList Successors => this;

        IEnumerable<INotifiable> INotifiable.Dependencies { get { return Enumerable.Empty<INotifiable>(); } }

        /// <inheritdoc />
        public ExecutionMetaData ExecutionMetaData { get; } = new ExecutionMetaData();

        /// <inheritdoc />
        public INotificationResult Notify(IList<INotificationResult> sources)
        {
            throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public void Dispose() { }

        #region SuccessorList


        private bool isDummySet = false;
        private readonly List<INotifiable> successors = new List<INotifiable>();

        /// <inheritdoc />
        public bool HasSuccessors => !isDummySet && successors.Count > 0;

        /// <inheritdoc />
        public bool IsAttached => isDummySet || successors.Count > 0;

        /// <inheritdoc />
        public int Count => successors.Count;

        public IEnumerable<INotifiable> AllSuccessors => successors;


        /// <inheritdoc />
        public void Set(INotifiable node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            successors.Add(node);
            if (isDummySet)
            {
                isDummySet = false;
            }
        }


        /// <inheritdoc />
        public void SetDummy()
        {
            if (successors.Count == 0 && !isDummySet)
            {
                isDummySet = true;
            }
        }


        /// <inheritdoc />
        public void Unset(INotifiable node, bool leaveDummy = false)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (!successors.Remove(node))
            {
                throw new InvalidOperationException("The specified node is not registered as the successor.");
            }
        }


        /// <inheritdoc />
        public void UnsetAll()
        {
            if (IsAttached)
            {
                isDummySet = false;
                successors.Clear();
            }
        }

        public INotifiable GetSuccessor(int index)
        {
            return successors[index];
        }

        #endregion

        private class OppositeComputation : SynchronizationComputation<TOut, TIn>
        {
            private readonly List<IDisposable> dependencies = new List<IDisposable>();

            public override ICollection<IDisposable> Dependencies
            {
                get
                {
                    return dependencies;
                }
            }

            public OppositeComputation(SynchronizationComputation<TIn, TOut> opposite, TransformationRuleBase<TOut, TIn> rule)
                : base(rule, opposite) { }

            public override void Transform()
            {
                throw new InvalidOperationException();
            }

            public override object CreateOutput(System.Collections.IEnumerable context)
            {
                throw new InvalidOperationException();
            }

            public override bool IsOriginalComputation
            {
                get
                {
                    return false;
                }
            }
        }
    }
}
