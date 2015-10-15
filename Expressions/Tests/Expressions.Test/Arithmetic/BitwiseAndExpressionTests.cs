using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Test.Arithmetic
{
    [TestClass]
    public class BitwiseAndExpressionTests
    {

        [TestMethod]
        public void BitwiseAnd_Int_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<int>(7);

            var test = Observable.Expression(() => 21 & dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(5, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void BitwiseAnd_Int_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<int>(7);

            var test = Observable.Expression(() => 21 & dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(5, e.OldValue);
                Assert.AreEqual(1, e.NewValue);
            };

            Assert.AreEqual(5, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Value);
        }

        [TestMethod]
        public void BitwiseAnd_Long_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<long>(7);

            var test = Observable.Expression(() => 21 & dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(5, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void BitwiseAnd_Long_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<long>(7);

            var test = Observable.Expression(() => 21 & dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(5L, e.OldValue);
                Assert.AreEqual(1L, e.NewValue);
            };

            Assert.AreEqual(5L, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsTrue(update);
            Assert.AreEqual(1L, test.Value);
        }

        [TestMethod]
        public void BitwiseAnd_UInt_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<uint>(7);

            var test = Observable.Expression(() => 21 & dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(5u, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void BitwiseAnd_UInt_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<uint>(7);

            var test = Observable.Expression(() => 21 & dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(5u, e.OldValue);
                Assert.AreEqual(1u, e.NewValue);
            };

            Assert.AreEqual(5u, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsTrue(update);
            Assert.AreEqual(1u, test.Value);
        }

        [TestMethod]
        public void BitwiseAnd_ULong_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<ulong>(7);

            var test = Observable.Expression(() => 21 & dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(5ul, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void BitwiseAnd_ULong_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<ulong>(7);

            var test = Observable.Expression(() => 21 & dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(5ul, e.OldValue);
                Assert.AreEqual(1ul, e.NewValue);
            };

            Assert.AreEqual(5ul, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsTrue(update);
            Assert.AreEqual(1ul, test.Value);
        }
    }
}
