using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NMF.Serialization.Xmi
{
    public class XmiSerializationContext : XmlSerializationContext
    {
        public XmiSerializationContext(object root) : base(root) { }

        private Dictionary<object, string> ids = new Dictionary<object, string>();

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

        public IEnumerable<KeyValuePair<string, object>> IDs
        {
            get
            {
                return ids.Select(pair => new KeyValuePair<string, object>(pair.Value, pair.Key));
            }
        }
    }
}
