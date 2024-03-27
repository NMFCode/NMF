using System;
using NMF.Models.Collections;
using NMF.Models.Meta;

namespace NMF.Models.Dynamic
{
    internal class DynamicAssociationList : AssociationList<IModelElement>
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
    }

    internal class DynamicAssociationOrderedSet : AssociationOrderedSet<IModelElement>
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
    }

    internal class DynamicAssociationSet : AssociationSet<IModelElement>
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
    }
}
