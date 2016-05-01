using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using NMF.Expressions.Linq.Orleans.Message;
using NMF.Expressions.Linq.Orleans.Model;
using Orleans.Streams;
using Orleans.Streams.Endpoints;

namespace NMF.Expressions.Linq.Orleans
{
    /// <summary>
    /// Sends messages via multiple children output. Maintains link between sent objects and a particular stream via reference identity.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MappingStreamMessageSenderComposite<T> : StreamMessageSenderComposite<T>
    {
        protected ConditionalWeakTable<object, StreamMessageSender<T>> ObjectToSenderMapping;
        private int _currentSenderIndex;

        public MappingStreamMessageSenderComposite(IStreamProvider provider, int numberOfChildren = 1) : base(provider, numberOfChildren)
        {
            ObjectToSenderMapping = new ConditionalWeakTable<object, StreamMessageSender<T>>();
            _currentSenderIndex = 0;
        }

        public void EnqueueAddModelItems(IList<IModelRemoteValue<T>> remoteValues)
        {
            var senderGrouping = remoteValues.GroupBy(GetSenderForValue);
            foreach (var group in senderGrouping)
            {
                var senderMessage = new ModelItemAddMessage<T>(group.ToList());
                group.Key.EnqueueMessage(senderMessage);
            }
        }

        public void EnqueueRemoveModelItems(IList<IModelRemoteValue<T>> remoteValues)
        {
            var senderGrouping = remoteValues.GroupBy(GetSenderForValue);
            foreach (var group in senderGrouping)
            {
                var senderMessage = new ModelItemRemoveMessage<T>(group.ToList());
                group.Key.EnqueueMessage(senderMessage);
            }
        }

        private StreamMessageSender<T> GetSenderForValue(IModelRemoteValue<T> remoteValue)
        {
            StreamMessageSender<T> sender;
            if (ObjectToSenderMapping.TryGetValue(remoteValue.ReferenceComparable, out sender))
                return sender;

            sender = Senders[_currentSenderIndex];
            ObjectToSenderMapping.Add(remoteValue.ReferenceComparable, sender);

            _currentSenderIndex = (_currentSenderIndex + 1)% Senders.Count;

            return sender;
        }
    }
}