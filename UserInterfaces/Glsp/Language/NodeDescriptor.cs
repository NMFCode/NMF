using NMF.Expressions;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes a descriptor for nodes in the diagram
    /// </summary>
    /// <typeparam name="T">The semantic type of elements described by this descriptor</typeparam>
    public abstract class NodeDescriptor<T> : DescriptorBase
    {
        private readonly Stack<GElementSkeleton<T>> _skeletons = new Stack<GElementSkeleton<T>>();
        private readonly GElementSkeleton<T> _baseSkeleton;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public NodeDescriptor()
        {
            _baseSkeleton = CreateSkeleton();
            _skeletons.Push(_baseSkeleton);
        }

        internal virtual GElementSkeleton<T> CreateSkeleton() => new GNodeSkeleton<T>();

        internal GElementSkeleton<T> CurrentSkeleton => _skeletons.Peek();

        /// <summary>
        /// Creates a new compartment for the nodes represented by this semantic element
        /// </summary>
        /// <param name="type">The GElement type for the compartment</param>
        /// <param name="layout">The default layout for this compartment, e.g. hbox or vbox</param>
        /// <param name="guard">A predicate expression to control the creation of this compartment</param>
        /// <returns>A disposable that can be disposed to return to the parent element</returns>
        /// <remarks>This method is intended to be used to create a using block inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected IDisposable Compartment(string type, string layout = "vbox", Expression<Func<T, bool>> guard = null)
        {
            var skeleton = new GElementSkeleton<T>
            {
                StaticType = type
            };
            CurrentSkeleton.NodeContributions.Add(new CompartmentContribution<T> { Compartment = skeleton, Guard = guard });
            _skeletons.Push(skeleton);
            if (layout != null)
            {
                Forward("layout", layout);
            }
            return new InnerCompartment(this);
        }

        /// <summary>
        /// Sets the CSS classes applicable to this element
        /// </summary>
        /// <param name="cssClass">The CSS class</param>
        /// <param name="guard">A guard predicate or null, if the CSS class should be assigned unconditionally</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void CssClass(string cssClass, Expression<Func<T, bool>> guard = null)
        {
            if (guard != null && cssClass != null)
            {
                var expression = Expression.Lambda<Func<T, string>>(
                    Expression.Condition(guard.Body, Expression.Constant(cssClass), Expression.Constant(null)),
                    guard.Parameters[0]);
                CssClass(expression);
            }
            else if (cssClass != null)
            {
                CurrentSkeleton.StaticCssClasses.Add(cssClass);
            }
        }

        /// <summary>
        /// Refines the given other node descriptor
        /// </summary>
        /// <typeparam name="TOther">The semantic type of elements described by the other node descriptor</typeparam>
        /// <param name="other">The refined node descriptor</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Refine<TOther>(NodeDescriptor<TOther> other)
        {
            other.CurrentSkeleton.Refinements.Add(CurrentSkeleton);
        }

        /// <summary>
        /// Sets the CSS class applicable to this element
        /// </summary>
        /// <param name="selector">A selector expression which CSS class is applicable</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void CssClass(Expression<Func<T, string>> selector)
        {
            if (selector != null)
            {
                CurrentSkeleton.DynamicCssClasses.Add(ObservingFunc<T, string>.FromExpression(selector));
            }
        }

        /// <summary>
        /// Sets the GLSP type created for this node
        /// </summary>
        /// <param name="type">The type</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Type(string type)
        {
            CurrentSkeleton.StaticType = type;
        }

        /// <summary>
        /// Sets the GLSP type created for this node
        /// </summary>
        /// <param name="selector">An expression that calculates the type</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Type(Expression<Func<T, string>> selector)
        {
            CurrentSkeleton.DynamicType = selector;
        }

        /// <summary>
        /// Forwards the given value to the generated GElement
        /// </summary>
        /// <param name="key">The property key</param>
        /// <param name="value">The value for the included property</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Forward(string key, string value)
        {
            CurrentSkeleton.StaticForwards.Add((key, value));
        }

        /// <summary>
        /// Forwards the given value to the generated GElement
        /// </summary>
        /// <param name="key">The property key</param>
        /// <param name="selector">An expression calculating the actual value for the included property</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Forward(string key, Expression<Func<T, string>> selector)
        {
            CurrentSkeleton.DynamicForwards.Add((key, ObservingFunc<T, string>.FromExpression(selector)));
        }

        /// <summary>
        /// Specifies that nodes should be created as subnodes of the given descriptor
        /// </summary>
        /// <typeparam name="TOther">The semantic type of the dependent elements</typeparam>
        /// <param name="targetDescriptor">The node descriptor describing the sub-elements</param>
        /// <param name="selector">A function to obtain a collection of semantic elements</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Nodes<TOther>(NodeDescriptor<TOther> targetDescriptor, Func<T, ICollectionExpression<TOther>> selector)
        {
            CurrentSkeleton.NodeContributions.Add(new NodeCollectionContribution<T, TOther>
            {
                Selector = selector,
                Skeleton = targetDescriptor._baseSkeleton
            });
        }

        /// <summary>
        /// Specifies that a GLabel element should be created under the current node
        /// </summary>
        /// <param name="labelSelector">An expression calculating the text of the label</param>
        /// <param name="type">The GElement type of the label</param>
        /// <param name="canEdit">True, if the label can be added, otherwise False</param>
        /// <param name="guard">An expression to guard the visibility of the label, or null</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Label(Expression<Func<T, string>> labelSelector, string type = "label", bool canEdit = true, Expression<Func<T, bool>> guard = null)
        {
            var skeleton = new GLabelSkeleton<T>
            {
                StaticType = type,
                LabelValue = labelSelector,
                CanEdit = canEdit
            };
            CurrentSkeleton.NodeContributions.Add(new CompartmentContribution<T> { Compartment = skeleton, Guard = guard });
        }

        /// <summary>
        /// Specifies that the nodes should contain edges
        /// </summary>
        /// <typeparam name="TTransition">The semantic type of edges</typeparam>
        /// <param name="edgeDescriptor">A descriptor for the edges</param>
        /// <param name="selector">A function to calculate a collection of edges to create</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Edges<TTransition>(EdgeDescriptor<TTransition> edgeDescriptor, Func<T, ICollectionExpression<TTransition>> selector)
        {
            CurrentSkeleton.EdgeContributions.Add(new EdgeCollectionContribution<T, TTransition>
            {
                Selector = selector,
                Skeleton = edgeDescriptor._baseSkeleton
            });
        }

        /// <summary>
        /// Specifies that the nodes should contain edges
        /// </summary>
        /// <typeparam name="TSource">The semantic type of source nodes</typeparam>
        /// <typeparam name="TTarget">The semantic type of target nodes</typeparam>
        /// <param name="edgeDescriptor">A descriptor of edges from sources to the target</param>
        /// <param name="selector">A function to calculate pairs of source and target node</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Edges<TSource, TTarget>(EdgeDescriptor<TSource, TTarget> edgeDescriptor, Func<T, ICollectionExpression<(TSource, TTarget)>> selector)
        {
            CurrentSkeleton.EdgeContributions.Add(new EdgeCollectionContribution<T, (TSource, TTarget)>
            {
                Selector = selector,
                Skeleton = edgeDescriptor._baseSkeleton
            });
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
        protected void Edges<TSource, TTarget>(NodeDescriptor<TSource> sourceDescriptor, NodeDescriptor<TTarget> targetDescriptor, Func<T, ICollectionExpression<(TSource, TTarget)>> selector)
        {
            Edges(new AdHocEdgeDescriptor<TSource, TTarget>(sourceDescriptor, targetDescriptor), selector);
        }

        private class InnerCompartment : IDisposable
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

        /// <inheritdoc/>
        protected internal override IEnumerable<ShapeTypeHint> CalculateShapeHints()
        {
            return Enumerable.Empty<ShapeTypeHint>();
        }

        /// <inheritdoc/>
        protected internal override IEnumerable<EdgeTypeHint> CalculateEdgeHints()
        {
            return Enumerable.Empty<EdgeTypeHint>();
        }
    }
}
