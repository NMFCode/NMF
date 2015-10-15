using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Test.Reversable
{
    public abstract class ReversableExpressionTests
    {
        protected abstract void SetValue<T>(Expression<Func<T>> expression, T value);
    }
}
