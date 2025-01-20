using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NMF.Synchronizations.Inconsistencies
{
    /// <summary>
    /// Denotes an incrementally maintained inconsistency that a property has different values in LHS and RHS
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DebuggerDisplay("{Representation}")]
    public class IncrementalPropertyConsistencyCheck<T> : IDisposable, IInconsistency
    {
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

        /// <summary>
        /// Creates a new inconsistency
        /// </summary>
        /// <param name="source1">The LHS source</param>
        /// <param name="source2">The RHS source</param>
        /// <param name="context">The context in which the inconsistency arose</param>
        public IncrementalPropertyConsistencyCheck(INotifyReversableValue<T> source1, INotifyReversableValue<T> source2, ISynchronizationContext context)
        {
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
            if (obj is IncrementalPropertyConsistencyCheck<T> other) return Equals(other);
            return false;
        }

        /// <inheritdoc />
        public bool Equals(IInconsistency other)
        {
            return Equals(other as IncrementalPropertyConsistencyCheck<T>);
        }

        /// <inheritdoc />
        public bool Equals(IncrementalPropertyConsistencyCheck<T> other)
        {
            return other != null && other.Context == Context && other.SourceLeft == SourceLeft && other.SourceRight == SourceRight;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Representation;
        }
    }
}
