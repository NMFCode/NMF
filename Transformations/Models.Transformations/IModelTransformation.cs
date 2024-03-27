using NMF.Transformations.Core;
using System;
using System.Collections.Generic;

namespace NMF.Transformations
{
    internal interface IModelTransformation
    {
        IDictionary<Type, GeneralTransformationRule> ModelRules { get; }
    }
}
