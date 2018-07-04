using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Collections.ObjectModel;
using NMF.Expressions.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Expressions.Linq;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Expressions.Utilities.Tests
{
    [TestClass]
    public class LookupTests
    {
        private ObservableList<Dummy<int>> CreateDummyList()
        {
            return new ObservableList<Dummy<int>>()
            {
                new ObservableDummy<int>(0),
                new ObservableDummy<int>(8),
                new ObservableDummy<int>(15),
                new ObservableDummy<int>(42),
                new ObservableDummy<int>(42),
                new ObservableDummy<int>(42),
                new ObservableDummy<int>(8),
                new ObservableDummy<int>(0),
                new ObservableDummy<int>(0),
                new ObservableDummy<int>(0)
            };
        }

        [TestMethod]
        public void BatchLookupCorrect()
        {
            var source = CreateDummyList();
            var dummy0 = source[0];
            var dummy8 = source[1];
            var dummy15 = source[2];

            var test = source.ToLookup(d => d.Item);

            Assert.AreEqual(4, test[0].Count());
            Assert.IsTrue(test[0].Contains(dummy0));
            Assert.IsTrue(test[8].Contains(dummy8));
            Assert.IsTrue(test[15].Contains(dummy15));
        }

        [TestMethod]
        public void IncrementalLookupAddedValue()
        {
            var source = CreateDummyList();
            var dummy = new ObservableDummy<int>(42);

            var lookup = source.ToLookup(d => d.Item);
            var test = lookup[42].AsNotifiable();
            var changed = false;

            test.CollectionChanged += (o, e) =>
            {
                changed = true;
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreEqual(dummy, e.NewItems[0]);
            };

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(changed);

            source.Add(dummy);

            Assert.IsTrue(changed);
            Assert.AreEqual(4, test.Count());
        }

        [TestMethod]
        public void IncrementalLookupRemovedValue()
        {
            var source = CreateDummyList();
            var dummy = source[3];
            var lookup = source.ToLookup(d => d.Item);
            var test = lookup[42].AsNotifiable();
            var changed = false;

            test.CollectionChanged += (o, e) =>
            {
                changed = true;
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(dummy, e.OldItems[0]);
            };

            Assert.AreEqual(3, test.Count());
            Assert.IsFalse(changed);

            source.Remove(dummy);

            Assert.IsTrue(changed);
            Assert.AreEqual(2, test.Count());
        }

        [TestMethod]
        public void IncrementalLookupChange()
        {
            var source = CreateDummyList();
            var dummy = source[0];

            var lookup = source.ToLookup(d => d.Item);
            var test1 = lookup[0].AsNotifiable();
            var test2 = lookup[42].AsNotifiable();
            var test1Changed = false;
            var test2Changed = false;

            test1.CollectionChanged += (o, e) =>
            {
                test1Changed = true;
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(dummy, e.OldItems[0]);
            };
            test2.CollectionChanged += (o, e) =>
            {
                test2Changed = true;
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreEqual(dummy, e.NewItems[0]);
            };

            Assert.AreEqual(4, test1.Count());
            Assert.AreEqual(3, test2.Count());
            Assert.IsFalse(test1Changed);
            Assert.IsFalse(test2Changed);

            dummy.Item = 42;

            Assert.IsTrue(test1Changed);
            Assert.IsTrue(test2Changed);
            Assert.AreEqual(3, test1.Count());
            Assert.AreEqual(4, test2.Count());
        }

        [TestMethod]
        public void CustomersTest()
        {
            var oc = new ObservableCollection<Customer>();

            var customersByLocationLookup = oc.WithUpdates().ToLookup(o => o.Location);
            var r = new Random(1);

            for (var i = 0; i < 10000; i++)
            {
                var c = new Customer() { Name = "Customer " + i, Location = "Location " + r.Next(50) };
                oc.Add(c);
            }

            Assert.AreEqual(oc.Where(o => o.Location == "Location 30").Count(), customersByLocationLookup["Location 30"].Count());
        }

        public class Customer
        {
            public string Name { get; set; }
            public string Location { get; set; }
        }
    }
}
