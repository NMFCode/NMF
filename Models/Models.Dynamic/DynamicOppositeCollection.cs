using System;
using System.Collections.Generic;
using System.Text;
using NMF.Collections.ObjectModel;
using NMF.Models.Meta;

namespace NMF.Models.Dynamic
{
    internal class DynamicOppositeList : ObservableOppositeList<IModelElement, IModelElement>
    {
        private IReference _backReference;

        public DynamicOppositeList(IModelElement parent, IReference backReference) : base(parent)
        {
            _backReference = backReference;
        }

        protected override void SetOpposite(IModelElement item, IModelElement newParent)
        {
            if (_backReference.UpperBound == 1)
            {
                item.SetReferencedElement(_backReference, newParent);
            }
            else
            {
                var collection = item.GetReferencedElements(_backReference);
                if (newParent == null)
                {
                    collection.Remove(Parent);
                }
                else
                {
                    collection.Add(Parent);
                }
            }
        }
    }

    internal class DynamicOppositeSet : ObservableOppositeSet<IModelElement, IModelElement>
    {
        private IReference _backReference;

        public DynamicOppositeSet(IModelElement parent, IReference backReference) : base(parent)
        {
            _backReference = backReference;
        }

        protected override void SetOpposite(IModelElement item, IModelElement newParent)
        {
            if (_backReference.UpperBound == 1)
            {
                item.SetReferencedElement(_backReference, newParent);
            }
            else
            {
                var collection = item.GetReferencedElements(_backReference);
                if (newParent == null)
                {
                    collection.Remove(Parent);
                }
                else
                {
                    collection.Add(Parent);
                }
            }
        }
    }

    internal class DynamicOppositeOrderedSet : ObservableOppositeOrderedSet<IModelElement, IModelElement>
    {

        private IReference _backReference;

        public DynamicOppositeOrderedSet(IModelElement parent, IReference backReference) : base(parent)
        {
            _backReference = backReference;
        }

        protected override void SetOpposite(IModelElement item, IModelElement newParent)
        {
            if (_backReference.UpperBound == 1)
            {
                item.SetReferencedElement(_backReference, newParent);
            }
            else
            {
                var collection = item.GetReferencedElements(_backReference);
                if (newParent == null)
                {
                    collection.Remove(Parent);
                }
                else
                {
                    collection.Add(Parent);
                }
            }
        }
    }
}
