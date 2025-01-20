using System.Collections;
using NMF.Expressions;

namespace NMF.Models.Dynamic
{
    internal class AssociationProperty : Cell<IModelElement>, IReferenceProperty
    {
        public IList Collection => null;

        public INotifyReversableExpression<IModelElement> ReferencedElement => this;

        public bool IsContainment => false;

        public object GetValue(int index)
        {
            return ReferencedElement.ValueObject;
        }

        protected override void OnValueChanging(ValueChangedEventArgs e)
        {
            base.OnValueChanging(e);
            if (e.OldValue != null)
            {
                ((IModelElement)e.OldValue).Deleted -= ReferenceDeleted;
            }
            if (e.NewValue != null)
            {
                ((IModelElement)e.NewValue).Deleted += ReferenceDeleted;
            }
        }

        private void ReferenceDeleted(object sender, UriChangedEventArgs e)
        {
            base.Value = null;
        }
    }
}
