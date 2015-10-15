using NMF.Models.Meta;
using NMF.Utilities;
using NMF.Transformations.Core;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Transformations;

namespace NMF.Models.Meta
{
    partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to transform parameters
        /// </summary>
        public class Parameter2Parameter : TransformationRule<IParameter, CodeParameterDeclarationExpression>
        {
            /// <summary>
            /// Initializes the parameter
            /// </summary>
            /// <param name="input">The base NMeta parameter</param>
            /// <param name="output">The generated parameter</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IParameter input, CodeParameterDeclarationExpression output, ITransformationContext context)
            {
                switch (input.Direction)
                {
                    case Direction.Out:
                        output.Direction = FieldDirection.Out;
                        break;
                    case Direction.InOut:
                        output.Direction = FieldDirection.Ref;
                        break;
                    case Direction.In:
                    default:
                        output.Direction = FieldDirection.In;
                        break;
                }
                output.Name = input.Name.ToCamelCase();

                output.Type = CreateTypeReference(input, attr => output.CustomAttributes.Add(attr), null, context);
            }
        }

    }
}
