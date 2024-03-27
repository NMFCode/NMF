using System;

namespace NMF.Serialization
{
    internal abstract class IdentifierDelay
    {

        public string Identifier
        {
            get;
            set;
        }

        public object Target
        {
            get;
            set;
        }

        public abstract void OnResolveIdentifiedObject(object instance, XmlSerializationContext context);

        public abstract ITypeSerializationInfo TargetType { get; }

        public abstract Type TargetMinType { get; }
    }

    internal class SetPropertyDelay : IdentifierDelay
    {
        public IPropertySerializationInfo Property
        {
            get;
            set;
        }

        public override void OnResolveIdentifiedObject(object instance, XmlSerializationContext context)
        {
            Property.SetValue(Target, instance, context);
        }

        public override ITypeSerializationInfo TargetType
        {
            get { return Property.PropertyType; }
        }

        public override Type TargetMinType
        {
            get
            {
                return Property.PropertyMinType;
            }
        }
    }

    internal class AddToPropertyDelay : IdentifierDelay
    {
        public IPropertySerializationInfo Property { get; private set; }

        public AddToPropertyDelay(IPropertySerializationInfo property)
        {
            Property = property;
        }

        public override ITypeSerializationInfo TargetType
        {
            get { return Property.PropertyType.CollectionItemType; }
        }

        public override Type TargetMinType
        {
            get
            {
                if (Property.PropertyType is XmlTypeSerializationInfo tsi) return tsi.CollectionItemRawType;
                return null;
            }
        }

        public override void OnResolveIdentifiedObject(object instance, XmlSerializationContext context)
        {
            Property.AddToCollection(Target, instance, context);
        }
    }

    internal class AddToCollectionDelay : IdentifierDelay
    {
        public AddToCollectionDelay(ITypeSerializationInfo type)
        {
            Type = type;
        }

        public ITypeSerializationInfo Type { get; private set; }
        
        public override void OnResolveIdentifiedObject(object instance, XmlSerializationContext context)
        {
            Type.AddToCollection(Target, instance);
        }

        public override ITypeSerializationInfo TargetType
        {
            get { return Type.CollectionItemType; }
        }

        public override Type TargetMinType
        {
            get
            {
                return null;
            }
        }
    }
}
