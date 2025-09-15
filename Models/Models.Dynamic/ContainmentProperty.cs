using System;
using System.Collections;
using NMF.Expressions;
using NMF.Models.Meta;

namespace NMF.Models.Dynamic
{
    internal class ContainmentProperty : Cell<IModelElement>, IReferenceProperty
    {
        public ContainmentProperty(IModelElement parent, IReference reference)
        {
            Parent = parent;
            Reference = reference;
        }

        public IModelElement Parent { get; }

        public IReference Reference { get; }

        public IList Collection => null;

        public INotifyReversableExpression<IModelElement> ReferencedElement => this;

        public bool IsContainment => true;

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
            if (ReferencedElement.Value != null || !Reference.CanAdd(element))
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

        protected override void OnValueChanging(ValueChangedEventArgs e)
        {
            if (!Reference.CanAdd(e.NewValue as IModelElement))
            {
                throw new InvalidOperationException($"Cannot assign {e.NewValue.GetType()} to containment {Reference.Name}.");
            }
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
