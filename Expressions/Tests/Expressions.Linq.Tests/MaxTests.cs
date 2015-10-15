using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class MaxTests
    {
        [TestMethod]
        public void Max_NoObservableSourceNewMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { -1, -2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);
            Assert.AreEqual(-1, coll.WithUpdates().Max());
            Assert.IsFalse(update);

            coll.Add(0);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Max_ObservableSourceNewMaxAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, -2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-1, e.OldValue);
                Assert.AreEqual(0, e.NewValue);
            };

            Assert.AreEqual(-1, test.Value);
            Assert.IsFalse(update);

            coll.Add(0);

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Value);
        }

        [TestMethod]
        public void Max_NoMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);

            coll.Add(-4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Max_ExistingMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);

            coll.Add(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MaxComparer_NoObservableSourceNewMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);
            Assert.AreEqual(-3, coll.WithUpdates().Max(new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MaxComparer_ObservableSourceNewMaxAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-3, e.OldValue);
                Assert.AreEqual(4, e.NewValue);
            };

            Assert.AreEqual(-3, test.Value);
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Value);
        }

        [TestMethod]
        public void MaxComparer_NoMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);

            coll.Add(0);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MaxComparer_ExistingMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);

            coll.Add(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMax_NoObservableSourceNewMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);
            Assert.AreEqual(3, coll.WithUpdates().Max());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMax_ObservableSourceNewMaxAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(3, e.OldValue);
                Assert.AreEqual(4, e.NewValue);
            };

            Assert.AreEqual(3, test.Value);
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Value);
        }

        [TestMethod]
        public void NullableMax_NoMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);

            coll.Add(0);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMax_ExistingMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);

            coll.Add(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMaxComparer_NoObservableSourceNewMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);
            Assert.AreEqual(-3, coll.WithUpdates().Max(new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMaxComparer_ObservableSourceNewMaxAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-3, e.OldValue);
                Assert.AreEqual(4, e.NewValue);
            };

            Assert.AreEqual(-3, test.Value);
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Value);
        }

        [TestMethod]
        public void NullableMaxComparer_NoMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);

            coll.Add(0);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMaxComparer_ExistingMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);

            coll.Add(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Max_NoObservableSourceNewMaxRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { -1, -2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);
            Assert.AreEqual(-1, coll.WithUpdates().Max());
            Assert.IsFalse(update);

            coll.Remove(-1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Max_ObservableSourceMaxRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, -2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-1, e.OldValue);
                Assert.AreEqual(-2, e.NewValue);
            };

            Assert.AreEqual(-1, test.Value);
            Assert.IsFalse(update);

            coll.Remove(-1);

            Assert.IsTrue(update);
            Assert.AreEqual(-2, test.Value);
        }

        [TestMethod]
        public void Max_NoMaxRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);

            coll.Remove(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Max_DoubleMaxRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MaxComparer_NoObservableSourceMaxRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);
            Assert.AreEqual(-3, coll.WithUpdates().Max(new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll.Remove(-3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MaxComparer_ObservableSourceMaxRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-3, e.OldValue);
                Assert.AreEqual(2, e.NewValue);
            };

            Assert.AreEqual(-3, test.Value);
            Assert.IsFalse(update);

            coll.Remove(-3);

            Assert.IsTrue(update);
            Assert.AreEqual(2, test.Value);
        }

        [TestMethod]
        public void MaxComparer_NoMaxRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);

            coll.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MaxComparer_DoubleMaxRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, 2, -3, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);

            coll.Remove(-3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMax_NoObservableSourceMaxRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);
            Assert.AreEqual(3, coll.WithUpdates().Max());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMax_ObservableSourceMaxRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(3, e.OldValue);
                Assert.AreEqual(2, e.NewValue);
            };

            Assert.AreEqual(3, test.Value);
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(2, test.Value);
        }

        [TestMethod]
        public void NullableMax_NoMaxRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);

            coll.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMax_DoubleMaxRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, null, 3, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMaxComparer_NoObservableSourceMaxRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);
            Assert.AreEqual(-3, coll.WithUpdates().Max(new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll.Remove(-3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMaxComparer_ObservableSourceMaxRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-3, e.OldValue);
                Assert.AreEqual(2, e.NewValue);
            };

            Assert.AreEqual(-3, test.Value);
            Assert.IsFalse(update);

            coll.Remove(-3);

            Assert.IsTrue(update);
            Assert.AreEqual(2, test.Value);
        }

        [TestMethod]
        public void NullableMaxComparer_NoMaxRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);

            coll.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMaxComparer_DoubleMaxRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { -1, 2, null, -3, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Max(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);

            coll.Remove(-3);

            Assert.IsFalse(update);
        }
    }
}
