using NMF.Serialization.Xmi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Repository.Serialization
{
    internal class ModelIdAttribute : XmiArtificialIdAttribute
    {
        internal static ModelIdAttribute instance = new ModelIdAttribute();

        public override bool ShouldSerializeValue(object obj, object value)
        {
            return obj is not IModelElement modelElement || modelElement.AbsoluteUri == null;
        }
    }
}
