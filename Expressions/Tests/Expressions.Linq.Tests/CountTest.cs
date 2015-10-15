using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class CountTest
    {
        [TestMethod]
        public void Count_NoObservableSourceItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Count());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Count_ObservableSourceItemAdded_NoUpdateWhenDetached()
        {
            var update = false;
            var coll = new NotifyCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Count());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);
            Assert.IsFalse(update);

            test.Detach();

            coll.Add(4);

            Assert.IsFalse(update);

            test.Attach();

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Value);
            update = true;

            coll.Add(5);

            Assert.IsTrue(update);
        }


        [TestMethod]
        public void Count_ObservableSourceItemAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Count());

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
        public void Count_NoObservableSourceReset_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Count());

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Count_ObservableSourceReset_Update()
        {
            var update = false;
            var coll = new NotifyCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Count());

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void CountPredicate_NoObservableSourceItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3, -1, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Count(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void CountPredicate_ObservableSourceItemAdded_NoUpdateWhenDetached()
        {
            var update = false;
            var coll = new NotifyCollection<int>() { 1, 2, 3, -1, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Count(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);
            Assert.IsFalse(update);

            test.Detach();

            coll.Add(4);

            Assert.IsFalse(update);

            test.Attach();

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Value);
            update = true;

            coll.Add(5);

            Assert.IsTrue(update);
        }


        [TestMethod]
        public void CountPredicate_ObservableSourceItemAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3, -1, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Count(i => i > 0));

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
        public void CountPredicate_NoObservableSourceReset_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3, -1, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Count(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void CountPredicate_ObservableSourceReset_Update()
        {
            var update = false;
            var coll = new NotifyCollection<int>() { 1, 2, 3, -1, -3 };

            var test = Observable.Expression(() => coll.WithUpdates().Count(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsTrue(update);
        }
    }
}
