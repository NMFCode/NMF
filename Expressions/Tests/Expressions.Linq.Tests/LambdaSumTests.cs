using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NMF.Expressions.Test;
using System.Collections.ObjectModel;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class LambdaSumTests
    {
        [TestMethod]
        public void LambdaIntSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), new Dummy<int>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaIntSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), new Dummy<int>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6, e.OldValue);
                Assert.AreEqual(10, e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(10, test.Value);
        }

        [TestMethod]
        public void LambdaIntSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<int>(3);
            var coll = new List<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaIntSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<int>(3);
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6, e.OldValue);
                Assert.AreEqual(3, e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(3, test.Value);
        }

        [TestMethod]
        public void LambdaLongSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<long>>() { new Dummy<long>(1), new Dummy<long>(2), new Dummy<long>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6L, test.Value);
            Assert.AreEqual(6L, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<long>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaLongSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<long>>() { new Dummy<long>(1), new Dummy<long>(2), new Dummy<long>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6L, e.OldValue);
                Assert.AreEqual(10L, e.NewValue);
            };

            Assert.AreEqual(6L, test.Value);
            Assert.AreEqual(6L, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<long>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(10L, test.Value);
        }

        [TestMethod]
        public void LambdaLongSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<long>(3);
            var coll = new List<Dummy<long>>() { new Dummy<long>(1), new Dummy<long>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6L, test.Value);
            Assert.AreEqual(6L, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaLongSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<long>(3);
            var coll = new ObservableCollection<Dummy<long>>() { new Dummy<long>(1), new Dummy<long>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6L, e.OldValue);
                Assert.AreEqual(3L, e.NewValue);
            };

            Assert.AreEqual(6L, test.Value);
            Assert.AreEqual(6L, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(3L, test.Value);
        }

        [TestMethod]
        public void LambdaFloatSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<float>>() { new Dummy<float>(1), new Dummy<float>(2), new Dummy<float>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6f, test.Value);
            Assert.AreEqual(6f, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<float>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaFloatSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<float>>() { new Dummy<float>(1), new Dummy<float>(2), new Dummy<float>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6f, e.OldValue);
                Assert.AreEqual(10f, e.NewValue);
            };

            Assert.AreEqual(6f, test.Value);
            Assert.AreEqual(6f, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<float>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(10f, test.Value);
        }

        [TestMethod]
        public void LambdaFloatSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<float>(3);
            var coll = new List<Dummy<float>>() { new Dummy<float>(1), new Dummy<float>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6f, test.Value);
            Assert.AreEqual(6f, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaFloatSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<float>(3);
            var coll = new ObservableCollection<Dummy<float>>() { new Dummy<float>(1), new Dummy<float>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6f, e.OldValue);
                Assert.AreEqual(3f, e.NewValue);
            };

            Assert.AreEqual(6f, test.Value);
            Assert.AreEqual(6f, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(3f, test.Value);
        }

        [TestMethod]
        public void LambdaDoubleSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<double>>() { new Dummy<double>(1), new Dummy<double>(2), new Dummy<double>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6d, test.Value);
            Assert.AreEqual(6d, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<double>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaDoubleSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<double>>() { new Dummy<double>(1), new Dummy<double>(2), new Dummy<double>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6.0, e.OldValue);
                Assert.AreEqual(10.0, e.NewValue);
            };

            Assert.AreEqual(6d, test.Value);
            Assert.AreEqual(6d, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<double>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(10d, test.Value);
        }

        [TestMethod]
        public void LambdaDoubleSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<double>(3);
            var coll = new List<Dummy<double>>() { new Dummy<double>(1), new Dummy<double>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6d, test.Value);
            Assert.AreEqual(6d, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaDoubleSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<double>(3);
            var coll = new ObservableCollection<Dummy<double>>() { new Dummy<double>(1), new Dummy<double>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6.0, e.OldValue);
                Assert.AreEqual(3.0, e.NewValue);
            };

            Assert.AreEqual(6d, test.Value);
            Assert.AreEqual(6d, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(3d, test.Value);
        }

        [TestMethod]
        public void LambdaDecimalSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<decimal>>() { new Dummy<decimal>(1), new Dummy<decimal>(2), new Dummy<decimal>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6m, test.Value);
            Assert.AreEqual(6m, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<decimal>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaDecimalSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<decimal>>() { new Dummy<decimal>(1), new Dummy<decimal>(2), new Dummy<decimal>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6m, e.OldValue);
                Assert.AreEqual(10m, e.NewValue);
            };

            Assert.AreEqual(6m, test.Value);
            Assert.AreEqual(6m, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<decimal>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(10m, test.Value);
        }

        [TestMethod]
        public void LambdaDecimalSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<decimal>(3);
            var coll = new List<Dummy<decimal>>() { new Dummy<decimal>(1), new Dummy<decimal>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6m, test.Value);
            Assert.AreEqual(6m, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaDecimalSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<decimal>(3);
            var coll = new ObservableCollection<Dummy<decimal>>() { new Dummy<decimal>(1), new Dummy<decimal>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(6m, e.OldValue);
                Assert.AreEqual(3m, e.NewValue);
            };

            Assert.AreEqual(6m, test.Value);
            Assert.AreEqual(6m, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(3m, test.Value);
        }

        [TestMethod]
        public void LambdaNullableIntSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), new Dummy<int?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableIntSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), new Dummy<int?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new int?(6), e.OldValue);
                Assert.AreEqual(new int?(10), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int?>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(new int?(10), test.Value);
        }

        [TestMethod]
        public void LambdaNullableIntSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<int?>(3);
            var coll = new List<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableIntSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<int?>(3);
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new int?(6), e.OldValue);
                Assert.AreEqual(new int?(3), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(new int?(3), test.Value);
        }

        [TestMethod]
        public void LambdaNullableLongSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<long?>>() { new Dummy<long?>(1), new Dummy<long?>(2), new Dummy<long?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<long?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableLongSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<long?>>() { new Dummy<long?>(1), new Dummy<long?>(2), new Dummy<long?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new long?(6), e.OldValue);
                Assert.AreEqual(new long?(10), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<long?>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(new long?(10), test.Value);
        }

        [TestMethod]
        public void LambdaNullableLongSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<long?>(3);
            var coll = new List<Dummy<long?>>() { new Dummy<long?>(1), new Dummy<long?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableLongSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<long?>(3);
            var coll = new ObservableCollection<Dummy<long?>>() { new Dummy<long?>(1), new Dummy<long?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new long?(6), e.OldValue);
                Assert.AreEqual(new long?(3), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(new long?(3), test.Value);
        }

        [TestMethod]
        public void LambdaNullableFloatSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<float?>>() { new Dummy<float?>(1), new Dummy<float?>(2), new Dummy<float?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<float?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableFloatSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<float?>>() { new Dummy<float?>(1), new Dummy<float?>(2), new Dummy<float?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new float?(6f), e.OldValue);
                Assert.AreEqual(new float?(10f), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<float?>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(new float?(10f), test.Value);
        }

        [TestMethod]
        public void LambdaNullableFloatSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<float?>(3);
            var coll = new List<Dummy<float?>>() { new Dummy<float?>(1), new Dummy<float?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableFloatSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<float?>(3);
            var coll = new ObservableCollection<Dummy<float?>>() { new Dummy<float?>(1), new Dummy<float?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new float?(6f), e.OldValue);
                Assert.AreEqual(new float?(3f), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(new float?(3f), test.Value);
        }

        [TestMethod]
        public void LambdaNullableDoubleSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<double?>>() { new Dummy<double?>(1), new Dummy<double?>(2), new Dummy<double?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<double?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableDoubleSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<double?>>() { new Dummy<double?>(1), new Dummy<double?>(2), new Dummy<double?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(6), e.OldValue);
                Assert.AreEqual(new double?(10), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<double?>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(10), test.Value);
        }

        [TestMethod]
        public void LambdaNullableDoubleSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<double?>(3);
            var coll = new List<Dummy<double?>>() { new Dummy<double?>(1), new Dummy<double?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableDoubleSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<double?>(3);
            var coll = new ObservableCollection<Dummy<double?>>() { new Dummy<double?>(1), new Dummy<double?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(6), e.OldValue);
                Assert.AreEqual(new double?(3), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(3), test.Value);
        }

        [TestMethod]
        public void LambdaNullableDecimalSum_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<decimal?>>() { new Dummy<decimal?>(1), new Dummy<decimal?>(2), new Dummy<decimal?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<decimal?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableDecimalSum_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<decimal?>>() { new Dummy<decimal?>(1), new Dummy<decimal?>(2), new Dummy<decimal?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new decimal?(6m), e.OldValue);
                Assert.AreEqual(new decimal?(10m), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<decimal?>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(new decimal?(10m), test.Value);
        }

        [TestMethod]
        public void LambdaNullableDecimalSum_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<decimal?>(3);
            var coll = new List<Dummy<decimal?>>() { new Dummy<decimal?>(1), new Dummy<decimal?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableDecimalSum_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<decimal?>(3);
            var coll = new ObservableCollection<Dummy<decimal?>>() { new Dummy<decimal?>(1), new Dummy<decimal?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Sum(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new decimal?(6m), e.OldValue);
                Assert.AreEqual(new decimal?(3m), e.NewValue);
            };

            Assert.AreEqual(6, test.Value);
            Assert.AreEqual(6, testColl.Sum(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(new decimal?(3m), test.Value);
        }
    }
}
