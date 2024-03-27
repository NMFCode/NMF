using System;
using System.Linq.Expressions;

namespace NMF.Expressions.Test.Reversable
{
    public abstract class ReversableExpressionTests
    {
        protected abstract void SetValue<T>(Expression<Func<T>> expression, T value);
    }
}
