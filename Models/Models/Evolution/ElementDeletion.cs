using System;
using System.ComponentModel;
using NMF.Models.Repository;
using NMF.Serialization;

namespace NMF.Models.Evolution
{
    [XmlConstructor(1)]
    public class ElementDeletion : IModelChange
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private readonly IModelElement _deletedElement;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        private readonly IModelElement _deletedElementParent;

        [XmlConstructorParameter(0)]
        public Uri AbsoluteUri { get; set; }

        public ElementDeletion(Uri absoluteUri, IModelElement deletedElement)
        {
            if (absoluteUri == null)
                throw new ArgumentException(nameof(absoluteUri));
            if (deletedElement == null)
                throw new ArgumentNullException(nameof(deletedElement));

            AbsoluteUri = absoluteUri;
            _deletedElement = deletedElement;
            _deletedElementParent = _deletedElement.Parent;
        }

        public void Apply(IModelRepository repository)
        {
            var element = repository.Resolve(AbsoluteUri);
            element.Delete();
        }

        public void Invert(IModelRepository repository)
        {
            //Is done automatically by NMF
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
