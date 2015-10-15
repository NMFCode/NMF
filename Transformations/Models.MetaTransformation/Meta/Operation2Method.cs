using NMF.CodeGen;
using NMF.Models.Meta;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Meta
{
    partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to transform operations
        /// </summary>
        public class Operation2Method : TransformationRule<IOperation, CodeMemberMethod>
        {
            /// <summary>
            /// Initialize the generated operation
            /// </summary>
            /// <param name="input">The input NMeta operation</param>
            /// <param name="output">The generated code method</param>
            /// <param name="context">The transformation context</param>
            public override void Transform(IOperation input, CodeMemberMethod output, ITransformationContext context)
            {
                output.Name = input.Name.ToPascalCase();
                output.ValidateArguments();
                output.ThrowException<NotImplementedException>();

                var parameterDocDict = new Dictionary<string, string>();
                foreach (var par in input.Parameters)
                {
                    parameterDocDict.Add(par.Name.ToCamelCase(), par.Summary);
                }
                output.WriteDocumentation(input.Summary, null, parameterDocDict, input.Remarks);
            }

            /// <summary>
            /// Registers the dependencies, i.e. transform the parameters
            /// </summary>
            public override void RegisterDependencies()
            {
                RequireMany(Rule<Parameter2Parameter>(), op => op.Parameters, (op, pars) => op.Parameters.AddRange(pars.ToArray()));
            }
        }

    }
}
