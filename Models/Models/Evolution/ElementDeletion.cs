using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;

namespace NMF.Models.Evolution
{
    public class ElementDeletion : IModelChange
    {
        public Uri AbsoluteUri { get { return Element.AbsoluteUri; } }

        public IModelElement Element { get; set; }

        public ElementDeletion(IModelElement deletedElement)
        {
            if (deletedElement == null)
                throw new ArgumentNullException(nameof(deletedElement));

            Element = deletedElement;
        }

        public void Apply(IModelRepository repository)
        {
            var element = repository.Resolve(AbsoluteUri);
            element.Delete();
        }

        public void Undo(IModelRepository repository)
        {
            new ElementCreation(Element).Apply(repository);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as ElementDeletion;
            if (other == null)
                return false;
            else
                return this.Element.Equals(other.Element);
        }

        public override int GetHashCode()
        {
            return Element.GetHashCode()
                ^ -1; // so it's not the same as ElementCreation
        }
    }
}
