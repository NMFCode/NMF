using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NMF.Expressions.Test;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class AvgTests
    {
        [TestMethod]
        public void IntAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void IntAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0, e.OldValue);
                Assert.AreEqual(2.5, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(2.5, test.Value);
        }

        [TestMethod]
        public void IntAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void IntAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0, e.OldValue);
                Assert.AreEqual(1.5, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(1.5, test.Value);
        }

        [TestMethod]
        public void LongAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<long>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LongAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<long>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0, e.OldValue);
                Assert.AreEqual(2.5, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(2.5, test.Value);
        }

        [TestMethod]
        public void LongAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<long>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LongAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<long>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0, e.OldValue);
                Assert.AreEqual(1.5, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(1.5, test.Value);
        }

        [TestMethod]
        public void FloatAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<float>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void FloatAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<float>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0f, e.OldValue);
                Assert.AreEqual(2.5f, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(2.5, test.Value);
        }

        [TestMethod]
        public void FloatAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<float>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void FloatAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<float>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0f, e.OldValue);
                Assert.AreEqual(1.5f, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(1.5, test.Value);
        }

        [TestMethod]
        public void DoubleAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<double>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void DoubleAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<double>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0, e.OldValue);
                Assert.AreEqual(2.5, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(2.5, test.Value);
        }

        [TestMethod]
        public void DoubleAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<double>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void DoubleAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<double>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0, e.OldValue);
                Assert.AreEqual(1.5, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(1.5, test.Value);
        }

        [TestMethod]
        public void DecimalAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<decimal>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void DecimalAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<decimal>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0m, e.OldValue);
                Assert.AreEqual(2.5m, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(2.5m, test.Value);
        }

        [TestMethod]
        public void DecimalAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<decimal>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void DecimalAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<decimal>() { 1, 2, 3 };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0m, e.OldValue);
                Assert.AreEqual(1.5m, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(1.5m, test.Value);
        }

        [TestMethod]
        public void NullableIntAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<int?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableIntAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(2.0), e.OldValue);
                Assert.AreEqual(new double?(2.5), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(2.5), test.Value);
        }

        [TestMethod]
        public void NullableIntAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<int?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableIntAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(2.0), e.OldValue);
                Assert.AreEqual(new double?(1.5), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(1.5), test.Value);
        }

        [TestMethod]
        public void NullableLongAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<long?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableLongAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<long?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(2.0), e.OldValue);
                Assert.AreEqual(new double?(2.5), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(2.5), test.Value);
        }

        [TestMethod]
        public void NullableLongAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<long?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableLongAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<long?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(2.0), e.OldValue);
                Assert.AreEqual(new double?(1.5), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(1.5), test.Value);
        }

        [TestMethod]
        public void NullableFloatAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<float?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableFloatAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<float?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new float?(2.0f), e.OldValue);
                Assert.AreEqual(new float?(2.5f), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(new float?(2.5f), test.Value);
        }

        [TestMethod]
        public void NullableFloatAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<float?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableFloatAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<float?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new float?(2.0f), e.OldValue);
                Assert.AreEqual(new float?(1.5f), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(new float?(1.5f), test.Value);
        }

        [TestMethod]
        public void NullableDoubleAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<double?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableDoubleAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<double?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(2.0), e.OldValue);
                Assert.AreEqual(new double?(2.5), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(2.5), test.Value);
        }

        [TestMethod]
        public void NullableDoubleAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<double?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableDoubleAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<double?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(2.0), e.OldValue);
                Assert.AreEqual(new double?(1.5), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(1.5), test.Value);
        }

        [TestMethod]
        public void NullableDecimalAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<decimal?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableDecimalAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<decimal?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new decimal?(2.0m), e.OldValue);
                Assert.AreEqual(new decimal?(2.5m), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(new decimal?(2.5m), test.Value);
        }

        [TestMethod]
        public void NullableDecimalAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var coll = new List<decimal?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableDecimalAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<decimal?>() { 1, 2, 3, null };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new decimal?(2.0m), e.OldValue);
                Assert.AreEqual(new decimal?(1.5m), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(new decimal?(1.5m), test.Value);
        }

        [TestMethod]
        public void IntAverage_NoObservableSourceReset_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Average());

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void IntAverage_ObservableSourceReset_Update()
        {
            var update = false;
            var coll = new NotifyCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.Average());

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void NullableIntAverage_NoObservableSourceReset_NoUpdate()
        {
            var update = false;
            var coll = new List<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Average());

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableIntAverage_ObservableSourceReset_Update()
        {
            var update = false;
            var coll = new NotifyCollection<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.Average());

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void LambdaIntAverage_NoObservableSourceReset_NoUpdate()
        {
            var update = false;
            var coll = new List<Dummy<int>>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaIntAverage_ObservableSourceReset_Update()
        {
            var update = false;
            var coll = new NotifyCollection<Dummy<int>>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void LambdaNullableIntAverage_NoObservableSourceReset_NoUpdate()
        {
            var update = false;
            var coll = new List<Dummy<int?>>() { 1, 2, new Dummy<int?>(), 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableIntAverage_ObservableSourceReset_Update()
        {
            var update = false;
            var coll = new NotifyCollection<Dummy<int?>>() { 1, 2, new Dummy<int?>(), 3 };

            var test = Observable.Expression(() => coll.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsTrue(update);
        }
    }
}
