using NMF.Transformations;
using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    internal interface IModelTransformation
    {
        IDictionary<Type, GeneralTransformationRule> ModelRules { get; }
    }
}
