using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class DivideExpressionTests
    {
        [TestMethod]
        public void Divide_Int_NoObservable_NoUpdate()
        {
            var updated = false;
            var dummy = new Dummy<int>() { Item = 16 };

            var test = new ObservingFunc<Dummy<int>, int>(d => d.Item / 2);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.AreEqual(8, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.AreEqual(8, result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Divide_Int_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<int>() { Item = 16 };

            var test = new ObservingFunc<Dummy<int>, int>(d => d.Item / 2);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) =>
            {
                Assert.AreEqual(8, e.OldValue);
                updated = true;
            };

            Assert.AreEqual(8, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.IsTrue(updated);
            Assert.AreEqual(42, result.Value);
        }

        [TestMethod]
        public void Divide_Long_NoObservable_NoUpdate()
        {
            var updated = false;
            var dummy = new Dummy<long>() { Item = 16 };

            var test = new ObservingFunc<Dummy<long>, long>(d => d.Item / 2L);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.AreEqual(8L, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.AreEqual(8L, result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Divide_Long_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<long>() { Item = 16 };

            var test = new ObservingFunc<Dummy<long>, long>(d => d.Item / 2L);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) =>
            {
                Assert.AreEqual(8L, e.OldValue);
                updated = true;
            };

            Assert.AreEqual(8L, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.IsTrue(updated);
            Assert.AreEqual(42L, result.Value);
        }

        [TestMethod]
        public void Divide_Float_NoObservable_NoUpdate()
        {
            var updated = false;
            var dummy = new Dummy<float>() { Item = 16 };

            var test = new ObservingFunc<Dummy<float>, float>(d => d.Item / 2);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.AreEqual(8, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.AreEqual(8, result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Divide_Float_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<float>() { Item = 16 };

            var test = new ObservingFunc<Dummy<float>, float>(d => d.Item / 2);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) =>
            {
                Assert.AreEqual(8.0f, e.OldValue);
                updated = true;
            };

            Assert.AreEqual(8.0f, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.IsTrue(updated);
            Assert.AreEqual(42.0f, result.Value);
        }

        [TestMethod]
        public void Divide_Double_NoObservable_NoUpdate()
        {
            var updated = false;
            var dummy = new Dummy<double>() { Item = 16 };

            var test = new ObservingFunc<Dummy<double>, double>(d => d.Item / 2);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.AreEqual(8.0, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.AreEqual(8, result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Divide_Double_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<double>() { Item = 16 };

            var test = new ObservingFunc<Dummy<double>, double>(d => d.Item / 2);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) =>
            {
                Assert.AreEqual(8.0, e.OldValue);
                updated = true;
            };

            Assert.AreEqual(8.0, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.IsTrue(updated);
            Assert.AreEqual(42.0, result.Value);
        }

        [TestMethod]
        public void Divide_UInt_NoObservable_NoUpdate()
        {
            var updated = false;
            var dummy = new Dummy<uint>() { Item = 16 };

            var test = new ObservingFunc<Dummy<uint>, uint>(d => d.Item / 2);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.AreEqual(8u, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.AreEqual(8u, result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Divide_UInt_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<uint>() { Item = 16 };

            var test = new ObservingFunc<Dummy<uint>, uint>(d => d.Item / 2);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) =>
            {
                Assert.AreEqual(8u, e.OldValue);
                updated = true;
            };

            Assert.AreEqual(8u, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.IsTrue(updated);
            Assert.AreEqual(42u, result.Value);
        }

        [TestMethod]
        public void Divide_ULong_NoObservable_NoUpdate()
        {
            var updated = false;
            var dummy = new Dummy<ulong>() { Item = 16 };

            var test = new ObservingFunc<Dummy<ulong>, ulong>(d => d.Item / 2);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.AreEqual(8ul, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.AreEqual(8ul, result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Divide_ULong_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<ulong>() { Item = 16 };

            var test = new ObservingFunc<Dummy<ulong>, ulong>(d => d.Item / 2);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) =>
            {
                Assert.AreEqual(8ul, e.OldValue);
                updated = true;
            };

            Assert.AreEqual(8ul, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.IsTrue(updated);
            Assert.AreEqual(42ul, result.Value);
        }

        [TestMethod]
        public void Divide_Decimal_NoObservable_NoUpdate()
        {
            var updated = false;
            var dummy = new Dummy<decimal>() { Item = 16 };

            var test = new ObservingFunc<Dummy<decimal>, decimal>(d => d.Item / 2);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.AreEqual(8, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.AreEqual(8, result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Divide_Decimal_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<decimal>() { Item = 16 };

            var test = new ObservingFunc<Dummy<decimal>, decimal>(d => d.Item / 2);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) =>
            {
                Assert.AreEqual(8m, e.OldValue);
                updated = true;
            };

            Assert.AreEqual(8, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 84;

            Assert.IsTrue(updated);
            Assert.AreEqual(42, result.Value);
        }
    }
}
