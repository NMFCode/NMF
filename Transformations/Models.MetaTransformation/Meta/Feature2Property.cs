using NMF.Transformations;
using System.CodeDom;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        /// <summary>
        /// Denotes the abstract transformation rule from a feature to a property
        /// </summary>
        public class Feature2Property : AbstractTransformationRule<ITypedElement, CodeMemberProperty>
        {
        }
    }
}
