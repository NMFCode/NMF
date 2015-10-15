using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Serialization.Xml
{
    public class XmlSerializationEntity : ISerializationEntity
    {
        private XmlSerializer serializer;

        private Dictionary<ITypeSerializationInfo, Dictionary<string, object>> idStore = new Dictionary<ITypeSerializationInfo, Dictionary<string, object>>();

        public XmlSerializationEntity(XmlSerializer serializer, Uri uri, object root)
        {
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (root == null) throw new ArgumentNullException("root");

            Serializer = serializer;
            Uri = uri;
            Root = root;
        }

        public Uri Uri
        {
            get;
            private set;
        }

        public XmlSerializer Serializer
        {
            get;
            private set;
        }

        public object Root
        {
            get;
            private set;
        }

        public object Resolve(Uri relativeUri, Type type)
        {
            return Resolve(relativeUri.OriginalString, Serializer.GetSerializationInfo(type, false));
        }

        ISerializer ISerializationEntity.Serializer
        {
            get { return serializer; }
        }

        public virtual void RegisterId(string id, object value, ISerializationContext context)
        {
            if (value != null)
            {
                var type = Serializer.GetSerializationInfo(value.GetType(), true);
                InsertIntoIdStore(id, value, type);
                context.Register(value, new Uri(id, UriKind.Relative), this);
            }
        }

        private void InsertIntoIdStore(string id, object value, ITypeSerializationInfo type)
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
            if (type.BaseTypes != null)
            {
                foreach (var baseType in type.BaseTypes)
                {
                    InsertIntoIdStore(id, value, baseType);
                }
            }
        }

        public virtual bool ContainsId(string id, ITypeSerializationInfo type)
        {
            if (idStore.ContainsKey(type))
            {
                return idStore[type].ContainsKey(id);
            }
            else
            {
                return false;
            }
        }

        public virtual object Resolve(string id, ITypeSerializationInfo type)
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

        public virtual Uri GetLocalUri(object item)
        {
            if (item == null) return null;
            var typeInfo = Serializer.GetSerializationInfo(item.GetType(), true);
            if (typeInfo.IsIdentified)
            {
                var id = typeInfo.IdentifierProperty.GetValue(item, this);
                return id != null ? new Uri(id.ToString(), UriKind.Relative) : null;
            }
            else
            {
                return null;
            }
        }
    }
}
