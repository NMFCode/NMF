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
        public class Type2Type : AbstractTransformationRule<IType, CodeTypeDeclaration>
        {
            /// <summary>
            /// Registers the dependencies, i.e. transform also the namespace of a type
            /// </summary>
            public override void RegisterDependencies()
            {
                Call(Rule<Namespace2Namespace>(), type => type.Namespace);
            }
        }
    }
}
