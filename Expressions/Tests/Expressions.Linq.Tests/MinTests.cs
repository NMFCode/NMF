using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class MinTests
    {
        [TestMethod]
        public void Min_NoObservableSourceNewMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.AreEqual(1, coll.WithUpdates().Min());
            Assert.IsFalse(update);

            coll.Add(0);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Min_ObservableSourceNewMinAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1, e.OldValue);
                Assert.AreEqual(0, e.NewValue);
            };

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            coll.Add(0);

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Value);
        }

        [TestMethod]
        public void Min_NoMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Min_ExistingMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);

            coll.Add(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MinComparer_NoObservableSourceNewMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);
            Assert.AreEqual(-1, coll.WithUpdates().Min(new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll.Add(0);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MinComparer_ObservableSourceNewMinAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

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
        public void MinComparer_NoMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);

            coll.Add(-4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MinComparer_ExistingMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);

            coll.Add(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMin_NoObservableSourceNewMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.AreEqual(1, coll.WithUpdates().Min());
            Assert.IsFalse(update);

            coll.Add(0);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMin_ObservableSourceNewMinAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1, e.OldValue);
                Assert.AreEqual(0, e.NewValue);
            };

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            coll.Add(0);

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Value);
        }

        [TestMethod]
        public void NullableMin_NoMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMin_ExistingMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);

            coll.Add(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMinComparer_NoObservableSourceNewMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);
            Assert.AreEqual(-1, coll.WithUpdates().Min(new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll.Add(0);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMinComparer_ObservableSourceNewMinAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

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
        public void NullableMinComparer_NoMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);

            coll.Add(-4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMinComparer_ExistingMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);

            coll.Add(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Min_NoObservableSourceMinRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.AreEqual(1, coll.WithUpdates().Min());
            Assert.IsFalse(update);

            coll.Remove(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Min_ObservableSourceMinRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1, e.OldValue);
                Assert.AreEqual(2, e.NewValue);
            };

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            coll.Remove(1);

            Assert.IsTrue(update);
            Assert.AreEqual(2, test.Value);
        }

        [TestMethod]
        public void Min_NoMinRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);

            coll.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Min_DoubleMinRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);

            coll.Remove(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MinComparer_NoObservableSourceMinRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);
            Assert.AreEqual(-1, coll.WithUpdates().Min(new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll.Remove(-1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MinComparer_ObservableSourceMinRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-1, e.OldValue);
                Assert.AreEqual(2, e.NewValue);
            };

            Assert.AreEqual(-1, test.Value);
            Assert.IsFalse(update);

            coll.Remove(-1);

            Assert.IsTrue(update);
            Assert.AreEqual(2, test.Value);
        }

        [TestMethod]
        public void MinComparer_NoMinRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);

            coll.Remove(-3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MinComparer_DoubleMinRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, -1, 2, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);

            coll.Remove(-1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMin_NoObservableSourceMinRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.AreEqual(1, coll.WithUpdates().Min());
            Assert.IsFalse(update);

            coll.Remove(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMin_ObservableSourceMinRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1, e.OldValue);
                Assert.AreEqual(2, e.NewValue);
            };

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            coll.Remove(1);

            Assert.IsTrue(update);
            Assert.AreEqual(2, test.Value);
        }

        [TestMethod]
        public void NullableMin_NoMinRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);

            coll.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMin_DoubleMinRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { 1, 1, 2, null, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);

            coll.Remove(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMinComparer_NoObservableSourceMinRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);
            Assert.AreEqual(-1, coll.WithUpdates().Min(new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll.Remove(-1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMinComparer_ObservableSourceMinRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-1, e.OldValue);
                Assert.AreEqual(2, e.NewValue);
            };

            Assert.AreEqual(-1, test.Value);
            Assert.IsFalse(update);

            coll.Remove(-1);

            Assert.IsTrue(update);
            Assert.AreEqual(2, test.Value);
        }

        [TestMethod]
        public void NullableMinComparer_NoMinRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);

            coll.Remove(-3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void NullableMinComparer_DoubleMinRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int?>() { -1, -1, 2, null, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Min(new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);

            coll.Remove(-1);

            Assert.IsFalse(update);
        }
    }
}
