using System;
using NMF.Expressions.Linq.Orleans.Model;

namespace NMF.Expressions.Linq.Orleans.Message
{
    [Serializable]
    public class ModelPropertyChangedMessage : ModelStreamMessage
    {
        public IModelRemoteValue Value { get; private set; }
        public string PropertyName { get; private set; }

        public ModelPropertyChangedMessage(IModelRemoteValue value, Uri relativeRootUri, string propertyName) : base(relativeRootUri)
        {
            Value = value;
            PropertyName = propertyName;
        }
    }
}