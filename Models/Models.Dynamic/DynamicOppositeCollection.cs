using System;
using System.Collections.Generic;
using System.Text;
using NMF.Collections.ObjectModel;
using NMF.Models.Meta;

namespace NMF.Models.Dynamic
{
    internal class DynamicOppositeList : ObservableOppositeList<IModelElement, IModelElement>
    {
        public IReference Reference { get; }

        public DynamicOppositeList(IModelElement parent, IReference reference) : base(parent)
        {
            Reference = reference;
        }

        protected override void SetOpposite(IModelElement item, IModelElement newParent)
        {
            if (Reference.Opposite.UpperBound == 1)
            {
                item.SetReferencedElement(Reference.Opposite, newParent);
                if (newParent == null)
                {
                    RemoveEventHandler(item);
                }
                else
                {
                    AddEventHandler(item);
                }
            }
            else
            {
                var collection = item.GetReferencedElements(Reference.Opposite);
                if (newParent == null)
                {
                    collection.Remove(Parent);
                    RemoveEventHandler(item);
                }
                else
                {
                    collection.Add(Parent);
                    AddEventHandler(item);
                }
            }
        }

        private void AddEventHandler(IModelElement item)
        {
            if (Reference.IsContainment)
            {
                item.ParentChanged += OnItemParentChanged;
            }
            else
            {
                item.Deleted += OnItemDeleted;
            }
        }

        private void RemoveEventHandler(IModelElement item)
        {
            if (Reference.IsContainment)
            {
                item.ParentChanged -= OnItemParentChanged;
            }
            else
            {
                item.Deleted -= OnItemDeleted;
            }
        }

        private void OnItemDeleted(object sender, UriChangedEventArgs e)
        {
            Remove((IModelElement)sender);
        }

        private void OnItemParentChanged(object sender, NMF.Expressions.ValueChangedEventArgs e)
        {
            if (e.NewValue != Parent)
            {
                Remove((IModelElement)sender);
            }
        }

        protected override void InsertItem(int index, IModelElement item)
        {
            if (!((IClass)Reference.ReferenceType).IsAssignableFrom(item.GetClass()))
            {
                throw new InvalidOperationException($"Item is of type {item.GetClass().Name} instead of expected type {Reference.ReferenceType.Name}");
            }
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, IModelElement item)
        {
            if (!((IClass)Reference.ReferenceType).IsAssignableFrom(item.GetClass()))
            {
                throw new InvalidOperationException($"Item is of type {item.GetClass().Name} instead of expected type {Reference.ReferenceType.Name}");
            }
            base.SetItem(index, item);
        }
    }

    internal class DynamicOppositeSet : ObservableOppositeSet<IModelElement, IModelElement>
    {
        public IReference Reference { get; }

        public DynamicOppositeSet(IModelElement parent, IReference reference) : base(parent)
        {
            Reference = reference;
        }

        protected override void SetOpposite(IModelElement item, IModelElement newParent)
        {
            if (Reference.Opposite.UpperBound == 1)
            {
                item.SetReferencedElement(Reference.Opposite, newParent);
                if (newParent == null)
                {
                    RemoveEventHandler(item);
                }
                else
                {
                    AddEventHandler(item);
                }
            }
            else
            {
                var collection = item.GetReferencedElements(Reference.Opposite);
                if (newParent == null)
                {
                    collection.Remove(Parent);
                    RemoveEventHandler(item);
                }
                else
                {
                    collection.Add(Parent);
                    AddEventHandler(item);
                }
            }
        }

        private void AddEventHandler(IModelElement item)
        {
            if (Reference.IsContainment)
            {
                item.ParentChanged += OnItemParentChanged;
            }
            else
            {
                item.Deleted += OnItemDeleted;
            }
        }

        private void RemoveEventHandler(IModelElement item)
        {
            if (Reference.IsContainment)
            {
                item.ParentChanged -= OnItemParentChanged;
            }
            else
            {
                item.Deleted -= OnItemDeleted;
            }
        }

        private void OnItemDeleted(object sender, UriChangedEventArgs e)
        {
            Remove((IModelElement)sender);
        }

        private void OnItemParentChanged(object sender, NMF.Expressions.ValueChangedEventArgs e)
        {
            if (e.NewValue != Parent)
            {
                Remove((IModelElement)sender);
            }
        }

        public override bool Add(IModelElement item)
        {
            if (!((IClass)Reference.ReferenceType).IsAssignableFrom(item.GetClass()))
            {
                throw new InvalidOperationException($"Item is of type {item.GetClass().Name} instead of expected type {Reference.ReferenceType.Name}");
            }
            return base.Add(item);
        }
    }

    internal class DynamicOppositeOrderedSet : ObservableOppositeOrderedSet<IModelElement, IModelElement>
    {

        public IReference Reference { get; }

        public DynamicOppositeOrderedSet(IModelElement parent, IReference reference) : base(parent)
        {
            Reference = reference;
        }

        protected override void SetOpposite(IModelElement item, IModelElement newParent)
        {
            if (Reference.Opposite.UpperBound == 1)
            {
                item.SetReferencedElement(Reference.Opposite, newParent);
                if (newParent == null)
                {
                    RemoveEventHandler(item);
                }
                else
                {
                    AddEventHandler(item);
                }
            }
            else
            {
                var collection = item.GetReferencedElements(Reference.Opposite);
                if (newParent == null)
                {
                    collection.Remove(Parent);
                    RemoveEventHandler(item);
                }
                else
                {
                    collection.Add(Parent);
                    AddEventHandler(item);
                }
            }
        }

        private void AddEventHandler(IModelElement item)
        {
            if (Reference.IsContainment)
            {
                item.ParentChanged += OnItemParentChanged;
            }
            else
            {
                item.Deleted += OnItemDeleted;
            }
        }

        private void RemoveEventHandler(IModelElement item)
        {
            if (Reference.IsContainment)
            {
                item.ParentChanged -= OnItemParentChanged;
            }
            else
            {
                item.Deleted -= OnItemDeleted;
            }
        }

        private void OnItemDeleted(object sender, UriChangedEventArgs e)
        {
            Remove((IModelElement)sender);
        }

        private void OnItemParentChanged(object sender, NMF.Expressions.ValueChangedEventArgs e)
        {
            if (e.NewValue != Parent)
            {
                Remove((IModelElement)sender);
            }
        }

        public override void Insert(int index, IModelElement item)
        {
            if (!((IClass)Reference.ReferenceType).IsAssignableFrom(item.GetClass()))
            {
                throw new InvalidOperationException($"Item is of type {item.GetClass().Name} instead of expected type {Reference.ReferenceType.Name}");
            }
            base.Insert(index, item);
        }

        protected override void Replace(int index, IModelElement oldValue, IModelElement newValue)
        {
            if (!((IClass)Reference.ReferenceType).IsAssignableFrom(newValue.GetClass()))
            {
                throw new InvalidOperationException($"Item is of type {newValue.GetClass().Name} instead of expected type {Reference.ReferenceType.Name}");
            }
            base.Replace(index, oldValue, newValue);
        }
    }
}
