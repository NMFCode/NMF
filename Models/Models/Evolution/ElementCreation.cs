using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;
using NMF.Serialization;
using System.ComponentModel;

namespace NMF.Models.Evolution
{
    public class ElementCreation : IModelChange
    {
        public Uri AbsoluteUri { get { return Element.AbsoluteUri; } }
        
        public IModelElement Element { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ElementCreation() { }

        public ElementCreation(IModelElement createdElement)
        {
            if (createdElement == null)
                throw new ArgumentNullException(nameof(createdElement));
            
            Element = createdElement;
        }

        public void Apply(IModelRepository repository)
        {
            //NMF does this automatically when adding a new element to a parent
        }

        public void Invert(IModelRepository repository)
        {
            //Is done automatically by NMF
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
            return Element?.GetHashCode() ?? 0;
        }
    }
}
