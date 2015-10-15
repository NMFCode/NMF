using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NMF.Expressions.Test;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class SumTests
    {
        [TestMethod]
        public void IntSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void IntSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6, e.OldValue);
                Assert.AreEqual(10, e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(10, test.Value);
        }

        [TestMethod]
        public void IntSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void IntSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6, e.OldValue);
                Assert.AreEqual(3, e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(3, test.Value);
        }

        [TestMethod]
        public void LongSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<long>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6L, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LongSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<long>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6L, e.OldValue);
                Assert.AreEqual(10L, e.NewValue);
            };

            Assert.AreEqual(6L, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(10L, test.Value);
        }

        [TestMethod]
        public void LongSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<long>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6L, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LongSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<long>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6L, e.OldValue);
                Assert.AreEqual(3L, e.NewValue);
            };

            Assert.AreEqual(6L, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(3L, test.Value);
        }

        [TestMethod]
        public void FloatSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<float>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6f, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void FloatSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<float>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6f, e.OldValue);
                Assert.AreEqual(10f, e.NewValue);
            };

            Assert.AreEqual(6f, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(10f, test.Value);
        }

        [TestMethod]
        public void FloatSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<float>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6f, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void FloatSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<float>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6f, e.OldValue);
                Assert.AreEqual(3f, e.NewValue);
            };

            Assert.AreEqual(6f, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(3f, test.Value);
        }

        [TestMethod]
        public void DoubleSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<double>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6d, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void DoubleSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<double>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6d, e.OldValue);
                Assert.AreEqual(10d, e.NewValue);
            };

            Assert.AreEqual(6d, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(10d, test.Value);
        }

        [TestMethod]
        public void DoubleSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<double>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6d, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void DoubleSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<double>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6d, e.OldValue);
                Assert.AreEqual(3d, e.NewValue);
            };

            Assert.AreEqual(6d, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(3d, test.Value);
        }

        [TestMethod]
        public void DecimalSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<decimal>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6m, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void DecimalSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<decimal>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6m, e.OldValue);
                Assert.AreEqual(10m, e.NewValue);
            };

            Assert.AreEqual(6m, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(10m, test.Value);
        }

        [TestMethod]
        public void DecimalSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<decimal>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6m, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void DecimalSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<decimal>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6m, e.OldValue);
                Assert.AreEqual(3m, e.NewValue);
            };

            Assert.AreEqual(6m, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(3m, test.Value);
        }

        [TestMethod]
        public void NullableIntSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<int?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableIntSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new int?(6), e.OldValue);
                Assert.AreEqual(new int?(10), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(new int?(10), test.Value);
        }

        [TestMethod]
        public void NullableIntSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<int?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableIntSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new int?(6), e.OldValue);
                Assert.AreEqual(new int?(3), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(new int?(3), test.Value);
        }

        [TestMethod]
        public void NullableLongSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<long?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6L, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableLongSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<long?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new long?(6), e.OldValue);
                Assert.AreEqual(new long?(10), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(new long?(10), test.Value);
        }

        [TestMethod]
        public void NullableLongSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<long?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6L, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableLongSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<long?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new long?(6), e.OldValue);
                Assert.AreEqual(new long?(3), e.NewValue);
            };

            Assert.AreEqual(6L, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(new long?(3), test.Value);
        }

        [TestMethod]
        public void NullableFloatSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<float?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6f, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableFloatSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<float?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new float?(6f), e.OldValue);
                Assert.AreEqual(new float?(10f), e.NewValue);
            };

            Assert.AreEqual(6f, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(new float?(10f), test.Value);
        }

        [TestMethod]
        public void NullableFloatSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<float?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6f, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableFloatSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<float?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new float?(6f), e.OldValue);
                Assert.AreEqual(new float?(3f), e.NewValue);
            };

            Assert.AreEqual(6f, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(new float?(3f), test.Value);
        }

        [TestMethod]
        public void NullableDoubleSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<double?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6d, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableDoubleSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<double?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(6d), e.OldValue);
                Assert.AreEqual(new double?(10d), e.NewValue);
            };

            Assert.AreEqual(6d, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(10d), test.Value);
        }

        [TestMethod]
        public void NullableDoubleSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<double?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6d, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableDoubleSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<double?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(6d), e.OldValue);
                Assert.AreEqual(new double?(3d), e.NewValue);
            };

            Assert.AreEqual(6d, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(3d), test.Value);
        }

        [TestMethod]
        public void NullableDecimalSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<decimal?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6m, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableDecimalSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<decimal?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new decimal?(6m), e.OldValue);
                Assert.AreEqual(new decimal?(10m), e.NewValue);
            };

            Assert.AreEqual(6m, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(new decimal?(10m), test.Value);
        }

        [TestMethod]
        public void NullableDecimalSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<decimal?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6m, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableDecimalSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<decimal?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new decimal?(6m), e.OldValue);
                Assert.AreEqual(new decimal?(3m), e.NewValue);
            };

            Assert.AreEqual(6m, test.Value);
            Assert.AreEqual(6, testColl.Sum());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(new decimal?(3m), test.Value);
        }
    }
}
