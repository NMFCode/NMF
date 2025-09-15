﻿using System.Collections;
using NMF.Expressions;
using NMF.Models.Meta;

namespace NMF.Models.Dynamic
{
    internal class OppositeProperty : Cell<IModelElement>, IReferenceProperty
    {
        public IModelElement Parent { get; }
        public IReference Reference { get; }

        public OppositeProperty(IModelElement parent, IReference reference)
        {
            Parent = parent;
            Reference = reference;
        }

        public IList Collection => null;

        public bool IsContainment => Reference.IsContainment;

        public INotifyReversableExpression<IModelElement> ReferencedElement => this;

        public int Count => ReferencedElement.Value == null ? 0 : 1;

        public object GetValue(int index)
        {
            return ReferencedElement.ValueObject;
        }

        public bool Contains(IModelElement element)
        {
            return element == ReferencedElement.Value;
        }

        protected override void OnValueChanging(ValueChangedEventArgs e)
        {
            if (e.OldValue is IModelElement oldValue)
            {
                if (Reference.IsContainment)
                {
                    oldValue.Delete();
                }
                else
                {
                    oldValue.Deleted -= ReferenceDeleted;
                }
            }
            if (e.NewValue is IModelElement newValue)
            {
                if (Reference.IsContainment)
                {
                    newValue.Parent = Parent;
                }
                else
                {
                    newValue.Deleted += ReferenceDeleted;
                }
            }

            base.OnValueChanging(e);
        }

        protected override void OnValueChanged(ValueChangedEventArgs e)
        {
            if (e.OldValue is IModelElement oldValue)
            {
                if (Reference.Opposite.UpperBound == 1)
                {
                    oldValue.SetReferencedElement(Reference.Opposite, null);
                }
                else
                {
                    oldValue.GetReferencedElements(Reference.Opposite).Remove(Parent);
                }
            }
            if (e.NewValue is IModelElement newValue)
            {
                if (Reference.Opposite.UpperBound == 1)
                {
                    newValue.SetReferencedElement(Reference.Opposite, Parent);
                }
                else
                {
                    newValue.GetReferencedElements(Reference.Opposite).Add(Parent);
                }
            }
            if (Reference.Opposite.IsContainment)
            {
                Parent.Parent = e.NewValue as IModelElement;
            }

            base.OnValueChanged(e);
        }

        private void ReferenceDeleted(object sender, UriChangedEventArgs e)
        {
            base.Value = null;
        }

        public bool TryAdd(IModelElement element)
        {
            if (ReferencedElement.Value != null)
            {
                return false;
            }

            if (!Reference.CanAdd(element))
            {
                return false;
            }

            ReferencedElement.Value = element;

            return true;
        }

        public bool TryRemove(IModelElement element)
        {
            if (ReferencedElement.Value == element)
            {
                ReferencedElement.Value = null;
                return true;
            }
            return false;
        }

        public void Reset()
        {
            ReferencedElement.Value = null;
        }
    }
}
