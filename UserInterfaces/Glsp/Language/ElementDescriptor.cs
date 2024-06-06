﻿using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Notation;
using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using NMF.Models;
using NMF.Models.Meta;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes a descriptor for elements in the diagram
    /// </summary>
    /// <typeparam name="T">The semantic type of elements described by this descriptor</typeparam>
    public abstract class ElementDescriptor<T> : DescriptorBase
    {
        internal readonly GElementSkeleton<T> _baseSkeleton;
        
        internal Dictionary<string, Func<T>> Profiles { get; } = new Dictionary<string, Func<T>>();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ElementDescriptor()
        {
            _baseSkeleton = CreateSkeleton();
        }

        /// <summary>
        /// Creates a new element
        /// </summary>
        /// <param name="parent">The semantic parent element</param>
        /// <param name="profile">The profile for the created element</param>
        /// <returns>A new element</returns>
        public virtual T CreateElement(string profile, object parent)
        {
            if (profile != null && Profiles.TryGetValue(profile, out var creator))
            {
                return creator();
            }
            return ModelHelper.CreateInstance<T>();
        }


        /// <summary>
        /// Gets the label used for the tool palette
        /// </summary>
        /// <param name="profile">The profile</param>
        /// <returns>The text used for tools</returns>
        public virtual string ToolLabel(string profile)
        {
            return $"New {profile ?? ModelHelper.ImplementationType<T>().Name}";
        }

        /// <summary>
        /// Renders a popup
        /// </summary>
        /// <param name="element">The element for which a popup should be rendered</param>
        /// <param name="dimension">The dimension of the popup</param>
        /// <returns>A HTML string rendering the popup</returns>
        public virtual string RenderPopup(T element, Bounds dimension)
        {
            if (element is IModelElement modelElement)
            {
                var sb = new StringBuilder();
                sb.Append("<table>");
                RenderAttributes(modelElement, modelElement.GetClass(), sb);
                sb.Append("</table>");
                return sb.ToString();
            }
            return null;
        }

        private void RenderAttributes(IModelElement modelElement, IClass @class, StringBuilder sb)
        {
            foreach (var att in @class.Attributes)
            {
                if (att.UpperBound == 1)
                {
                    RenderAttributeValue(att.Name, modelElement.GetAttributeValue(att), sb);
                }
                else
                {
                    RenderAttributeValue(att.Name, string.Join(", ", modelElement.GetAttributeValues(att).OfType<object>().Select(o => o?.ToString() ?? "(null)")), sb);
                }
            }

            foreach (var baseClass in @class.BaseTypes)
            {
                RenderAttributes(modelElement, baseClass, sb);
            }
        }

        private void RenderAttributeValue(string name, object value, StringBuilder sb)
        {
            sb.Append($"<tr><td>{name}</td><td>{ HtmlEncoder.Default.Encode(value?.ToString() ?? "(null)" )}</td></tr>");
        }

        /// <summary>
        /// Calculates a human-readable name for the given element
        /// </summary>
        /// <param name="element">the element</param>
        /// <returns>A human-readable name</returns>
        public virtual string GetElementName(T element)
        {
            return element?.ToString();
        }

        /// <summary>
        /// True, if an instance of this element type can be created, otherwise False
        /// </summary>
        public virtual bool CanCreateElement => ModelHelper.CanCreateInstance<T>();

        /// <summary>
        /// Gets the element type id for elements created by this descriptor
        /// </summary>
        public string ElementTypeId => _baseSkeleton.Type ?? _baseSkeleton.TypeName;

        internal abstract GElementSkeleton<T> CreateSkeleton();

        internal virtual GElementSkeleton<T> CurrentSkeleton => _baseSkeleton;

        internal override GElementSkeletonBase GetRootSkeleton() => _baseSkeleton;

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
                    Expression.Condition(guard.Body, Expression.Constant(cssClass), Expression.Constant(null, typeof(string))),
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
            CurrentSkeleton.Type = type;
        }

        /// <summary>
        /// Forwards the given value to the generated GElement
        /// </summary>
        /// <param name="key">The property key</param>
        /// <param name="value">The value for the included property</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Forward(string key, object value)
        {
            CurrentSkeleton.StaticForwards.Add((key, value));
        }

        /// <summary>
        /// Forwards the given value to the generated GElement
        /// </summary>
        /// <param name="key">The property key</param>
        /// <param name="selector">An expression calculating the actual value for the included property</param>
        /// <remarks>This method is intended to be used inside of <see cref="DescriptorBase.DefineLayout" /></remarks>
        protected void Forward(string key, Expression<Func<T, object>> selector)
        {
            CurrentSkeleton.DynamicForwards.Add((key, ObservingFunc<T, object>.FromExpression(selector)));
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

        protected IChildSyntax Operation(string toolName, Func<T, IGlspSession, Task> operation, string key = null)
        {
            var kind = key ?? toolName.ToCamelCase();
            var op = new GElementOperation<T>(kind, toolName, operation);
            CurrentSkeleton.Operations.Add(kind, op);
            return new ChildSyntax(op);
        }

        protected IChildSyntax Operation(string toolName, Action<T, IGlspSession> operation, string key = null)
        {
            return Operation(toolName, (it, session) =>
            {
                operation(it, session);
                return Task.CompletedTask;
            }, key);
        }

        protected void Profile(string profileName)
        {
            Profile(profileName, null);
        }

        protected void Profile(string profileName, Func<T> creator)
        {
            Profiles.Add(profileName, creator);
        }
    }
}
