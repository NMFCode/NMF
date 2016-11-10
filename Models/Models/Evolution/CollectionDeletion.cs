using System;
using System.Collections.Generic;
using NMF.Models.Repository;
using NMF.Serialization;

namespace NMF.Models.Evolution
{
    [XmlConstructor(4)]
    public abstract class CollectionDeletionBase<T> : IModelChange
    {
        [XmlConstructorParameter(0)]
        public Uri AbsoluteUri { get; set; }

        [XmlConstructorParameter(1)]
        public string CollectionPropertyName { get; set; }

        [XmlConstructorParameter(2)]
        public ICollection<T> OldItems { get; } //fragen ob das wirklich serialisiert werden sollte

        public void Apply(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var coll = property.GetValue(parent, null) as ICollection<T>;

            foreach (var item in OldItems)
            {
                coll.Remove(item);
            }
        }
        public CollectionDeletionBase(Uri absoluteUri, string collectionPropertyName, ICollection<T> oldItems)
        {

            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));
            if (string.IsNullOrEmpty(collectionPropertyName))
                throw new ArgumentNullException(nameof(collectionPropertyName));
            if (oldItems == null)
                throw new ArgumentNullException(nameof(oldItems));

            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
            OldItems = oldItems;
        }

        public void Invert(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var coll = property.GetValue(parent, null) as ICollection<T>;

            if (coll != null)
            {
                foreach (var item in OldItems)
                {
                    coll.Add(item);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ CollectionPropertyName?.GetHashCode() ?? 0
                ^ OldItems.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as CollectionDeletionBase<T>;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                    && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                    && this.OldItems.Equals(other.OldItems);
        }
    }
}
