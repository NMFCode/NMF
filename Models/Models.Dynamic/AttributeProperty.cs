using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NMF.Expressions;
using NMF.Models.Meta;
using Type = System.Type;

namespace NMF.Models.Dynamic
{
    internal class AttributeProperty : Cell<object>, IAttributeProperty
    {
        private Type _attributeType;

        public AttributeProperty(IAttribute attribute)
        {
            var mappedType = attribute.Type.GetExtension<MappedType>();
            if (mappedType != null)
            {
                _attributeType = mappedType.SystemType;
            }
            else if (attribute.Type is IEnumeration)
            {
                _attributeType = typeof(string);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public IList Collection => null;

        public new INotifyReversableExpression<object> Value => this;

        public INotifyReversableExpression<IModelElement> Reference => throw new NotSupportedException();

        public object GetValue(int index)
        {
            return Value.Value;
        }

        protected override void OnValueChanging(ValueChangedEventArgs e)
        {
            if (e.NewValue != null && !_attributeType.IsInstanceOfType(e.NewValue))
            {
                throw new ArgumentException($"Value {e.NewValue} is not of expected type {_attributeType.Name}.");
            }
            base.OnValueChanging(e);
        }
    }
}
