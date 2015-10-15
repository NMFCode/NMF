using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom;
using NMF.Transformations.Core;
using NMF.Transformations;

namespace NMF.CodeGen
{
    /// <summary>
    /// Represents a transformation rule that is used to generate events
    /// </summary>
    /// <typeparam name="T">The type of the model elements from which to generate events</typeparam>
    public abstract class EventGenerator<T> : TransformationRule<T, CodeMemberEvent>
        where T : class
    {
        /// <summary>
        /// Creates the output event for the given input model element
        /// </summary>
        /// <param name="input">The input model element</param>
        /// <param name="context">The transformation context</param>
        /// <returns>The transformation output uninitialized</returns>
        public override CodeMemberEvent CreateOutput(T input, ITransformationContext context)
        {
            return new CodeMemberEvent();
        }

        /// <summary>
        /// Gets the name of the event
        /// </summary>
        /// <param name="input">The input model element from which to generate the event</param>
        /// <returns>The name of the event</returns>
        protected abstract string GetName(T input);

        /// <summary>
        /// Gets the type reference to the event arguments data class
        /// </summary>
        /// <param name="input">The input model element</param>
        /// <param name="context">The transformation context</param>
        /// <returns>A reference to the used event args class or null. If null, the default class used is System.EventArgs</returns>
        protected abstract CodeTypeReference GetEventArgsType(T input, ITransformationContext context);

        /// <summary>
        /// Initializes the output event
        /// </summary>
        /// <param name="input">The input model element</param>
        /// <param name="output">The output model element</param>
        /// <param name="context">The transformation context</param>
        public override void Transform(T input, CodeMemberEvent output, ITransformationContext context)
        {
            CodeTypeReference eventArgsType = GetEventArgsType(input, context);

            output.Name = GetName(input);

            if (eventArgsType != null)
            {
                output.Type = new CodeTypeReference(typeof(EventHandler<>).Name, eventArgsType);
            }
            else
            {
                eventArgsType = new CodeTypeReference(typeof(EventArgs).Name);
                output.Type = new CodeTypeReference(typeof(EventHandler).Name);
            }

            var onChangedMethod = CodeDomHelper.CreateOnChangedMethod(output, eventArgsType);

            output.DependentMembers(true).Add(onChangedMethod);
        }
    }
}
