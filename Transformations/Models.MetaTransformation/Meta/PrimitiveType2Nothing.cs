using NMF.CodeGen;
using NMF.Models.Meta;
using NMF.Transformations;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// The transformation rule to transform an NMeta type to a code type declaration
        /// </summary>
        public class PrimitiveType2Nothing : TransformationRule<IPrimitiveType, CodeTypeDeclaration>
        {
            public override CodeTypeDeclaration CreateOutput(IPrimitiveType input, Transformations.Core.ITransformationContext context)
            {
                return null;
            }

            /// <summary>
            /// Registers the dependencies, i.e. transform also the namespace of a type
            /// </summary>
            public override void RegisterDependencies()
            {
                MarkInstantiatingFor(Rule<Type2Type>());
            }
        }
    }
}
