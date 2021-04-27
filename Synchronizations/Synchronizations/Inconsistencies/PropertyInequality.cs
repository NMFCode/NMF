using NMF.Synchronizations.Inconsistencies;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NMF.Synchronizations
{
    /// <summary>
    /// Denotes an inconsistency that a property had different values
    /// </summary>
    /// <typeparam name="TLeft">The LHS context type</typeparam>
    /// <typeparam name="TRight">The RHS context type</typeparam>
    /// <typeparam name="TValue">The type of the property</typeparam>
    [DebuggerDisplay("{Representation}")]
    public class PropertyInequality<TLeft, TRight, TValue> : IInconsistency
    {
        /// <summary>
        /// Gets the LHS context element
        /// </summary>
        public TLeft LeftContext { get; }

        /// <summary>
        /// Gets the RHS context element
        /// </summary>
        public TRight RightContext { get; }

        /// <summary>
        /// Gets the LHS property value
        /// </summary>
        public TValue LeftValue { get; }

        /// <summary>
        /// Gets the RHS property value
        /// </summary>
        public TValue RightValue { get; }

        /// <summary>
        /// Gets a function that sets the value at the LHS
        /// </summary>
        public Action<TLeft, TValue> LeftSetter { get; }

        /// <summary>
        /// Gets a function that sets the value at the RHS
        /// </summary>
        public Action<TRight, TValue> RightSetter { get; }

        /// <inheritdoc />
        public bool CanResolveLeft => LeftSetter != null;

        /// <inheritdoc />
        public bool CanResolveRight => RightSetter != null;

        /// <summary>
        /// Gets a human-readable description of this inconsistency
        /// </summary>
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

        /// <inheritdoc />

        public void ResolveLeft()
        {
            LeftSetter(LeftContext, RightValue);
        }

        /// <inheritdoc />

        public void ResolveRight()
        {
            RightSetter(RightContext, LeftValue);
        }

        /// <inheritdoc />

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

        /// <inheritdoc />

        public override bool Equals(object obj)
        {
            if (ReferenceEquals( obj, this)) return true;
            if (obj is PropertyInequality<TLeft, TRight, TValue> other) return Equals(other);
            return false;
        }

        /// <inheritdoc />

        public bool Equals(IInconsistency other)
        {
            return Equals(other as PropertyInequality<TLeft, TRight, TValue>);
        }

        /// <inheritdoc />

        public bool Equals(PropertyInequality<TLeft, TRight, TValue> other)
        {
            return other != null && EqualityComparer<TLeft>.Default.Equals(other.LeftContext, LeftContext) && EqualityComparer<TRight>.Default.Equals(other.RightContext, RightContext)
                && other.LeftSetter == LeftSetter && other.RightSetter == RightSetter
                && EqualityComparer<TValue>.Default.Equals(other.LeftValue, LeftValue) && EqualityComparer<TValue>.Default.Equals(other.RightValue, RightValue);
        }

        /// <inheritdoc />

        public override string ToString()
        {
            return Representation;
        }
    }
}
