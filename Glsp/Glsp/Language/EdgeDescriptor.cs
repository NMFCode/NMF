using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Types;
using NMF.Models;
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
        }

        /// <summary>
        /// Gets the router kind
        /// </summary>
        protected internal virtual RouterKind RouterKind => RouterKind.None;

        internal override GElementSkeleton<TTransition> CreateSkeleton()
        {
            return _skeleton = new GEdgeSkeleton<TTransition>(this);
        }

        /// <summary>
        /// Sets the source of the edge represented by this descriptor
        /// </summary>
        /// <typeparam name="TSource">The semantic type of the source node</typeparam>
        /// <param name="descriptor">The descriptor to describe which source node should be used</param>
        /// <param name="selector">An expression to calculate the source node from the semantic model of the transition</param>
        /// <param name="canChangeSource">True, if the source element can be changed, otherwise False</param>
        protected void SourceNode<TSource>(NodeDescriptor<TSource> descriptor, Expression<Func<TTransition, TSource>> selector, bool canChangeSource = true)
        {
            _skeleton.Source = new EdgeHelper<TTransition, TSource>
            {
                Selector = selector,
                Skeleton = descriptor.CurrentSkeleton,
                CanChange = canChangeSource
            };
        }

        /// <summary>
        /// Sets the target of the edge represented by this descriptor
        /// </summary>
        /// <typeparam name="TTarget">The semantic type of the target node</typeparam>
        /// <param name="descriptor">The descriptor to describe which target node should be used</param>
        /// <param name="selector">An expression to calculate the target element from the semantic model of the transition</param>
        /// <param name="canChangeTarget">True, if the target element can be changed, otherwise False</param>
        protected void TargetNode<TTarget>(NodeDescriptor<TTarget> descriptor, Expression<Func<TTransition, TTarget>> selector, bool canChangeTarget = true)
        {
            _skeleton.Target = new EdgeHelper<TTransition, TTarget>
            {
                Selector = selector,
                Skeleton = descriptor.CurrentSkeleton,
                CanChange = canChangeTarget
            };
        }

        /// <summary>
        /// Specifies that a GLabel element should be created under the current node
        /// </summary>
        /// <param name="labelSelector">An expression calculating the text of the label</param>
        /// <param name="type">The GElement type of the label</param>
        /// <param name="canEdit">True, if the label can be added, otherwise False</param>
        /// <param name="guard">An expression to guard the visibility of the label, or null</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected IEdgeLabelSyntax<TTransition> Label(Expression<Func<TTransition, string>> labelSelector, string type = "label", bool canEdit = true, Expression<Func<TTransition, bool>> guard = null)
        {
            var skeleton = new GLabelSkeleton<TTransition>(this)
            {
                Type = type,
                LabelValue = labelSelector,
                CanEdit = canEdit,
                EdgeLabelPlacement = new EdgeLabelPlacement(0.5, false, "on", "free", null)
            };
            var contribution = new CompartmentContribution<TTransition> { CompartmentSkeleton = skeleton, Guard = guard };
            CurrentSkeleton.NodeContributions.Add(contribution);
            return new EdgeLabelSyntax<TTransition>(skeleton, contribution);
        }

        /// <inheritdoc />
        protected internal override IEnumerable<TypeHint> CalculateTypeHints()
        {
            yield return new EdgeTypeHint
            {
                Deletable = true,
                Dynamic = false,
                ElementTypeId = ElementTypeId,
                Repositionable = true,
                Routable = true,
                SourceElementTypeIds = _skeleton.Source.Skeleton
                    .Closure(sk => sk.Refinements)
                    .Select(sk => sk.ElementTypeId)
                    .Where(t => t != null)
                    .ToArray(),
                TargetElementTypeIds = _skeleton.Target.Skeleton
                    .Closure(sk => sk.Refinements)
                    .Select(sk => sk.ElementTypeId)
                    .Where(t => t != null)
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
        /// <inheritdoc />
        public override string ToolLabel(string profile) => $"Connect {ModelHelper.ImplementationType<TSource>().Name} to {ModelHelper.ImplementationType<TTarget>().Name}";

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
            Type($"edge{SourceDescriptor.ElementTypeId}To{TargetDescriptor.ElementTypeId}");
            SourceNode(SourceDescriptor, pair => pair.Item1, false);
            TargetNode(TargetDescriptor, pair => pair.Item2, false);
        }

        /// <inheritdoc />
        public override (TSource, TTarget) CreateElement(string profile, object parent)
        {
            return (default, default);
        }

        /// <inheritdoc />
        public override bool CanCreateElement => true;
    }
}
