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
    }

    class XmlAddToCollectionDelay : XmlIdentifierDelay
    {
        public ITypeSerializationInfo Type { get; set; }

        public override void OnResolveIdentifiedObject(object instance, XmlSerializationContext context)
        {
            Type.AddToCollection(Target, instance);
        }

        public override ITypeSerializationInfo TargetType
        {
            get { return Type.CollectionItemType; }
        }
    }
}
