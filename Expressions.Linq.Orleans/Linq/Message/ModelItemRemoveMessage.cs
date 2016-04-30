using System;
using System.Collections.Generic;
using NMF.Expressions.Linq.Orleans.Model;
using Orleans.Streams.Messages;

namespace NMF.Expressions.Linq.Orleans.Message
{
    [Serializable]
    public class ModelItemRemoveMessage<T> : IStreamMessage
    {
        public IList<IModelRemoteValue<T>> Items { get; private set; }

        public ModelItemRemoveMessage(IList<IModelRemoteValue<T>> items)
        {
            Items = items;
        }
    }
}