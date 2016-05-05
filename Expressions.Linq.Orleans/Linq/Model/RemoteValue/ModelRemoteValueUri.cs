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

        public T Retrieve(ILocalResolveContext resolveContext)
        {
            if (GlobalIdentifier != null && resolveContext.ObjectLookup.ContainsKey(GlobalIdentifier))
            {
                return (T)resolveContext.ObjectLookup[GlobalIdentifier];
            }

            return (T)resolveContext.LookupModel.Resolve(RootRelativeUri);
        }

        public object ReferenceComparable => _modelElement;

        public object GlobalIdentifier => RootRelativeUri;

        object IModelRemoteValue.Retrieve(ILocalResolveContext resolveContext)
        {
            return Retrieve(resolveContext);
        }
    }
}