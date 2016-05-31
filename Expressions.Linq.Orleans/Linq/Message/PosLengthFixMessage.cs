using System;
using NMF.Models.Tests.Railway;
using Orleans.Streams.Messages;

namespace NMF.Expressions.Linq.Orleans.Message
{
    [Serializable]
    public class PosLengthFixMessage : IStreamMessage
    {
        private Uri _uri;

        public PosLengthFixMessage(Uri elementUri)
        {
            _uri = elementUri;
        }

        public void Execute(Models.Model model)
        {
            var localSegment = (ISegment) model.Resolve(_uri);
            localSegment.Length = -localSegment.Length + 1;
        }
    }
}