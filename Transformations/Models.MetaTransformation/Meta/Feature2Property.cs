using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Transformations;
using System.CodeDom;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        public class Feature2Property : AbstractTransformationRule<ITypedElement, CodeMemberProperty>
        {
        }
    }
}
