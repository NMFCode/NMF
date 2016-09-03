using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Test;
using Sys = System.Linq.Enumerable;
using System.Collections.Specialized;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class SelectManyTest
    {
        [TestMethod]
        public void SelectMany_NoObservableSourceSubSourceAdded_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<ICollection<Dummy<string>>>> coll = new List<Dummy<ICollection<Dummy<string>>>>();
            var dummy = new Dummy<string>() { Item = "23" };
            var dummy2 = new Dummy<ICollection<Dummy<string>>>()
            {
                Item = new List<Dummy<string>>() { dummy }
            };

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsFalse(Sys.Contains(test, "23"));
            Assert.IsFalse(update);

            coll.Add(dummy2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SelectMany_ObservableSourceSubSourceAdded_Updates()
        {
            var update = 0;
            ICollection<Dummy<ICollection<Dummy<string>>>> coll = new ObservableCollection<Dummy<ICollection<Dummy<string>>>>();
            var dummy = new Dummy<string>() { Item = "23" };
            var dummy2 = new Dummy<ICollection<Dummy<string>>>()
            {
                Item = new List<Dummy<string>>() { dummy }
            };

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) =>
            {
                update++;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual("23", e.NewItems[0]);
            };

            Assert.IsFalse(Sys.Contains(test, "23"));
            Assert.AreEqual(0, update);

            coll.Add(dummy2);

            Assert.AreEqual(1, update);
            Assert.IsTrue(Sys.Contains(test, "23"));
        }
        
        [TestMethod]
        public void SelectMany_NoObservableSourceSubSourceRemoved_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<ICollection<Dummy<string>>>> coll = new List<Dummy<ICollection<Dummy<string>>>>();
            var dummy = new Dummy<string>() { Item = "23" };
            var dummy2 = new Dummy<ICollection<Dummy<string>>>()
            {
                Item = new List<Dummy<string>>() { dummy }
            };
            coll.Add(dummy2);

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsTrue(Sys.Contains(test, "23"));
            Assert.IsFalse(update);

            coll.Remove(dummy2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SelectMany_ObservableSourceSubSourceRemoved_Updates()
        {
            var update = false;
            ICollection<Dummy<ICollection<Dummy<string>>>> coll = new ObservableCollection<Dummy<ICollection<Dummy<string>>>>();
            var dummy = new Dummy<string>() { Item = "23" };
            var dummy2 = new Dummy<ICollection<Dummy<string>>>()
            {
                Item = new List<Dummy<string>>() { dummy }
            };
            coll.Add(dummy2);

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual("23", e.OldItems[0]);
            };

            Assert.IsTrue(Sys.Contains(test, "23"));
            Assert.IsFalse(update);

            coll.Remove(dummy2);

            Assert.IsTrue(update);
            Assert.IsTrue(!test.Any());
        }

        [TestMethod]
        public void SelectMany_NoObservableSourceItemSubSourceChanges_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<ICollection<Dummy<string>>>> coll = new List<Dummy<ICollection<Dummy<string>>>>();
            var dummy = new Dummy<string>() { Item = "23" };
            var dummy2 = new Dummy<ICollection<Dummy<string>>>()
            {
                Item = new List<Dummy<string>>() { dummy }
            };
            coll.Add(dummy2);

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsTrue(Sys.Contains(test, "23"));
            Assert.IsFalse(update);

            dummy2.Item = new List<Dummy<string>>();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SelectMany_ObservableSourceItemSubSourceChanges_Updates()
        {
            var update = false;
            ICollection<Dummy<ICollection<Dummy<string>>>> coll = new List<Dummy<ICollection<Dummy<string>>>>();
            var dummy = new Dummy<string>() { Item = "23" };
            var dummy2 = new ObservableDummy<ICollection<Dummy<string>>>()
            {
                Item = new List<Dummy<string>>() { dummy }
            };
            coll.Add(dummy2);

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                switch (e.Action)
                {
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                        Assert.AreEqual(0, e.NewItems.Count);
                        break;
                    case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                        Assert.AreEqual("23", e.OldItems[0]);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            };

            Assert.IsTrue(Sys.Contains(test, "23"));
            Assert.IsFalse(update);

            dummy2.Item = new List<Dummy<string>>();

            Assert.IsTrue(update);
            Assert.IsTrue(!test.Any());
        }
        
        [TestMethod]
        public void SelectMany_NoObservableSubSourceItemChanges_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<ICollection<Dummy<string>>>> coll = new List<Dummy<ICollection<Dummy<string>>>>();
            var dummy = new Dummy<string>() { Item = "23" };
            var dummy2 = new Dummy<ICollection<Dummy<string>>>()
            {
                Item = new List<Dummy<string>>() { dummy }
            };
            coll.Add(dummy2);

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsTrue(Sys.Contains(test, "23"));
            Assert.IsFalse(update);

            dummy.Item = "42";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SelectMany_ObservableSubSourceItemChanges_Updates()
        {
            var update = false;
            ICollection<Dummy<ICollection<Dummy<string>>>> coll = new List<Dummy<ICollection<Dummy<string>>>>();
            var dummy = new ObservableDummy<string>() { Item = "23" };
            var dummy2 = new Dummy<ICollection<Dummy<string>>>()
            {
                Item = new List<Dummy<string>>() { dummy }
            };
            coll.Add(dummy2);

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("23", e.OldItems[0]);
                Assert.AreEqual("42", e.NewItems[0]);
            };

            Assert.IsTrue(Sys.Contains(test, "23"));
            Assert.IsFalse(update);

            dummy.Item = "42";

            Assert.IsTrue(update);
            Assert.IsTrue(Sys.Contains(test, "42"));
        }
        
        [TestMethod]
        public void SelectMany_NoObservableSubSourceItemRemoved_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<ICollection<Dummy<string>>>> coll = new List<Dummy<ICollection<Dummy<string>>>>();
            var dummy = new Dummy<string>() { Item = "23" };
            var dummy2 = new Dummy<ICollection<Dummy<string>>>()
            {
                Item = new List<Dummy<string>>() { dummy }
            };
            coll.Add(dummy2);

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsTrue(Sys.Contains(test, "23"));
            Assert.IsFalse(update);

            dummy2.Item.Remove(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SelectMany_ObservableSubSourceItemRemoved_Updates()
        {
            var update = false;
            ICollection<Dummy<ICollection<Dummy<string>>>> coll = new List<Dummy<ICollection<Dummy<string>>>>();
            var dummy = new Dummy<string>() { Item = "23" };
            var dummy2 = new Dummy<ICollection<Dummy<string>>>()
            {
                Item = new ObservableCollection<Dummy<string>>() { dummy }.WithUpdates()
            };
            coll.Add(dummy2);

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("23", e.OldItems[0]);
            };

            Assert.IsTrue(Sys.Contains(test, "23"));
            Assert.IsFalse(update);

            dummy2.Item.Remove(dummy);

            Assert.IsTrue(update);
            Assert.IsTrue(!test.Any());
        }

        [TestMethod]
        public void SelectMany_NoObservableSubSourceItemAdded_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<ICollection<Dummy<string>>>> coll = new List<Dummy<ICollection<Dummy<string>>>>();
            var dummy = new Dummy<string>() { Item = "23" };
            var dummy2 = new Dummy<ICollection<Dummy<string>>>()
            {
                Item = new List<Dummy<string>>()
            };
            coll.Add(dummy2);

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsFalse(Sys.Contains(test, "23"));
            Assert.IsFalse(update);

            dummy2.Item.Add(dummy);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SelectMany_ObservableSubSourceItemAdded_Updates()
        {
            var update = 0;
            ICollection<Dummy<ICollection<Dummy<string>>>> coll = new List<Dummy<ICollection<Dummy<string>>>>();
            var dummy = new Dummy<string>() { Item = "23" };
            var dummy2 = new Dummy<ICollection<Dummy<string>>>()
            {
                Item = new ObservableCollection<Dummy<string>>().WithUpdates()
            };
            coll.Add(dummy2);

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) =>
            {
                update++;
                Assert.AreEqual("23", e.NewItems[0]);
            };

            Assert.IsFalse(Sys.Contains(test, "23"));
            Assert.AreEqual(0, update);

            dummy2.Item.Add(dummy);

            Assert.AreEqual(1, update);
            Assert.IsTrue(Sys.Contains(test, "23"));
        }
        
        [TestMethod]
        public void SelectMany_NoObservableSourceReset_NoUpdate()
        {
            var update = false;

            var coll = new List<Dummy<ICollection<Dummy<string>>>>()
            {
                new Dummy<ICollection<Dummy<string>>>(
                    new List<Dummy<string>>() { new Dummy<string>("42") })
            };

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SelectMany_ObservableSourceReset_Update()
        {
            var update = false;

            var coll = new ObservableCollection<Dummy<ICollection<Dummy<string>>>>()
            {
                new Dummy<ICollection<Dummy<string>>>(
                    new List<Dummy<string>>() { new Dummy<string>("42") })
            };

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

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
        public void SelectMany_NoObservableSubSourceReset_NoUpdate()
        {
            var update = false;

            var coll = new List<Dummy<ICollection<Dummy<string>>>>()
            {
                new Dummy<ICollection<Dummy<string>>>(
                    new List<Dummy<string>>() { new Dummy<string>("42") })
            };

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) => update = true;

            coll[0].Item.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SelectMany_ObservableSubSourceReset_Update()
        {
            var update = false;

            var coll = new List<Dummy<ICollection<Dummy<string>>>>()
            {
                new Dummy<ICollection<Dummy<string>>>(
                    new ObservableCollection<Dummy<string>>() { new Dummy<string>("42") }.WithUpdates())
            };

            var test = coll.WithUpdates().SelectMany(d => d.Item, (d1, d2) => d2.Item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual("42", e.OldItems[0]);
                update = true;
            };

            coll[0].Item.Clear();

            Assert.IsTrue(update);
            Assert.IsFalse(test.Any());
        }
    }
}
