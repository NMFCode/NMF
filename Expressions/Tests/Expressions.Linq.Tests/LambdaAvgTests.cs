using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using NMF.Expressions.Test;
using System.Collections.ObjectModel;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class LambdaAvgTests
    {
        [TestMethod]
        public void LambdaIntAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), new Dummy<int>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaIntAverage_ObservableSourceItemAdded_NoUpdatesWhenDetached()
        {
            var update = false;
            var coll = new NotifyCollection<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), new Dummy<int>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            test.Detach();

            var testDummy = new ObservableDummy<int>(5);
            coll.Add(testDummy);

            Assert.IsFalse(update);

            testDummy.Item = 4;

            Assert.IsFalse(update);

            test.Attach();

            Assert.IsTrue(update);
            Assert.AreEqual(2.5, test.Value);
            update = false;

            testDummy.Item = 5;

            Assert.IsTrue(update);
            update = false;

            coll.Remove(testDummy);
        }

        [TestMethod]
        public void LambdaIntAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), new Dummy<int>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0, e.OldValue);
                Assert.AreEqual(2.5, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(2.5, test.Value);
        }

        [TestMethod]
        public void LambdaIntAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<int>(3);
            var coll = new List<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaIntAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<int>(3);
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0, e.OldValue);
                Assert.AreEqual(1.5, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(1.5, test.Value);
        }

        [TestMethod]
        public void LambdaLongAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<long>>() { new Dummy<long>(1), new Dummy<long>(2), new Dummy<long>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<long>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaLongAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<long>>() { new Dummy<long>(1), new Dummy<long>(2), new Dummy<long>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0, e.OldValue);
                Assert.AreEqual(2.5, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<long>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(2.5, test.Value);
        }

        [TestMethod]
        public void LambdaLongAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<long>(3);
            var coll = new List<Dummy<long>>() { new Dummy<long>(1), new Dummy<long>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaLongAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<long>(3);
            var coll = new ObservableCollection<Dummy<long>>() { new Dummy<long>(1), new Dummy<long>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0, e.OldValue);
                Assert.AreEqual(1.5, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(1.5, test.Value);
        }

        [TestMethod]
        public void LambdaFloatAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<float>>() { new Dummy<float>(1), new Dummy<float>(2), new Dummy<float>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<float>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaFloatAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<float>>() { new Dummy<float>(1), new Dummy<float>(2), new Dummy<float>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0f, e.OldValue);
                Assert.AreEqual(2.5f, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<float>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(2.5, test.Value);
        }

        [TestMethod]
        public void LambdaFloatAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<float>(3);
            var coll = new List<Dummy<float>>() { new Dummy<float>(1), new Dummy<float>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaFloatAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<float>(3);
            var coll = new ObservableCollection<Dummy<float>>() { new Dummy<float>(1), new Dummy<float>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0f, e.OldValue);
                Assert.AreEqual(1.5f, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(1.5, test.Value);
        }

        [TestMethod]
        public void LambdaDoubleAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<double>>() { new Dummy<double>(1), new Dummy<double>(2), new Dummy<double>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<double>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaDoubleAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<double>>() { new Dummy<double>(1), new Dummy<double>(2), new Dummy<double>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0, e.OldValue);
                Assert.AreEqual(2.5, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<double>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(2.5, test.Value);
        }

        [TestMethod]
        public void LambdaDoubleAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<double>(3);
            var coll = new List<Dummy<double>>() { new Dummy<double>(1), new Dummy<double>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaDoubleAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<double>(3);
            var coll = new ObservableCollection<Dummy<double>>() { new Dummy<double>(1), new Dummy<double>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0, e.OldValue);
                Assert.AreEqual(1.5, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(1.5, test.Value);
        }

        [TestMethod]
        public void LambdaDecimalAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<decimal>>() { new Dummy<decimal>(1), new Dummy<decimal>(2), new Dummy<decimal>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<decimal>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaDecimalAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<decimal>>() { new Dummy<decimal>(1), new Dummy<decimal>(2), new Dummy<decimal>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0m, e.OldValue);
                Assert.AreEqual(2.5m, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<decimal>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(2.5m, test.Value);
        }

        [TestMethod]
        public void LambdaDecimalAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<decimal>(3);
            var coll = new List<Dummy<decimal>>() { new Dummy<decimal>(1), new Dummy<decimal>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaDecimalAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<decimal>(3);
            var coll = new ObservableCollection<Dummy<decimal>>() { new Dummy<decimal>(1), new Dummy<decimal>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2.0m, e.OldValue);
                Assert.AreEqual(1.5m, e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(1.5m, test.Value);
        }

        [TestMethod]
        public void LambdaNullableIntAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), new Dummy<int?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableIntAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), new Dummy<int?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(2.0), e.OldValue);
                Assert.AreEqual(new double?(2.5), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int?>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(2.5), test.Value);
        }

        [TestMethod]
        public void LambdaNullableIntAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<int?>(3);
            var coll = new List<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableIntAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<int?>(3);
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(2.0), e.OldValue);
                Assert.AreEqual(new double?(1.5), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(1.5), test.Value);
        }

        [TestMethod]
        public void LambdaNullableLongAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<long?>>() { new Dummy<long?>(1), new Dummy<long?>(2), new Dummy<long?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<long?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableLongAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<long?>>() { new Dummy<long?>(1), new Dummy<long?>(2), new Dummy<long?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(2.0), e.OldValue);
                Assert.AreEqual(new double?(2.5), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<long?>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(2.5), test.Value);
        }

        [TestMethod]
        public void LambdaNullableLongAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<long?>(3);
            var coll = new List<Dummy<long?>>() { new Dummy<long?>(1), new Dummy<long?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableLongAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<long?>(3);
            var coll = new ObservableCollection<Dummy<long?>>() { new Dummy<long?>(1), new Dummy<long?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(2.0), e.OldValue);
                Assert.AreEqual(new double?(1.5), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(1.5), test.Value);
        }

        [TestMethod]
        public void LambdaNullableFloatAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<float?>>() { new Dummy<float?>(1), new Dummy<float?>(2), new Dummy<float?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<float?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableFloatAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<float?>>() { new Dummy<float?>(1), new Dummy<float?>(2), new Dummy<float?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new float?(2.0f), e.OldValue);
                Assert.AreEqual(new float?(2.5f), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<float?>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(new float?(2.5f), test.Value);
        }

        [TestMethod]
        public void LambdaNullableFloatAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<float?>(3);
            var coll = new List<Dummy<float?>>() { new Dummy<float?>(1), new Dummy<float?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableFloatAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<float?>(3);
            var coll = new ObservableCollection<Dummy<float?>>() { new Dummy<float?>(1), new Dummy<float?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new float?(2.0f), e.OldValue);
                Assert.AreEqual(new float?(1.5f), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(new float?(1.5f), test.Value);
        }

        [TestMethod]
        public void LambdaNullableDoubleAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<double?>>() { new Dummy<double?>(1), new Dummy<double?>(2), new Dummy<double?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<double?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableDoubleAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<double?>>() { new Dummy<double?>(1), new Dummy<double?>(2), new Dummy<double?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(2.0), e.OldValue);
                Assert.AreEqual(new double?(2.5), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<double?>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(2.5), test.Value);
        }

        [TestMethod]
        public void LambdaNullableDoubleAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<double?>(3);
            var coll = new List<Dummy<double?>>() { new Dummy<double?>(1), new Dummy<double?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableDoubleAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<double?>(3);
            var coll = new ObservableCollection<Dummy<double?>>() { new Dummy<double?>(1), new Dummy<double?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new double?(2.0), e.OldValue);
                Assert.AreEqual(new double?(1.5), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(new double?(1.5), test.Value);
        }

        [TestMethod]
        public void LambdaNullableDecimalAverage_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            var coll = new List<Dummy<decimal?>>() { new Dummy<decimal?>(1), new Dummy<decimal?>(2), new Dummy<decimal?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<decimal?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableDecimalAverage_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<decimal?>>() { new Dummy<decimal?>(1), new Dummy<decimal?>(2), new Dummy<decimal?>(3) };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new decimal?(2.0m), e.OldValue);
                Assert.AreEqual(new decimal?(2.5m), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<decimal?>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(new decimal?(2.5m), test.Value);
        }

        [TestMethod]
        public void LambdaNullableDecimalAverage_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<decimal?>(3);
            var coll = new List<Dummy<decimal?>>() { new Dummy<decimal?>(1), new Dummy<decimal?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableDecimalAverage_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            var dummy = new Dummy<decimal?>(3);
            var coll = new ObservableCollection<Dummy<decimal?>>() { new Dummy<decimal?>(1), new Dummy<decimal?>(2), dummy };
            var testColl = coll.WithUpdates();

            var test = Observable.Expression(() => testColl.Average(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(new decimal?(2.0m), e.OldValue);
                Assert.AreEqual(new decimal?(1.5m), e.NewValue);
            };

            Assert.AreEqual(2, test.Value);
            Assert.AreEqual(2, testColl.Average(d => d.Item));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(update);
            Assert.AreEqual(new decimal?(1.5m), test.Value);
        }
    }
}
