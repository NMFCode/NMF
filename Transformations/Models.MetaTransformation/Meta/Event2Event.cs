using NMF.CodeGen;
using NMF.Models.Meta;
using NMF.Transformations.Core;
using NMF.Utilities;
using NMF.Transformations;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to transform NMeta events to code events
        /// </summary>
        public class Event2Event : EventGenerator<IEvent>
        {
            /// <summary>
            /// Gets the type reference to the event args class
            /// </summary>
            /// <param name="input">The NMeta event</param>
            /// <param name="context">The transformation context</param>
            /// <returns>A code type reference to the event args</returns>
            protected override CodeTypeReference GetEventArgsType(IEvent input, ITransformationContext context)
            {
                if (input.Type == null)
                {
                    return null;
                }
                else
                {
                    var eventArgs = context.Trace.ResolveIn(Rule<Event2EventArgs>(), input);
                    return eventArgs.GetReferenceForType();
                }
            }

            public override void Transform(IEvent input, CodeMemberEvent output, ITransformationContext context)
            {
                base.Transform(input, output, context);
                output.WriteDocumentation(input.Summary, input.Remarks);
            }

            /// <summary>
            /// Gets the name of the generated event
            /// </summary>
            /// <param name="input">The NMeta event</param>
            /// <returns>The name of the generated event</returns>
            protected override string GetName(IEvent input)
            {
                return input.Name.ToPascalCase();
            }

            /// <summary>
            /// Registers the dependencies, i.e. requires the 
            /// </summary>
            public override void RegisterDependencies()
            {
                Require(Rule<Event2EventArgs>(), ev => ev.Type != null, (ev, dec) => ev.DependentTypes(true).Add(dec));
            }
        }
    }
}
