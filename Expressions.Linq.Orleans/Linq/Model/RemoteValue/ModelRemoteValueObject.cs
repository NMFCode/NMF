using System;
using Orleans.Collections;

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


        protected override T CreateLocalObject(ILocalReceiveContext resolveContext, ReceiveAction receiveAction)
        {
            return Value;
        }
    }
}