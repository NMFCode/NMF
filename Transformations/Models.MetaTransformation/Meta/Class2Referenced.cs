using NMF.CodeGen;
using NMF.Transformations;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule that generates the collection class for the ReferencedElements reference
        /// </summary>
        public class Class2Referenced : Class2Children
        {

            protected override List<IReference> GetImplementingReferences(IClass scope, ITransformationContext context)
            {
                var generatedType = context.Trace.ResolveIn(Rule<Class2Type>(), scope);
                var r2p = Rule<Reference2Property>();
                return (from c in scope.Closure(c => c.BaseTypes)
                        from r in c.References
                        where generatedType.Members.Contains(context.Trace.ResolveIn(r2p, r))
                        select r).ToList();
            }

            /// <summary>
            /// Creates the uninitialized output type declaration
            /// </summary>
            /// <param name="scope">The scope in which the reference is refined</param>
            /// <param name="context">The transformation context</param>
            /// <returns>The newly created code type declaration</returns>
            public override CodeTypeDeclaration CreateOutput(IClass scope, ITransformationContext context)
            {
                if (!scope.References.Any()) return null;
                return CodeDomHelper.CreateTypeDeclarationWithReference(scope.Name.ToPascalCase() + "ReferencedElementsCollection");
            }
        }
    }
}
