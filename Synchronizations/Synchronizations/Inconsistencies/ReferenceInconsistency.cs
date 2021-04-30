using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations.Core;
using System.Collections.Generic;

namespace NMF.Synchronizations.Inconsistencies
{
    /// <summary>
    /// Denotes an inconsistency in a reference
    /// </summary>
    public class ReferenceInconsistency<TLeft, TRight, TDepLeft, TDepRight> : IInconsistency
    {
        /// <summary>
        /// Gets the LHS dependent element
        /// </summary>
        public TDepLeft LeftValue { get; }

        /// <summary>
        /// Gets the RHS dependent element
        /// </summary>
        public TDepRight RightValue { get; }

        private SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> Rule { get; }

        /// <summary>
        /// The correspondence of elements on the basis of which the inconsistency was found
        /// </summary>
        public SynchronizationComputation<TLeft, TRight> BaseCorrespondence { get; }

        internal ReferenceInconsistency(SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> rule, SynchronizationComputation<TLeft, TRight> baseCorrespondence, TDepLeft leftValue, TDepRight rightValue)
        {
            Rule = rule;
            BaseCorrespondence = baseCorrespondence;
            LeftValue = leftValue;
            RightValue = rightValue;
        }


        /// <inheritdoc />
        public bool CanResolveLeft => Rule.leftSetter != null;

        /// <inheritdoc />
        public bool CanResolveRight => Rule.rightSetter != null;

        /// <inheritdoc />
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

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals( obj, this)) return true;
            if (obj is ReferenceInconsistency<TLeft, TRight, TDepLeft, TDepRight> other) return Equals(other);
            return false;
        }

        /// <inheritdoc />
        public bool Equals(IInconsistency other)
        {
            return Equals(other as ReferenceInconsistency<TLeft, TRight, TDepLeft, TDepRight>);
        }

        /// <inheritdoc />
        public bool Equals(ReferenceInconsistency<TLeft, TRight, TDepLeft, TDepRight> other)
        {
            return other != null && Rule == other.Rule && BaseCorrespondence == other.BaseCorrespondence
                && EqualityComparer<TDepLeft>.Default.Equals(other.LeftValue, LeftValue) && EqualityComparer<TDepRight>.Default.Equals(other.RightValue, RightValue);
        }

        /// <inheritdoc />
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
                    Rule.leftSetter(BaseCorrespondence.Input, context, default);
                }
            }
            finally
            {
                context.Direction = direction;
            }
        }

        /// <inheritdoc />
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
                    Rule.rightSetter(BaseCorrespondence.Opposite.Input, context, default);
                }
            }
            finally
            {
                context.Direction = direction;
            }
        }
    }
}
