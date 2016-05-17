using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;

namespace NMF.Models.Evolution
{
    public class CollectionInsertion : IModelChange
    {
        public CollectionInsertion(Uri absoluteUri, string collectionPropertyName, object newElement, int index)
        {
            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));
            if (string.IsNullOrEmpty(collectionPropertyName))
                throw new ArgumentNullException(nameof(collectionPropertyName));

            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
            Element = newElement;
            Index = index;
        }

        public Uri AbsoluteUri { get; private set; }

        public string CollectionPropertyName { get; private set; }

        public int Index { get; set; }

        public object Element { get; set; }

        public void Apply(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            object collection = property.GetValue(parent, null);

            /* TODO ugly! Check if the collection type implements an interface
             * starting with "IList". We cannot check this directly, because runtime
             * types of generic types have dynamic names like "IList`1". If it is
             * a list we'll use its "Insert" method. Otherwise the collection must
             * be an instance of "ICollection<T>", so we use the "Add" method.
             * This would be a lot easier if the collection would implement
             * "ICollection" in addition to its generic variant, since we could
             * simply cast our collection instance and avoid reflection.
             */
            
            var iList = property.PropertyType.GetInterfaces().FirstOrDefault(i => i.Name.StartsWith("IList"));
            if (iList != null)
            {
                iList.GetMethod("Insert").Invoke(collection, new object[] { Index, Element });
            }
            else
            {
                var iCollection = property.PropertyType.GetInterfaces().FirstOrDefault(i => i.Name.StartsWith("ICollection"));
                iCollection.GetMethod("Add").Invoke(collection, new[] { Element });
            }
        }

        public void Undo(IModelRepository repository)
        {
            new CollectionDeletion(AbsoluteUri, CollectionPropertyName, Element, Index).Apply(repository);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as CollectionInsertion;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                    && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                    && this.Element.Equals(other.Element)
                    && this.Index.Equals(other.Index);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri.GetHashCode()
                ^ CollectionPropertyName.GetHashCode()
                ^ Element.GetHashCode()
                ^ Index.GetHashCode()
                ^ -1; // so it's not the same as CollectionDeletion
        }
    }
}
