using System;
using System.Collections;
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
        private ExecutionEngine executionEngine;

        [TestInitialize]
        public void StoreExecutionEngine()
        {
            executionEngine = ExecutionEngine.Current;
        }

        [TestCleanup]
        public void RestoreExecutionEngine()
        {
            if (ExecutionEngine.Current.TransactionActive)
            {
                ExecutionEngine.Current.RollbackTransaction();
            }
            ExecutionEngine.Current = executionEngine;
        }

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

        [TestMethod]
        public void OrderByTransaction_ChangeRemove_Update()
        {
            ExecutionEngine.Current = new SequentialExecutionEngine();

            var update = false;
            var dummy1 = new ObservableDummy<int>(1);
            var dummy2 = new Dummy<int>(3);
            var dummy3 = new Dummy<int>(5);
            INotifyCollection<Dummy<int>> coll = new NotifyCollection<Dummy<int>>() { dummy1, dummy2, dummy3 };

            var test = coll.OrderBy(d => d.Item);
            test.CollectionChanged += (o, e) => update = true;
            
            test.AssertSequence(dummy1, dummy2, dummy3);
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            ExecutionEngine.Current.BeginTransaction();

            dummy1.Item = 0;
            coll.Remove(dummy1);
            Assert.IsFalse(update);

            ExecutionEngine.Current.CommitTransaction();

            test.AssertSequence(dummy2, dummy3);
            Assert.AreEqual(2, test.Sequences.Count());
            Assert.IsTrue(update);
        }

        [TestMethod]
        public void OrderByTransaction_ChangeReplace_Update()
        {
            ExecutionEngine.Current = new SequentialExecutionEngine();

            var update = false;
            var dummy1 = new ObservableDummy<int>(1);
            var dummy2 = new Dummy<int>(3);
            var dummy3 = new Dummy<int>(5);
            var coll = new NotifyCollection<Dummy<int>>() { dummy1, dummy2, dummy3 };
            var collCasted = (INotifyCollection<Dummy<int>>)coll;
            var newDummy = new Dummy<int>(2);

            var test = collCasted.OrderBy(d => d.Item);
            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence(dummy1, dummy2, dummy3);
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            ExecutionEngine.Current.BeginTransaction();

            dummy1.Item = 0;
            coll[0] = newDummy;
            Assert.IsFalse(update);

            ExecutionEngine.Current.CommitTransaction();

            test.AssertSequence(newDummy, dummy2, dummy3);
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsTrue(update);
        }

        [TestMethod]
        public void WhereOrderByTransaction_ChangeRemove_Update() // Works without Transaction
        {
            ExecutionEngine.Current = new SequentialExecutionEngine();

            var update = false;
            var dummy1 = new ObservableDummy<int>(1);
            var dummy2 = new Dummy<int>(3);
            var dummy3 = new Dummy<int>(5);
            INotifyCollection<Dummy<int>> coll = new NotifyCollection<Dummy<int>>() { dummy1, dummy2, dummy3 };

            var test = coll.Where(d => d.Item > 0).OrderBy(d => d.Item);
            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence(dummy1, dummy2, dummy3);
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            ExecutionEngine.Current.BeginTransaction();

            dummy1.Item = 0;
            Assert.IsFalse(update);

            ExecutionEngine.Current.CommitTransaction();

            test.AssertSequence(dummy2, dummy3);
            Assert.AreEqual(2, test.Sequences.Count());
            Assert.IsTrue(update);
        }

        [TestMethod]
        public void WhereSelectOrderByTransaction_RemoveCausingChange_Update() // Works without Transaction
        {
            ExecutionEngine.Current = new SequentialExecutionEngine();

            var update = false;
            var dummy1 = new ObservableDummy<int>(1);
            var dummy2 = new Dummy<int>(3);
            var dummy3 = new Dummy<int>(5);
            INotifyCollection<Dummy<int>> coll = new NotifyCollection<Dummy<int>>() { dummy1, dummy2, dummy3 };

            var test = coll.Where(d => d.Item > 0).Select(d => new Dummy<int>(d.Item * 2)).OrderBy(d => d.Item);
            test.CollectionChanged += (o, e) => update = true;

            ((IEnumerable<Dummy<int>>)test).Select(d => d.Item).AssertSequence(dummy1.Item * 2, dummy2.Item * 2, dummy3.Item * 2);
            Assert.AreEqual(3, test.Sequences.Count());
            Assert.IsFalse(update);

            ExecutionEngine.Current.BeginTransaction();

            dummy1.Item = 0;
            Assert.IsFalse(update);

            ExecutionEngine.Current.CommitTransaction();

            // Calling Select directly on test leads to quadrupling of the entries in test
            ((IEnumerable<Dummy<int>>)test).Select(d => d.Item).AssertSequence(dummy2.Item * 2, dummy3.Item * 2);
            Assert.AreEqual(2, test.Sequences.Count());
            Assert.IsTrue(update);
        }
    }
}
