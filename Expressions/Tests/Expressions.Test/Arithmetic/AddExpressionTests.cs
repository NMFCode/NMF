using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class AddExpressionTests
    {

        [TestMethod]
        public void AddChecked_Int_NoObservable_NoUpdate()
        {
            checked
            {
                var updated = false;
                var dummy = new Dummy<int>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(12, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.AreEqual(12, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void AddChecked_Int_Observable_Update()
        {
            checked
            {
                var updated = false;
                var dummy = new ObservableDummy<int>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(12, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(12, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.IsTrue(updated);
                Assert.AreEqual(42, result.Value);
            }
        }
        
        [TestMethod]
        public void AddChecked_Long_NoObservable_NoUpdate()
        {
            checked
            {
                var updated = false;
                var dummy = new Dummy<long>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(12L, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.AreEqual(12L, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void AddChecked_Long_Observable_Update()
        {
            checked
            {
                var updated = false;
                var dummy = new ObservableDummy<long>() { Item = 5 };

                var test = new ObservingFunc<Dummy<long>, long>(d => d.Item + 7L);

                var result = test.Observe(dummy);
            result.Successors.SetDummy();

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(12L, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(12L, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.IsTrue(updated);
                Assert.AreEqual(42L, result.Value);
            }
        }
        
        [TestMethod]
        public void AddChecked_UInt_NoObservable_NoUpdate()
        {
            checked
            {
                var updated = false;
                var dummy = new Dummy<uint>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(12u, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.AreEqual(12u, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void AddChecked_UInt_Observable_Update()
        {
            checked
            {
                var updated = false;
                var dummy = new ObservableDummy<uint>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(12u, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(12u, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.IsTrue(updated);
                Assert.AreEqual(42u, result.Value);
            }
        }
        
        [TestMethod]
        public void AddChecked_ULong_NoObservable_NoUpdate()
        {
            checked
            {
                var updated = false;
                var dummy = new Dummy<ulong>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(12ul, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.AreEqual(12ul, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void AddChecked_ULong_Observable_Update()
        {
            checked
            {
                var updated = false;
                var dummy = new ObservableDummy<ulong>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(12ul, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(12ul, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.IsTrue(updated);
                Assert.AreEqual(42ul, result.Value);
            }
        }
        
        [TestMethod]
        public void Add_Int_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<int>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(12, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.AreEqual(12, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Add_Int_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<int>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(12, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(12, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.IsTrue(updated);
                Assert.AreEqual(42, result.Value);
            }
        }
        
        [TestMethod]
        public void Add_Long_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<long>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(12L, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.AreEqual(12L, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Add_Long_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<long>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(12L, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(12L, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.IsTrue(updated);
                Assert.AreEqual(42L, result.Value);
            }
        }
        
        [TestMethod]
        public void Add_Float_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<float>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(12, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.AreEqual(12, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Add_Float_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<float>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(12.0f, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(12.0f, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.IsTrue(updated);
                Assert.AreEqual(42.0f, result.Value);
            }
        }
        
        [TestMethod]
        public void Add_Double_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<double>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(12.0, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.AreEqual(12, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Add_Double_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<double>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(12.0, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(12.0, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.IsTrue(updated);
                Assert.AreEqual(42.0, result.Value);
            }
        }
        
        [TestMethod]
        public void Add_UInt_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<uint>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(12u, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.AreEqual(12u, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Add_UInt_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<uint>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(12u, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(12u, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.IsTrue(updated);
                Assert.AreEqual(42u, result.Value);
            }
        }
        
        [TestMethod]
        public void Add_ULong_NoObservable_NoUpdate()
        {
            unchecked
            {
                var updated = false;
                var dummy = new Dummy<ulong>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) => updated = true;

                Assert.AreEqual(12ul, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.AreEqual(12ul, result.Value);
                Assert.IsFalse(updated);
            }
        }

        [TestMethod]
        public void Add_ULong_Observable_Update()
        {
            unchecked
            {
                var updated = false;
                var dummy = new ObservableDummy<ulong>() { Item = 5 };

                var result = Observable.Expression(() => dummy.Item + 7);

                result.ValueChanged += (o, e) =>
                {
                    Assert.AreEqual(12ul, e.OldValue);
                    updated = true;
                };

                Assert.AreEqual(12ul, result.Value);
                Assert.IsFalse(updated);

                dummy.Item = 35;

                Assert.IsTrue(updated);
                Assert.AreEqual(42ul, result.Value);
            }
        }
        
        [TestMethod]
        public void Add_Decimal_NoObservable_NoUpdate()
        {
            var updated = false;
            var dummy = new Dummy<decimal>() { Item = 5 };

            var result = Observable.Expression(() => dummy.Item + 7);

            result.ValueChanged += (o, e) => updated = true;

            Assert.AreEqual(12, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 35;

            Assert.AreEqual(12, result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Add_Decimal_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<decimal>() { Item = 5 };

            var result = Observable.Expression(() => dummy.Item + 7);

            result.ValueChanged += (o, e) =>
            {
                Assert.AreEqual(12m, e.OldValue);
                updated = true;
            };

            Assert.AreEqual(12, result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 35;

            Assert.IsTrue(updated);
            Assert.AreEqual(42, result.Value);
        }
    }
}
