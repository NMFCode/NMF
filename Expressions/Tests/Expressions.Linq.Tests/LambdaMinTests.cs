using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using NMF.Expressions.Test;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class LambdaMinTests
    {
        [TestMethod]
        public void LambdaMin_NoObservableSourceNewMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), new Dummy<int>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.AreEqual(1, coll.WithUpdates().Min(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int>(0));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaMin_ObservableSourceNewMinAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), new Dummy<int>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1, e.OldValue);
                Assert.AreEqual(0, e.NewValue);
            };

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            coll.Add(new Dummy<int>(0));

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Value);
        }

        [TestMethod]
        public void LambdaMin_NoMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), new Dummy<int>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);

            coll.Add(new Dummy<int>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaMin_ExistingMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(1), new Dummy<int>(2), new Dummy<int>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);

            coll.Add(new Dummy<int>(1));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaMinComparer_NoObservableSourceNewMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<Dummy<int>>() { new Dummy<int>(-1), new Dummy<int>(2), new Dummy<int>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);
            Assert.AreEqual(-1, coll.WithUpdates().Min(d => d.Item, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int>(0));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaMinComparer_ObservableSourceNewMinAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(-1), new Dummy<int>(2), new Dummy<int>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item, new AbsoluteValueComparer()));

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
        public void LambdaMinComparer_NoMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(-1), new Dummy<int>(2), new Dummy<int>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);

            coll.Add(new Dummy<int>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaMinComparer_ExistingMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int>>() { new Dummy<int>(-1), new Dummy<int>(2), new Dummy<int>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);

            coll.Add(new Dummy<int>(1));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableMin_NoObservableSourceNewMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);
            Assert.AreEqual(1, coll.WithUpdates().Min(d => d.Item));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int?>(0));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableMin_ObservableSourceNewMinAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1, e.OldValue);
                Assert.AreEqual(0, e.NewValue);
            };

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(update);

            coll.Add(new Dummy<int?>(0));

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Value);
        }

        [TestMethod]
        public void LambdaNullableMin_NoMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);

            coll.Add(new Dummy<int?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableMin_ExistingMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Value);

            coll.Add(new Dummy<int?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableMinComparer_NoObservableSourceNewMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<Dummy<int?>>() { new Dummy<int?>(-1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);
            Assert.AreEqual(-1, coll.WithUpdates().Min(d => d.Item, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll.Add(new Dummy<int?>(0));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableMinComparer_ObservableSourceNewMinAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(-1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(-1, e.OldValue);
                Assert.AreEqual(0, e.NewValue);
            };

            Assert.AreEqual(-1, test.Value);
            Assert.IsFalse(update);

            coll.Add(new Dummy<int?>(0));

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Value);
        }

        [TestMethod]
        public void LambdaNullableMinComparer_NoMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(-1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);

            coll.Add(new Dummy<int?>(4));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaNullableMinComparer_ExistingMinAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<Dummy<int?>>() { new Dummy<int?>(-1), new Dummy<int?>(2), new Dummy<int?>(), new Dummy<int?>(-3) };

            var test = Observable.Expression(() => coll.WithUpdates().Min(d => d.Item, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(-1, test.Value);

            coll.Add(new Dummy<int?>(1));

            Assert.IsFalse(update);
        }
    }
}
