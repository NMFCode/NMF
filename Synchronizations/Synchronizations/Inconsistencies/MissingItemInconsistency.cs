using NMF.Transformations;
using NMF.Transformations.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace NMF.Synchronizations.Inconsistencies
{
    /// <summary>
    /// Denotes the inconsistency that an item is missing in a synchronized collection
    /// </summary>
    /// <typeparam name="TValue">The type of the item</typeparam>
    [DebuggerDisplay( "{Representation}" )]
    public class MissingItemInconsistency<TValue> : IInconsistency
    {
        /// <summary>
        /// Gets the context in which this inconsistency occured
        /// </summary>
        public ISynchronizationContext Context { get; }

        /// <summary>
        /// Gets the source collection where the element is contained
        /// </summary>
        public ICollection<TValue> SourceCollection { get; }

        /// <summary>
        /// Gets the target collection where the element is missing
        /// </summary>
        public ICollection<TValue> TargetCollection { get; }

        /// <summary>
        /// Gets the source element that is missing
        /// </summary>
        public TValue Source { get; }

        /// <summary>
        /// True, if the element is missing in the left side, otherwise false
        /// </summary>
        public bool IsLeftMissing { get; }

        /// <summary>
        /// Gets a human-readable summary of the inconsistency
        /// </summary>
        [Browsable( false )]
        [EditorBrowsable( EditorBrowsableState.Never )]
        public string Representation
        {
            get
            {
                return $"{Source} is present in {SourceCollection} but missing in {TargetCollection}";
            }
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="context">the context in which the inconsistency occured</param>
        /// <param name="sourceCollection">the source collection</param>
        /// <param name="targetCollection">the target collection</param>
        /// <param name="source">the source</param>
        /// <param name="isLeftMissing">true, if the item is missing left, otherwise false</param>
        public MissingItemInconsistency( ISynchronizationContext context, ICollection<TValue> sourceCollection, ICollection<TValue> targetCollection, TValue source, bool isLeftMissing )
        {
            this.Context = context;
            this.SourceCollection = sourceCollection;
            this.TargetCollection = targetCollection;
            this.Source = source;
            this.IsLeftMissing = isLeftMissing;
        }

        /// <inheritdoc cref="IInconsistency" />
        public bool CanResolveLeft => IsLeftMissing
            ? SourceCollection != null && !SourceCollection.IsReadOnly
            : TargetCollection != null && !TargetCollection.IsReadOnly;


        /// <inheritdoc cref="IInconsistency" />
        public bool CanResolveRight => IsLeftMissing
            ? TargetCollection != null && !TargetCollection.IsReadOnly
            : SourceCollection != null && !SourceCollection.IsReadOnly;

        /// <inheritdoc cref="object" />
        public override int GetHashCode()
        {
            var hashCode = 0;
            unchecked
            {
                if(SourceCollection != null) hashCode = (23 * hashCode) + SourceCollection.GetHashCode();
                if(TargetCollection != null) hashCode = (23 * hashCode) + TargetCollection.GetHashCode();
                if(Source != null) hashCode = (23 * hashCode) + Source.GetHashCode();
                hashCode = (23 * hashCode) + IsLeftMissing.GetHashCode();
            }
            return hashCode;
        }


        /// <inheritdoc cref="object" />
        public override bool Equals( object obj )
        {
            if(ReferenceEquals( obj, this )) return true;
            if(obj is MissingItemInconsistency<TValue> other) return Equals( other );
            return false;
        }

        /// <inheritdoc />
        public bool Equals( IInconsistency other )
        {
            return Equals( other as MissingItemInconsistency<TValue> );
        }

        private bool Equals( MissingItemInconsistency<TValue> other )
        {
            return other != null && other.SourceCollection == SourceCollection && other.TargetCollection == TargetCollection && EqualityComparer<TValue>.Default.Equals(other.Source, Source) && other.IsLeftMissing == IsLeftMissing;
        }

        /// <inheritdoc cref="IInconsistency" />
        public void ResolveLeft()
        {
            if(IsLeftMissing)
            {
                TargetCollection.Add( Source );
            }
            else
            {
                SourceCollection.Remove( Source );
            }
            Context.Inconsistencies.Remove( this );
        }

        /// <inheritdoc cref="IInconsistency" />
        public void ResolveRight()
        {
            if(IsLeftMissing)
            {
                SourceCollection.Remove( Source );
            }
            else
            {
                TargetCollection.Add( Source );
            }
        }
    }

    /// <summary>
    /// Denotes the inconsistency that an element is missing in a synchronized collection
    /// </summary>
    /// <typeparam name="TSource">The LHS type of elements</typeparam>
    /// <typeparam name="TTarget">The RHS type of elements</typeparam>
    [DebuggerDisplay("{Representation}")]
    public class MissingItemInconsistency<TSource, TTarget> : IInconsistency
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

        /// <summary>
        /// A human-readable representation of the inconsistency
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string Representation
        {
            get
            {
                return $"{Source} is present in {SourceCollection} but missing in {TargetCollection}";
            }
        }

        /// <summary>
        /// Creates a new inconsistency
        /// </summary>
        /// <param name="context">The context in which the inconsistency was found</param>
        /// <param name="rule">The synchronization rule for which the inconsistency was found</param>
        /// <param name="sourceCollection">The source collection of elements</param>
        /// <param name="targetCollection">The target collection of elements</param>
        /// <param name="source">The element that is missing</param>
        /// <param name="isLeftMissing">True, if the element is missing in the LHS, otherwise false</param>
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
                if (SourceCollection != null) hashCode = (23 * hashCode) + SourceCollection.GetHashCode();
                if (TargetCollection != null) hashCode = (23 * hashCode) + TargetCollection.GetHashCode();
                if (Source != null) hashCode = (23 * hashCode) + Source.GetHashCode();
                hashCode = (23 * hashCode) + IsLeftMissing.GetHashCode();
            }
            return hashCode;
        }


        /// <inheritdoc cref="object" />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals( obj, this)) return true;
            if (obj is MissingItemInconsistency<TSource, TTarget> other) return Equals(other);
            return false;
        }

        /// <inheritdoc />
        public bool Equals(IInconsistency other)
        {
            return Equals(other as MissingItemInconsistency<TSource, TTarget>);
        }

        private bool Equals(MissingItemInconsistency<TSource, TTarget> other)
        {
            return other != null && other.Rule == Rule && other.SourceCollection == SourceCollection && other.TargetCollection == TargetCollection && EqualityComparer<TSource>.Default.Equals( other.Source, Source) && other.IsLeftMissing == IsLeftMissing;
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
                        TargetCollection.Add((TTarget)comp.Output);
                    }
                    else
                    {
                        comp.OutputInitialized += (o, e) => TargetCollection.Add((TTarget)comp.Output);
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
                        TargetCollection.Add((TTarget)comp.Output);
                    }
                    else
                    {
                        comp.OutputInitialized += (o, e) => TargetCollection.Add((TTarget)comp.Output);
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
