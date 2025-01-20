using NMF.Serialization.Xmi;

namespace NMF.Models.Repository.Serialization
{
    internal class ModelIdAttribute : XmiArtificialIdAttribute
    {
        internal static readonly ModelIdAttribute instance = new ModelIdAttribute();

        public override bool ShouldSerializeValue(object obj, object value)
        {
            return obj is not IModelElement modelElement || modelElement.AbsoluteUri == null;
        }
    }
}
