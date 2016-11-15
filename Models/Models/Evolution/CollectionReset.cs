using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NMF.Models.Repository;

namespace NMF.Models.Evolution
{
    public class CollectionReset<T> : IModelChange
    {
        private ModelChangeRecorder _recorder;

        public Uri AbsoluteUri { get; }

        public string CollectionPropertyName { get; set; }

        public CollectionReset(Uri absoluteUri, string collectionPropertyName, ModelChangeRecorder recorder)
        {
            AbsoluteUri = absoluteUri;
            CollectionPropertyName = collectionPropertyName;
            _recorder = recorder;
        }
        public void Apply(IModelRepository repository)
        {
            throw new NotImplementedException();
        }

        public void Invert(IModelRepository repository)
        {
            var items = _recorder.GetItemsBeforeReset(this); //TODO überlegen, außerdem rausfinden ob die ganze Klasse nach Manuels Meinung überhaupt Sinn macht
            var parent = repository.Resolve(AbsoluteUri);
            var property = parent.GetType().GetProperty(CollectionPropertyName);
            var coll = property.GetValue(parent, null) as ICollection<T>;

            coll.Clear();
            foreach (var item in items.Cast<T>().ToList())
            {
                coll.Add(item);
            }
        }
    }
}
