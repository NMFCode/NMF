using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Test.Arithmetic
{
    [TestClass]
    public class ShiftExpressionTests
    {

        [TestMethod]
        public void LeftShift_Int_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<int>(1);

            var test = Observable.Expression(() => 1 << dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LeftShift_Int_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<int>(1);

            var test = Observable.Expression(() => 1 << dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2, e.OldValue);
                Assert.AreEqual(8, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsTrue(update);
            Assert.AreEqual(8, test.Value);
        }

        [TestMethod]
        public void LeftShift_Long_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<long>(1);

            var test = Observable.Expression(() => dummy.Item << 1);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LeftShift_Long_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<long>(1);

            var test = Observable.Expression(() => dummy.Item << 1);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2L, e.OldValue);
                Assert.AreEqual(6L, e.NewValue);
            };

            Assert.AreEqual(2L, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsTrue(update);
            Assert.AreEqual(6L, test.Value);
        }


        [TestMethod]
        public void RightShift_Int_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<int>(1);

            var test = Observable.Expression(() => 16 >> dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(8, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void RightShift_Int_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<int>(1);

            var test = Observable.Expression(() => 16 >> dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(8, e.OldValue);
                Assert.AreEqual(2, e.NewValue);
            };

            Assert.AreEqual(8, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsTrue(update);
            Assert.AreEqual(2, test.Value);
        }


        [TestMethod]
        public void RightShift_Long_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<long>(2);

            var test = Observable.Expression(() => dummy.Item >> 1);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 4;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void RightShift_Long_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<long>(2);

            var test = Observable.Expression(() => dummy.Item >> 1);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1L, e.OldValue);
                Assert.AreEqual(2L, e.NewValue);
            };

            Assert.AreEqual(1L, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 4;

            Assert.IsTrue(update);
            Assert.AreEqual(2L, test.Value);
        }
    }
}
