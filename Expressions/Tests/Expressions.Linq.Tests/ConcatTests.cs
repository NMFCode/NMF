using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class ConcatTests
    {
        [TestMethod]
        public void Concat_NoObservableSource1ItemAdded_NoUpdate()
        {
            var update = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 4, 5, 6 };

            var test = coll1.WithUpdates().Concat(coll2);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence(1, 2, 3, 4, 5, 6);
            Assert.IsFalse(update);

            coll1.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Concat_ObservableSource1ItemAdded_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 4, 5, 6 };

            var test = coll1.WithUpdates().Concat(coll2);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(0, e.NewItems[0]);
                Assert.IsNull(e.OldItems);
            };

            test.AssertSequence(1, 2, 3, 4, 5, 6);
            Assert.IsFalse(update);

            coll1.Add(0);

            Assert.IsTrue(update);
            test.AssertSequence(1, 2, 3, 0, 4, 5, 6);
        }

        [TestMethod]
        public void Concat_NoObservableSource2ItemAdded_NoUpdate()
        {
            var update = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 4, 5, 6 };

            var test = coll1.WithUpdates().Concat(coll2);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence(1, 2, 3, 4, 5, 6);
            Assert.IsFalse(update);

            coll2.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Concat_ObservableSource2ItemAdded_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 4, 5, 6 };

            var test = coll1.WithUpdates().Concat(coll2.WithUpdates());

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(0, e.NewItems[0]);
                Assert.IsNull(e.OldItems);
            };

            test.AssertSequence(1, 2, 3, 4, 5, 6);
            Assert.IsFalse(update);

            coll2.Add(0);

            Assert.IsTrue(update);
            test.AssertSequence(1, 2, 3, 4, 5, 6, 0);
        }

        [TestMethod]
        public void Concat_NoObservableSource1ItemRemoved_NoUpdate()
        {
            var update = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 4, 5, 6 };

            var test = coll1.WithUpdates().Concat(coll2);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence(1, 2, 3, 4, 5, 6);
            Assert.IsFalse(update);

            coll1.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Concat_ObservableSource1ItemRemoved_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 4, 5, 6 };

            var test = coll1.WithUpdates().Concat(coll2);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(3, e.OldItems[0]);
                Assert.IsNull(e.NewItems);
            };

            test.AssertSequence(1, 2, 3, 4, 5, 6);
            Assert.IsFalse(update);

            coll1.Remove(3);

            Assert.IsTrue(update);
            test.AssertSequence(1, 2, 4, 5, 6);
        }

        [TestMethod]
        public void Concat_NoObservableSource2ItemRemoved_NoUpdate()
        {
            var update = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 4, 5, 6 };

            var test = coll1.WithUpdates().Concat(coll2);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence(1, 2, 3, 4, 5, 6);
            Assert.IsFalse(update);

            coll2.Remove(6);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Concat_ObservableSource2ItemRemoved_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 4, 5, 6 };

            var test = coll1.WithUpdates().Concat(coll2.WithUpdates());

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(6, e.OldItems[0]);
                Assert.IsNull(e.NewItems);
            };

            test.AssertSequence(1, 2, 3, 4, 5, 6);
            Assert.IsFalse(update);

            coll2.Remove(6);

            Assert.IsTrue(update);
            test.AssertSequence(1, 2, 3, 4, 5);
        }

        [TestMethod]
        public void Concat_NoObservableSource1Reset_NoUpdate()
        {
            var update = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 4, 5, 6 };

            var test = coll1.WithUpdates().Concat(coll2);

            test.CollectionChanged += (o, e) => update = true;

            coll1.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Concat_ObservableSource1Reset_Update()
        {
            var reset = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 4, 5, 6 };

            var test = coll1.WithUpdates().Concat(coll2);

            test.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    reset = true;
                }
            };

            coll1.Clear();

            Assert.IsTrue(reset);
            test.AssertSequence(4, 5, 6);
        }

        [TestMethod]
        public void Concat_NoObservableSource2Reset_NoUpdate()
        {
            var update = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 4, 5, 6 };

            var test = coll1.WithUpdates().Concat(coll2);

            test.CollectionChanged += (o, e) => update = true;

            coll2.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Concat_ObservableSource2Reset_Update()
        {
            var reset = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 4, 5, 6 };

            var test = coll1.WithUpdates().Concat(coll2.WithUpdates());

            test.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    reset = true;
                }
            };

            coll2.Clear();

            Assert.IsTrue(reset);
            test.AssertSequence(1, 2, 3);
        }
    }
}
