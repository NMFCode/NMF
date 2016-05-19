using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;

namespace NMF.Models.Evolution
{
    public class ElementCreation : IModelChange
    {
        public Uri AbsoluteUri { get { return Element.AbsoluteUri; } }

        public IModelElement Element { get; private set; }

        public ElementCreation(IModelElement createdElement)
        {
            if (createdElement == null)
                throw new ArgumentNullException(nameof(createdElement));

            Element = createdElement;
        }

        public void Apply(IModelRepository repository)
        {
            throw new NotImplementedException();
        }

        public void Undo(IModelRepository repository)
        {
            new ElementDeletion(Element).Apply(repository);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as ElementCreation;
            if (other == null)
                return false;
            else
                return this.Element.Equals(other.Element);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri.GetHashCode()
                ^ Element.GetHashCode();
        }
    }
}
