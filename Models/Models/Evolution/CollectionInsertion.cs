using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;
using System.Collections;
using NMF.Serialization;

namespace NMF.Models.Evolution
{
    public class CollectionInsertion : IModelChange
    {
        public CollectionInsertion(Uri absoluteUri, string collectionPropertyName, IEnumerable newElements, int index)
        {
            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));
            if (string.IsNullOrEmpty(collectionPropertyName))
                throw new ArgumentNullException(nameof(collectionPropertyName));

            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
            Elements = newElements.Cast<object>().ToList();
            Index = index;
        }

        [XmlAttribute(true)]
        public Uri AbsoluteUri { get; set; }

        [XmlAttribute(true)]
        public string CollectionPropertyName { get; set; }

        [XmlAttribute(true)]
        public int Index { get; set; }

        public List<object> Elements { get; set; }

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

            var interfaces = property.PropertyType.GetInterfaces();
            var iList = interfaces.FirstOrDefault(i => i.Name.StartsWith("IList"));
            if (iList != null)
            {
                var insertMethod = iList.GetMethod("Insert");
                int offset = 0;
                foreach (var item in Elements)
                    insertMethod.Invoke(collection, new object[] { Index + offset++, item });
            }
            else
            {
                var iCollection = interfaces.FirstOrDefault(i => i.Name.StartsWith("ICollection"));
                var addMethod = iCollection.GetMethod("Add");
                foreach (var item in Elements)
                    addMethod.Invoke(collection, new[] { item });
            }
        }

        public void Undo(IModelRepository repository)
        {
            new CollectionDeletion(AbsoluteUri, CollectionPropertyName, Elements, Index).Apply(repository);
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
                    && this.Elements.SequenceEqual(other.Elements)
                    && this.Index.Equals(other.Index);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri.GetHashCode()
                ^ CollectionPropertyName.GetHashCode()
                ^ Elements.GetHashCode()
                ^ Index.GetHashCode()
                ^ -1; // so it's not the same as CollectionDeletion
        }
    }
}
