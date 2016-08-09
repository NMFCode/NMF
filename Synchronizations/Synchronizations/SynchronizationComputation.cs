using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Expressions;

namespace NMF.Synchronizations
{
    public abstract class SynchronizationComputation<TIn, TOut> : Computation, INotifyValue<TOut>
        where TIn : class
        where TOut : class
    {
        private TIn input;
        private bool omitSearch = false;

        public event EventHandler<ValueChangedEventArgs> OutputChanged;

        public bool OmitCandidateSearch
        {
            get;
            protected set;
        }

        protected virtual void OnOutputChanged(ValueChangedEventArgs e)
        {
            OutputChanged?.Invoke(this, e);
        }

        public TIn Input
        {
            get
            {
                return input;
            }
            set
            {
                if (input != value)
                {
                    var changeEvent = new ValueChangedEventArgs(input, value);
                    if (input != null)
                    {
                        TransformationContext.Trace.RevokeEntry(this);
                    }
                    var disposal = Dependencies;
                    while (disposal.Count > 0)
                    {
                        disposal.Dequeue().Dispose();
                    }
                    input = value;
                    if (input != null)
                    {
                        TransformationContext.Trace.PublishEntry(this);
                    }
                    Opposite.OnOutputChanged(changeEvent);
                }
            }
        }

        public SynchronizationComputation<TOut, TIn> Opposite { get; private set; }

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

        void INotifyValue<TOut>.Detach() { }

        void INotifyValue<TOut>.Attach() { }

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

        public ISynchronizationContext SynchronizationContext
        {
            get
            {
                return TransformationContext as ISynchronizationContext;
            }
        }

        public virtual bool IsOriginalComputation
        {
            get
            {
                return true;
            }
        }

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

        bool INotifyValue<TOut>.IsAttached
        {
            get
            {
                return true;
            }
        }

        public virtual Queue<IDisposable> Dependencies
        {
            get
            {
                return Opposite.Dependencies;
            }
        }

        private class OppositeComputation : SynchronizationComputation<TOut, TIn>
        {
            private Queue<IDisposable> dependencies = new Queue<IDisposable>();

            public override Queue<IDisposable> Dependencies
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
