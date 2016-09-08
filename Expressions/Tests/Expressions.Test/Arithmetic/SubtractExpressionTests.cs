using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class SubtractExpressionTests
    {
        [TestMethod]
        public void SubtractChecked_Int_NoObservable_NoUpdate()
        {
            checked
            {
                var updated = false;
                var dummy = new Dummy<int>() { Item = 5 };

                var test = new ObservingFunc<Dummy<int>, int>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(2, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.AreEqual(2, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void SubtractChecked_Int_Observable_Update()
        {
            checked
            {
                var updated = false;
                var dummy = new ObservableDummy<int>() { Item = 5 };

                var test = new ObservingFunc<Dummy<int>, int>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(2, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(2, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.IsTrue(updated);
                Assert.AreEqual(42, result.Value);
            }
        }

        [TestMethod]
        public void SubtractChecked_Long_NoObservable_NoUpdate()
        {
            checked
            {
                var updated = false;
                var dummy = new Dummy<long>() { Item = 5 };

                var test = new ObservingFunc<Dummy<long>, long>(d => d.Item - 3L);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(2L, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.AreEqual(2L, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void SubtractChecked_Long_Observable_Update()
        {
            checked
            {
                var updated = false;
                var dummy = new ObservableDummy<long>() { Item = 5 };

                var test = new ObservingFunc<Dummy<long>, long>(d => d.Item - 3L);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(2L, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(2L, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.IsTrue(updated);
                Assert.AreEqual(42L, result.Value);
            }
        }

        [TestMethod]
        public void SubtractChecked_UInt_NoObservable_NoUpdate()
        {
            checked
            {
                var updated = false;
                var dummy = new Dummy<uint>() { Item = 5 };

                var test = new ObservingFunc<Dummy<uint>, uint>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(2u, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.AreEqual(2u, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void SubtractChecked_UInt_Observable_Update()
        {
            checked
            {
                var updated = false;
                var dummy = new ObservableDummy<uint>() { Item = 5 };

                var test = new ObservingFunc<Dummy<uint>, uint>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(2u, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(2u, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.IsTrue(updated);
                Assert.AreEqual(42u, result.Value);
            }
        }

        [TestMethod]
        public void SubtractChecked_ULong_NoObservable_NoUpdate()
        {
            checked
            {
                var updated = false;
                var dummy = new Dummy<ulong>() { Item = 5 };

                var test = new ObservingFunc<Dummy<ulong>, ulong>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(2ul, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.AreEqual(2ul, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void SubtractChecked_ULong_Observable_Update()
        {
            checked
            {
                var updated = false;
                var dummy = new ObservableDummy<ulong>() { Item = 5 };

                var test = new ObservingFunc<Dummy<ulong>, ulong>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(2ul, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(2ul, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.IsTrue(updated);
                Assert.AreEqual(42ul, result.Value);
            }
        }

        [TestMethod]
        public void Subtract_Int_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<int>() { Item = 5 };

                var test = new ObservingFunc<Dummy<int>, int>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(2, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.AreEqual(2, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Subtract_Int_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<int>() { Item = 5 };

                var test = new ObservingFunc<Dummy<int>, int>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(2, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(2, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.IsTrue(updated);
                Assert.AreEqual(42, result.Value);
            }
        }

        [TestMethod]
        public void Subtract_Long_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<long>() { Item = 5 };

                var test = new ObservingFunc<Dummy<long>, long>(d => d.Item - 3L);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(2L, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.AreEqual(2L, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Subtract_Long_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<long>() { Item = 5 };

                var test = new ObservingFunc<Dummy<long>, long>(d => d.Item - 3L);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(2L, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(2L, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.IsTrue(updated);
                Assert.AreEqual(42L, result.Value);
            }
        }

        [TestMethod]
        public void Subtract_Float_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<float>() { Item = 5 };

                var test = new ObservingFunc<Dummy<float>, float>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(2, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.AreEqual(2, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Subtract_Float_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<float>() { Item = 5 };

                var test = new ObservingFunc<Dummy<float>, float>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(2.0f, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(2.0f, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.IsTrue(updated);
                Assert.AreEqual(42.0f, result.Value);
            }
        }

        [TestMethod]
        public void Subtract_Double_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<double>() { Item = 5 };

                var test = new ObservingFunc<Dummy<double>, double>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(2.0, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.AreEqual(2, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Subtract_Double_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<double>() { Item = 5 };

                var test = new ObservingFunc<Dummy<double>, double>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(2.0, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(2.0, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.IsTrue(updated);
                Assert.AreEqual(42.0, result.Value);
            }
        }

        [TestMethod]
        public void Subtract_UInt_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<uint>() { Item = 5 };

                var test = new ObservingFunc<Dummy<uint>, uint>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(2u, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.AreEqual(2u, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Subtract_UInt_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<uint>() { Item = 5 };

                var test = new ObservingFunc<Dummy<uint>, uint>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(2u, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(2u, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.IsTrue(updated);
                Assert.AreEqual(42u, result.Value);
            }
        }

        [TestMethod]
        public void Subtract_ULong_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<ulong>() { Item = 5 };

                var test = new ObservingFunc<Dummy<ulong>, ulong>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(2ul, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.AreEqual(2ul, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Subtract_ULong_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<ulong>() { Item = 5 };

                var test = new ObservingFunc<Dummy<ulong>, ulong>(d => d.Item - 3);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(2ul, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(2ul, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 45;

                Assert.IsTrue(updated);
                Assert.AreEqual(42ul, result.Value);
            }
        }

        [TestMethod]
        public void Subtract_Decimal_NoObservable_NoUpdate()
        {
            var updated = false;
            var dummy = new Dummy<decimal>() { Item = 5 };

            var test = new ObservingFunc<Dummy<decimal>, decimal>(d => d.Item - 3);

            var result = test.Observe(dummy);
            result.Successors.SetDummy();

            result.ValueChanged += (o, e) => updated = true;

            Assert.AreEqual(2, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 45;

            Assert.AreEqual(2, result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Subtract_Decimal_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<decimal>() { Item = 5 };

            var test = new ObservingFunc<Dummy<decimal>, decimal>(d => d.Item - 3);

            var result = test.Observe(dummy);
            result.Successors.SetDummy();

            result.ValueChanged += (o, e) =>
            {
                Assert.AreEqual(2m, e.OldValue);
                updated = true;
            };

            Assert.AreEqual(2, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 45;

            Assert.IsTrue(updated);
            Assert.AreEqual(42, result.Value);
        }
    }
}
