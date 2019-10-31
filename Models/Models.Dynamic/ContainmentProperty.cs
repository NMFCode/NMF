using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NMF.Expressions;

namespace NMF.Models.Dynamic
{
    internal class ContainmentProperty : Cell<IModelElement>, IReferenceProperty
    {
        public IList Collection => null;

        public INotifyReversableExpression<IModelElement> ReferencedElement => this;

        public bool IsContainment => true;

        public object GetValue(int index)
        {
            return ReferencedElement.ValueObject;
        }

        protected override void OnValueChanging(ValueChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ((IModelElement)e.OldValue).Deleted -= ReferenceDeleted;
            }
            if (e.NewValue != null)
            {
                ((IModelElement)e.NewValue).Deleted += ReferenceDeleted;
            }
            base.OnValueChanging(e);
        }

        private void ReferenceDeleted(object sender, UriChangedEventArgs e)
        {
            base.Value = null;
        }
    }
}
