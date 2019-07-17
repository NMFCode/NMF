using NMF.Synchronizations.Inconsistencies;
using NMF.Transformations;
using NMF.Transformations.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace NMF.Synchronizations.Inconsistencies
{
    [DebuggerDisplay("{Representation}")]
    public class MissingItemInconsistency<TSource, TTarget> : IInconsistency where TSource : class where TTarget : class
    {
        /// <summary>
        /// Gets the context in which this inconsistency occured
        /// </summary>
        public ISynchronizationContext Context { get; }

        /// <summary>
        /// Gets the transformation rule required to fix the inconsistency
        /// </summary>
        public TransformationRuleBase<TSource, TTarget> Rule { get; }

        /// <summary>
        /// Gets the source collection where the element is contained
        /// </summary>
        public ICollection<TSource> SourceCollection { get; }

        /// <summary>
        /// Gets the target collection where the element is missing
        /// </summary>
        public ICollection<TTarget> TargetCollection { get; }

        /// <summary>
        /// Gets the source element that is missing
        /// </summary>
        public TSource Source { get; }

        /// <summary>
        /// True, if the element is missing in the left side, otherwise false
        /// </summary>
        public bool IsLeftMissing { get; }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Representation
        {
            get
            {
                return $"{Source} is present in {SourceCollection} but missing in {TargetCollection}";
            }
        }

        public MissingItemInconsistency(ISynchronizationContext context, TransformationRuleBase<TSource, TTarget> rule, ICollection<TSource> sourceCollection, ICollection<TTarget> targetCollection, TSource source, bool isLeftMissing)
        {
            this.Context = context;
            this.Rule = rule;
            this.SourceCollection = sourceCollection;
            this.TargetCollection = targetCollection;
            this.Source = source;
            this.IsLeftMissing = isLeftMissing;
        }

        /// <inheritdoc cref="IInconsistency" />
        public bool CanResolveLeft => IsLeftMissing ? !SourceCollection.IsReadOnly : !TargetCollection.IsReadOnly;


        /// <inheritdoc cref="IInconsistency" />
        public bool CanResolveRight => IsLeftMissing ? !TargetCollection.IsReadOnly : !SourceCollection.IsReadOnly;
        
        /// <inheritdoc cref="object" />
        public override int GetHashCode()
        {
            var hashCode = Rule.GetHashCode();
            unchecked
            {
                hashCode = (23 * hashCode) + SourceCollection.GetHashCode();
                hashCode = (23 * hashCode) + TargetCollection.GetHashCode();
                hashCode = (23 * hashCode) + Source.GetHashCode();
                hashCode = (23 * hashCode) + IsLeftMissing.GetHashCode();
            }
            return hashCode;
        }


        /// <inheritdoc cref="object" />
        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(obj, this)) return true;
            if (obj is MissingItemInconsistency<TSource, TTarget> other) return Equals(other);
            return false;
        }

        public bool Equals(IInconsistency other)
        {
            return Equals(other as MissingItemInconsistency<TSource, TTarget>);
        }

        private bool Equals(MissingItemInconsistency<TSource, TTarget> other)
        {
            return other != null && other.Rule == Rule && other.SourceCollection == SourceCollection && other.TargetCollection == TargetCollection && other.Source == Source && other.IsLeftMissing == IsLeftMissing;
        }

        /// <inheritdoc cref="IInconsistency" />
        public void ResolveLeft()
        {
            var direction = Context.Direction;
            try
            {
                Context.Direction = SynchronizationDirection.RightToLeftForced;
                if (IsLeftMissing)
                {
                    var comp = Context.CallTransformation(Rule, Source);
                    if (!comp.IsDelayed)
                    {
                        TargetCollection.Add(comp.Output as TTarget);
                    }
                    else
                    {
                        comp.OutputInitialized += (o, e) => TargetCollection.Add(comp.Output as TTarget);
                    }
                }
                else
                {
                    SourceCollection.Remove(Source);
                }
                Context.Inconsistencies.Remove(this);
            }
            finally
            {
                Context.Direction = direction;
            }
        }

        /// <inheritdoc cref="IInconsistency" />
        public void ResolveRight()
        {
            var direction = Context.Direction;
            try
            {
                Context.Direction = SynchronizationDirection.LeftToRightForced;
                if (IsLeftMissing)
                {
                    SourceCollection.Remove(Source);
                }
                else
                {
                    var comp = Context.CallTransformation(Rule, Source);
                    if (!comp.IsDelayed)
                    {
                        TargetCollection.Add(comp.Output as TTarget);
                    }
                    else
                    {
                        comp.OutputInitialized += (o, e) => TargetCollection.Add(comp.Output as TTarget);
                    }
                }
                Context.Inconsistencies.Remove(this);
            }
            finally
            {
                Context.Direction = direction;
            }
        }
    }
}
