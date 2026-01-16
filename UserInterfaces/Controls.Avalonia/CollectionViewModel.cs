using Avalonia.Controls.Templates;
using NMF.Models;
using PropertyModels.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INotifyPropertyChanged = System.ComponentModel.INotifyPropertyChanged;

namespace NMF.Controls
{
    internal class CollectionViewModel : INotifyPropertyChanged
    {
        public CollectionViewModel(IList items, IEnumerable<IModelElement> available, IDataTemplate itemTemplate)
            : this(items, (available as IList) ?? available.ToList(), itemTemplate) { }

        public CollectionViewModel(IList items, IList available, IDataTemplate itemTemplate)
        {
            Items = items;
            Available = available;
            ItemTemplate = itemTemplate;

            foreach (var modelElement in items.OfType<IModelElement>())
            {
                modelElement.PropertyChanged += ModelElement_PropertyChanged;
            }
            if (items is INotifyCollectionChanged notifyCollectionChanged)
            {
                notifyCollectionChanged.CollectionChanged += ItemsChanged;
            }
        }

        private void ModelElement_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IModelElement.IdentifierString))
            {
                RaiseSummaryChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ItemsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems.OfType<IModelElement>())
                {
                    item.PropertyChanged -= ModelElement_PropertyChanged;
                }
            }
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems.OfType<IModelElement>())
                {
                    item.PropertyChanged += ModelElement_PropertyChanged;
                }
            }
            RaiseSummaryChanged();
        }

        private void RaiseSummaryChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Summary)));
        }

        public IDataTemplate ItemTemplate { get; }

        public IList Items { get; }

        public IList Available { get; }

        public string Summary => string.Join(", ", Items.Cast<object>());
    }
}
