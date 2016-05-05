using System;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{
    [Serializable]
    public class ModelRemoteValueUri<T> : IModelRemoteValue<T> where T : IModelElement
    {
        [NonSerialized]
        private readonly IModelElement _modelElement;

        public ModelRemoteValueUri(IModelElement modelElement)
        {
            _modelElement = modelElement;
            RootRelativeUri = modelElement.RelativeUri;
        }

        public Uri RootRelativeUri { get; private set; }

        public T Retrieve(IResolvableModel lookupModel)
        {
            return (T)lookupModel.Resolve(RootRelativeUri);
        }

        public object ReferenceComparable => _modelElement;

        object IModelRemoteValue.Retrieve(IResolvableModel lookupModel)
        {
            return Retrieve(lookupModel);
        }
    }
}