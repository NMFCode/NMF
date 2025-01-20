using System.Collections;
using NMF.Expressions;

namespace NMF.Models.Dynamic
{
    internal class ContainmentProperty : Cell<IModelElement>, IReferenceProperty
    {
        public ContainmentProperty(IModelElement parent)
        {
            Parent = parent;
        }

        public IModelElement Parent { get; }
        public IList Collection => null;

        public INotifyReversableExpression<IModelElement> ReferencedElement => this;

        public bool IsContainment => true;

        public object GetValue(int index)
        {
            return ReferencedElement.ValueObject;
        }

        protected override void OnValueChanging(ValueChangedEventArgs e)
        {
            base.OnValueChanging(e);
            if (e.OldValue != null)
            {
                ((IModelElement)e.OldValue).Delete();
            }
            if (e.NewValue != null)
            {
                ((IModelElement)e.NewValue).Parent = Parent;
            }
        }
    }
}
