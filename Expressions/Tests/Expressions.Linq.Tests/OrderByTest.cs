using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Test;
using System.Linq;
using System.Collections.Specialized;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class OrderByTest
    {
        [TestMethod]
        public void OrderBy_NoObservableSourceItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<string>() { "C", "A", "D" };

            var test = coll.WithUpdates().OrderBy(item => item);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence("A", "C", "D");
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            coll.Add("B");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void OrderBy_ObservableSourceItemAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<string>() { "C", "A", "D" };

            var test = coll.WithUpdates().OrderBy(item => item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual("B", e.NewItems[0]);
                update = true;
            };

            test.AssertSequence("A", "C", "D");
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            coll.Add("B");

            Assert.IsTrue(update);
            test.AssertSequence("A", "B", "C", "D");
            Assert.AreEqual(4, test.Sequences.Count());
        }

        [TestMethod]
        public void OrderBy_NoObservableSourceItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<string>() { "C", "A", "D" };

            var test = coll.WithUpdates().OrderBy(item => item);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence("A", "C", "D");
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            coll.Remove("C");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void OrderBy_ObservableSourceItemRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<string>() { "C", "A", "D" };

            var test = coll.WithUpdates().OrderBy(item => item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual("C", e.OldItems[0]);
                update = true;
            };

            test.AssertSequence("A", "C", "D");
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            coll.Remove("C");

            Assert.IsTrue(update);
            test.AssertSequence("A", "D");
        }

        [TestMethod]
        public void OrderBy_NoObservableItemChanges_NoUpdate()
        {
            var update = false;
            var dummy1 = new Dummy<int>(1);
            var dummy2 = new Dummy<int>(3);
            var dummy3 = new Dummy<int>(5);
            var coll = new List<Dummy<int>>() { dummy1, dummy2, dummy3 };

            var test = coll.WithUpdates().OrderBy(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence(dummy1, dummy2, dummy3);
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            dummy1.Item = 4;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void OrderBy_ObservableItemChanges_Update()
        {
            var update = false;
            var dummy1 = new ObservableDummy<int>(1);
            var dummy2 = new Dummy<int>(3);
            var dummy3 = new Dummy<int>(5);
            var coll = new List<Dummy<int>>() { dummy1, dummy2, dummy3 };

            var test = coll.WithUpdates().OrderBy(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreSame(dummy1, e.NewItems[0]);
                Assert.AreSame(dummy1, e.OldItems[0]);
                update = true;
            };

            test.AssertSequence(dummy1, dummy2, dummy3);
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            dummy1.Item = 4;

            Assert.IsTrue(update);
            test.AssertSequence(dummy2, dummy1, dummy3);
        }
        
        [TestMethod]
        public void OrderBy_NoObservableSourceReset_NoUpdate()
        {
            var update = false;
            var coll = new List<string>() { "C", "A", "D" };

            var test = coll.WithUpdates().OrderBy(item => item);

            test.CollectionChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void OrderBy_ObservableSourceReset_Update()
        {
            var update = false;
            var coll = new ObservableCollection<string>() { "C", "A", "D" };

            var test = coll.WithUpdates().OrderBy(item => item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
                update = true;
            };

            coll.Clear();

            Assert.IsTrue(update);
            Assert.IsFalse(test.Any());
        }

        [TestMethod]
        public void OrderByDescending_NoObservableSourceItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<string>() { "C", "A", "D" };

            var test = coll.WithUpdates().OrderByDescending(item => item);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence("D", "C", "A");
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            coll.Add("B");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void OrderByDescending_ObservableSourceItemAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<string>() { "C", "A", "D" };

            var test = coll.WithUpdates().OrderByDescending(item => item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual("B", e.NewItems[0]);
                update = true;
            };

            test.AssertSequence("D", "C", "A");
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            coll.Add("B");

            Assert.IsTrue(update);
            test.AssertSequence("D", "C", "B", "A");
            Assert.AreEqual(4, test.Sequences.Count());
        }

        [TestMethod]
        public void OrderByDescending_NoObservableSourceItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<string>() { "C", "A", "D" };

            var test = coll.WithUpdates().OrderByDescending(item => item);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence("D", "C", "A");
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            coll.Remove("C");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void OrderByDescending_ObservableSourceItemRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<string>() { "C", "A", "D" };

            var test = coll.WithUpdates().OrderByDescending(item => item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual("C", e.OldItems[0]);
                update = true;
            };

            test.AssertSequence("D", "C", "A");
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            coll.Remove("C");

            Assert.IsTrue(update);
            test.AssertSequence("D", "A");
        }

        [TestMethod]
        public void OrderByDescending_NoObservableItemChanges_NoUpdate()
        {
            var update = false;
            var dummy1 = new Dummy<int>(1);
            var dummy2 = new Dummy<int>(3);
            var dummy3 = new Dummy<int>(5);
            var coll = new List<Dummy<int>>() { dummy1, dummy2, dummy3 };

            var test = coll.WithUpdates().OrderByDescending(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence(dummy3, dummy2, dummy1);
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            dummy1.Item = 4;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void OrderByDescending_ObservableItemChanges_Update()
        {
            var update = false;
            var dummy1 = new ObservableDummy<int>(1);
            var dummy2 = new Dummy<int>(3);
            var dummy3 = new Dummy<int>(5);
            var coll = new List<Dummy<int>>() { dummy1, dummy2, dummy3 };

            var test = coll.WithUpdates().OrderByDescending(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreSame(dummy1, e.NewItems[0]);
                Assert.AreSame(dummy1, e.OldItems[0]);
                update = true;
            };

            test.AssertSequence(dummy3, dummy2, dummy1);
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            dummy1.Item = 4;

            Assert.IsTrue(update);
            test.AssertSequence(dummy3, dummy1, dummy2);
        }
    }
}
