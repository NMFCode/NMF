using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Serialization
{
    public class UnknownAttributeEventArgs : EventArgs
    {
        public object Context { get; }
        public string Namespace { get; }

        public string Name { get; }

        public string Value { get; }

        public UnknownAttributeEventArgs(object context, string ns, string name, string value)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (value == null) throw new ArgumentNullException(nameof(value));

            Namespace = ns;
            Name = name;
            Value = value;
            Context = context;
        }
    }
}
