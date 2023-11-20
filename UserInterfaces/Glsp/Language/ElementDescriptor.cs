using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Notation;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Types;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes a descriptor for elements in the diagram
    /// </summary>
    /// <typeparam name="T">The semantic type of elements described by this descriptor</typeparam>
    public abstract class ElementDescriptor<T> : DescriptorBase
    {
        internal readonly GElementSkeleton<T> _baseSkeleton;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ElementDescriptor()
        {
            _baseSkeleton = CreateSkeleton();
        }

        internal abstract GElementSkeleton<T> CreateSkeleton();

        internal virtual GElementSkeleton<T> CurrentSkeleton => _baseSkeleton;


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
        protected void Refine<TOther>(ElementDescriptor<TOther> other)
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

        /// <inheritdoc />
        protected internal override GGraph CreateGraph(object semanticRoot, IDiagram diagram, ISkeletonTrace trace)
        {
            if (semanticRoot is T rootElement)
            {
                return _baseSkeleton.CreateGraph(rootElement, trace, diagram);
            }
            else
            {
                throw new InvalidOperationException($"Root element {semanticRoot} is not an element of expected type {typeof(T).Name}.");
            }
        }
    }
}
