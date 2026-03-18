using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NMF.Synchronizations.Inconsistencies
{
    /// <summary>
    /// Denotes an incrementally maintained inconsistency that a property has different values in LHS and RHS
    /// </summary>
    /// <typeparam name="T">the type of values</typeparam>
    /// <typeparam name="TLeft">the type of the left context</typeparam>
    /// <typeparam name="TRight">the type of the right context</typeparam>
    [DebuggerDisplay("{Representation}")]
    public class IncrementalPropertyConsistencyCheck<TLeft, TRight, T> : IDisposable, IInconsistency
    {
        private class DefaultDescriptor : IInconsistencyDescriptor<TLeft, TRight, T, T>
        {
            public string DescribeLeft(TLeft left, TRight right, T depLeft, T depRight)
            {
                return $"Apply value '{depRight}' to {left}";
            }

            public string DescribeRight(TLeft left, TRight right, T depLeft, T depRight)
            {
                return $"Apply value '{depLeft}' to {right}";
            }
        }

        private static readonly DefaultDescriptor _default = new DefaultDescriptor();

        /// <summary>
        /// Gets an incrementally maintained value for the LHS property value
        /// </summary>
        public INotifyReversableValue<T> SourceLeft { get; }

        /// <summary>
        /// Gets an incrementally maintained value for the RHS property value
        /// </summary>
        public INotifyReversableValue<T> SourceRight { get; }

        /// <summary>
        /// Gets the synchronization context in which the inconsistency was found
        /// </summary>
        public ISynchronizationContext Context { get; }

        /// <summary>
        /// Gets a human-readable description of the inconsistency
        /// </summary>
        public string Representation
        {
            get
            {
                return $"{SourceLeft} value {SourceLeft.Value} != {SourceRight} value {SourceRight.Value}";
            }
        }

        /// <inheritdoc />
        public bool CanResolveLeft => SourceLeft.IsReversable;

        /// <inheritdoc />
        public bool CanResolveRight => SourceRight.IsReversable;

        private readonly TLeft _left;
        private readonly TRight _right;

        /// <inheritdoc />
        public object LeftElement => _left;

        /// <inheritdoc />
        public object RightElement => _right;

        private readonly IInconsistencyDescriptor<TLeft, TRight, T, T> _descriptor;

        /// <summary>
        /// Creates a new inconsistency
        /// </summary>
        /// <param name="left">the left element</param>
        /// <param name="right">the right element</param>
        /// <param name="descriptor">a descriptor</param>
        /// <param name="source1">The LHS source</param>
        /// <param name="source2">The RHS source</param>
        /// <param name="context">The context in which the inconsistency arose</param>
        public IncrementalPropertyConsistencyCheck(TLeft left, TRight right, IInconsistencyDescriptor<TLeft, TRight, T, T> descriptor, INotifyReversableValue<T> source1, INotifyReversableValue<T> source2, ISynchronizationContext context)
        {
            _left = left;
            _right = right;
            _descriptor = descriptor ?? _default;

            SourceLeft = source1;
            SourceRight = source2;

            SourceLeft.Successors.SetDummy();
            SourceRight.Successors.SetDummy();
            SourceLeft.ValueChanged += Source_ValueChanged;
            SourceRight.ValueChanged += Source_ValueChanged;

            Context = context;

            if (!EqualityComparer<T>.Default.Equals(SourceLeft.Value, SourceRight.Value))
            {
                Context.Inconsistencies.Add(this);
            }
        }

        private void Source_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (EqualityComparer<T>.Default.Equals(SourceLeft.Value, SourceRight.Value))
            {
                Context.Inconsistencies.Remove(this);
            }
            else
            {
                Context.Inconsistencies.Add(this);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            SourceLeft.ValueChanged -= Source_ValueChanged;
            SourceRight.ValueChanged -= Source_ValueChanged;
            SourceLeft.Successors.UnsetAll();
            SourceRight.Successors.UnsetAll();
        }

        /// <inheritdoc />
        public void ResolveLeft()
        {
            SourceLeft.Value = SourceRight.Value;
        }

        /// <inheritdoc />
        public void ResolveRight()
        {
            SourceRight.Value = SourceLeft.Value;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hashCode = Context.GetHashCode();
            unchecked
            {
                hashCode = (23 * hashCode) + SourceLeft.GetHashCode();
                hashCode = (23 * hashCode) + SourceRight.GetHashCode();
            }
            return hashCode;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals( obj, this)) return true;
            if (obj is IncrementalPropertyConsistencyCheck<TLeft, TRight, T> other) return Equals(other);
            return false;
        }

        /// <inheritdoc />
        public bool Equals(IInconsistency other)
        {
            return Equals(other as IncrementalPropertyConsistencyCheck<TLeft, TRight, T>);
        }

        /// <inheritdoc />
        public bool Equals(IncrementalPropertyConsistencyCheck<TLeft, TRight, T> other)
        {
            return other != null && other.Context == Context && other.SourceLeft == SourceLeft && other.SourceRight == SourceRight;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Representation;
        }

        /// <inheritdoc />
        public string DescribeLeft()
        {
            return _descriptor.DescribeLeft(_left, _right, SourceLeft.Value, SourceRight.Value);
        }

        /// <inheritdoc />
        public string DescribeRight()
        {
            return _descriptor.DescribeRight(_left, _right, SourceLeft.Value, SourceRight.Value);
        }
    }
}
