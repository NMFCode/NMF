using NMF.Synchronizations.Inconsistencies;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NMF.Synchronizations
{
    [DebuggerDisplay("{Representation}")]
    public class PropertyInequality<TLeft, TRight, TValue> : IInconsistency
    {
        public TLeft LeftContext { get; }

        public TRight RightContext { get; }

        public TValue LeftValue { get; }

        public TValue RightValue { get; }

        public Action<TLeft, TValue> LeftSetter { get; }

        public Action<TRight, TValue> RightSetter { get; }

        public bool CanResolveLeft => LeftSetter != null;

        public bool CanResolveRight => RightSetter != null;

        public string Representation
        {
            get
            {
                return $"{LeftValue} in {LeftContext} != {RightValue} in {RightContext}";
            }
        }

        public PropertyInequality(TLeft leftContext, Action<TLeft, TValue> leftSetter, TValue leftValue, TRight rightContext, Action<TRight, TValue> rightSetter, TValue rightValue)
        {
            LeftContext = leftContext;
            LeftValue = leftValue;
            LeftSetter = leftSetter;
            RightValue = rightValue;
            RightSetter = rightSetter;
            RightContext = rightContext;
        }            

        public void ResolveLeft()
        {
            LeftSetter(LeftContext, RightValue);
        }

        public void ResolveRight()
        {
            RightSetter(RightContext, LeftValue);
        }

        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if (LeftContext != null) hashCode = (23 * hashCode) + LeftContext.GetHashCode();
                if (LeftValue != null) hashCode = (23 * hashCode) + LeftValue.GetHashCode();
                if (LeftSetter != null) hashCode = (23 * hashCode) + LeftSetter.GetHashCode();
                if (RightValue != null) hashCode = (23 * hashCode) + RightValue.GetHashCode();
                if (RightSetter != null) hashCode = (23 * hashCode) + RightSetter.GetHashCode();
                if (RightContext != null) hashCode = (23 * hashCode) + RightContext.GetHashCode();
            }
            return hashCode;
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, this)) return true;
            if (obj is PropertyInequality<TLeft, TRight, TValue> other) return Equals(other);
            return false;
        }

        public bool Equals(IInconsistency other)
        {
            return Equals(other as PropertyInequality<TLeft, TRight, TValue>);
        }

        public bool Equals(PropertyInequality<TLeft, TRight, TValue> other)
        {
            return other != null && EqualityComparer<TLeft>.Default.Equals(other.LeftContext, LeftContext) && EqualityComparer<TRight>.Default.Equals(other.RightContext, RightContext)
                && other.LeftSetter == LeftSetter && other.RightSetter == RightSetter
                && EqualityComparer<TValue>.Default.Equals(other.LeftValue, LeftValue) && EqualityComparer<TValue>.Default.Equals(other.RightValue, RightValue);
        }

        public override string ToString()
        {
            return Representation;
        }
    }
}
