using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    public class UnknownTypeEventArgs : EventArgs
    {
        public IPropertySerializationInfo Property { get; }

        public string Namespace { get; }

        public string LocalName { get; }

        public ITypeSerializationInfo Type { get; set; }

        public UnknownTypeEventArgs(IPropertySerializationInfo property, string ns, string localName)
        {
            if (localName == null) throw new ArgumentNullException(nameof(localName));

            Namespace = ns;
            LocalName = localName;
            Property = property;
        }
    }
}
