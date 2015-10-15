using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace NMF.Expressions.Test.Reversable
{
    [TestClass]
    public class ReversableDivideExpressionTests : ReversableExpressionTests
    {
        [TestMethod]
        public void ReversableDivide_Int_CorrectResult()
        {
            var dummy = new ObservableDummy<int>();
            SetValue(() => dummy.Item / 7, 6);
            Assert.AreEqual(42, dummy.Item);
        }

        [TestMethod]
        public void ReversableDivide_UInt_CorrectResult()
        {
            var dummy = new ObservableDummy<uint>();
            SetValue(() => dummy.Item / 7, 6u);
            Assert.AreEqual(42u, dummy.Item);
        }

        [TestMethod]
        public void ReversableDivide_Long_CorrectResult()
        {
            var dummy = new ObservableDummy<long>();
            SetValue(() => dummy.Item / 7, 6L);
            Assert.AreEqual(42, dummy.Item);
        }

        [TestMethod]
        public void ReversableDivide_ULong_CorrectResult()
        {
            var dummy = new ObservableDummy<ulong>();
            SetValue(() => dummy.Item / 7, 6ul);
            Assert.AreEqual(42ul, dummy.Item);
        }

        [TestMethod]
        public void ReversableDivide_Float_CorrectResult()
        {
            var dummy = new ObservableDummy<float>();
            SetValue(() => dummy.Item / 7, 6f);
            Assert.AreEqual(42, dummy.Item);
        }

        [TestMethod]
        public void ReversableDivide_Double_CorrectResult()
        {
            var dummy = new ObservableDummy<double>();
            SetValue(() => dummy.Item / 7, 6.0);
            Assert.AreEqual(42, dummy.Item);
        }

        [TestMethod]
        public void ReversableDivide_Decimal_CorrectResult()
        {
            var dummy = new ObservableDummy<decimal>();
            SetValue(() => dummy.Item / 7, 6m);
            Assert.AreEqual(42m, dummy.Item);
        }

        protected override void SetValue<T>(Expression<Func<T>> expression, T value)
        {
            var reversable = Observable.Reversable(expression);
            reversable.Value = value;
        }
    }

    [TestClass]
    public class ReversableDivideExpressionRewriterTests : ReversableDivideExpressionTests
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
