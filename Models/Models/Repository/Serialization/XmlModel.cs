using NMF.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository.Serialization
{
    [XmlElementName("XMI")]
    [XmlNamespaceAttribute("http://www.omg.org/XMI")]
    [XmlNamespacePrefixAttribute("xmi")]
    [ModelRepresentationClassAttribute("http://www.omg.org/XMI")]
    public class XmlModel : Model
    {
        internal Dictionary<string, IModelElement> IdStore { get; set; }

        public override IModelElement Resolve(string path)
        {
            if (IdStore != null && path != null)
            {
                IModelElement element;
                if (IdStore.TryGetValue(path, out element) || (path.Length > 1 && path[0] == '#' && 
                    IdStore.TryGetValue(path.Substring(1), out element)))
                {
                    return element;
                }
            }
            return base.Resolve(path);
        }
    }
}
