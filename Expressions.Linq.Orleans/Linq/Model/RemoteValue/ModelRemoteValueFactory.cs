using System;
using NMF.Models;
using Orleans.Collections;
using Orleans.Streams.Stateful;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public static class ModelRemoteValueFactory
    {
        public static IObjectRemoteValue CreateModelRemoteValue(object obj, ILocalSendContext sendContext)
        {
            var element = obj as IModelElement;
            if (element != null)
                return new ModelRemoteValueUri<IModelElement>(element);
            var tuple = obj as IModelElementTuple;
            if (tuple != null)
                return new ModelRemoteValueTuple<IModelElementTuple>(tuple, sendContext);

            return new ModelRemoteValueObject<object>(obj);
        }

        public static IObjectRemoteValue<T> CreateModelRemoteValue<T>(T obj, ILocalSendContext sendContext)
        {
            if (obj is ModelElement)
            {
                var type = typeof(ModelRemoteValueUri<>);
                var genericType = type.MakeGenericType(typeof(T));
                return (IObjectRemoteValue<T>)Activator.CreateInstance(genericType, new object[] { obj });
            }
            if (obj is IModelElementTuple)
            {
                var type = typeof(ModelRemoteValueTuple<>);
                var genericType = type.MakeGenericType(typeof(T));
                return (IObjectRemoteValue<T>)Activator.CreateInstance(genericType, new object[] { obj, sendContext });
            }

            return new ModelRemoteValueObject<T>(obj);
        }
    }
}