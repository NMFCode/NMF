using System.CodeDom;
using NMF.Transformations;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// Denotes the transformation rule from a class to a proxy declaration
        /// </summary>
        public class Class2Proxy : TransformationRule<IClass, CodeTypeDeclaration>
        {

        }
    }
}
