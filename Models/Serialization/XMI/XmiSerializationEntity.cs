using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NMF.Serialization.Xmi
{
    /// <summary>
    /// Denotes a deserialization context for XMI
    /// </summary>
    public class XmiSerializationContext : XmlSerializationContext
    {
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="root">the root object of the deserialization</param>
        public XmiSerializationContext(object root) : base(root) { }

        private readonly Dictionary<object, string> ids = new Dictionary<object, string>();

        /// <summary>
        /// Gets the identifier of the given object
        /// </summary>
        /// <param name="input">the object</param>
        /// <returns>the objects identifier</returns>
        public string GetId(object input)
        {
            string id;
            if (!ids.TryGetValue(input, out id))
            {
                id = "$" + ids.Count;
                ids.Add(input, id);
            }
            return id;
        }

        /// <summary>
        /// Sets the identifier of the given object
        /// </summary>
        /// <param name="input">the object</param>
        /// <param name="id">the identifier</param>
        public void SetId(object input, string id)
        {
            if (!ids.ContainsKey(input))
            {
                ids.Add(input, id);
            }
            else
            {
                ids[input] = id;
            }
        }

        /// <summary>
        /// Gets the stored identfiers
        /// </summary>
        public IEnumerable<KeyValuePair<string, object>> IDs
        {
            get
            {
                return ids.Select(pair => new KeyValuePair<string, object>(pair.Value, pair.Key));
            }
        }
    }
}
