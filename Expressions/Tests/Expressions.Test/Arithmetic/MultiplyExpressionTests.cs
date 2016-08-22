using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class MultiplyExpressionTests
    {

        [TestMethod]
        public void MultiplyChecked_Int_NoObservable_NoUpdate()
        {
            checked
            {
                var updated = false;
                var dummy = new Dummy<int>() { Item = 3 };

                var test = new ObservingFunc<Dummy<int>, int>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(21, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.AreEqual(21, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void MultiplyChecked_Int_Observable_Update()
        {
            checked
            {
                var updated = false;
                var dummy = new ObservableDummy<int>() { Item = 3 };

                var test = new ObservingFunc<Dummy<int>, int>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(21, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(21, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.IsTrue(updated);
                Assert.AreEqual(42, result.Value);
            }
        }

        [TestMethod]
        public void MultiplyChecked_Long_NoObservable_NoUpdate()
        {
            checked
            {
                var updated = false;
                var dummy = new Dummy<long>() { Item = 3 };

                var test = new ObservingFunc<Dummy<long>, long>(d => d.Item * 7L);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(21L, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.AreEqual(21L, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void MultiplyChecked_Long_Observable_Update()
        {
            checked
            {
                var updated = false;
                var dummy = new ObservableDummy<long>() { Item = 3 };

                var test = new ObservingFunc<Dummy<long>, long>(d => d.Item * 7L);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(21L, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(21L, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.IsTrue(updated);
                Assert.AreEqual(42L, result.Value);
            }
        }

        [TestMethod]
        public void MultiplyChecked_UInt_NoObservable_NoUpdate()
        {
            checked
            {
                var updated = false;
                var dummy = new Dummy<uint>() { Item = 3 };

                var test = new ObservingFunc<Dummy<uint>, uint>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(21u, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.AreEqual(21u, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void MultiplyChecked_UInt_Observable_Update()
        {
            checked
            {
                var updated = false;
                var dummy = new ObservableDummy<uint>() { Item = 3 };

                var test = new ObservingFunc<Dummy<uint>, uint>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(21u, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(21u, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.IsTrue(updated);
                Assert.AreEqual(42u, result.Value);
            }
        }

        [TestMethod]
        public void MultiplyChecked_ULong_NoObservable_NoUpdate()
        {
            checked
            {
                var updated = false;
                var dummy = new Dummy<ulong>() { Item = 3 };

                var test = new ObservingFunc<Dummy<ulong>, ulong>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(21ul, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.AreEqual(21ul, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void MultiplyChecked_ULong_Observable_Update()
        {
            checked
            {
                var updated = false;
                var dummy = new ObservableDummy<ulong>() { Item = 3 };

                var test = new ObservingFunc<Dummy<ulong>, ulong>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(21ul, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(21ul, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.IsTrue(updated);
                Assert.AreEqual(42ul, result.Value);
            }
        }

        [TestMethod]
        public void Multiply_Int_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<int>() { Item = 3 };

                var test = new ObservingFunc<Dummy<int>, int>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(21, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.AreEqual(21, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Multiply_Int_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<int>() { Item = 3 };

                var test = new ObservingFunc<Dummy<int>, int>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(21, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(21, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.IsTrue(updated);
                Assert.AreEqual(42, result.Value);
            }
        }

        [TestMethod]
        public void Multiply_Long_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<long>() { Item = 3 };

                var test = new ObservingFunc<Dummy<long>, long>(d => d.Item * 7L);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(21L, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.AreEqual(21L, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Multiply_Long_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<long>() { Item = 3 };

                var test = new ObservingFunc<Dummy<long>, long>(d => d.Item * 7L);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(21L, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(21L, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.IsTrue(updated);
                Assert.AreEqual(42L, result.Value);
            }
        }

        [TestMethod]
        public void Multiply_Float_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<float>() { Item = 3 };

                var test = new ObservingFunc<Dummy<float>, float>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(21, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.AreEqual(21, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Multiply_Float_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<float>() { Item = 3 };

                var test = new ObservingFunc<Dummy<float>, float>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(21.0f, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(21.0f, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.IsTrue(updated);
                Assert.AreEqual(42.0f, result.Value);
            }
        }

        [TestMethod]
        public void Multiply_Double_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<double>() { Item = 3 };

                var test = new ObservingFunc<Dummy<double>, double>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(21.0, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.AreEqual(21, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Multiply_Double_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<double>() { Item = 3 };

                var test = new ObservingFunc<Dummy<double>, double>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(21.0, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(21.0, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.IsTrue(updated);
                Assert.AreEqual(42.0, result.Value);
            }
        }

        [TestMethod]
        public void Multiply_UInt_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<uint>() { Item = 3 };

                var test = new ObservingFunc<Dummy<uint>, uint>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(21u, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.AreEqual(21u, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Multiply_UInt_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<uint>() { Item = 3 };

                var test = new ObservingFunc<Dummy<uint>, uint>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(21u, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(21u, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.IsTrue(updated);
                Assert.AreEqual(42u, result.Value);
            }
        }

        [TestMethod]
        public void Multiply_ULong_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<ulong>() { Item = 3 };

                var test = new ObservingFunc<Dummy<ulong>, ulong>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(21ul, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.AreEqual(21ul, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Multiply_ULong_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<ulong>() { Item = 3 };

                var test = new ObservingFunc<Dummy<ulong>, ulong>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(21ul, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(21ul, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.IsTrue(updated);
                Assert.AreEqual(42ul, result.Value);
            }
        }

        [TestMethod]
        public void Multiply_Decimal_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<decimal>() { Item = 3 };

                var test = new ObservingFunc<Dummy<decimal>, decimal>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(21, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.AreEqual(21, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Multiply_Decimal_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<decimal>() { Item = 3 };

                var test = new ObservingFunc<Dummy<decimal>, decimal>(d => d.Item * 7);

                var result = test.Observe(dummy);
            result.Successors.Add(null);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(21m, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(21, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 6;

                Assert.IsTrue(updated);
                Assert.AreEqual(42, result.Value);
            }
        }
    }
}
