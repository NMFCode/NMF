using NMF.CodeGen;
using NMF.Models.Meta;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Meta
{
    partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to transform properties of a data type
        /// </summary>
        public class DataTypeAttribute2Property : TransformationRule<IAttribute, CodeMemberProperty>
        {
            private static CodeTypeReference GetCollectionType(ITypedElement property, CodeTypeReference elementType, ITransformationContext context)
            {
                return new CodeTypeReference(typeof(IEnumerable<>).Name, elementType);
            }

            /// <summary>
            /// Initializes the generated property for the given attribute
            /// </summary>
            /// <param name="input">The NMeta attribute</param>
            /// <param name="output">The generated code property</param>
            /// <param name="context">The transformaion context</param>
            public override void Transform(IAttribute input, CodeMemberProperty output, ITransformationContext context)
            {
                output.HasGet = true;
                output.HasSet = false;

                output.WriteDocumentation(input.Summary ?? string.Format("The {0} property", input.Name), input.Remarks);

                output.Name = input.Name.ToPascalCase();
                output.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                output.Type = CreateTypeReference(input, null, GetCollectionType, context);
                var initValue = input.DefaultValue == null ? null : CodeDomHelper.CreatePrimitiveExpression(input.DefaultValue, output.Type);
                var fieldRef = output.CreateBackingField(output.Type, initValue);
                output.ImplementGetter(fieldRef);
            }
        }
    }
}
