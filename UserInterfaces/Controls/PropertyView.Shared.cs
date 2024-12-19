using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models;
using NMF.Models.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Controls
{
    public partial class PropertyView
    {
        /// <summary>
        /// Gets raised when all elements should be found
        /// </summary>
        public event EventHandler<FindAllElementsEventArgs> FindAllElements;

        private static Type GetCollectionItemType(Type type)
        {
            var collection = Array.Find(type.GetInterfaces(),
                                i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));
            type = collection?.GetGenericArguments()[0];
            return type;
        }

        private readonly Dictionary<Type, INotifyEnumerable<IModelElement>> _cacheOfAvailableElements = new Dictionary<Type, INotifyEnumerable<IModelElement>>();

        private IEnumerable<IModelElement> GetPossibleItemsFor(PropertyDescriptor property, IModelElement element, Type type)
        {
            if (FindAllElements != null)
            {
                var e = new FindAllElementsEventArgs(element, property, type, null);
                FindAllElements(this, e);
                if (e.AllowableElements != null)
                {
                    return e.AllowableElements;
                }
            }
            var model = element.Model;
            if (model == null) return null;
            if (!_cacheOfAvailableElements.TryGetValue(type, out var possibleItems))
            {
                possibleItems = GetPossibleItemsCore(type, model);
                _cacheOfAvailableElements.Add(type, possibleItems);
            }
            return possibleItems;
        }

        private static INotifyEnumerable<IModelElement> GetPossibleItemsCore(Type type, Model model)
        {
            var repository = model.Repository;
            if (repository == null)
            {
                return model.Descendants().AsNotifiable().Where(e => type.IsInstanceOfType(e));
            }
            else
            {
                IEnumerable<Model> models;
                if (!(repository is ModelRepository modelRepo))
                {
                    models = repository.Models.Values.Distinct();
                }
                else
                {
                    models = modelRepo.Models.Values.Distinct().Concat(modelRepo.Parent.Models.Values.Distinct());
                }
                return models.WithUpdates().SelectMany(m => m.Descendants()).Where(e => type.IsInstanceOfType(e));
            }
        }
    }
}
