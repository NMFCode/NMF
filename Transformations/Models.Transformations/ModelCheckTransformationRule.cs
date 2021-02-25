using NMF.Models;
using NMF.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Transformations
{
    /// <summary>
    /// Denotes a model check rule of the given type
    /// </summary>
    /// <typeparam name="T">The type of elements</typeparam>
    public class ModelCheckTransformationRule<T> : InPlaceTransformationRule<T>
        where T : class
    {
        /// <inheritdoc />
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
