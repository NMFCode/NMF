using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Test;
using Sys = System.Linq.Enumerable;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class GroupByTest
    {
        [TestMethod]
        public void GroupBy_NoObservableSourceItemAdded_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<string>> coll = new List<Dummy<string>>();

            var test = coll.WithUpdates().GroupBy(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsTrue(!test.Any());
            Assert.IsFalse(update);

            coll.Add(new Dummy<string>() { Item = "42" });

            Assert.IsTrue(!test.Any());
            Assert.IsFalse(update);
        }

        [TestMethod]
        public void GroupBy_ObservableSourceItemAdded_Update()
        {
            var update = false;
            ICollection<Dummy<string>> coll = new ObservableCollection<Dummy<string>>();

            var test = coll.WithUpdates().GroupBy(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.IsTrue(ContainsGroup(e.NewItems, "42"));
                Assert.AreEqual(1, e.NewItems.Count);
                update = true;
            };

            Assert.IsTrue(!test.Any());
            Assert.IsFalse(update);

            coll.Add(new Dummy<string>() { Item = "42" });

            Assert.IsFalse(!test.Any());
            Assert.IsTrue(Sys.Any(test, group => group.Key == "42"));
            Assert.IsTrue(update);
        }

        [TestMethod]
        public void GroupBy_NoObservableSourceItemRemoved_NoUpdates()
        {
            var update = false;
            ICollection<Dummy<string>> coll = new List<Dummy<string>>();
            var dummy = new Dummy<string>("42");
            coll.Add(dummy);

            var test = coll.WithUpdates().GroupBy(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsTrue(Sys.Any(test, group => group.Key == "42"));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsTrue(test.Any());
            Assert.IsFalse(update);
        }

        [TestMethod]
        public void GroupBy_ObservableSourceItemRemoved_Update()
        {
            var update = false;
            ICollection<Dummy<string>> coll = new ObservableCollection<Dummy<string>>();
            var dummy = new Dummy<string>("42");
            coll.Add(dummy);

            var test = coll.WithUpdates().GroupBy(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.IsTrue(ContainsGroup(e.OldItems, "42"));
                Assert.AreEqual(1, e.OldItems.Count);
                update = true;
            };

            Assert.IsTrue(Sys.Any(test, group => group.Key == "42"));
            Assert.IsFalse(update);

            coll.Remove(dummy);

            Assert.IsFalse(test.Any());
            Assert.IsTrue(update);
        }

        [TestMethod]
        public void GroupBy_NoObservableKeyChangesToNewGroup_NoUpdate()
        {
            var update = false;

            ICollection<Dummy<string>> coll = new ObservableCollection<Dummy<string>>();
            var dummy1 = new Dummy<string>("A");
            var dummy2 = new Dummy<string>("A");
            coll.Add(dummy1);
            coll.Add(dummy2);

            var test = coll.WithUpdates().GroupBy(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            var group = Sys.Single(test);
            Assert.IsTrue(Sys.Contains(group, dummy1));
            Assert.IsTrue(Sys.Contains(group, dummy2));
            Assert.IsFalse(update);

            dummy2.Item = "B";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void GroupBy_ObservableKeyChangesToNewGroup_Update()
        {
            var update = false;

            ICollection<Dummy<string>> coll = new ObservableCollection<Dummy<string>>();
            var dummy1 = new Dummy<string>("A");
            var dummy2 = new ObservableDummy<string>("A");
            coll.Add(dummy1);
            coll.Add(dummy2);

            var test = coll.WithUpdates().GroupBy(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.IsTrue(ContainsGroup(e.NewItems, "B"));
                update = true;
            };

            var group = Sys.Single(test);
            Assert.IsTrue(Sys.Contains(group, dummy1));
            Assert.IsTrue(Sys.Contains(group, dummy2));
            Assert.IsFalse(update);

            dummy2.Item = "B";

            Assert.IsTrue(update);
            Assert.AreEqual(2, test.Count());
        }

        [TestMethod]
        public void GroupBy_NoObservableKeyChangesEraseGroup_NoUpdate()
        {
            var update = false;

            ICollection<Dummy<string>> coll = new ObservableCollection<Dummy<string>>();
            var dummy1 = new Dummy<string>("A");
            var dummy2 = new Dummy<string>("B");
            coll.Add(dummy1);
            coll.Add(dummy2);

            var test = coll.WithUpdates().GroupBy(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            Assert.IsTrue(Sys.Contains(Sys.First(test, group => group.Key == "A"), dummy1));
            Assert.IsTrue(Sys.Contains(Sys.First(test, group => group.Key == "B"), dummy2));
            Assert.IsFalse(update);

            dummy2.Item = "A";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void GroupBy_ObservableKeyChangesEraseGroup_Update()
        {
            var update = false;

            ICollection<Dummy<string>> coll = new ObservableCollection<Dummy<string>>();
            var dummy1 = new Dummy<string>("A");
            var dummy2 = new ObservableDummy<string>("B");
            coll.Add(dummy1);
            coll.Add(dummy2);

            var test = coll.WithUpdates().GroupBy(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                Assert.IsTrue(ContainsGroup(e.OldItems, "B"));
                update = true;
            };

            Assert.IsTrue(Sys.Contains(Sys.Single(test, group => group.Key == "A"), dummy1));
            Assert.IsTrue(Sys.Contains(Sys.Single(test, group => group.Key == "B"), dummy2));
            Assert.IsFalse(update);

            dummy2.Item = "A";

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(Sys.Contains(Sys.Single(test, group => group.Key == "A"), dummy2));
        }

        [TestMethod]
        public void GroupBy_NoObservableKeyChangesBetweenGroups_NoUpdate()
        {
            var update = false;

            ICollection<Dummy<string>> coll = new ObservableCollection<Dummy<string>>();
            var dummy1 = new Dummy<string>("A");
            var dummy2 = new Dummy<string>("B");
            var dummyChange = new Dummy<string>("A");
            coll.Add(dummy1);
            coll.Add(dummy2);
            coll.Add(dummyChange);

            var test = coll.WithUpdates().GroupBy(d => d.Item);

            test.CollectionChanged += (o, e) => update = true;

            var groupA = Sys.Single(test, g => g.Key == "A");
            var groupB = Sys.Single(test, g => g.Key == "B");
            Assert.IsNotNull(groupA);
            Assert.IsNotNull(groupB);
            Assert.IsTrue(Sys.Contains(groupA, dummy1));
            Assert.IsTrue(Sys.Contains(groupA, dummyChange));
            Assert.IsTrue(Sys.Contains(groupB, dummy2));
            Assert.IsFalse(update);

            var notifierA = groupA as INotifyCollectionChanged;
            var notifierB = groupB as INotifyCollectionChanged;

            Assert.IsNotNull(notifierA);
            Assert.IsNotNull(notifierB);

            notifierA.CollectionChanged += (o, e) => update = true;
            notifierB.CollectionChanged += (o, e) => update = true;

            dummyChange.Item = "B";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void GroupBy_ObservableKeyChangesBetweenGroups_Update()
        {
            var updateGroups = false;
            var updateGroupA = false;
            var updateGroupB = false;

            ICollection<Dummy<string>> coll = new ObservableCollection<Dummy<string>>();
            var dummy1 = new Dummy<string>("A");
            var dummy2 = new Dummy<string>("B");
            var dummyChange = new ObservableDummy<string>("A");
            coll.Add(dummy1);
            coll.Add(dummy2);
            coll.Add(dummyChange);

            var test = coll.WithUpdates().GroupBy(d => d.Item);

            test.CollectionChanged += (o, e) => updateGroups = true;

            var groupA = Sys.Single(test, g => g.Key == "A") as ObservableGroup<string, Dummy<string>>;
            var groupB = Sys.Single(test, g => g.Key == "B") as ObservableGroup<string, Dummy<string>>;
            Assert.IsNotNull(groupA);
            Assert.IsNotNull(groupB);
            Assert.IsTrue(Sys.Contains(groupA, dummy1));
            Assert.IsTrue(Sys.Contains(groupA, dummyChange));
            Assert.IsTrue(Sys.Contains(groupB, dummy2));
            Assert.IsFalse(updateGroups);

            var notifierA = groupA as INotifyCollectionChanged;
            var notifierB = groupB as INotifyCollectionChanged;

            Assert.IsNotNull(notifierA);
            Assert.IsNotNull(notifierB);

            notifierA.CollectionChanged += (o, e) =>
            {
                Assert.IsTrue(e.OldItems.Contains(dummyChange));
                updateGroupA = true;
            };
            notifierB.CollectionChanged += (o, e) =>
            {
                Assert.IsTrue(e.NewItems.Contains(dummyChange));
                updateGroupB = true;
            };
            dummyChange.Item = "B";

            Assert.IsFalse(updateGroups);
            Assert.IsTrue(updateGroupA);
            Assert.IsTrue(updateGroupB);

            Assert.IsTrue(Sys.Contains(groupA, dummy1));
            Assert.IsTrue(Sys.Contains(groupB, dummy2));
            Assert.IsTrue(Sys.Contains(groupB, dummyChange));
        }

        [TestMethod]
        public void GroupBy_NoObservableSourceReset_NoUpdate()
        {
            var update = false;

            var coll = new List<int>() { 1, 2, 3, 4, 5, 6 };

            var test = coll.WithUpdates().GroupBy(i => i % 3);

            test.CollectionChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
            Assert.AreEqual(3, test.Count());
        }

        [TestMethod]
        public void GroupBy_ObservableSourceReset_Update()
        {
            var update = false;

            var coll = new NotifyCollection<int>() { 1, 2, 3, 4, 5, 6 };

            var test = coll.GroupBy(i => i % 3);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
                update = true;
            };

            coll.Clear();

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Count());
        }

        private static bool ContainsGroup(IEnumerable list, string name)
        {
            foreach (System.Linq.IGrouping<string, Dummy<string>> group in list)
            {
                if (group.Key == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
