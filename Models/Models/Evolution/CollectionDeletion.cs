using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;
using System.Collections;
using NMF.Serialization;

namespace NMF.Models.Evolution
{
    public class CollectionDeletion : IModelChange
    {
        public CollectionDeletion(Uri absoluteUri, string collectionPropertyName, IEnumerable elementsToDelete, int index)
        {
            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));
            if (string.IsNullOrEmpty(collectionPropertyName))
                throw new ArgumentNullException(nameof(collectionPropertyName));

            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
            Elements = elementsToDelete.Cast<object>().ToList();
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

            //TODO same uglyness as in ModelCollectionInsertion

            var iCollection = property.PropertyType.GetInterfaces().FirstOrDefault(i => i.Name.StartsWith("ICollection"));
            var removeMethod = iCollection.GetMethod("Remove");
            foreach (var item in Elements)
                removeMethod.Invoke(collection, new[] { item });
        }

        public void Undo(IModelRepository repository)
        {
            new CollectionInsertion(AbsoluteUri, CollectionPropertyName, Elements, Index).Apply(repository);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as CollectionDeletion;
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
                ^ Index.GetHashCode();
        }
    }
}
