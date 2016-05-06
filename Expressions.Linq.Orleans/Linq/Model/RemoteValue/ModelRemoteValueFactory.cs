using System;
using NMF.Models;
using Orleans.Collections;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public static class ModelRemoteValueFactory
    {
        public static IObjectRemoteValue CreateModelRemoteValue(object obj, ILocalSendContext sendContext)
        {
            if (obj is IModelElement)
                return new ModelRemoteValueUri<IModelElement>((IModelElement)obj);
            if (obj is IModelElementTuple)
                return new ModelRemoteValueTuple<IModelElementTuple>((IModelElementTuple) obj, sendContext);
            else
                return new ModelRemoteValueObject<object>(obj);
        }

        public static IObjectRemoteValue<T> CreateModelRemoteValue<T>(T obj, ILocalSendContext sendContext)
        {
            //IModelRemoteValue<T> resultObject = null;
            //if(resultObjectLookup.ObjectLookup.TryGetValue(obj, out resultObject))
            //if(resu)
            if (obj is ModelElement)
            {
                var type = typeof(ModelRemoteValueUri<>);
                var genericType = type.MakeGenericType(new Type[] { typeof(T) });
                return (IObjectRemoteValue<T>)Activator.CreateInstance(genericType, new object[] { obj });
            }
            if (obj is IModelElementTuple)
            {
                var type = typeof(ModelRemoteValueTuple<>);
                var genericType = type.MakeGenericType(new Type[] { typeof(T) });
                return (IObjectRemoteValue<T>)Activator.CreateInstance(genericType, new object[] { obj, sendContext });
            }
            else
            {
                return new ModelRemoteValueObject<T>(obj);
            }
        }
    }
}