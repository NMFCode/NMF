using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Evolution
{
    public class ModelCreation : IModelChange
    {
        public Uri AbsoluteUri { get; private set; }

        public string PropertyName { get; private set; }

        public IModelElement Element { get; private set; }

        public ModelCreation(Uri absoluteUri, string propertyName, IModelElement element)
        {
            if (absoluteUri == null)
                throw new ArgumentNullException(nameof(absoluteUri));
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException(nameof(propertyName));

            AbsoluteUri = absoluteUri;
            PropertyName = propertyName;
            Element = element;
        }

        public void Apply(IModelRepository repository)
        {
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(PropertyName);
            property.SetValue(parent, Element, null);
        }

        public void Undo(IModelRepository repository)
        {
            new ModelDeletion(AbsoluteUri, PropertyName, Element).Apply(repository);
        }
    }
}
