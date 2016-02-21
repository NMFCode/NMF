using NMF.CodeGen;
using NMF.Models.Meta;
using NMF.Transformations.Core;
using NMF.Utilities;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Transformations;

namespace NMF.Models.Meta
{
    public partial class Meta2ClassesTransformation
    {
        public class Class2Proxy : TransformationRule<IClass, CodeTypeDeclaration>
        {

        }
    }
}
