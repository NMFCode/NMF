using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq.Expressions;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class DefaultExpressionTests
    {
        [TestMethod]
        public void Default_Class_CorrectResult()
        {
            var test = GetDefaultExpression<string>();

            Assert.IsNull(test.Value);
        }

        [TestMethod]
        public void Default_Int_CorrectResult()
        {
            var test = GetDefaultExpression<int>();

            Assert.AreEqual(0, test.Value);
        }

        [TestMethod]
        public void Default_Interface_CorrectResult()
        {
            var test = GetDefaultExpression<IComparable<int>>();

            Assert.IsNull(test.Value);
        }

        private INotifyValue<T> GetDefaultExpression<T>()
        {
            return Observable.Expression(() => default(T));
        }

        [TestMethod]
        public void Default_DefaultExpression_CorrectResult()
        {
            var test = Observable.Expression(Expression.Lambda<Func<string>>(
                Expression.Default(typeof(string))));

            Assert.IsNull(test.Value);
        }
    }
}
