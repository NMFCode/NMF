using NMF.Expressions;
using NMF.Synchronizations.Inconsistencies;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NMF.Synchronizations.Inconsistencies
{
    [DebuggerDisplay("{Representation}")]
    public class IncrementalPropertyConsistencyCheck<T> : IDisposable, IInconsistency
    {
        public INotifyReversableValue<T> SourceLeft { get; }
        public INotifyReversableValue<T> SourceRight { get; }
        public ISynchronizationContext Context { get; }

        public string Representation
        {
            get
            {
                return $"{SourceLeft} value {SourceLeft.Value} != {SourceRight} value {SourceRight.Value}";
            }
        }

        public bool CanResolveLeft => SourceLeft.IsReversable;

        public bool CanResolveRight => SourceRight.IsReversable;

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

        public void Dispose()
        {
            SourceLeft.ValueChanged -= Source_ValueChanged;
            SourceRight.ValueChanged -= Source_ValueChanged;
            SourceLeft.Successors.UnsetAll();
            SourceRight.Successors.UnsetAll();
        }

        public void ResolveLeft()
        {
            SourceLeft.Value = SourceRight.Value;
        }

        public void ResolveRight()
        {
            SourceRight.Value = SourceLeft.Value;
        }

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

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, this)) return true;
            if (obj is IncrementalPropertyConsistencyCheck<T> other) return Equals(other);
            return false;
        }

        public bool Equals(IInconsistency other)
        {
            return Equals(other as IncrementalPropertyConsistencyCheck<T>);
        }

        public bool Equals(IncrementalPropertyConsistencyCheck<T> other)
        {
            return other != null && other.Context == Context && other.SourceLeft == SourceLeft && other.SourceRight == SourceRight;
        }

        public override string ToString()
        {
            return Representation;
        }
    }
}
