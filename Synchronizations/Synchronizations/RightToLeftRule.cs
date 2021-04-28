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
    internal class RightToLeftRule<TLeft, TRight> : TransformationRuleBase<TRight, TLeft>, ISynchronizationTransformationRule
        where TRight : class
        where TLeft : class
    {
        private readonly SynchronizationRule<TLeft, TRight> rule;
        private readonly bool needDependencies;

        public RightToLeftRule(SynchronizationRule<TLeft, TRight> rule)
        {
            this.rule = rule;
            var createLeftOutput = rule.GetType().GetMethod("CreateLeftOutput", BindingFlags.NonPublic | BindingFlags.Instance);
            this.needDependencies = createLeftOutput.ReflectedType != typeof(SynchronizationRule<TLeft, TRight>);
        }

        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            return new RTLComputation(rule, context, (TRight)input[0]);
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

        public override string ToString()
        {
            return $"RTL for {rule.GetType().Name}";
        }

        private class RTLComputation : SynchronizationComputation<TRight, TLeft>
        {
            public RTLComputation(SynchronizationRule<TLeft, TRight> rule, IComputationContext context, TRight right)
                : base(rule.RightToLeft, rule.LeftToRight, context, right) { }

            public override void Transform()
            {
                ((SynchronizationRule<TLeft, TRight>)SynchronizationRule).Synchronize(Opposite, SynchronizationDirection.RightToLeft, TransformationContext as ISynchronizationContext);
            }

            public override object CreateOutput(IEnumerable context)
            {
                var rule = (SynchronizationRule<TLeft, TRight>)SynchronizationRule;
                bool existing;
                var result = rule.CreateLeftOutputInternal(Input, context, SynchronizationContext, out existing);
                OmitCandidateSearch = !existing;
                return result;
            }

            protected override void OnOutputInitialized(EventArgs e)
            {
                base.OnOutputInitialized(e);
                if (Output != null)
                {
                    ((SynchronizationRule<TLeft, TRight>)SynchronizationRule).InitializeOutput(Opposite);
                }
            }
        }

        public override bool NeedDependenciesForOutputCreation
        {
            get { return needDependencies; }
        }
    }
}
