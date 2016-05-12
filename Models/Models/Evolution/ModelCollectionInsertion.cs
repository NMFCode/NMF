using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;

namespace NMF.Models.Evolution
{
    public class ModelCollectionInsertion : IModelChange
    {
        public ModelCollectionInsertion(Uri absoluteUri, string collectionPropertyName, IModelElement newElement, int index)
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

        public int Index { get; private set; }

        public IModelElement Element { get; private set; }

        public void Apply()
        {
            var parent = MetaRepository.Instance.Resolve(AbsoluteUri);
            var collection = parent.GetType().GetProperty(CollectionPropertyName)?.GetValue(parent, null) as ICollection<IModelElement>;
            //TODO error handling if collection is null
            var list = collection as IList<IModelElement>;
            if (list != null)
                list.Insert(Index, Element);
            else
                collection.Add(Element);
            //TODO Element.Parent = parent;
        }

        public void Undo()
        {
            new ModelCollectionDeletion(AbsoluteUri, CollectionPropertyName, Element, Index).Apply();
        }
    }
}
