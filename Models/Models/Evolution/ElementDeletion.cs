using System;
using System.Collections.Generic;
using System.ComponentModel;
using NMF.Models.Repository;
using NMF.Serialization;

namespace NMF.Models.Evolution
{
    [XmlConstructor(1)]
    public class ElementDeletion : IModelChange
    {[XmlConstructorParameter(0)]
        public Uri AbsoluteUri { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IModelElement Element { get; }
        private IModelElement _parent;
        public ElementDeletion(Uri absoluteUri)
        {
            if (absoluteUri == null)
                throw new ArgumentException(nameof(absoluteUri));

            AbsoluteUri = absoluteUri;
        }

        public ElementDeletion(Uri absoluteUri, IModelElement element, IModelElement parent) : this(absoluteUri)
        {
            Element = element;
            _parent = parent;
        }

        public void Apply(IModelRepository repository)
        {
            var element = repository.Resolve(AbsoluteUri);
            element.Delete();
        }

        public void Invert(IModelRepository repository)
        {
            Element.Parent = _parent;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as ElementDeletion;
            return other != null && AbsoluteUri.Equals(other.AbsoluteUri);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ -1; // so it's not the same as ElementCreation
        }
    }
}
