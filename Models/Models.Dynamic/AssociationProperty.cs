using System.Collections;
using NMF.Expressions;

namespace NMF.Models.Dynamic
{
    internal class AssociationProperty : Cell<IModelElement>, IReferenceProperty
    {
        public IList Collection => null;

        public INotifyReversableExpression<IModelElement> ReferencedElement => this;

        public bool IsContainment => false;

        public int Count => ReferencedElement.Value == null ? 0 : 1;

        public object GetValue(int index)
        {
            return ReferencedElement.ValueObject;
        }

        public bool Contains(IModelElement element)
        {
            return element == ReferencedElement.Value;
        }

        public bool TryAdd(IModelElement element)
        {
            if (ReferencedElement.Value == null && element != null)
            {
                ReferencedElement.Value = element;
                return true;
            }
            return false;
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
