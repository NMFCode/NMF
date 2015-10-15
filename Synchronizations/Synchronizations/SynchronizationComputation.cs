using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    public abstract class SynchronizationComputation<TIn, TOut> : Computation
        where TIn : class
        where TOut : class
    {
        private TIn input;
        private bool omitSearch = false;

        public bool OmitCandidateSearch
        {
            get;
            protected set;
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
                    if (input != null)
                    {
                        TransformationContext.Trace.RevokeEntry(this);
                    }
                    input = value;
                    if (input != null)
                    {
                        TransformationContext.Trace.PublishEntry(this);
                    }
                }
            }
        }

        public SynchronizationComputation<TOut, TIn> Opposite { get; private set; }

        public SynchronizationComputation(TransformationRuleBase<TIn, TOut> rule, TransformationRuleBase<TOut, TIn> reverseRule, IComputationContext context, TIn input)
            : base(rule, context)
        {
            Input = input;
            Opposite = new OppositeComputation(this, reverseRule);
        }

        private SynchronizationComputation(TransformationRuleBase<TIn, TOut> rule, SynchronizationComputation<TOut, TIn> opposite)
            : base(rule, opposite.Context)
        {
            Opposite = opposite;
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

        private class OppositeComputation : SynchronizationComputation<TOut, TIn>
        {
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
