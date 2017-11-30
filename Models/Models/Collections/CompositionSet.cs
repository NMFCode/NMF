using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Models.Collections
{
    public class CompositionSet<T> : OppositeSet<IModelElement, T> where T : class, IModelElement
    {
        public CompositionSet(IModelElement parent) : base(parent)
        {
        }

        protected override void SetOpposite(T item, IModelElement newParent)
        {
            if (newParent == null)
            {
                item.ParentChanged -= RemoveItem;
                if (item.Parent == Parent) item.Delete();
            }
            else
            {
                item.ParentChanged += RemoveItem;
                item.Parent = Parent;
            }
        }

        private void RemoveItem(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValue != Parent)
            {
                var item = sender as T;
                base.Remove(item);
            }
        }
    }

    public class ObservableCompositionSet<T> : ObservableOppositeSet<IModelElement, T> where T : class, IModelElement
    {
        public ObservableCompositionSet(IModelElement parent) : base(parent)
        {
        }

        protected override void SetOpposite(T item, IModelElement newParent)
        {
            if (newParent == null)
            {
                item.ParentChanged -= RemoveItem;
                if (item.Parent == Parent) item.Delete();
            }
            else
            {
                item.ParentChanged += RemoveItem;
                item.Parent = Parent;
            }
        }

        private void RemoveItem(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValue != Parent)
            {
                var item = sender as T;
                base.Remove(item);
            }
        }
    }
}
