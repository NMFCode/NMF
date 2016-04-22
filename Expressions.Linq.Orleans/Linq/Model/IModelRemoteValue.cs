using System;
using NMF.Models;

namespace NMF.Expressions.Linq.Orleans.Model
{
    public static class ModelRemoteValueFactory
    {
        // TODO check if name factory works
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
                var type = typeof (ModelRemoteValueUri<>);
                var genericType = type.MakeGenericType(new Type[] {typeof (T)});
                return (IModelRemoteValue<T>) Activator.CreateInstance(genericType, new object[] {obj});
            }
            else
            {
                return new ModelRemoteValueObject<T>(obj);
            }
        }
    }

    [Serializable]
    public class ModelRemoteValueObject<T> : IModelRemoteValue<T>
    {
        public ModelRemoteValueObject(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }

        public T Retrieve(Models.Model lookupModel)
        {
            return Value;
        }

        object IModelRemoteValue.Retrieve(Models.Model lookupModel)
        {
            return Retrieve(lookupModel);
        }
    }

    [Serializable]
    public class ModelRemoteValueUri<T> : IModelRemoteValue<T> where T : IModelElement
    {
        public ModelRemoteValueUri(IModelElement modelElement)
        {
            RootRelativeUri = modelElement.RelativeUri;
        }

        public Uri RootRelativeUri { get; private set; }

        public T Retrieve(Models.Model lookupModel)
        {
            return (T) lookupModel.Resolve(RootRelativeUri);
        }

        object IModelRemoteValue.Retrieve(Models.Model lookupModel)
        {
            return Retrieve(lookupModel);
        }
    }

    public interface IModelRemoteValue<T> : IModelRemoteValue
    {
        new T Retrieve(Models.Model lookupModel);
    }

    public interface IModelRemoteValue
    {
        object Retrieve(Models.Model lookupModel);
    }
}