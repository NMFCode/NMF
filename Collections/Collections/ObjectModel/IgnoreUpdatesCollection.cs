using NMF.Expressions;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Collections.ObjectModel
{
    internal class IgnoreUpdatesCollection<T> : CustomCollection<T>
    {
        public IgnoreUpdatesCollection(IEnumerableExpression<T> sourceCollection) : base(sourceCollection) { }

        public override void Add(T item)
        {
        }

        public override void Clear()
        {
        }

        public override bool Remove(T item)
        {
            return true;
        }
    }
}
