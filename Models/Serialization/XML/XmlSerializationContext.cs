using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace NMF.Serialization
{
    /// <summary>
    /// Denotes the context of a XML deserialization
    /// </summary>
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

        /// <summary>
        /// Creates a new context for a deserialization
        /// </summary>
        /// <param name="root">The root object</param>
        public XmlSerializationContext(object root)
        {
            Root = root;
        }

        private readonly Dictionary<ITypeSerializationInfo, Dictionary<string, object>> idStore = new Dictionary<ITypeSerializationInfo, Dictionary<string, object>>();
        private readonly Dictionary<ITypeSerializationInfo, List<Dictionary<string, object>>> pathStore = new Dictionary<ITypeSerializationInfo, List<Dictionary<string, object>>>();
        private readonly HashSet<ObjectPropertyPair> blockedProperties = new HashSet<ObjectPropertyPair>();

        private readonly Queue<XmlIdentifierDelay> lostProperties = new Queue<XmlIdentifierDelay>();
        private readonly Queue<ISupportInitialize> inits = new Queue<ISupportInitialize>();

        internal Queue<XmlIdentifierDelay> LostProperties { get { return lostProperties; } }
        internal Queue<ISupportInitialize> Inits { get { return inits; } }

        /// <summary>
        /// Ends the deserialization
        /// </summary>
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

        /// <summary>
        /// Registers an object for the given id
        /// </summary>
        /// <param name="id">The id that is registered</param>
        /// <param name="value">The object that is registered</param>
        /// <param name="type">The type for which the value is registered</param>
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
                if (current is not NameClash clash)
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

        /// <summary>
        /// Determines whether the context knows an element of the given id
        /// </summary>
        /// <param name="id">The id of the element</param>
        /// <param name="type">The expected type of the element</param>
        /// <returns>True, if the id can be found, otherwise False</returns>
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

        /// <summary>
        /// Gets called when there is a name clash
        /// </summary>
        /// <param name="id">The id that was requested</param>
        /// <param name="type">The type</param>
        /// <param name="candidates">The candidates</param>
        /// <param name="source">The source</param>
        /// <returns>The object that should be chosen in the case of a clash</returns>
        protected virtual object OnNameClash(string id, ITypeSerializationInfo type, IEnumerable<object> candidates, object source)
        {
            throw new XmlResolveNameClashException(id, type, candidates);
        }

        /// <summary>
        /// Resolves the given id
        /// </summary>
        /// <param name="id">The id that is resolved</param>
        /// <param name="type">The expected type</param>
        /// <param name="minType">The minimum type that is required</param>
        /// <param name="failOnConflict">If false, the method will return null in case of a conflict, otherwise conflict resolution is applied</param>
        /// <param name="source"></param>
        /// <returns>The resolved object</returns>
        public virtual object Resolve(string id, ITypeSerializationInfo type, Type minType = null, bool failOnConflict = true, object source = null)
        {
            if (id == null) return null;
            Dictionary<string, object> dict;
            object obj;
            if (idStore.TryGetValue(type, out dict) && dict.TryGetValue(id, out obj))
            {
                if (obj is NameClash clash)
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
                            if (result is not NameClash clash)
                            {
                                clash = new NameClash();
                                clash.Objects.Add(result);
                                result = clash;
                            }
                            clash.Objects.Add(obj);
                        }
                    }
                }
                if (result is NameClash resultClash)
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

        /// <summary>
        /// Determines whether the given property is blocked for the given instance
        /// </summary>
        /// <param name="instance">the instance</param>
        /// <param name="property">the property</param>
        /// <returns>True, if the property is blocked, which means that it should be ignored for the deserialization</returns>
        public bool IsBlocked(object instance, IPropertySerializationInfo property)
        {
            if (property == null || property.Opposite == null) return false;
            var pair = new ObjectPropertyPair() { Object = instance, Property = property };
            return blockedProperties.Contains(pair);
        }

        /// <summary>
        /// Blocks the given property for the given instance
        /// </summary>
        /// <param name="value">the instance</param>
        /// <param name="property">the property</param>
        public void BlockProperty(object value, IPropertySerializationInfo property)
        {
            if (property == null || value == null) return;
            var pair = new ObjectPropertyPair() { Object = value, Property = property };
            blockedProperties.Add(pair);
        }

        /// <summary>
        /// Gets the deserialization root
        /// </summary>
        public object Root
        {
            get;
            private set;
        }
    }

    /// <summary>
    /// Denotes the exception that an identifier has clashed
    /// </summary>
    public class XmlResolveNameClashException : Exception
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="id">the id</param>
        /// <param name="type">theexpected type of elements</param>
        /// <param name="candidates">The objects with the given id</param>
        public XmlResolveNameClashException(string id, ITypeSerializationInfo type, IEnumerable<object> candidates)
            : base($"Resolving failed because the id {id} is not unique for type {type}.")
        {
            Id = id;
            Type = type;
            Candidates = candidates;
        }

        /// <summary>
        /// The objects with the given id
        /// </summary>
        public IEnumerable<object> Candidates { get; private set; }

        /// <summary>
        /// The identifier
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The expected type of elements
        /// </summary>
        public ITypeSerializationInfo Type { get; private set; }
    }
}
