using System;
using NMF.Models.Collections;
using NMF.Models.Meta;

namespace NMF.Models.Dynamic
{
    internal class DynamicAssociationList : AssociationList<IModelElement>, IModelElementCollection
    {
        public IClass Type { get; }

        public DynamicAssociationList(IClass type)
        {
            Type = type;
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

        public bool TryAdd(IModelElement element)
        {
            if (element != null && Type.IsAssignableFrom(element.GetClass()))
            {
                base.SetItem(Count, element);
                return true;
            }
            return false;
        }

        public bool TryRemove(IModelElement element)
        {
            return Remove(element);
        }
    }

    internal class DynamicAssociationOrderedSet : AssociationOrderedSet<IModelElement>, IModelElementCollection
    {
        public IClass Type { get; }

        public DynamicAssociationOrderedSet(IClass type)
        {
            Type = type;
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
    }

    internal class DynamicAssociationSet : AssociationSet<IModelElement>, IModelElementCollection
    {
        public IClass Type { get; }

        public DynamicAssociationSet(IClass type)
        {
            Type = type;
        }

        public override bool Add(IModelElement item)
        {
            if (item != null && !Type.IsAssignableFrom(item.GetClass()))
            {
                throw new InvalidOperationException($"Cannot cast element of type {item.GetClass().Name} to {Type.Name}.");
            }
            return base.Add(item);
        }

        public bool TryAdd(IModelElement element)
        {
            if (element != null && Type.IsAssignableFrom(element.GetClass()))
            {
                base.Add(element);
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
