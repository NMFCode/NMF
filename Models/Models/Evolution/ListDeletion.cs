using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;
using System.Collections;
using NMF.Serialization;
using System.ComponentModel;

namespace NMF.Models.Evolution
{
    [XmlConstructor(4)]
    public abstract class ListDeletionBase<T> : IModelChange
    {
        [XmlConstructorParameter(0)]
        public Uri AbsoluteUri { get; set; }
        
        [XmlConstructorParameter(1)]
        public string CollectionPropertyName { get; set; }
        
        [XmlConstructorParameter(2)]
        public int StartingIndex { get; set; }

        [XmlConstructorParameter(3)]
        public int Count { get; set; }

        private IModelElement _collectionContainer;
        private IModelElement _containerParent;
        
        public ListDeletionBase(Uri absoluteUri, string collectionPropertyName, int startingIndex, int count)
        {
            
            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));
            if (string.IsNullOrEmpty(collectionPropertyName))
                throw new ArgumentNullException(nameof(collectionPropertyName));
            if (startingIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startingIndex));
            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
            StartingIndex = startingIndex;
            Count = count;
        }

        public ListDeletionBase(Uri absoluteUri, string collectionPropertyName, int startingIndex, int count,
            IModelElement collectionContainer, IModelElement containerParent) : this(absoluteUri, collectionPropertyName, startingIndex, count)
        {
            _collectionContainer = collectionContainer;
            _containerParent = containerParent;
        }

        public void Apply(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var list = property.GetValue(parent, null) as IList;
            
            for (int i = Math.Min(StartingIndex + Count - 1, list.Count - 1); i >= StartingIndex; i--)
            {
                list.RemoveAt(i);
            }
        }

        public void Invert(IModelRepository repository)
        {
            _collectionContainer.Parent = _collectionContainer.Parent ?? _containerParent;
            var parent = repository.Resolve(AbsoluteUri) ?? _collectionContainer;
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var list = property.GetValue(parent, null) as IList;
            var oldItems = GetOldElements(repository);

            if (list != null)
            {
                for (var i = 0; i < Count; i++)
                {
                    list.Insert(StartingIndex + i, oldItems[i]);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        protected abstract List<T> GetOldElements(IModelRepository repository);
    }

    [XmlConstructor(4)]
    public class ListDeletionComposition<T> : ListDeletionBase<T>
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<T> OldElements { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ListDeletionComposition(Uri absoluteUri, string collectionPropertyName, int startingIndex, int count)
            : base(absoluteUri, collectionPropertyName, startingIndex, count)
        {
            OldElements = new List<T>();
        }

        public ListDeletionComposition(Uri absoluteUri, string collectionPropertyName, int startingIndex, int count, List<T> oldElements)
            : base(absoluteUri, collectionPropertyName, startingIndex, count)
        {
            OldElements = oldElements;
        }

        public ListDeletionComposition(Uri absoluteUri, string collectionPropertyName, int startingIndex, int count, List<T> oldElements, IModelElement collectionContainer, IModelElement containerParent)
            : base(absoluteUri, collectionPropertyName, startingIndex, count, collectionContainer, containerParent)
        {
            OldElements = oldElements;
        }

        protected override List<T> GetOldElements(IModelRepository repository)
        {
            return OldElements;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as ListDeletionComposition<T>;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                    && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                    && this.StartingIndex.Equals(other.StartingIndex)
                    && this.Count.Equals(other.Count);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ CollectionPropertyName?.GetHashCode() ?? 0
                ^ StartingIndex.GetHashCode()
                ^ Count.GetHashCode();
        }
    }

    [XmlConstructor(4)]
    public class ListDeletionAssociation<T> : ListDeletionBase<T> where T : class, IModelElement
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<Uri> OldElementUris { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ListDeletionAssociation(Uri absoluteUri, string collectionPropertyName, int startingIndex, int count)
            : base(absoluteUri, collectionPropertyName, startingIndex, count)
        {
            OldElementUris = new List<Uri>();
        }

        public ListDeletionAssociation(Uri absoluteUri, string collectionPropertyName, int startingIndex, int count, List<Uri> oldElementUris)
            : base(absoluteUri, collectionPropertyName, startingIndex, count)
        {
            OldElementUris = oldElementUris;
        }

        protected override List<T> GetOldElements(IModelRepository repository)
        {
            return OldElementUris.Select(u => repository.Resolve(u)).Cast<T>().ToList();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as ListDeletionAssociation<T>;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                    && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                    && this.StartingIndex.Equals(other.StartingIndex)
                    && this.Count.Equals(other.Count);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ CollectionPropertyName?.GetHashCode() ?? 0
                ^ StartingIndex.GetHashCode()
                ^ Count.GetHashCode();
        }
    }
}
