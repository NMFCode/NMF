using System;
using Orleans.Collections;
using Orleans.Streams.Stateful;

namespace NMF.Expressions.Linq.Orleans.Model
{
    [Serializable]
    public class ModelRemoteValueObject<T> : ObjectRemoteValueBase<T>
    {
        public T Value { get; }

        public override object ReferenceComparable => Value;

        /// <summary>
        ///     Can be used to identify objects in a global scope.
        /// </summary>
        public override Guid GlobalIdentifier { get; }

        public ModelRemoteValueObject(T value)
        {
            Value = value;
            GlobalIdentifier = Guid.NewGuid();
        }

        protected override T CreateLocalObject(ILocalReceiveContext receiveContext, LocalContextAction localContextAction)
        {
            return Value;
        }
    }
}