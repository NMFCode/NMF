using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class OfTypeTests
    {
        [TestMethod]
        public void OfTypeTests_NoObservableSourceItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<object>() { 23, "42", null };

            var test = coll.WithUpdates().OfType<string>();

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence("42");
            Assert.IsFalse(update);

            coll.Add("Foo");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void OfTypeTests_ObservableSourceItemAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<object>() { 23, "42", null };

            var test = coll.WithUpdates().OfType<string>();

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual("Foo", e.NewItems[0]);
                Assert.IsNull(e.OldItems);
            };

            test.AssertSequence("42");
            Assert.IsFalse(update);

            coll.Add("Foo");

            Assert.IsTrue(update);
            test.AssertSequence("42", "Foo");
        }

        [TestMethod]
        public void OfTypeTests_ItemOfOtherTypeAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<object>() { 23, "42", null };

            var test = coll.WithUpdates().OfType<string>();

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence("42");
            Assert.IsFalse(update);

            coll.Add(2.0);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void OfTypeTests_NoObservableSourceItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<object>() { 23, "42", null };

            var test = coll.WithUpdates().OfType<string>();

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence("42");
            Assert.IsFalse(update);

            coll.Remove("42");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void OfTypeTests_ObservableSourceItemRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<object>() { 23, "42", null };

            var test = coll.WithUpdates().OfType<string>();

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual("42", e.OldItems[0]);
                Assert.IsNull(e.NewItems);
            };

            test.AssertSequence("42");
            Assert.IsFalse(update);

            coll.Remove("42");

            Assert.IsTrue(update);
            Assert.IsFalse(test.Any());
        }

        [TestMethod]
        public void OfTypeTests_ItemOfOtherTypeRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<object>() { 23, "42", null };

            var test = coll.WithUpdates().OfType<string>();

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence("42");
            Assert.IsFalse(update);

            coll.Remove(23);

            Assert.IsFalse(update);
        }
    }
}
