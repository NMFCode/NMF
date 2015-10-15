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
            var modelElement = obj as IModelElement;
            return modelElement == null || modelElement.AbsoluteUri == null;
        }
    }
}
