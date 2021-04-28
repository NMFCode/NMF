using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    abstract class XmlIdentifierDelay
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

    class XmlSetPropertyDelay : XmlIdentifierDelay
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

    class XmlAddToPropertyDelay : XmlIdentifierDelay
    {
        public IPropertySerializationInfo Property { get; private set; }

        public XmlAddToPropertyDelay(IPropertySerializationInfo property)
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

    class XmlAddToCollectionDelay : XmlIdentifierDelay
    {
        public XmlAddToCollectionDelay(ITypeSerializationInfo type)
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
