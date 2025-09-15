using System;
using NMF.Collections.ObjectModel;
using NMF.Models.Meta;

namespace NMF.Models.Dynamic
{
    internal class DynamicOppositeList : ObservableOppositeList<IModelElement, IModelElement>, IModelElementCollection
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
            if (!Reference.CanAdd(item))
            {
                throw new InvalidOperationException($"Item is of type {item.GetClass().Name} instead of expected type {Reference.ReferenceType.Name}");
            }
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, IModelElement item)
        {
            if (!Reference.CanAdd(item))
            {
                throw new InvalidOperationException($"Item is of type {item.GetClass().Name} instead of expected type {Reference.ReferenceType.Name}");
            }
            base.SetItem(index, item);
        }

        public bool TryAdd(IModelElement element)
        {
            if (Reference.CanAdd(element))
            {
                base.InsertItem(Count, element);
                return true;
            }
            return false;
        }

        public bool TryRemove(IModelElement element)
        {
            return Remove(element);
        }
    }

    internal class DynamicOppositeSet : ObservableOppositeSet<IModelElement, IModelElement>, IModelElementCollection
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
            if (!Reference.CanAdd(item))
            {
                throw new InvalidOperationException($"Item is of type {item.GetClass().Name} instead of expected type {Reference.ReferenceType.Name}");
            }
            return base.Add(item);
        }

        public bool TryAdd(IModelElement element)
        {
            if (Reference.CanAdd(element))
            {
                return base.Add(element);
            }
            return false;
        }

        public bool TryRemove(IModelElement element)
        {
            return Remove(element);
        }
    }

    internal class DynamicOppositeOrderedSet : ObservableOppositeOrderedSet<IModelElement, IModelElement>, IModelElementCollection
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
            if (!Reference.CanAdd(item))
            {
                throw new InvalidOperationException($"Item is of type {item.GetClass().Name} instead of expected type {Reference.ReferenceType.Name}");
            }
            base.Insert(index, item);
        }

        protected override void Replace(int index, IModelElement oldValue, IModelElement newValue)
        {
            if (!Reference.CanAdd(newValue))
            {
                throw new InvalidOperationException($"Item is of type {newValue.GetClass().Name} instead of expected type {Reference.ReferenceType.Name}");
            }
            base.Replace(index, oldValue, newValue);
        }

        public bool TryAdd(IModelElement element)
        {
            if (Reference.CanAdd(element) && !Contains(element))
            {
                base.Insert(Count, element);
                return true;
            }
            return false;
        }

        public bool TryRemove(IModelElement element)
        {
            return Remove(element);
        }
    }
}
