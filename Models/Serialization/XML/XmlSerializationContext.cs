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
        private Dictionary<Type, List<Dictionary<string, object>>> pathStore = new Dictionary<Type, List<Dictionary<string, object>>>();
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
            }
        }

        public virtual void Cleanup()
        {
            while (lostProperties.Count > 0)
            {
                XmlIdentifierDelay p = lostProperties.Dequeue();
                var resolved = Resolve(p.Identifier, p.TargetType.Type);
                if (resolved == null)
                {
                    throw new InvalidOperationException(string.Format("The reference {0} could not be resolved", p.Identifier));
                }
                p.OnResolveIdentifiedObject(resolved, this);
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
                dict[id] = new NameClash();
            }
        }

        private class NameClash { }

        public virtual bool ContainsId(string id, Type type)
        {
            Dictionary<string, object> dict;
            if (idStore.TryGetValue(type, out dict))
            {
                return dict.ContainsKey(id);
            }
            else
            {
                List<Dictionary<string, object>> paths = GetOrCreateTypeStores(type);
                foreach (var typeStore in paths)
                {
                    if (typeStore.ContainsKey(id))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public virtual object Resolve(string id, Type type)
        {
            Dictionary<string, object> dict;
            object obj;
            if (idStore.TryGetValue(type, out dict) && dict.TryGetValue(id, out obj))
            {
                if (obj is NameClash)
                {
                    throw new InvalidOperationException(string.Format("The id {0} is not unique for type {1}!", id, type.Name));
                }
                return obj;
            }
            else
            {
                List<Dictionary<string, object>> paths = GetOrCreateTypeStores(type);
                foreach (var typeStore in paths)
                {
                    if (typeStore.TryGetValue(id, out obj))
                    {
                        if (obj is NameClash)
                        {
                            throw new InvalidOperationException(string.Format("The id {0} is not unique for type {1}!", id, type.Name));
                        }
                        return obj;
                    }
                }
                return null;
            }
        }

        private List<Dictionary<string, object>> GetOrCreateTypeStores(Type type)
        {
            // we have to search all paths
            List<Dictionary<string, object>> paths;
            if (!pathStore.TryGetValue(type, out paths))
            {
                paths = new List<Dictionary<string, object>>();
                foreach (var pair in idStore)
                {
                    if (type.IsAssignableFrom(pair.Key))
                    {
                        paths.Add(pair.Value);
                    }
                }
                pathStore.Add(type, paths);
            }

            return paths;
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
