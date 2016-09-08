using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Test;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class AllTests
    {
        [TestMethod]
        public void All_NoObservableSourceItemAdded_NoUpdate()
        {
            var update = false;

            var coll = new List<int>() { 1, 2, 3 }.WithUpdates();

            var test = Observable.Expression(() => coll.All(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Add(-1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void All_ObservableSourceItemAdded_Update()
        {
            var update = false;

            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().All(i => i > 0));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
                update = true;
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Add(-1);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void All_NoObservableSourceItemRemoved_NoUpdate()
        {
            var update = false;

            var coll = new List<int>() { 1, 2, 3, -1 };

            var test = Observable.Expression(() => coll.WithUpdates().All(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            coll.Remove(-1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void All_ObservableSourceItemRemoved_Update()
        {
            var update = false;

            var coll = new ObservableCollection<int>() { 1, 2, 3, -1 };

            var test = Observable.Expression(() => coll.WithUpdates().All(i => i > 0));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
                update = true;
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            coll.Remove(-1);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void All_NoObservableItem_NoUpdate()
        {
            var update = false;

            var dummy = new Dummy<bool>(true);
            var coll = new List<Dummy<bool>>() { dummy };

            var test = Observable.Expression(() => coll.WithUpdates().All(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            dummy.Item = false;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void All_ObservableItem_Update()
        {
            var update = false;

            var dummy = new ObservableDummy<bool>(true);
            var coll = new List<Dummy<bool>>() { dummy }.WithUpdates();

            var test = Observable.Expression(() => coll.All(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
                update = true;
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            dummy.Item = false;

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void All_NoObservableSourceReset_NoUpdate()
        {
            var update = false;

            var coll = new List<int>() { 1, 2, 3, -1 };

            var test = Observable.Expression(() => coll.WithUpdates().All(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void All_ObservableSourceReset_Update()
        {
            var update = false;

            var coll = new ObservableCollection<int>() { 1, 2, 3, -1 };

            var test = Observable.Expression(() => coll.WithUpdates().All(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }
    }
}
