using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NMF.Serialization
{
    public class XmlSerializationContext
    {
        private struct ObjectPropertyPair : IEquatable<ObjectPropertyPair>
        {
            public object Object { get; set; }

            public IPropertySerializationInfo Property { get; set; }

            public bool Equals(ObjectPropertyPair other)
            {
                return Object == other.Object && Property == other.Property;
            }

            public override bool Equals(object obj)
            {
                if (obj != null && obj is ObjectPropertyPair) return Equals((ObjectPropertyPair)obj);
                return false;
            }

            public override int GetHashCode()
            {
                var hashCode = 0;
                if (Object != null) hashCode = Object.GetHashCode();
                if (Property != null) hashCode ^= Property.GetHashCode();
                return hashCode;
            }
        }

        public XmlSerializationContext(object root)
        {
            Root = root;
        }

        private Dictionary<Type, Dictionary<string, object>> idStore = new Dictionary<Type, Dictionary<string, object>>();
        private HashSet<ObjectPropertyPair> blockedProperties = new HashSet<ObjectPropertyPair>();

        private Queue<XmlIdentifierDelay> lostProperties = new Queue<XmlIdentifierDelay>();
        private Queue<ISupportInitialize> inits = new Queue<ISupportInitialize>();

        internal Queue<XmlIdentifierDelay> LostProperties { get { return lostProperties; } }
        internal Queue<ISupportInitialize> Inits { get { return inits; } }

        public virtual void RegisterId(string id, object value)
        {
            if (value != null)
            {
                var type = value.GetType();
                RegisterId(id, value, type);
                foreach (var iFace in type.GetInterfaces())
                {
                    RegisterId(id, value, iFace);
                }
            }
        }

        public virtual void Cleanup()
        {
            while (lostProperties.Count > 0)
            {
                XmlIdentifierDelay p = lostProperties.Dequeue();
                p.OnResolveIdentifiedObject(Resolve(p.Identifier, p.TargetType.Type), this);
            }
            while (inits.Count > 0)
            {
                inits.Dequeue().EndInit();
            }
        }

        private void RegisterId(string id, object value, Type type)
        {
            Dictionary<string, object> dict;
            if (!idStore.TryGetValue(type, out dict))
            {
                dict = new Dictionary<string, object>();
                idStore.Add(type, dict);
            }
            if (!dict.ContainsKey(id))
            {
                dict.Add(id, value);
            }
            else
            {
                dict[id] = value;
            }
            if (type.BaseType != null)
            {
                RegisterId(id, value, type.BaseType);
            }
        }

        public virtual bool ContainsId(string id, Type type)
        {
            Dictionary<string, object> dict;
            if (type != null && idStore.TryGetValue(type, out dict))
            {
                return dict.ContainsKey(id);
            }
            else
            {
                return false;
            }
        }

        public virtual object Resolve(string id, Type type)
        {
            Dictionary<string, object> dict;
            object obj;
            if (idStore.TryGetValue(type, out dict) && dict.TryGetValue(id, out obj))
            {
                return obj;
            }
            else
            {
                return null;
            }
        }

        public bool IsOppositeSet(object instance, IPropertySerializationInfo property)
        {
            if (property == null || property.Opposite == null) return false;
            var pair = new ObjectPropertyPair() { Object = instance, Property = property };
            return blockedProperties.Contains(pair);
        }

        public void BlockOpposite(object value, IPropertySerializationInfo property)
        {
            if (property == null || property.Opposite == null) return;
            var pair = new ObjectPropertyPair() { Object = value, Property = property.Opposite };
            blockedProperties.Add(pair);
        }

        public object Root
        {
            get;
            private set;
        }
    }
}
