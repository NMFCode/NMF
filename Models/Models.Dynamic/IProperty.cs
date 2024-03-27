using System.Collections;
using NMF.Expressions;

namespace NMF.Models.Dynamic
{
    internal interface IAttributeProperty
    {
        object GetValue(int index);

        IList Collection { get; }

        INotifyReversableExpression<object> Value { get; }
    }

    internal interface IReferenceProperty
    {
        object GetValue(int index);

        IList Collection { get; }

        bool IsContainment { get; }

        INotifyReversableExpression<IModelElement> ReferencedElement { get; }

    }
}
