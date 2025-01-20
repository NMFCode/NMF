using NMF.Transformations;
using System.CodeDom;

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
