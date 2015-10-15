using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Test.Reversable;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Linq.Tests.Reversable
{
    [TestClass]
    public class ReversableFirstOrDefaultTests : ReversableExpressionTests
    {
        [TestMethod]
        public void ReversableFirstOrDefault_SetToNull_Correct()
        {
            var collection = new NotifyCollection<string>() { "a", "b", "c" };
            SetValue(() => collection.FirstOrDefault(), null);
            Assert.AreEqual(0, collection.Count);
        }

        [TestMethod]
        public void ReversableFirstOrDefault_SetToExistingItem_Correct()
        {
            var collection = new NotifyCollection<string>() { "a", "b", "c" };
            SetValue(() => collection.FirstOrDefault(), "b");
            Assert.AreEqual(3, collection.Count);
            Assert.IsTrue(collection.Contains("a"));
            Assert.IsTrue(collection.Contains("b"));
            Assert.IsTrue(collection.Contains("c"));
        }

        [TestMethod]
        public void ReversableFirstOrDefault_SetToOtherItem_Correct()
        {
            var collection = new NotifyCollection<string>() { "a", "b", "c" };
            SetValue(() => collection.FirstOrDefault(), "d");
            Assert.AreEqual(4, collection.Count);
            Assert.IsTrue(collection.Contains("a"));
            Assert.IsTrue(collection.Contains("b"));
            Assert.IsTrue(collection.Contains("c"));
            Assert.IsTrue(collection.Contains("d"));
        }

        [TestMethod]
        public void ReversableFirstOrDefault_WithPredicate_SetToNull_Correct()
        {
            var collection = new NotifyCollection<string>() { "a", "b", "c" };
            SetValue(() => collection.FirstOrDefault(s => string.Compare(s, "a") > 0), null);
            Assert.AreEqual(1, collection.Count);
            Assert.IsTrue(collection.Contains("a"));
        }

        [TestMethod]
        public void ReversableFirstOrDefault_WithPredicate_SetToExistingItem_Correct()
        {
            var collection = new NotifyCollection<string>() { "a", "b", "c" };
            SetValue(() => collection.FirstOrDefault(s => string.Compare(s, "a") > 0), "b");
            Assert.AreEqual(3, collection.Count);
            Assert.IsTrue(collection.Contains("a"));
            Assert.IsTrue(collection.Contains("b"));
            Assert.IsTrue(collection.Contains("c"));
        }

        [TestMethod]
        public void ReversableFirstOrDefault_WithPredicate_SetToOtherItem_Correct()
        {
            var collection = new NotifyCollection<string>() { "a", "b", "c" };
            SetValue(() => collection.FirstOrDefault(s => string.Compare(s, "a") > 0), "d");
            Assert.AreEqual(4, collection.Count);
            Assert.IsTrue(collection.Contains("a"));
            Assert.IsTrue(collection.Contains("b"));
            Assert.IsTrue(collection.Contains("c"));
            Assert.IsTrue(collection.Contains("d"));
        }

        protected override void SetValue<T>(System.Linq.Expressions.Expression<Func<T>> expression, T value)
        {
            var reversable = Observable.Reversable(expression);
            reversable.Value = value;
        }
    }

    [TestClass]
    public class ReversableFirstOrDefaultExpressionRewriterTests : ReversableFirstOrDefaultTests
    {
        protected override void SetValue<T>(Expression<Func<T>> expression, T value)
        {
            var setExpression = SetExpressionRewriter.CreateSetter(expression);
            Assert.IsNotNull(setExpression);
            var setter = setExpression.Compile();
            setter(value);
        }
    }
}
