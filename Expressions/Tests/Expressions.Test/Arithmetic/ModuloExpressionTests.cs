using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Test.Arithmetic
{
    [TestClass]
    public class ModuloExpressionTests
    {
        [TestMethod]
        public void Modulo_Int_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<int>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Modulo_Int_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<int>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1, e.OldValue);
                Assert.AreEqual(0, e.NewValue);
            };

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Value);
        }

        [TestMethod]
        public void Modulo_Int_ModuloDoesNotChange_NoUpdate()
        {
            var update = false;
            var dummy = new ObservableDummy<int>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Modulo_Long_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<long>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Modulo_Long_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<long>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1L, e.OldValue);
                Assert.AreEqual(0L, e.NewValue);
            };

            Assert.AreEqual(1L, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Value);
        }

        [TestMethod]
        public void Modulo_Long_ModuloDoesNotChange_NoUpdate()
        {
            var update = false;
            var dummy = new ObservableDummy<long>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Modulo_UInt_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<uint>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1u, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Modulo_UInt_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<uint>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1u, e.OldValue);
                Assert.AreEqual(0u, e.NewValue);
            };

            Assert.AreEqual(1u, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(0u, test.Value);
        }

        [TestMethod]
        public void Modulo_UInt_ModuloDoesNotChange_NoUpdate()
        {
            var update = false;
            var dummy = new ObservableDummy<uint>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1u, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Modulo_ULong_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<ulong>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1ul, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Modulo_ULong_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<ulong>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1ul, e.OldValue);
                Assert.AreEqual(0ul, e.NewValue);
            };

            Assert.AreEqual(1ul, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(0ul, test.Value);
        }

        [TestMethod]
        public void Modulo_ULong_ModuloDoesNotChange_NoUpdate()
        {
            var update = false;
            var dummy = new ObservableDummy<ulong>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1ul, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Modulo_Float_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<float>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Modulo_Float_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<float>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1f, e.OldValue);
                Assert.AreEqual(0f, e.NewValue);
            };

            Assert.AreEqual(1f, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(0f, test.Value);
        }

        [TestMethod]
        public void Modulo_Float_ModuloDoesNotChange_NoUpdate()
        {
            var update = false;
            var dummy = new ObservableDummy<float>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }
        [TestMethod]
        public void Modulo_Double_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<double>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Modulo_Double_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<double>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1d, e.OldValue);
                Assert.AreEqual(0d, e.NewValue);
            };

            Assert.AreEqual(1d, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(0d, test.Value);
        }

        [TestMethod]
        public void Modulo_Double_ModuloDoesNotChange_NoUpdate()
        {
            var update = false;
            var dummy = new ObservableDummy<double>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Modulo_Decimal_NoObservableSource_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<decimal>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Modulo_Decimal_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<decimal>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1m, e.OldValue);
                Assert.AreEqual(0m, e.NewValue);
            };

            Assert.AreEqual(1m, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(0m, test.Value);
        }

        [TestMethod]
        public void Modulo_Decimal_ModuloDoesNotChange_NoUpdate()
        {
            var update = false;
            var dummy = new ObservableDummy<decimal>(7);

            var test = Observable.Expression(() => dummy.Item % 2);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 3;

            Assert.IsFalse(update);
        }
    }
}
