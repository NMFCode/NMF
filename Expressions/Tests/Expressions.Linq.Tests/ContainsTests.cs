using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class ContainsTests
    {
        [TestMethod]
        public void Contains_NoObservableSourceElementAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(4));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Contains_ObservableSourceElementAdded_NoUpdateWhenDetached()
        {
            var update = false;
            var coll = new NotifyCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.Contains(4));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            test.Detach();
            update = false;

            coll.Add(4);

            Assert.IsFalse(update);

            test.Attach();

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
            update = false;

            coll.Remove(4);

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void Contains_ObservableSourceElementAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(4));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            coll.Add(4);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void Contains_ElementAddedSecondTime_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(3));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Add(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Contains_ElementOtherItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(4));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            coll.Add(5);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Contains_NoObservableSourceElementRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(3));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Contains_ObservableSourceElementRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(3));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void Contains_SecondElementRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(3));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Contains_ElementOtherItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(3));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ContainsComparer_NoObservableSourceElementAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(4, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            coll.Add(-4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ContainsComparer_ObservableSourceElementAdded_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(4, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            coll.Add(-4);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void ContainsComparer_ElementAddedSecondTime_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(-3, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Add(-3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ContainsComparer_ElementOtherItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(-4, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            coll.Add(5);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ContainsComparer_NoObservableSourceElementRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(-3, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ContainsComparer_ObservableSourceElementRemoved_Update()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(-3, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void ContainsComparer_SecondElementRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(-3, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(3);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ContainsComparer_ElementOtherItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new ObservableCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(-3, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            coll.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Contains_NoObservableSourceReset_NoUpdate()
        {
            var update = false;
            var coll = new List<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(2));

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Contains_ObservableSourceReset_Update()
        {
            var update = false;
            var coll = new NotifyCollection<int>() { 1, 2, 3 };

            var test = Observable.Expression(() => coll.WithUpdates().Contains(2));

            test.ValueChanged += (o, e) => update = true;

            coll.Clear();

            Assert.IsTrue(update);
        }
    }
}
