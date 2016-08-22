using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class GreaterThanExpressionTests
    {
        [TestMethod]
        public void GreaterThan_Int_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<int>() { Item = 1 };

            var test = new ObservingFunc<Dummy<int>, bool>(d => d.Item > 0);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = -1;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void GreaterThan_Int_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<int>() { Item = 1 };

            var test = new ObservingFunc<Dummy<int>, bool>(d => d.Item > 0);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = -1;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void GreaterThan_Long_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<long>() { Item = 1 };

            var test = new ObservingFunc<Dummy<long>, bool>(d => d.Item > 0);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = -1;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void GreaterThan_Long_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<long>() { Item = 1 };

            var test = new ObservingFunc<Dummy<long>, bool>(d => d.Item > 0);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = -1;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void GreaterThan_UInt_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<uint>() { Item = 2 };

            var test = new ObservingFunc<Dummy<uint>, bool>(d => d.Item > 1);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 0;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void GreaterThan_UInt_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<uint>() { Item = 2 };

            var test = new ObservingFunc<Dummy<uint>, bool>(d => d.Item > 1);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 0;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void GreaterThan_ULong_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<ulong>() { Item = 2 };

            var test = new ObservingFunc<Dummy<ulong>, bool>(d => d.Item > 1);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 0;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void GreaterThan_ULong_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<ulong>() { Item = 2 };

            var test = new ObservingFunc<Dummy<ulong>, bool>(d => d.Item > 1);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 0;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void GreaterThan_Float_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<float>() { Item = 1 };

            var test = new ObservingFunc<Dummy<float>, bool>(d => d.Item > 0);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = -1;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void GreaterThan_Float_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<float>() { Item = 1 };

            var test = new ObservingFunc<Dummy<float>, bool>(d => d.Item > 0);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = -1;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void GreaterThan_Double_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<double>() { Item = 1 };

            var test = new ObservingFunc<Dummy<double>, bool>(d => d.Item > 0);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = -1;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void GreaterThan_Double_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<double>() { Item = 1 };

            var test = new ObservingFunc<Dummy<double>, bool>(d => d.Item > 0);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = -1;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void GreaterThan_Decimal_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<decimal>() { Item = 1 };

            var test = new ObservingFunc<Dummy<decimal>, bool>(d => d.Item > 0);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = -1;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void GreaterThan_Decimal_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<decimal>() { Item = 1 };

            var test = new ObservingFunc<Dummy<decimal>, bool>(d => d.Item > 0);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = -1;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }
    }
}
