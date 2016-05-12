using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Models.Repository;

namespace NMF.Models.Evolution
{
    public class ModelCollectionDeletion : IModelChange
    {
        public ModelCollectionDeletion(Uri absoluteUri, string collectionPropertyName, IModelElement elementToDelete, int index)
        {
            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));
            if (string.IsNullOrEmpty(collectionPropertyName))
                throw new ArgumentNullException(nameof(collectionPropertyName));

            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
            Element = elementToDelete;
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
            collection.Remove(Element);
            //TODO Element.Parent = null;
        }

        public void Undo()
        {
            new ModelCollectionInsertion(AbsoluteUri, CollectionPropertyName, Element, Index).Apply();
        }
    }
}
