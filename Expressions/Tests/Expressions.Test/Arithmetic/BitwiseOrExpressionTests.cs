using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Test.Arithmetic
{
    [TestClass]
    public class BitwiseOrExpressionTests
    {
        [TestMethod]
        public void BitwiseOr_Int_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<int>(7);

            var test = Observable.Expression(() => 21 | dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 13;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void BitwiseOr_Int_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<int>(7);

            var test = Observable.Expression(() => 21 | dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23, e.OldValue);
                Assert.AreEqual(29, e.NewValue);
            };

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 13;

            Assert.IsTrue(update);
            Assert.AreEqual(29, test.Value);
        }

        [TestMethod]
        public void BitwiseOr_Long_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<long>(7);

            var test = Observable.Expression(() => 21 | dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 13;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void BitwiseOr_Long_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<long>(7);

            var test = Observable.Expression(() => 21 | dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23L, e.OldValue);
                Assert.AreEqual(29L, e.NewValue);
            };

            Assert.AreEqual(23L, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 13;

            Assert.IsTrue(update);
            Assert.AreEqual(29L, test.Value);
        }

        [TestMethod]
        public void BitwiseOr_UInt_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<uint>(7);

            var test = Observable.Expression(() => 21 | dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23u, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 13;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void BitwiseOr_UInt_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<uint>(7);

            var test = Observable.Expression(() => 21 | dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23u, e.OldValue);
                Assert.AreEqual(29u, e.NewValue);
            };

            Assert.AreEqual(23u, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 13;

            Assert.IsTrue(update);
            Assert.AreEqual(29u, test.Value);
        }

        [TestMethod]
        public void BitwiseOr_ULong_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<ulong>(7);

            var test = Observable.Expression(() => 21 | dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23ul, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 13;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void BitwiseOr_ULong_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<ulong>(7);

            var test = Observable.Expression(() => 21 | dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23ul, e.OldValue);
                Assert.AreEqual(29ul, e.NewValue);
            };

            Assert.AreEqual(23ul, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 13;

            Assert.IsTrue(update);
            Assert.AreEqual(29ul, test.Value);
        }
    }
}
