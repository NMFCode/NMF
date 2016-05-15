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

        public void Apply(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            object collection = property.GetValue(parent, null);

            //TODO same uglyness as in ModelCollectionInsertion

            var iCollection = property.PropertyType.GetInterfaces().FirstOrDefault(i => i.Name.StartsWith("ICollection"));
            iCollection.GetMethod("Remove").Invoke(collection, new[] { Element });
        }

        public void Undo(IModelRepository repository)
        {
            new ModelCollectionInsertion(AbsoluteUri, CollectionPropertyName, Element, Index).Apply(repository);
        }
    }
}
