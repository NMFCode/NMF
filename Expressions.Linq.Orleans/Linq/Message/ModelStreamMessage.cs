using System;
using System.ComponentModel;
using NMF.Models;
using Orleans.Streams.Messages;

namespace NMF.Expressions.Linq.Orleans.Message
{
    public class ModelStreamMessage : IStreamMessage
    {
        public Uri RelativeRootUri { get; private set; }

        public ModelStreamMessage(Uri relativeRootUri)
        {
            RelativeRootUri = relativeRootUri;
        }
    }
}