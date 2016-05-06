using System;
using System.ComponentModel;
using NMF.Models;
using Orleans.Collections;
using Orleans.Streams.Messages;

namespace NMF.Expressions.Linq.Orleans.Message
{
    public class ModelStreamMessage : IStreamMessage
    {
        public IObjectRemoteValue ElementAffected { get; private set; }

        public ModelStreamMessage(IObjectRemoteValue elementAffected)
        {
            ElementAffected = elementAffected;
        }
    }
}