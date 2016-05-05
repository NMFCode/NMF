using System;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{

    [Serializable]
    public class ModelRemoteValueObject<T> : IModelRemoteValue<T>
    {
        public ModelRemoteValueObject(T value)
        {
            Value = value;
            GlobalIdentifier = Guid.NewGuid();
        }

        public T Value { get; private set; }

        object IModelRemoteValue.Retrieve(ILocalResolveContext resolveContext)
        {
            return Retrieve(resolveContext);
        }

        public T Retrieve(ILocalResolveContext resolveContext)
        {
            return Value;
        }

        public object ReferenceComparable => Value;

        /// <summary>
        /// Can be used to identify objects in a global scope.
        /// </summary>
        public object GlobalIdentifier { get; }
    }



}