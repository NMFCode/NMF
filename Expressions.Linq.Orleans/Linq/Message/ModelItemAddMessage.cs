using System;
using System.Collections.Generic;
using NMF.Expressions.Linq.Orleans.Model;
using Orleans.Collections;
using Orleans.Streams.Messages;

namespace NMF.Expressions.Linq.Orleans.Message
{
    [Serializable]
    public class ModelItemAddMessage<T> : IStreamMessage
    {
        public IList<IObjectRemoteValue<T>> Items { get; private set; }

        public ModelItemAddMessage(IList<IObjectRemoteValue<T>> items)
        {
            Items = items;
        }
    }
}