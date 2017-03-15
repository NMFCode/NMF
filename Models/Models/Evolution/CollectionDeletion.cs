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
    public abstract class CollectionDeletionBase<T> : IModelChange
    {
        [XmlConstructorParameter(0)]
        public Uri AbsoluteUri { get; set; }

        [XmlConstructorParameter(1)]
        public string CollectionPropertyName { get; set; }

        private IModelElement _collectionContainer;
        private IModelElement _containerParent;

        public void Apply(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var coll = property.GetValue(parent, null) as ICollection<T>;
            var oldItems = GetOldElements(repository);

            foreach (var item in oldItems)
            {
                coll.Remove(item);
            }
        }
        public CollectionDeletionBase(Uri absoluteUri, string collectionPropertyName)
        {

            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));
            if (string.IsNullOrEmpty(collectionPropertyName))
                throw new ArgumentNullException(nameof(collectionPropertyName));

            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
        }

        public CollectionDeletionBase(Uri absoluteUri, string collectionPropertyName, IModelElement collectionContainer,
            IModelElement containerParent) : this(absoluteUri, collectionPropertyName)
        {
            _collectionContainer = collectionContainer;
            _containerParent = containerParent;
        }

        public void Invert(IModelRepository repository)
        {
            _collectionContainer.Parent = _collectionContainer.Parent ?? _containerParent;
            var parent = repository.Resolve(AbsoluteUri) ?? _collectionContainer;
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var coll = property.GetValue(parent, null) as ICollection<T>;

            if (coll != null)
            {
                var oldItems = GetOldElements(repository);
                foreach (var item in oldItems)
                {
                    coll.Add(item);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected abstract ICollection<T> GetOldElements(IModelRepository repository);
    }

    [XmlConstructor(2)]
    public class CollectionDeletionComposition<T> : CollectionDeletionBase<T>
    {
        public ICollection<T> OldElements { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public CollectionDeletionComposition(Uri absoluteUri, string collectionPropertyName)
            : base(absoluteUri, collectionPropertyName)
        {
            OldElements = new Collection<T>();
        }

        public CollectionDeletionComposition(Uri absoluteUri, string collectionPropertyName, ICollection<T> oldElements)
            : base(absoluteUri, collectionPropertyName)
        {
            OldElements = oldElements;
        }

        public CollectionDeletionComposition(Uri absoluteUri, string collectionPropertyName, ICollection<T> oldElements,
            IModelElement collectionContainer, IModelElement containerParent)
            : base(absoluteUri, collectionPropertyName, collectionContainer, containerParent)
        {
            OldElements = oldElements;
        }

        protected override ICollection<T> GetOldElements(IModelRepository repository)
        {
            return OldElements;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as CollectionDeletionComposition<T>;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                       && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                       && this.OldElements.SequenceEqual(other.OldElements);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                   ^ CollectionPropertyName?.GetHashCode() ?? 0
                   ^ OldElements.GetHashCode();
        }
    }

    [XmlConstructor(2)]
    public class CollectionDeletionAssociation<T> : CollectionDeletionBase<T> where T : class, IModelElement
    {
        public ICollection<Uri> OldElementUris { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public CollectionDeletionAssociation(Uri absoluteUri, string collectionPropertyName)
            : base(absoluteUri, collectionPropertyName)
        {
            OldElementUris = new Collection<Uri>();
        }

        public CollectionDeletionAssociation(Uri absoluteUri, string collectionPropertyName, ICollection<Uri> oldElementUris)
            : base(absoluteUri, collectionPropertyName)
        {
            OldElementUris = oldElementUris;
        }

        protected override ICollection<T> GetOldElements(IModelRepository repository)
        {
            return OldElementUris.Select(u => repository.Resolve(u)).Cast<T>().ToList();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as CollectionDeletionAssociation<T>;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                    && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                    && this.OldElementUris.SequenceEqual(other.OldElementUris);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ CollectionPropertyName?.GetHashCode() ?? 0
                ^ OldElementUris.GetHashCode();
        }
    }
}
