using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Types;
using NMF.Utilities;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes the base class to describe the appearance of transitions
    /// </summary>
    /// <typeparam name="TTransition">The semantic element type of the transition</typeparam>
    public abstract class EdgeDescriptor<TTransition> : ElementDescriptor<TTransition>
    {
        private GEdgeSkeleton<TTransition> _skeleton;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public EdgeDescriptor()
        {
            Type(DefaultTypes.Edge);
        }

        internal override GElementSkeleton<TTransition> CreateSkeleton()
        {
            return _skeleton = new GEdgeSkeleton<TTransition>();
        }

        /// <summary>
        /// Sets the source of the edge represented by this descriptor
        /// </summary>
        /// <typeparam name="TSource">The semantic type of the source node</typeparam>
        /// <param name="descriptor">The descriptor to describe which source node should be used</param>
        /// <param name="selector">An expression to calculate the source node from the semantic model of the transition</param>
        /// <param name="canChangeSource">True, if the source element can be changed, otherwise False</param>
        protected void SourceNode<TSource>(NodeDescriptor<TSource> descriptor, Expression<Func<TTransition, object>> selector, bool canChangeSource = true)
        {
            _skeleton.SourceSkeleton = descriptor.CurrentSkeleton;
            _skeleton.SourceSelector = selector;
            _skeleton.CanChangeSource = canChangeSource;
        }

        /// <summary>
        /// Sets the target of the edge represented by this descriptor
        /// </summary>
        /// <typeparam name="TTarget">The semantic type of the target node</typeparam>
        /// <param name="descriptor">The descriptor to describe which target node should be used</param>
        /// <param name="selector">An expression to calculate the target element from the semantic model of the transition</param>
        /// <param name="canChangeTarget">True, if the target element can be changed, otherwise False</param>
        protected void TargetNode<TTarget>(NodeDescriptor<TTarget> descriptor, Expression<Func<TTransition, object>> selector, bool canChangeTarget = true)
        {
            _skeleton.TargetSkeleton = descriptor.CurrentSkeleton;
            _skeleton.SourceSelector = selector;
            _skeleton.CanChangeTarget = canChangeTarget;
        }

        /// <inheritdoc />
        protected internal override IEnumerable<TypeHint> CalculateTypeHints()
        {
            yield return new EdgeTypeHint
            {
                Deletable = true,
                Dynamic = false,
                ElementTypeId = _skeleton.ElementTypeId,
                Repositionable = true,
                Routable = true,
                SourceElementTypeIds = _skeleton.SourceSkeleton
                    .Closure(sk => sk.Refinements)
                    .Select(sk => sk.ElementTypeId)
                    .ToArray(),
                TargetElementTypeIds = _skeleton.TargetSkeleton
                    .Closure(sk => sk.Refinements)
                    .Select(sk => sk.ElementTypeId)
                    .ToArray(),
            };
        }
    }

    /// <summary>
    /// Denotes the base class to describe edges that are not represented by semantic elements
    /// </summary>
    /// <typeparam name="TSource">The semantic type of the edge source</typeparam>
    /// <typeparam name="TTarget">The semantic type of the edge target</typeparam>
    public abstract class EdgeDescriptor<TSource, TTarget> : EdgeDescriptor<(TSource, TTarget)>
    {
        /// <summary>
        /// Gets the descriptor used for the source of the edge
        /// </summary>
        public abstract NodeDescriptor<TSource> SourceDescriptor { get; }

        /// <summary>
        /// Gets the descriptor used for the target of the edge
        /// </summary>
        public abstract NodeDescriptor<TTarget> TargetDescriptor { get; }

        /// <inheritdoc />
        protected internal override void DefineLayout()
        {
            SourceNode(SourceDescriptor, pair => pair.Item1, false);
            TargetNode(TargetDescriptor, pair => pair.Item2, false);
        }
    }
}
