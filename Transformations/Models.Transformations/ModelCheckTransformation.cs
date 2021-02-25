using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Denotes a homogeneous model transformation that executes all model check rules
    /// </summary>
    public class ModelCheckTransformation : ReflectiveTransformation
    {
        /// <inheritdoc />
        protected override IEnumerable<GeneralTransformationRule> CreateDefaultRules()
        {
            yield return new ModelCheckTransformationRule();
        }
    }
}
