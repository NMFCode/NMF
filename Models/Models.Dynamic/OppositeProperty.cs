using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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

        public object GetValue(int index)
        {
            return ReferencedElement.ValueObject;
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
    }
}
