using NMF.Models;
using NMF.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    public class ModelCheckTransformationRule<T> : InPlaceTransformationRule<T>
        where T : class
    {
        public override void RegisterDependencies()
        {
            MarkInstantiatingFor(Rule<ModelCheckTransformationRule>());
        }
    }

    internal class ModelCheckTransformationRule : InPlaceTransformationRule<IModelElement>
    {
        public override void RegisterDependencies()
        {
            CallMany(this, el => el.Children);
        }
    }
}
