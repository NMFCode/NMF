using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Serialization
{
    public class UnknownElementEventArgs : EventArgs
    {     
        public UnknownElementEventArgs(object context, string propertyXml)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (propertyXml == null) throw new ArgumentNullException("propertyXml");

            Context = context;
            PropertyXml = propertyXml;
        }

        public object Context { get; }

        public string PropertyXml { get; }
    }
}
