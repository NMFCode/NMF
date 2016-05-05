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
        }

        public T Value { get; private set; }

        public T Retrieve(IResolvableModel lookupModel)
        {
            return Value;
        }

        public object ReferenceComparable => Value;

        object IModelRemoteValue.Retrieve(IResolvableModel lookupModel)
        {
            return Retrieve(lookupModel);
        }
    }



}