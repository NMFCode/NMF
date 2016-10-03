using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;
using NMF.Serialization;
using System.ComponentModel;

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
            _deletedElement.Parent = _deletedElementParent;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as ElementDeletion;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ -1; // so it's not the same as ElementCreation
        }
    }
}
