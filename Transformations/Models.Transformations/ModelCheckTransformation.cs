using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    public class ModelCheckTransformation : ReflectiveTransformation
    {
        protected override IEnumerable<GeneralTransformationRule> CreateDefaultRules()
        {
            yield return new ModelCheckTransformationRule();
        }
    }
}
