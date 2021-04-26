using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations.Core;
using System.Collections.Generic;

namespace NMF.Synchronizations.Inconsistencies
{
    public class ReferenceInconsistency<TLeft, TRight, TDepLeft, TDepRight> : IInconsistency
        where TLeft : class
        where TRight : class
        where TDepLeft : class
        where TDepRight : class
    {
        public TDepLeft LeftValue { get; }
        public TDepRight RightValue { get; }
        private SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> Rule { get; }
        public SynchronizationComputation<TLeft, TRight> BaseCorrespondence { get; }

        internal ReferenceInconsistency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> rule, SynchronizationComputation<TLeft, TRight> baseCorrespondence, TDepLeft leftValue, TDepRight rightValue)
        {
            Rule = rule;
            BaseCorrespondence = baseCorrespondence;
            LeftValue = leftValue;
            RightValue = rightValue;
        }


        public bool CanResolveLeft => Rule.leftSetter != null;

        public bool CanResolveRight => Rule.rightSetter != null;

        public override int GetHashCode()
        {
            var hashCode = Rule.GetHashCode();
            unchecked
            {
                hashCode = (23 * hashCode) + BaseCorrespondence.GetHashCode();
                if (LeftValue != null) hashCode = (23 * hashCode) + LeftValue.GetHashCode();
                if (RightValue != null) hashCode = (23 * hashCode) + RightValue.GetHashCode();
            }
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, this)) return true;
            if (obj is ReferenceInconsistency<TLeft, TRight, TDepLeft, TDepRight> other) return Equals(other);
            return false;
        }

        public bool Equals(IInconsistency other)
        {
            return Equals(other as ReferenceInconsistency<TLeft, TRight, TDepLeft, TDepRight>);
        }

        public bool Equals(ReferenceInconsistency<TLeft, TRight, TDepLeft, TDepRight> other)
        {
            return other != null && Rule == other.Rule && BaseCorrespondence == other.BaseCorrespondence
                && EqualityComparer<TDepLeft>.Default.Equals(other.LeftValue, LeftValue) && EqualityComparer<TDepRight>.Default.Equals(other.RightValue, RightValue);
        }

        public void ResolveLeft()
        {
            var context = BaseCorrespondence.SynchronizationContext;
            var direction = context.Direction;
            try
            {
                context.Direction = SynchronizationDirection.RightToLeftForced;
                if (RightValue != null)
                {
                    var comp = context.CallTransformation(Rule.childRule.RightToLeft, new object[] { RightValue }, new Axiom(LeftValue))
                        as SynchronizationComputation<TDepRight, TDepLeft>;
                    comp.DoWhenOutputIsAvailable((inp, outp) => Rule.leftSetter(BaseCorrespondence.Input, context, outp));
                }
                else
                {
                    Rule.leftSetter(BaseCorrespondence.Input, context, null);
                }
            }
            finally
            {
                context.Direction = direction;
            }
        }

        public void ResolveRight()
        {
            var context = BaseCorrespondence.SynchronizationContext;
            var direction = context.Direction;
            try
            {
                context.Direction = SynchronizationDirection.LeftToRightForced;
                if (LeftValue != null)
                {
                    var comp = context.CallTransformation(Rule.childRule.LeftToRight, new object[] { LeftValue }, new Axiom(RightValue))
                        as SynchronizationComputation<TDepLeft, TDepRight>;
                    comp.DoWhenOutputIsAvailable((inp, outp) => Rule.rightSetter(BaseCorrespondence.Opposite.Input, context, outp));
                }
                else
                {
                    Rule.rightSetter(BaseCorrespondence.Opposite.Input, context, null);
                }
            }
            finally
            {
                context.Direction = direction;
            }
        }
    }
}
