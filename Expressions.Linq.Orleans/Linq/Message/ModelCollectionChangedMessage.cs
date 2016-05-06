using System;
using System.Collections.Specialized;
using NMF.Expressions.Linq.Orleans.Model;
using Orleans.Collections;
using Orleans.Streams.Messages;

namespace NMF.Expressions.Linq.Orleans.Message
{
    [Serializable]
    public class ModelCollectionChangedMessage : ModelStreamMessage
    {
        public NotifyCollectionChangedAction Action { get; private set; }
        public IObjectRemoteValue[] Elements { get; private set; }

        public ModelCollectionChangedMessage(NotifyCollectionChangedAction action, Uri relativeRootUri, IObjectRemoteValue[] elements) : base(relativeRootUri)
        {
            Action = action;
            Elements = elements;
        }
    }
}