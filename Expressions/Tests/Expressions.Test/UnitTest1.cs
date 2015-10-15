using System;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var func = Observable.Func((Dummy<int> d) => d.Item);
            var dummy = new Dummy<int>(42);
            var method = func.GetType().GetMethod("Evaluate", new []{ typeof(Dummy<int>) });
            var exp = Expression.Call(Expression.Constant(func), method, Expression.Constant(dummy));
            var test = Observable.Expression(Expression.Lambda<Func<int>>(exp));
        }
    }
}
