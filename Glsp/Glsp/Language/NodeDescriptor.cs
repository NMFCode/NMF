using NMF.Glsp.Processing;
using System.Linq.Expressions;
using System;
using NMF.Glsp.Protocol.Types;
using System.Collections.Generic;
using System.Linq;
using NMF.Expressions;
using NMF.Glsp.Protocol.Layout;
using NMF.Glsp.Language.Layouting;
using NMF.Models;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes the basic type to describe the appearance of elements as nodes
    /// </summary>
    /// <typeparam name="T">The semantic element type</typeparam>
    public abstract class NodeDescriptor<T> : ElementDescriptor<T>
    {
        internal readonly Stack<GElementSkeleton<T>> _skeletons = new Stack<GElementSkeleton<T>>();
        internal override GElementSkeleton<T> CreateSkeleton() => new GNodeSkeleton<T>(this);

        /// <summary>
        /// Creates a new instance
        /// </summary>
        protected NodeDescriptor()
        {
            _skeletons.Push(_baseSkeleton);
        }

        internal override GElementSkeleton<T> CurrentSkeleton => _skeletons.Peek();

        /// <summary>
        /// Sets the initial size of the node
        /// </summary>
        /// <param name="width">The width of the node in pixel</param>
        /// <param name="height">The height of the node in pixel</param>
        /// <remarks>This method is intended to be used to create a using block inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Size(double width, double height)
        {
            CurrentSkeleton.Dimension = new Dimension(width, height);
        }

        /// <summary>
        /// Sets the layout of this node
        /// </summary>
        /// <param name="layout"></param>
        /// <remarks>This method is intended to be used to create a using block inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Layout(LayoutStrategy layout)
        {
            CurrentSkeleton.LayoutStrategy = layout ?? CurrentSkeleton.LayoutStrategy;
        }

        /// <summary>
        /// Creates a new compartment for the nodes represented by this semantic element
        /// </summary>
        /// <param name="type">The GElement type for the compartment</param>
        /// <param name="layoutStrategy">The layout strategy for the compartment or Vbox, if nothing is specified</param>
        /// <param name="guard">A predicate expression to control the creation of this compartment</param>
        /// <returns>A disposable that can be disposed to return to the parent element</returns>
        /// <remarks>This method is intended to be used to create a using block inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected IDisposable Compartment(string type = DefaultTypes.Compartment, LayoutStrategy layoutStrategy = null, Expression<Func<T, bool>> guard = null)
        {
            var skeleton = new GCompartmentSkeleton<T>(this)
            {
                Type = type,
                LayoutStrategy = layoutStrategy ?? LayoutStrategy.Vbox
            };
            CurrentSkeleton.NodeContributions.Add(new CompartmentContribution<T> { CompartmentSkeleton = skeleton, Guard = guard });
            _skeletons.Push(skeleton);            
            return new InnerCompartment(this);
        }

        /// <summary>
        /// Embeds another rule into the current node descriptor
        /// </summary>
        /// <param name="innerDescriptor">The inner description</param>
        /// <param name="guard">A predicate expression to control the creation of this compartment</param>
        /// <remarks>This method is intended to be used to create a using block inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Embed(NodeDescriptor<T> innerDescriptor, Expression<Func<T, bool>> guard = null)
        {
            CurrentSkeleton.NodeContributions.Add(new CompartmentContribution<T>
            {
                CompartmentSkeleton = innerDescriptor._baseSkeleton,
                Guard = guard
            });
        }

        /// <summary>
        /// Specifies that a GLabel element should be created under the current node with a static text
        /// </summary>
        /// <param name="text">The text to display</param>
        /// <param name="type">The GElement type of the label</param>
        /// <param name="guard">An expression to guard the visibility of the label, or null</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Label(string text, string type = "label:static", Expression<Func<T, bool>> guard = null)
        {
            Label(_ => text, type, canEdit: false, guard: guard);
        }

        /// <summary>
        /// Specifies that a GLabel element should be created under the current node
        /// </summary>
        /// <param name="labelSelector">An expression calculating the text of the label</param>
        /// <param name="type">The GElement type of the label</param>
        /// <param name="canEdit">True, if the label can be added, otherwise False</param>
        /// <param name="guard">An expression to guard the visibility of the label, or null</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected INodeLabelSyntax<T> Label(Expression<Func<T, string>> labelSelector, string type = "label", bool canEdit = true, Expression<Func<T, bool>> guard = null)
        {
            var skeleton = new GLabelSkeleton<T>(this)
            {
                Type = type,
                LabelValue = labelSelector,
                CanEdit = canEdit
            };
            var contribution = new CompartmentContribution<T> { CompartmentSkeleton = skeleton, Guard = guard };
            CurrentSkeleton.NodeContributions.Add(contribution);
            return new NodeLabelSyntax<T>(skeleton, contribution);
        }

        /// <summary>
        /// Specifies that nodes should be created as subnodes of the given descriptor
        /// </summary>
        /// <typeparam name="TOther">The semantic type of the dependent elements</typeparam>
        /// <param name="targetDescriptor">The node descriptor describing the sub-elements</param>
        /// <param name="selector">A function to obtain a collection of semantic elements</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected IChildSyntax Nodes<TOther>(NodeDescriptor<TOther> targetDescriptor, Func<T, ICollectionExpression<TOther>> selector)
        {
            var contribution = new NodeCollectionContribution<T, TOther>
            {
                Selector = selector,
                Skeleton = targetDescriptor._baseSkeleton
            };
            CurrentSkeleton.NodeContributions.Add(contribution);
            return new ChildSyntax(contribution);
        }

        /// <summary>
        /// Specifies that labels should be created as subnodes of the given descriptor
        /// </summary>
        /// <typeparam name="TOther">The semantic type of the dependent elements</typeparam>
        /// <param name="targetDescriptor">The node descriptor describing the sub-elements</param>
        /// <param name="selector">A function to obtain a collection of semantic elements</param>
        /// <param name="includeInSelection">True, if the label elements should also be included in the selection, otherwise false</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected IChildSyntax Labels<TOther>(LabelDescriptor<TOther> targetDescriptor, Func<T, ICollectionExpression<TOther>> selector, bool includeInSelection = true)
        {
            ArgumentNullException.ThrowIfNull(targetDescriptor);
            ArgumentNullException.ThrowIfNull(selector);

            var contribution = new NodeCollectionContribution<T, TOther>
            {
                Selector = selector,
                Skeleton = targetDescriptor._baseSkeleton
            };
            CurrentSkeleton.NodeContributions.Add(contribution);
            if (includeInSelection)
            {
                SelectionExtensions.Add(e => selector(e).OfType<IModelElement>());
            }
            return new ChildSyntax(contribution);
        }

        /// <summary>
        /// Specifies that the nodes should contain edges
        /// </summary>
        /// <typeparam name="TTransition">The semantic type of edges</typeparam>
        /// <param name="edgeDescriptor">A descriptor for the edges</param>
        /// <param name="selector">A function to calculate a collection of edges to create</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected IChildSyntax Edges<TTransition>(EdgeDescriptor<TTransition> edgeDescriptor, Func<T, ICollectionExpression<TTransition>> selector)
        {
            var contribution = new EdgeCollectionContribution<T, TTransition>
            {
                Selector = selector,
                Skeleton = edgeDescriptor._baseSkeleton,
                EdgeDescriptor = edgeDescriptor
            };
            CurrentSkeleton.EdgeContributions.Add(contribution);
            return new ChildSyntax(contribution);
        }

        /// <summary>
        /// Specifies that the nodes should contain edges
        /// </summary>
        /// <typeparam name="TSource">The semantic type of source nodes</typeparam>
        /// <typeparam name="TTarget">The semantic type of target nodes</typeparam>
        /// <param name="edgeDescriptor">A descriptor of edges from sources to the target</param>
        /// <param name="selector">A function to calculate pairs of source and target node</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected IChildSyntax Edges<TSource, TTarget>(EdgeDescriptor<TSource, TTarget> edgeDescriptor, Func<T, ICollectionExpression<(TSource, TTarget)>> selector)
        {
            var contribution = new EdgeCollectionContribution<T, TSource, TTarget>
            {
                Selector = selector,
                Skeleton = edgeDescriptor._baseSkeleton,
                EdgeDescriptor = edgeDescriptor
            };
            CurrentSkeleton.EdgeContributions.Add(contribution);
            return new ChildSyntax(contribution);
        }

        /// <summary>
        /// Specifies that the nodes should contain edges
        /// </summary>
        /// <typeparam name="TSource">The semantic type of source nodes</typeparam>
        /// <typeparam name="TTarget">The semantic type of target nodes</typeparam>
        /// <param name="sourceDescriptor">The descriptor for the source nodes</param>
        /// <param name="targetDescriptor">The descriptor for the target nodes</param>
        /// <param name="selector">A function to calculate pairs of source and target node</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected IAdhocEdgeSyntax Edges<TSource, TTarget>(NodeDescriptor<TSource> sourceDescriptor, NodeDescriptor<TTarget> targetDescriptor, Func<T, ICollectionExpression<(TSource, TTarget)>> selector)
        {
            var descriptor = new AdHocEdgeDescriptor<TSource, TTarget>(sourceDescriptor, targetDescriptor);
            Language.AddRule(descriptor);
            var contribution = new EdgeCollectionContribution<T, TSource, TTarget>
            {
                Selector = selector,
                Skeleton = descriptor._baseSkeleton,
                EdgeDescriptor = descriptor
            };
            CurrentSkeleton.EdgeContributions.Add(contribution);
            return descriptor.CreateSyntax(contribution);
        }

        /// <summary>
        /// Sets the layout options for this node
        /// </summary>
        /// <param name="layoutOptions">layout options</param>
        protected void LayoutOptions(LayoutOptions layoutOptions)
        {
            if (layoutOptions != null)
            {
                Forward("layoutOptions", layoutOptions);
            }
        }

        /// <summary>
        /// Sets options for rounded corners
        /// </summary>
        /// <param name="roundedCornerOptions">Options for rounded corners</param>
        protected void RoundCorners(RoundedCornerOptions roundedCornerOptions)
        {
            if (roundedCornerOptions != null)
            {
                Forward("args", roundedCornerOptions);
            }
        }

        /// <summary>
        /// Denotes whether this node can be deleted
        /// </summary>
        protected virtual bool IsDeletable => true;

        /// <summary>
        /// Denotes whether this node can be resized
        /// </summary>
        protected virtual bool IsResizable => true;

        /// <inheritdoc/>
        protected internal override IEnumerable<TypeHint> CalculateTypeHints()
        {
            if (!CanCreateElement)
            {
                yield break;
            }

            yield return new ShapeTypeHint
            {
                ElementTypeId = _baseSkeleton.ElementTypeId,
                Reparentable = true,
                Deletable = true,
                Repositionable = true,
                Resizable = true,
                ContainableElementTypeIds = _baseSkeleton.ContainableTypeIds().ToArray()
            };
        }

        private sealed class InnerCompartment : IDisposable
        {
            private readonly NodeDescriptor<T> _nodeDescriptor;

            public InnerCompartment(NodeDescriptor<T> nodeDescriptor)
            {
                _nodeDescriptor = nodeDescriptor;
            }

            public void Dispose()
            {
                _nodeDescriptor._skeletons.Pop();
            }
        }
    }
}
