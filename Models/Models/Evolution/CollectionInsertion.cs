using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using NMF.Models.Repository;
using NMF.Serialization;

namespace NMF.Models.Evolution
{
    [XmlConstructor(2)]
    public abstract class CollectionInsertionBase<T> : IModelChange
    {
        [XmlAttribute(true)]
        [XmlConstructorParameter(0)]
        public Uri AbsoluteUri { get; set; }

        [XmlAttribute(true)]
        [XmlConstructorParameter(1)]
        public string CollectionPropertyName { get; set; }

        public CollectionInsertionBase(Uri absoluteUri, string collectionPropertyName)
        {
            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
        }

        public void Apply(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var coll = property.GetValue(parent, null) as ICollection<T>;

            if (coll != null)
            {
                var newElements = GetNewElements(repository);
                foreach (var element in newElements)
                {
                    coll.Add(element);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }
        
        public void Invert(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var coll = property.GetValue(parent, null) as ICollection<T>;
            var newElements = GetNewElements(repository);

            foreach (var elements in newElements)
            {
                coll.Remove(elements);
            }
        }

        protected abstract ICollection<T> GetNewElements(IModelRepository repository);
    }

    [XmlConstructor(2)]
    public class CollectionInsertionComposition<T> : CollectionInsertionBase<T>
    {
        public ICollection<T> NewElements { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public CollectionInsertionComposition(Uri absoluteUri, string collectionPropertyName)
            : base(absoluteUri, collectionPropertyName)
        {
            NewElements = new Collection<T>();
        }

        public CollectionInsertionComposition(Uri absoluteUri, string collectionPropertyName, ICollection<T> newElements)
            : base(absoluteUri, collectionPropertyName)
        {
            NewElements = newElements;
        }

        protected override ICollection<T> GetNewElements(IModelRepository repository)
        {
            return NewElements;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as CollectionInsertionComposition<T>;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                       && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                       && this.NewElements.SequenceEqual(other.NewElements);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                   ^ CollectionPropertyName?.GetHashCode() ?? 0
                   ^ NewElements.GetHashCode();
        }
    }

    [XmlConstructor(3)]
    public class CollectionInsertionAssociation<T> : CollectionInsertionBase<T> where T : class, IModelElement
    {
        public ICollection<Uri> NewElementUris { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public CollectionInsertionAssociation(Uri absoluteUri, string collectionPropertyName)
            : base(absoluteUri, collectionPropertyName)
        {
            NewElementUris = new Collection<Uri>();
        }

        public CollectionInsertionAssociation(Uri absoluteUri, string collectionPropertyName, ICollection<Uri> newElementUris)
            : base(absoluteUri, collectionPropertyName)
        {
            NewElementUris = newElementUris;
        }

        protected override ICollection<T> GetNewElements(IModelRepository repository)
        {
            return NewElementUris.Select(u => repository.Resolve(u)).Cast<T>().ToList();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as CollectionInsertionAssociation<T>;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                    && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                    && this.NewElementUris.SequenceEqual(other.NewElementUris);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ CollectionPropertyName?.GetHashCode() ?? 0
                ^ NewElementUris.GetHashCode();
        }
    }
}
