using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Synchronizations
{
    internal class LeftToRightRule<TLeft, TRight> : TransformationRuleBase<TLeft, TRight>, ISynchronizationTransformationRule
        where TRight : class
        where TLeft : class
    {
        private SynchronizationRule<TLeft, TRight> rule;
        private bool needDependencies;

        public LeftToRightRule(SynchronizationRule<TLeft, TRight> rule)
        {
            this.rule = rule;
            var createRightOutput = rule.GetType().GetMethod("CreateRightOutput", BindingFlags.Instance | BindingFlags.NonPublic);
            this.needDependencies = createRightOutput.ReflectedType != typeof(SynchronizationRule<TLeft, TRight>);
        }

        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            return new LTRComputation(rule, context, (TLeft)input[0]);
        }

        internal void SetTransformationDelay(byte value)
        {
            TransformationDelayLevel = value;
        }

        internal void SetOutputDelay(byte value)
        {
            OutputDelayLevel = value;
        }


        public SynchronizationRuleBase SynchronizationRule
        {
            get { return rule; }
        }

        private class LTRComputation : SynchronizationComputation<TLeft, TRight>
        {
            public LTRComputation(SynchronizationRule<TLeft, TRight> rule, IComputationContext context, TLeft left)
                : base(rule.LeftToRight, rule.RightToLeft, context, left) { }

            public override void Transform()
            {
                ((SynchronizationRule<TLeft, TRight>)SynchronizationRule).Synchronize(this, SynchronizationDirection.LeftToRight, TransformationContext as ISynchronizationContext);
            }

            protected override void OnOutputInitialized(EventArgs e)
            {
                base.OnOutputInitialized(e);
                if (Output != null)
                {
                    ((SynchronizationRule<TLeft, TRight>)SynchronizationRule).InitializeOutput(this);
                }
            }

            public override object CreateOutput(IEnumerable context)
            {
                var rule = (SynchronizationRule<TLeft, TRight>)SynchronizationRule;
                bool existing;
                var result = rule.CreateRightOutputInternal(Input, context, SynchronizationContext, out existing);
                OmitCandidateSearch = !existing;
                return result;
            }
        }

        public override bool NeedDependenciesForOutputCreation
        {
            get { return needDependencies; }
        }
    }
}
