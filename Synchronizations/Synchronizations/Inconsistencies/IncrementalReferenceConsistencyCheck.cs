using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations;
using NMF.Transformations.Core;
using System;

namespace NMF.Synchronizations.Inconsistencies
{
    public class IncrementalReferenceConsistencyCheck<TLeft, TRight, TDepLeft, TDepRight> : IDisposable, IInconsistency
        where TLeft : class
        where TRight : class
        where TDepLeft : class
        where TDepRight : class
    {
        public INotifyReversableValue<TDepLeft> Left { get; private set; }
        public INotifyReversableValue<TDepRight> Right { get; private set; }
        internal SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> Parent { get; private set; }
        public SynchronizationComputation<TLeft, TRight> Computation { get; private set; }

        public bool CanResolveLeft => Left.IsReversable;

        public bool CanResolveRight => Right.IsReversable;

        private bool isProcessingChange = false;

        internal IncrementalReferenceConsistencyCheck(INotifyReversableValue<TDepLeft> left, INotifyReversableValue<TDepRight> right, SynchronizationComputation<TLeft, TRight> computation, SynchronizationSingleDependency<TLeft, TRight, TDepLeft, TDepRight> parent)
        {
            Left = left;
            Right = right;
            Parent = parent;
            Computation = computation;

            Left.ValueChanged += Left_ValueChanged;
            Right.ValueChanged += Right_ValueChanged;

            if (Computation.SynchronizationContext.Direction == SynchronizationDirection.CheckOnly
                && Computation.TransformationContext.Trace.ResolveIn(Parent.childRule.LeftToRight, Left.Value) != Right.Value)
            {
                Computation.SynchronizationContext.Inconsistencies.Add(this);
            }
        }

        private void Right_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            ResolveLeft();
        }

        private void Left_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            ResolveRight();
        }

        public void Dispose()
        {
            Left.ValueChanged -= Left_ValueChanged;
            Right.ValueChanged -= Right_ValueChanged;
            Left.Dispose();
            Right.Dispose();
        }

        public void ResolveLeft()
        {
            if (!isProcessingChange)
            {
                var context = Computation.SynchronizationContext;
                var direction = context.Direction;
                context.Direction = SynchronizationDirection.RightToLeftForced;
                isProcessingChange = true;
                try
                {
                    Action<TLeft, ITransformationContext, TDepLeft> leftSetter = (l, _, val) => Left.Value = val;
                    Action<TRight, ITransformationContext, TDepRight> rightSetter = (r, _, val) => Right.Value = val;

                    Parent.CallRTLTransformationForInput(Computation.Opposite, SynchronizationDirection.RightToLeftForced, Right.Value, Left.Value, leftSetter, rightSetter);

                }
                finally
                {
                    isProcessingChange = false;
                    context.Direction = direction;
                }
            }
        }

        public void ResolveRight()
        {
            if (!isProcessingChange)
            {
                isProcessingChange = true;
                try
                {
                    Action<TLeft, ITransformationContext, TDepLeft> leftSetter = ( l, _, val ) => Left.Value = val;
                    Action<TRight, ITransformationContext, TDepRight> rightSetter = ( r, _, val ) => Right.Value = val;

                    Parent.CallLTRTransformationForInput(Computation, SynchronizationDirection.LeftToRightForced, Left.Value, Right.Value, leftSetter, rightSetter);

                }
                finally
                {
                    isProcessingChange = false;
                }
            }
        }

        public override int GetHashCode()
        {
            var hashCode = Parent.GetHashCode();
            unchecked
            {
                hashCode = (23 * hashCode) + Computation.GetHashCode();
                if (Left != null) hashCode = (23 * hashCode) + Left.GetHashCode();
                if (Right != null) hashCode = (23 * hashCode) + Right.GetHashCode();
            }
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, this)) return true;
            if (obj is IncrementalReferenceConsistencyCheck<TLeft, TRight, TDepLeft, TDepRight> other) return Equals(other);
            return false;
        }

        public bool Equals(IInconsistency other)
        {
            return Equals(other as IncrementalReferenceConsistencyCheck<TLeft, TRight, TDepLeft, TDepRight>);
        }

        public bool Equals(IncrementalReferenceConsistencyCheck<TLeft, TRight, TDepLeft, TDepRight> other)
        {
            return other != null && Parent == other.Parent && Computation == other.Computation
                && other.Left == Left && other.Right == Right;
        }

    }
}
