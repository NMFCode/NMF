using System;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public static class ModelRemoteValueFactory
    {
        public static IModelRemoteValue CreateModelChangeValue(object obj)
        {
            if (obj is IModelElement)
                return new ModelRemoteValueUri<IModelElement>((IModelElement)obj);
            else
                return new ModelRemoteValueObject<object>(obj);
        }

        public static IModelRemoteValue<T> CreateModelChangeValue<T>(T obj)
        {
            if (obj is ModelElement)
            {
                var type = typeof(ModelRemoteValueUri<>);
                var genericType = type.MakeGenericType(new Type[] { typeof(T) });
                return (IModelRemoteValue<T>)Activator.CreateInstance(genericType, new object[] { obj });
            }
            else
            {
                return new ModelRemoteValueObject<T>(obj);
            }
        }
    }
}