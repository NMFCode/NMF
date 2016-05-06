using System;
using NMF.Models;
using Orleans.Collections;

namespace NMF.Expressions.Linq.Orleans.Model
{
    [Serializable]
    public class ModelRemoteValueUri<T> : ObjectRemoteValueBase<T> where T : IModelElement
    {
        [NonSerialized]
        private readonly IModelElement _modelElement;

        public ModelRemoteValueUri(IModelElement modelElement)
        {
            _modelElement = modelElement;
            RootRelativeUri = modelElement.RelativeUri;
            GlobalIdentifier = Guid.NewGuid();
        }

        public Uri RootRelativeUri { get; private set; }

        protected override T CreateLocalObject(ILocalReceiveContext resolveContext, ReceiveAction receiveAction)
        {
            return (T)(resolveContext as LocalModelReceiveContext).LookupModel.Resolve(RootRelativeUri);
        }

        public override object ReferenceComparable => _modelElement;

        public override Guid GlobalIdentifier { get; }


    }
}