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
        [XmlConstructorParameter(0)]
        public Uri AbsoluteUri { get; set; }

        public ElementDeletion(Uri absoluteUri)
        {
            if (absoluteUri == null)
                throw new ArgumentException(nameof(absoluteUri));

            AbsoluteUri = absoluteUri;
        }

        public void Apply(IModelRepository repository)
        {
            var element = repository.Resolve(AbsoluteUri);
            element.Delete();
        }

        public void Undo()
        {
            //TODO
            throw new NotImplementedException();
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
