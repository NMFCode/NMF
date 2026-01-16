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

        int Count { get; }

        INotifyReversableExpression<IModelElement> ReferencedElement { get; }

        bool TryAdd(IModelElement element);

        bool TryRemove(IModelElement element);

        bool Contains(IModelElement element);

        void Reset();
    }

    internal interface IModelElementCollection : IList
    {
        bool TryAdd(IModelElement element);

        bool TryRemove(IModelElement element);
    }
}
