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
    [XmlConstructor(3)]
    public abstract class ListInsertionBase<T> : IModelChange
    {
        [XmlAttribute(true)]
        [XmlConstructorParameter(0)]
        public Uri AbsoluteUri { get; set; }

        [XmlAttribute(true)]
        [XmlConstructorParameter(1)]
        public string CollectionPropertyName { get; set; }

        [XmlAttribute(true)]
        [XmlConstructorParameter(2)]
        public int StartingIndex { get; set; }
        
        public ListInsertionBase(Uri absoluteUri, string collectionPropertyName, int startingIndex)
        {
            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));
            if (string.IsNullOrEmpty(collectionPropertyName))
                throw new ArgumentNullException(nameof(collectionPropertyName));

            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
            StartingIndex = startingIndex;
        }
        
        public void Apply(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var list = property.GetValue(parent, null) as IList<T>;
            var newElements = GetNewElements(repository);

            for (int i = 0; i < newElements.Count; i++)
            {
                list.Insert(StartingIndex + i, newElements[i]);
            }
        }

        public void Invert(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var list = property.GetValue(parent, null) as IList<T>;
            var newElements = GetNewElements(repository);

            foreach (var element in newElements)
                list?.Remove(element);
        }

        protected abstract List<T> GetNewElements(IModelRepository repository);
    }

    [XmlConstructor(3)]
    public class ListInsertionComposition<T> : ListInsertionBase<T>
    {
        public List<T> NewElements { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ListInsertionComposition(Uri absoluteUri, string collectionPropertyName, int startingIndex)
            : base(absoluteUri, collectionPropertyName, startingIndex)
        {
            NewElements = new List<T>();
        }

        public ListInsertionComposition(Uri absoluteUri, string collectionPropertyName, int startingIndex, List<T> newElements)
            : base(absoluteUri, collectionPropertyName, startingIndex)
        {
            NewElements = newElements;
        }

        protected override List<T> GetNewElements(IModelRepository repository)
        {
            return NewElements;
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as ListInsertionComposition<T>;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                    && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                    && this.StartingIndex.Equals(other.StartingIndex)
                    && this.NewElements.SequenceEqual(other.NewElements);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ CollectionPropertyName?.GetHashCode() ?? 0
                ^ StartingIndex.GetHashCode()
                ^ NewElements.GetHashCode();
        }
    }

    [XmlConstructor(3)]
    public class ListInsertionAssociation<T> : ListInsertionBase<T> where T : class, IModelElement
    {
        public List<Uri> NewElementUris { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ListInsertionAssociation(Uri absoluteUri, string collectionPropertyName, int startingIndex)
            : base(absoluteUri, collectionPropertyName, startingIndex)
        {
            NewElementUris = new List<Uri>();
        }

        public ListInsertionAssociation(Uri absoluteUri, string collectionPropertyName, int startingIndex, List<Uri> newElementUris)
            : base(absoluteUri, collectionPropertyName, startingIndex)
        {
            NewElementUris = newElementUris;
        }

        protected override List<T> GetNewElements(IModelRepository repository)
        {
            return NewElementUris.Select(u => repository.Resolve(u)).Cast<T>().ToList();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as ListInsertionAssociation<T>;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                    && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                    && this.StartingIndex.Equals(other.StartingIndex)
                    && this.NewElementUris.SequenceEqual(other.NewElementUris);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ CollectionPropertyName?.GetHashCode() ?? 0
                ^ StartingIndex.GetHashCode()
                ^ NewElementUris.GetHashCode();
        }
    }
}
