using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class AnyTest
    {
        [TestMethod]
        public void Any_NoObservableSourceItemAddedToEmptyCollection_NoUpdate()
        {
            var update = false;
            var coll = new List<int>();

            var test = Observable.Expression(() => coll.WithUpdates().Any());

            test.ValueChanged += (o, e) => update = true;

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            coll.Add(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Any_ObservableSourceItemAddedToEmptyCollection_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>();

            var test = Observable.Expression(() => coll.WithUpdates().Any());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            coll.Add(1);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void Any_ItemAddedToNonEmtyCollection_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1 };

            var test = Observable.Expression(() => coll.WithUpdates().Any());

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Add(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Any_NoObservableSourceItemRemovedThusEmptyCollection_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1 };

            var test = Observable.Expression(() => coll.WithUpdates().Any());

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Any_ObservableSourceItemRemovedThusEmptyCollection_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1 };

            var test = Observable.Expression(() => coll.WithUpdates().Any());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(1);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void Any_ItemRemovedStillNonEmtyCollection_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Any());

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Any_ObservableSource_NoUpdateWhenDetached()
        {
            var update = false;
            var coll = new NotifyCollection<int>();

            var test = Observable.Expression(() => coll.Any());

            test.ValueChanged += (o, e) => update = true;

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            test.Detach();

            coll.Add(1);

            Assert.IsFalse(update);

            test.Attach();

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
            update = true;

            coll.Remove(1);

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void Any_NoObservableSourceReset_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Any());

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Any_ObservableSourceReset_Update()
        {
            var update = false;
            var coll = new NotifyCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.Any());

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void LambdaAny_NoObservableSourceItemAddedToEmptyCollection_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { -1, -2 };

            var test = Observable.Expression(() => coll.WithUpdates().Any(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            coll.Add(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaAny_ObservableSourceItemAddedToEmptyCollection_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, -2 };

            var test = Observable.Expression(() => coll.WithUpdates().Any(i => i > 0));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            coll.Add(1);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void LambdaAny_ItemAddedToNonEmtyCollection_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, -2, 1 };

            var test = Observable.Expression(() => coll.WithUpdates().Any(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Add(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaAny_NoObservableSourceItemRemovedThusEmptyCollection_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { -1, -2, 1 };

            var test = Observable.Expression(() => coll.WithUpdates().Any(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaAny_ObservableSourceItemRemovedThusEmptyCollection_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, -2, 1 };

            var test = Observable.Expression(() => coll.WithUpdates().Any(i => i > 0));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(1);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void LambdaAny_ItemRemovedStillNonEmtyCollection_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { -1, -2, 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Any(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaAny_ObservableSource_NoUpdateWhenDetached()
        {
            var update = false;
            var coll = new NotifyCollection<int>() { -1, -2 };

            var test = Observable.Expression(() => coll.Any(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            test.Detach();

            coll.Add(1);

            Assert.IsFalse(update);

            test.Attach();

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
            update = false;

            coll.Remove(1);

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void LambdaAny_NoObservableSourceReset_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Any(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void LambdaAny_ObservableSourceReset_Update()
        {
            var update = false;
            var coll = new NotifyCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.Any(i => i > 0));

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsTrue(update);
        }
    }
}
