using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Test.Reversable
{
    [TestClass]
    public class ReversableAddExpressionTests : ReversableExpressionTests
    {
        [TestMethod]
        public void ReversableAdd_Int_CorrectResult()
        {
            var dummy = new ObservableDummy<int>();
            SetValue(() => dummy.Item + 2, 44);
            Assert.AreEqual(42, dummy.Item);
        }

        [TestMethod]
        public void ReversableAdd_UInt_CorrectResult()
        {
            var dummy = new ObservableDummy<uint>();
            SetValue(() => dummy.Item + 2, 44u);
            Assert.AreEqual(42u, dummy.Item);
        }

        [TestMethod]
        public void ReversableAdd_Long_CorrectResult()
        {
            var dummy = new ObservableDummy<long>();
            SetValue(() => dummy.Item + 2, 44L);
            Assert.AreEqual(42, dummy.Item);
        }

        [TestMethod]
        public void ReversableAdd_ULong_CorrectResult()
        {
            var dummy = new ObservableDummy<ulong>();
            SetValue(() => dummy.Item + 2, 44ul);
            Assert.AreEqual(42ul, dummy.Item);
        }

        [TestMethod]
        public void ReversableAdd_Float_CorrectResult()
        {
            var dummy = new ObservableDummy<float>();
            SetValue(() => dummy.Item + 2, 44f);
            Assert.AreEqual(42, dummy.Item);
        }

        [TestMethod]
        public void ReversableAdd_Double_CorrectResult()
        {
            var dummy = new ObservableDummy<double>();
            SetValue(() => dummy.Item + 2, 44.0);
            Assert.AreEqual(42, dummy.Item);
        }

        [TestMethod]
        public void ReversableAdd_Decimal_CorrectResult()
        {
            var dummy = new ObservableDummy<decimal>();
            SetValue(() => dummy.Item + 2, 44m);
            Assert.AreEqual(42m, dummy.Item);
        }

        protected override void SetValue<T>(Expression<Func<T>> expression, T value)
        {
            var setExpression = SetExpressionRewriter.CreateSetter(expression);
            Assert.IsNotNull(setExpression);
            var setter = setExpression.Compile();
            setter(value);
        }
    }

    [TestClass]
    public class ReversableAddExpressionRewriterTests : ReversableAddExpressionTests
    {

        [TestMethod]
        public void ReversableAdd_String_CorrectResult()
        {
            var dummy = new ObservableDummy<string>();
            SetValue(() => dummy.Item + "Subscription", "FooSubscription");
        }

        protected override void SetValue<T>(Expression<Func<T>> expression, T value)
        {
            var reversable = Observable.Reversable(expression);
            reversable.Value = value;
            Assert.AreEqual(value, reversable.Value);
        }
    }
}
