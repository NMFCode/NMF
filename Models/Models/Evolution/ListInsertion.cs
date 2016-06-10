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
    public class ListInsertion<T> : IModelChange
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
        
        public List<T> Elements { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public ListInsertion(Uri absoluteUri, string collectionPropertyName, int startingIndex)
        {
            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
            StartingIndex = startingIndex;
            Elements = new List<T>();
        }

        public ListInsertion(Uri absoluteUri, string collectionPropertyName, int startingIndex, IEnumerable<T> newElements)
        {
            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));
            if (string.IsNullOrEmpty(collectionPropertyName))
                throw new ArgumentNullException(nameof(collectionPropertyName));

            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
            Elements = newElements.ToList();
            StartingIndex = startingIndex;
        }

        public void Apply(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var list = property.GetValue(parent, null) as IList<T>;
            
            for (int i = 0; i < Elements.Count; i++)
            {
                list.Insert(StartingIndex + i, Elements[i]);
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;
            var other = obj as ListInsertion<T>;
            if (other == null)
                return false;
            else
                return this.AbsoluteUri.Equals(other.AbsoluteUri)
                    && this.CollectionPropertyName.Equals(other.CollectionPropertyName)
                    && this.StartingIndex.Equals(other.StartingIndex)
                    && this.Elements.SequenceEqual(other.Elements);
        }

        public override int GetHashCode()
        {
            return AbsoluteUri?.GetHashCode() ?? 0
                ^ CollectionPropertyName?.GetHashCode() ?? 0
                ^ StartingIndex.GetHashCode()
                ^ Elements.GetHashCode();
        }
    }
}
