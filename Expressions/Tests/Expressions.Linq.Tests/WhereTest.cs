﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Test;
using Sys = System.Linq.Enumerable;
using System.Collections.Specialized;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class WhereTest
    {

        [TestMethod]
        public void Where_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<bool>> coll = new List<Dummy<bool>>();
            var dummy = new Dummy<bool>() { Item = true };

            var test = coll.WithUpdates().Where(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsFalse(Sys.Contains(test, dummy));
            Assert.IsFalse(update);

            coll.Add(dummy);

            Assert.IsFalse(Sys.Contains(test, dummy));
            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Where_ObservableSourceItemAdded_Updates()
        {
            var update = false;
            ICollection<Dummy<bool>> coll = new ObservableCollection<Dummy<bool>>();
            var dummy = new Dummy<bool>() { Item = true };

            var test = coll.WithUpdates().Where(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(dummy, e.NewItems[0]);
            };

            Assert.IsFalse(Sys.Any(test));
            Assert.IsFalse(update);

            coll.Add(dummy);

            Assert.IsTrue(update);
            Assert.IsTrue(Sys.Contains(test, dummy));
        }

        [TestMethod]
        public void Where_NoObservableSourceSuppressedAdded_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<bool>> coll = new List<Dummy<bool>>();
            var dummy = new Dummy<bool>() { Item = false };

            var test = coll.WithUpdates().Where(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsFalse(Sys.Contains(test, dummy));
            Assert.IsFalse(update);

            coll.Add(dummy);

            Assert.IsFalse(update);
            Assert.IsFalse(Sys.Contains(test, dummy));
        }

        [TestMethod]
        public void Where_ObservableSourceSuppressedAdded_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<bool>> coll = new ObservableCollection<Dummy<bool>>();
            var dummy = new Dummy<bool>() { Item = false };

            var test = coll.WithUpdates().Where(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsFalse(Sys.Contains(test, dummy));
            Assert.IsFalse(update);

            coll.Add(dummy);

            Assert.IsFalse(update);
            Assert.IsFalse(test.Any());
        }

        [TestMethod]
        public void Where_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<bool>> coll = new List<Dummy<bool>>();
            var dummy = new Dummy<bool>() { Item = true };
            coll.Add(dummy);

            var test = coll.WithUpdates().Where(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsTrue(Sys.Contains(test, dummy));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(Sys.Contains(test, dummy));
            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Where_ObservableSourceItemRemoved_Updates()
        {
            var update = false;
            ICollection<Dummy<bool>> coll = new ObservableCollection<Dummy<bool>>();
            var dummy = new Dummy<bool>() { Item = true };
            coll.Add(dummy);

            var test = coll.WithUpdates().Where(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(dummy, e.OldItems[0]);
            };

            Assert.IsTrue(Sys.Contains(test, dummy));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(test.Any());
            Assert.IsTrue(update);
        }

        [TestMethod]
        public void Where_NoObservableSourceSuppressedRemoved_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<bool>> coll = new List<Dummy<bool>>();
            var dummy = new Dummy<bool>() { Item = false };
            coll.Add(dummy);

            var test = coll.WithUpdates().Where(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsFalse(Sys.Contains(test, dummy));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
            Assert.IsFalse(Sys.Contains(test, dummy));
        }

        [TestMethod]
        public void Where_ObservableSourceSuppressedRemoved_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<bool>> coll = new ObservableCollection<Dummy<bool>>();
            var dummy = new Dummy<bool>() { Item = false };
            coll.Add(dummy);

            var test = coll.WithUpdates().Where(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsFalse(Sys.Contains(test, dummy));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(update);
            Assert.IsFalse(test.Any());
        }

        [TestMethod]
        public void Where_NoObservableItem_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<bool>> coll = new List<Dummy<bool>>();
            var dummy = new Dummy<bool>() { Item = true };
            coll.Add(dummy);

            var test = coll.WithUpdates().Where(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsTrue(Sys.Contains(test, dummy));
            Assert.IsFalse(update);

            dummy.Item = false;

            Assert.IsTrue(Sys.Contains(test, dummy));
            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Where_ObservableItem_Updates()
        {
            var update = false;
            ICollection<Dummy<bool>> coll = new List<Dummy<bool>>();
            var dummy = new ObservableDummy<bool>() { Item = true };
            coll.Add(dummy);

            var test = coll.WithUpdates().Where(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(dummy, e.OldItems[0]);
            };

            Assert.IsTrue(Sys.Contains(test, dummy));
            Assert.IsFalse(update);

            dummy.Item = false;

            Assert.IsFalse(test.Any());
            Assert.IsTrue(update);
        }

        [TestMethod]
        public void WhereTransaction_ObservableItem_Updates()
        {
            var oldEngine = ExecutionEngine.Current;
            try
            {
                ExecutionEngine.Current = new SequentialExecutionEngine();

                var update = false;
                var dummy1 = new ObservableDummy<bool>(true);
                INotifyCollection<Dummy<bool>> coll = new NotifyCollection<Dummy<bool>>() { dummy1 };

                var test = coll.Where(d => d.Item);

                test.CollectionChanged += (o, e) =>
                {
                    update = true;
                    Assert.AreEqual(dummy1, e.OldItems[0]);
                };

                Assert.IsTrue(Sys.Contains(test, dummy1));
                Assert.IsFalse(update);

                ExecutionEngine.Current.BeginTransaction();

                dummy1.Item = false;
                Assert.IsFalse(update);

                ExecutionEngine.Current.CommitTransaction();

                Assert.IsFalse(test.Any());
                Assert.IsTrue(update);
            }
            finally
            {
                ExecutionEngine.Current = oldEngine;
            }
        }
    }
}
