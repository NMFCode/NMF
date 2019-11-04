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

        private Dictionary<ITypeSerializationInfo, Dictionary<string, object>> idStore = new Dictionary<ITypeSerializationInfo, Dictionary<string, object>>();
        private Dictionary<ITypeSerializationInfo, List<Dictionary<string, object>>> pathStore = new Dictionary<ITypeSerializationInfo, List<Dictionary<string, object>>>();
        private HashSet<ObjectPropertyPair> blockedProperties = new HashSet<ObjectPropertyPair>();

        private Queue<XmlIdentifierDelay> lostProperties = new Queue<XmlIdentifierDelay>();
        private Queue<ISupportInitialize> inits = new Queue<ISupportInitialize>();

        internal Queue<XmlIdentifierDelay> LostProperties { get { return lostProperties; } }
        internal Queue<ISupportInitialize> Inits { get { return inits; } }

        public virtual void Cleanup()
        {
            while (lostProperties.Count > 0)
            {
                XmlIdentifierDelay p = lostProperties.Dequeue();
                var resolved = Resolve(p.Identifier, p.TargetType, p.TargetMinType, failOnConflict: true, source: p.Target);
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

        public void RegisterId(string id, object value, ITypeSerializationInfo type)
        {
            if (id == null) return;
            Dictionary<string, object> dict;
            if (!idStore.TryGetValue(type, out dict))
            {
                dict = new Dictionary<string, object>();
                idStore.Add(type, dict);
            }
            object current;
            if (!dict.TryGetValue(id, out current))
            {
                dict.Add(id, value);
            }
            else
            {
                var clash = current as NameClash;
                if (clash == null)
                {
                    clash = new NameClash();
                    clash.Objects.Add(current);
                    dict[id] = clash;
                }
                clash.Objects.Add(value);
            }
        }

        private class NameClash
        {
            public readonly List<object> Objects = new List<object>();
        }

        public virtual bool ContainsId(string id, ITypeSerializationInfo type)
        {
            if (id == null) return false;
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

        protected virtual object OnNameClash(string id, ITypeSerializationInfo type, IEnumerable<object> candidates, object source)
        {
            throw new XmlResolveNameClashException(id, type, candidates);
        }

        public virtual object Resolve(string id, ITypeSerializationInfo type, Type minType = null, bool failOnConflict = true, object source = null)
        {
            if (id == null) return null;
            Dictionary<string, object> dict;
            object obj;
            if (idStore.TryGetValue(type, out dict) && dict.TryGetValue(id, out obj))
            {
                var clash = obj as NameClash;
                if (clash != null)
                {
                    if (failOnConflict)
                    {
                        return OnNameClash(id, type, clash.Objects.AsReadOnly(), source);
                    }
                    else
                    {
                        return null;
                    }
                }
                return obj;
            }
            else
            {
                object result = null;
                List<Dictionary<string, object>> paths = GetOrCreateTypeStores(type);
                foreach (var typeStore in paths)
                {
                    if (typeStore.TryGetValue(id, out obj))
                    {
                        if (result == null)
                        {
                            result = obj;
                        }
                        else
                        {
                            var clash = result as NameClash;
                            if (clash == null)
                            {
                                clash = new NameClash();
                                clash.Objects.Add(result);
                                result = clash;
                            }
                            clash.Objects.Add(obj);
                        }
                    }
                }
                var resultClash = result as NameClash;
                if (resultClash != null)
                {
                    if (failOnConflict)
                    {
                        return OnNameClash(id, type, resultClash.Objects.AsReadOnly(), source);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return result;
                }
            }
        }

        private List<Dictionary<string, object>> GetOrCreateTypeStores(ITypeSerializationInfo type)
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

        public bool IsBlocked(object instance, IPropertySerializationInfo property)
        {
            if (property == null || property.Opposite == null) return false;
            var pair = new ObjectPropertyPair() { Object = instance, Property = property };
            return blockedProperties.Contains(pair);
        }

        public void BlockProperty(object value, IPropertySerializationInfo property)
        {
            if (property == null || value == null) return;
            var pair = new ObjectPropertyPair() { Object = value, Property = property };
            blockedProperties.Add(pair);
        }

        public object Root
        {
            get;
            private set;
        }
    }

    public class XmlResolveNameClashException : Exception
    {
        public XmlResolveNameClashException(string id, ITypeSerializationInfo type, IEnumerable<object> candidates)
            : base($"Resolving failed because the id {id} is not unique for type {type}.")
        {
            Id = id;
            Type = type;
            Candidates = candidates;
        }

        public IEnumerable<object> Candidates { get; private set; }

        public string Id { get; private set; }

        public ITypeSerializationInfo Type { get; private set; }
    }
}
