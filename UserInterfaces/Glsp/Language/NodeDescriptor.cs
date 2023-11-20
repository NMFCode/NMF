using NMF.Glsp.Processing;
using System.Linq.Expressions;
using System;
using NMF.Glsp.Protocol.Types;
using System.Collections.Generic;
using System.Linq;
using NMF.Utilities;
using NMF.Expressions;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes the basic type to describe the appearance of elements as nodes
    /// </summary>
    /// <typeparam name="T">The semantic element type</typeparam>
    public abstract class NodeDescriptor<T> : ElementDescriptor<T>
    {
        internal readonly Stack<GElementSkeleton<T>> _skeletons = new Stack<GElementSkeleton<T>>();
        internal override GElementSkeleton<T> CreateSkeleton() => new GNodeSkeleton<T>();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public NodeDescriptor()
        {
            _skeletons.Push(_baseSkeleton);
            Type(DefaultTypes.Node);
        }

        internal override GElementSkeleton<T> CurrentSkeleton => _skeletons.Peek();

        /// <summary>
        /// Creates a new compartment for the nodes represented by this semantic element
        /// </summary>
        /// <param name="type">The GElement type for the compartment</param>
        /// <param name="layout">The default layout for this compartment, e.g. hbox or vbox</param>
        /// <param name="guard">A predicate expression to control the creation of this compartment</param>
        /// <returns>A disposable that can be disposed to return to the parent element</returns>
        /// <remarks>This method is intended to be used to create a using block inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected IDisposable Compartment(string type = DefaultTypes.Compartment, string layout = "vbox", Expression<Func<T, bool>> guard = null)
        {
            var skeleton = new GElementSkeleton<T>
            {
                StaticType = type
            };
            CurrentSkeleton.NodeContributions.Add(new CompartmentContribution<T> { Compartment = skeleton, Guard = guard });
            skeleton.PossibleParents.Add(CurrentSkeleton);
            _skeletons.Push(skeleton);
            if (layout != null)
            {
                Forward("layout", layout);
            }
            return new InnerCompartment(this);
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
            targetDescriptor._baseSkeleton.PossibleParents.Add(CurrentSkeleton);
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
            edgeDescriptor._baseSkeleton.PossibleParents.Add(CurrentSkeleton);
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
            edgeDescriptor._baseSkeleton.PossibleParents.Add(CurrentSkeleton);
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

        /// <inheritdoc/>
        protected internal override IEnumerable<TypeHint> CalculateTypeHints()
        {
            var stack = new Stack<CompartmentContribution<T>>();
            var skeleton = _baseSkeleton;

            yield return new ShapeTypeHint
            {
                ElementTypeId = skeleton.ElementTypeId,
                Reparentable = skeleton.PossibleParents.Any(),
                Deletable = true,
                Repositionable = true,
                Resizable = true,
                ContainableElementTypeIds = skeleton
                    .PossibleParents
                    .SelectMany(sk => sk.Closure(s => s.Refinements))
                    .Select(sk => sk.ElementTypeId)
                    .ToArray()
            };

            PushCompartments(skeleton, stack);
            while (stack.Count > 0)
            {
                var compartment = stack.Pop();
                skeleton = compartment.Compartment;

                yield return new ShapeTypeHint
                {
                    ElementTypeId = skeleton.ElementTypeId,
                    Reparentable = false,
                    Deletable = false,
                    Repositionable = false,
                    Resizable = false,
                };

                PushCompartments(skeleton, stack);
            }
        }

        private void PushCompartments(GElementSkeleton<T> skeleton, Stack<CompartmentContribution<T>> stack)
        {
            foreach (var compartment in skeleton.Compartments)
            {
                stack.Push(compartment);
            }
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
    }
}
