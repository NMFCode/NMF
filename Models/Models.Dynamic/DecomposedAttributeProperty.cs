using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NMF.Expressions;
using NMF.Expressions.Linq;

namespace NMF.Models.Dynamic
{
    internal class DecomposedAttributeProperty : IAttributeProperty, IDecomposedAttributeProperty
    {
        public void AddComponentAttributeProperty(IAttributeProperty attributeProperty)
        {
            var coalesceValue = attributeProperty.Value;
            if (attributeProperty.Collection != null && attributeProperty.Collection is IEnumerableExpression<object> enumerableExpression)
            {
                coalesceValue = (INotifyReversableExpression<object>)Observable.Reversable(() => enumerableExpression.FirstOrDefault());
            }
            Coalesce(coalesceValue);
        }

        private void Coalesce(INotifyReversableExpression<object> coalesceValue)
        {
            if (Value == null)
            {
                Value = coalesceValue;
            }
            else
            {
                Value = Observable.Coalesce(Value, coalesceValue);
            }
        }

        public void AddConstraint(IEnumerable<object> value)
        {
            Coalesce(new ReversibleConstant<object>(value.FirstOrDefault()));
        }

        public IList Collection => null;

        public INotifyReversableExpression<object> Value { get; private set; }

        public object GetValue(int index)
        {
            return Value.ValueObject;
        }
    }
}
