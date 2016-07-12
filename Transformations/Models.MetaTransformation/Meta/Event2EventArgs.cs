using NMF.Models.Meta;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.CodeGen;
using NMF.Utilities;
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
        /// The transformation rule to generate the event arguments class
        /// </summary>
        public class Event2EventArgs : TransformationRule<IEvent, CodeTypeDeclaration>
        {
            /// <summary>
            /// Creates the transformation rule output for the given NMeta event
            /// </summary>
            /// <param name="input">The NMeta event</param>
            /// <param name="context">The transformation context</param>
            /// <returns>The generated code type declaration</returns>
            public override CodeTypeDeclaration CreateOutput(IEvent input, ITransformationContext context)
            {
                return CodeDomHelper.CreateTypeDeclarationWithReference(input.Name.ToPascalCase() + "EventArgs");
            }

            /// <summary>
            /// Initializes the given NMeta event
            /// </summary>
            /// <param name="input">The input NMeta event</param>
            /// <param name="output">The generated event args type</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IEvent input, CodeTypeDeclaration output, ITransformationContext context)
            {
                output.BaseTypes.Add(typeof(EventArgs).ToTypeReference());

                output.WriteDocumentation(string.Format("The event arguments for the {0} event", input.Name));

                var constructor = new CodeConstructor();

                var attr2Property = Rule<DataTypeAttribute2Property>();
                foreach (var attr in input.Type.Attributes)
                {
                    var fieldName = attr.Name.ToCamelCase();
                    var property = context.Trace.ResolveIn(attr2Property, attr);
                    var field = new CodeMemberField(property.Type, fieldName);
                    var fieldRef = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName);

                    var cArg = new CodeParameterDeclarationExpression(field.Type, fieldName);
                    var cStatement = new CodeAssignStatement(fieldRef, new CodeArgumentReferenceExpression(fieldName));

                    constructor.Parameters.Add(cArg);
                    constructor.Statements.Add(cStatement);

                    output.Members.Add(property);
                    output.Members.Add(field);
                }
            }

            /// <summary>
            /// Registers the requirements, i.e. requires to transform the data type attributes
            /// </summary>
            public override void RegisterDependencies()
            {
                RequireMany(Rule<DataTypeAttribute2Property>(), ev => ev.Type.Attributes);
            }
        }
    }
}
