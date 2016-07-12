using NMF.CodeGen;
using NMF.Models.Meta;
using NMF.Transformations.Core;
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
        /// The transformation rule to generate enumerations from NMeta enumerations
        /// </summary>
        public class Enumeration2Type : EnumGenerator<IEnumeration>
        {
            /// <summary>
            /// Gets the enumeration members that should be generated based on the given NMeta enumeration
            /// </summary>
            /// <param name="input">The NMeta enumeration</param>
            /// <returns>A collection of enumeration members</returns>
            protected override IEnumerable<EnumGenerator<IEnumeration>.EnumMember> GetMembers(IEnumeration input)
            {
                foreach (var literal in input.Literals)
                {
                    yield return new EnumMember()
                    {
                        Name = literal.Name.ToPascalCase(),
                        Summary = literal.Summary,
                        Remarks = literal.Remarks,
                        Value = literal.Value
                    };
                }
            }

            /// <summary>
            /// Initializes the created enumeration
            /// </summary>
            /// <param name="input">The input NMeta enumeration</param>
            /// <param name="output">The generated code declaration</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IEnumeration input, CodeTypeDeclaration output, ITransformationContext context)
            {
                SetTypeReferenceForMappedType(input, CodeDomHelper.GetReferenceForType(output));
                base.Transform(input, output, context);
                output.WriteDocumentation(input.Summary, input.Remarks);
            }

            /// <summary>
            /// Gets the name of the enumeration
            /// </summary>
            /// <param name="input">The NMeta enumeration</param>
            /// <returns>The name of the enumeration to be generated</returns>
            protected override string GetName(IEnumeration input)
            {
                return input.Name.ToPascalCase();
            }

            /// <summary>
            /// Gets a value indicating whether the generated enumeration is flagged
            /// </summary>
            /// <param name="input">The NMeta enumeration</param>
            /// <returns>True, if the enumeration is flagged, otherwise false</returns>
            protected override bool GetIsFlagged(IEnumeration input)
            {
                return input.IsFlagged;
            }

            /// <summary>
            /// Marks the transformation rule instantiating for Type2Type
            /// </summary>
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Type2Type>());
            }
        }
    }
}
