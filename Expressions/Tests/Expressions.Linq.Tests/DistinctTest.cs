using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class DistinctTest
    {
        [TestMethod]
        public void Distinct_NoObservableSourceNewItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 2, 3 };

            var test = coll.WithUpdates().Distinct();

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Distinct_ObservableSource_NoUpdateWhenDetached()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 2, 3 };

            var test = coll.WithUpdates().Distinct();

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            test.Detach();
            update = false;

            coll.Add(4);

            Assert.IsFalse(update);

            test.Attach();

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Count());
            update = true;

            coll.Remove(4);

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void Distinct_ObservableSourceNewItemAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 2, 3 };

            var test = coll.WithUpdates().Distinct();

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.IsTrue(e.NewItems.Contains(4));
                Assert.IsNull(e.OldItems);
            };

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Count());
        }

        [TestMethod]
        public void Distinct_ExistingItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 2, 3 };

            var test = coll.WithUpdates().Distinct();

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            coll.Add(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Distinct_NoObservableSourceSingleItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 2, 3 };

            var test = coll.WithUpdates().Distinct();

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Distinct_ObservableSourceSingleItemRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 2, 3 };

            var test = coll.WithUpdates().Distinct();

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.IsTrue(e.OldItems.Contains(3));
                Assert.IsNull(e.NewItems);
            };

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(2, test.Count());
        }

        [TestMethod]
        public void Distinct_MultipleItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 2, 3 };

            var test = coll.WithUpdates().Distinct();

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            coll.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void DistinctComparer_NoObservableSourceNewItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, -2, 3 };

            var test = coll.WithUpdates().Distinct(new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void DistinctComparer_ObservableSourceNewItemAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, -2, 3 };

            var test = coll.WithUpdates().Distinct(new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.IsTrue(e.NewItems.Contains(4));
                Assert.IsNull(e.OldItems);
            };

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Count());
        }

        [TestMethod]
        public void DistinctComparer_ExistingItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, -2, 3 };

            var test = coll.WithUpdates().Distinct(new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            coll.Add(-3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void DistinctComparer_NoObservableSourceSingleItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, -2, 3 };

            var test = coll.WithUpdates().Distinct(new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void DistinctComparer_ObservableSourceSingleItemRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, -2, 3 };

            var test = coll.WithUpdates().Distinct(new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.IsTrue(e.OldItems.Contains(3));
                Assert.IsNull(e.NewItems);
            };

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.AreEqual(2, test.Count());
        }

        [TestMethod]
        public void DistinctComparer_MultipleItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, -2, 3 };

            var test = coll.WithUpdates().Distinct(new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(update);

            coll.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Distinct_NoObservableSourceReset_NoUpdate()
        {
            var update = false;

            var coll = new List<int>() { 1, 2, 2, 3 };

            var test = coll.WithUpdates().Distinct();

            test.CollectionChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Distinct_ObservableSourceReset_Update()
        {
            var update = false;

            var coll = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = coll.WithUpdates().Distinct();

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
            };

            coll.Clear();

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Count());
        }
    }
}
