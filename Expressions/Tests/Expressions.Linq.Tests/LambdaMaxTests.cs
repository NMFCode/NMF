using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using NMF.Expressions.Test;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class LambdaMaxTests
    {
        [TestMethod]
        public void LambdaMax_NoObservableSourceNewMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<Dummy<int>>() { new Dummy<int>(-1), new Dummy<int>(-2), new Dummy<int>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);
            Assert.AreEqual(-1, coll.WithUpdates().Max(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int>(0));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaMax_ObservableSourceNewMaxAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(-1), new Dummy<int>(-2), new Dummy<int>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-1, e.OldValue);
                Assert.AreEqual(0, e.NewValue);
            };

            Assert.AreEqual(-1, test.Value);
            Assert.IsFalse(update);

            coll.Add(new Dummy<int>(0));

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Value);
        }

        [TestMethod]
        public void LambdaMax_NoMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), new Dummy<int>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);

            coll.Add(new Dummy<int>(-4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaMax_ExistingMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), new Dummy<int>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);

            coll.Add(new Dummy<int>(3));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaMaxComparer_NoObservableSourceNewMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<Dummy<int>>() { new Dummy<int>(-1), new Dummy<int>(2), new Dummy<int>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);
            Assert.AreEqual(-3, coll.WithUpdates().Max(d => d.Item, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaMaxComparer_ObservableSourceNewMaxAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(-1), new Dummy<int>(2), new Dummy<int>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-3, e.OldValue);
                Assert.AreEqual(4, e.NewValue);
            };

            Assert.AreEqual(-3, test.Value);
            Assert.IsFalse(update);

            coll.Add(new Dummy<int>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Value);
        }

        [TestMethod]
        public void LambdaMaxComparer_NoMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(-1), new Dummy<int>(2), new Dummy<int>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);

            coll.Add(new Dummy<int>(-2));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaMaxComparer_ExistingMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(-1), new Dummy<int>(2), new Dummy<int>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);

            coll.Add(new Dummy<int>(3));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableMax_NoObservableSourceNewMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);
            Assert.AreEqual(3, coll.WithUpdates().Max(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableMax_ObservableSourceNewMaxAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(3, e.OldValue);
                Assert.AreEqual(4, e.NewValue);
            };

            Assert.AreEqual(3, test.Value);
            Assert.IsFalse(update);

            coll.Add(new Dummy<int?>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Value);
        }

        [TestMethod]
        public void LambdaNullableMax_NoMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);

            coll.Add(new Dummy<int?>(0));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableMax_ExistingMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(3, test.Value);

            coll.Add(new Dummy<int?>(3));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableMaxComparer_NoObservableSourceNewMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<Dummy<int?>>() { new Dummy<int?>(-1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);
            Assert.AreEqual(-3, coll.WithUpdates().Max(d => d.Item, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableMaxComparer_ObservableSourceNewMaxAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(-1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-3, e.OldValue);
                Assert.AreEqual(4, e.NewValue);
            };

            Assert.AreEqual(-3, test.Value);
            Assert.IsFalse(update);

            coll.Add(new Dummy<int?>(4));

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Value);
        }

        [TestMethod]
        public void LambdaNullableMaxComparer_NoMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(-1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);

            coll.Add(new Dummy<int?>(3));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableMaxComparer_ExistingMaxAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(-1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Max(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-3, test.Value);

            coll.Add(new Dummy<int?>(-1));

            Assert.IsFalse(update);
        }
    }
}
