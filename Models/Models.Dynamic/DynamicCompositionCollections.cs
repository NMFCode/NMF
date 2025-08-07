using System;
using NMF.Models.Collections;
using NMF.Models.Meta;

namespace NMF.Models.Dynamic
{
    internal class DynamicCompositionList : ObservableCompositionList<IModelElement>, IModelElementCollection
    {
        public DynamicCompositionList(ModelElement parent, IClass type) : base(parent)
        {
            Type = type;
        }
        public IClass Type { get; }

        public bool TryAdd(IModelElement element)
        {
            if (element != null && Type.IsAssignableFrom(element.GetClass()))
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

        protected override void InsertItem(int index, IModelElement item)
        {
            if (item != null && !Type.IsAssignableFrom(item.GetClass()))
            {
                throw new InvalidOperationException($"Cannot cast element of type {item.GetClass().Name} to {Type.Name}.");
            }
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, IModelElement item)
        {
            if (item != null && !Type.IsAssignableFrom(item.GetClass()))
            {
                throw new InvalidOperationException($"Cannot cast element of type {item.GetClass().Name} to {Type.Name}.");
            }
            base.SetItem(index, item);
        }
    }

    internal class DynamicCompositionOrderedSet : ObservableCompositionOrderedSet<IModelElement>, IModelElementCollection
    {
        public IClass Type { get; }

        public DynamicCompositionOrderedSet(ModelElement parent, IClass type) : base(parent)
        {
            Type = type;
        }

        public bool TryAdd(IModelElement element)
        {
            if (element != null && Type.IsAssignableFrom(element.GetClass()) && !Contains(element))
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

        public override void Insert(int index, IModelElement item)
        {
            if (item != null && !Type.IsAssignableFrom(item.GetClass()))
            {
                throw new InvalidOperationException($"Cannot cast element of type {item.GetClass().Name} to {Type.Name}.");
            }
            base.Insert(index, item);
        }

        protected override void Replace(int index, IModelElement oldValue, IModelElement newValue)
        {
            if (newValue != null && !Type.IsAssignableFrom(newValue.GetClass()))
            {
                throw new InvalidOperationException($"Cannot cast element of type {newValue.GetClass().Name} to {Type.Name}.");
            }
            base.Replace(index, oldValue, newValue);
        }
    }

    internal class DynamicCompositionSet : ObservableCompositionSet<IModelElement>, IModelElementCollection
    {
        public IClass Type { get; }

        public DynamicCompositionSet(ModelElement parent, IClass type) : base(parent)
        {
            Type = type;
        }

        public bool TryAdd(IModelElement element)
        {
            if (element != null && Type.IsAssignableFrom(element.GetClass()))
            {
                return base.Add(element);
            }
            return false;
        }

        public bool TryRemove(IModelElement element)
        {
            return Remove(element);
        }

        public override bool Add(IModelElement item)
        {
            if (item != null && !Type.IsAssignableFrom(item.GetClass()))
            {
                throw new InvalidOperationException($"Cannot cast element of type {item.GetClass().Name} to {Type.Name}.");
            }
            return base.Add(item);
        }
    }
}
