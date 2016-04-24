using System;
using System.Collections.Generic;
using NMF.Expressions.Linq.Orleans.Model;
using Orleans.Streams.Messages;

namespace NMF.Expressions.Linq.Orleans.Message
{
    [Serializable]
    public class ModelItemAddMessage<T> : IStreamMessage
    {
        public IList<IModelRemoteValue<T>> Items { get; private set; }

        public ModelItemAddMessage(IList<IModelRemoteValue<T>> items)
        {
            Items = items;
        }
    }
}