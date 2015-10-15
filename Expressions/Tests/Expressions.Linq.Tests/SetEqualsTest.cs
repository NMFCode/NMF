using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class SetEqualsTest
    {
        [TestMethod]
        public void SetEquals_NoObservableSource2_NoUpdate()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2, 3 };
            var source2 = new List<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsTrue(source1.SetEquals(source2));
            Assert.IsFalse(update);

            source2.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SetEquals_ObservableSource2ItemAddedSoFalse_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2, 3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
                update = true;
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            source2.Add(4);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void SetEquals_ObservableSource2ItemAddedSoTrue_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2, 3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
                update = true;
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            source2.Add(3);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void SetEquals_ObservableSource2ItemRemovedSoFalse_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2, 3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
                update = true;
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            source2.Remove(3);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void SetEquals_ObservableSource2ItemRemovedSoTrue_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2, 3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3, 4 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
                update = true;
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            source2.Remove(4);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void SetEquals_ObservableSource2ItemAdded_NoUpdate()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2, 3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsTrue(source1.SetEquals(source2));
            Assert.IsFalse(update);

            source2.Add(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SetEquals_ObservableSource2ItemRemoved_NoUpdate()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2, 3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsTrue(source1.SetEquals(source2));
            Assert.IsFalse(update);

            source2.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SetEquals_ObservableSource1ItemAddedSoFalse_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2, 3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
                update = true;
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            source1.Add(4);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void SetEquals_ObservableSource1ItemAddedSoTrue_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
                update = true;
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            source1.Add(3);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void SetEquals_ObservableSource1ItemRemovedSoFalse_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2, 3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
                update = true;
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            source1.Remove(3);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void SetEquals_ObservableSource1ItemRemovedSoTrue_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2, 3, 4 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
                update = true;
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            source1.Remove(4);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void SetEquals_ObservableSource1ItemAdded_NoUpdate()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2, 3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsTrue(source1.SetEquals(source2));
            Assert.IsFalse(update);

            source1.Add(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SetEquals_ObservableSource1ItemRemoved_NoUpdate()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { 1, 1, 2, 3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsTrue(source1.SetEquals(source2));
            Assert.IsFalse(update);

            source1.Remove(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SetEqualsComparer_NoObservableSource2_NoUpdate()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, 1, -2, -3 };
            var source2 = new List<int>() { 1, 2, -2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsTrue(source1.SetEquals(source2, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            source2.Add(4);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SetEqualsComparer_ObservableSource2ItemAddedSoFalse_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, 1, -2, -3 };
            var source2 = new NotifyCollection<int>() { 1, 2, -2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
                update = true;
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            source2.Add(4);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void SetEqualsComparer_ObservableSource2ItemAddedSoTrue_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, 1, -2, -3 };
            var source2 = new NotifyCollection<int>() { 1, 2, -2 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
                update = true;
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            source2.Add(3);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void SetEqualsComparer_ObservableSource2ItemRemovedSoFalse_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, 1, -2, -3 };
            var source2 = new NotifyCollection<int>() { 1, 2, -2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
                update = true;
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            source2.Remove(3);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void SetEqualsComparer_ObservableSource2ItemRemovedSoTrue_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, 1, -2, -3 };
            var source2 = new NotifyCollection<int>() { 1, 2, -2, 3, 4 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
                update = true;
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            source2.Remove(4);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void SetEqualsComparer_ObservableSource2ItemAdded_NoUpdate()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, -1, -2, -3 };
            var source2 = new NotifyCollection<int>() { 1, 2, -2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsTrue(source1.SetEquals(source2, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            source2.Add(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SetEqualsComparer_ObservableSource2ItemRemoved_NoUpdate()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, 1, -2, -3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsTrue(source1.SetEquals(source2, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            source2.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SetEqualsComparer_ObservableSource1ItemAddedSoFalse_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, 1, -2, -3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
                update = true;
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            source1.Add(4);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void SetEqualsComparer_ObservableSource1ItemAddedSoTrue_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, 1, -2 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, -3 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
                update = true;
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            source1.Add(3);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void SetEqualsComparer_ObservableSource1ItemRemovedSoFalse_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, 1, -2, 3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsTrue((bool)e.OldValue);
                Assert.IsFalse((bool)e.NewValue);
                update = true;
            };

            Assert.IsTrue(test.Value);
            Assert.IsFalse(update);

            source1.Remove(3);

            Assert.IsTrue(update);
            Assert.IsFalse(test.Value);
        }

        [TestMethod]
        public void SetEqualsComparer_ObservableSource1ItemRemovedSoTrue_Update()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, 1, -2, -3, 4 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) =>
            {
                Assert.IsFalse((bool)e.OldValue);
                Assert.IsTrue((bool)e.NewValue);
                update = true;
            };

            Assert.IsFalse(test.Value);
            Assert.IsFalse(update);

            source1.Remove(4);

            Assert.IsTrue(update);
            Assert.IsTrue(test.Value);
        }

        [TestMethod]
        public void SetEqualsComparer_ObservableSource1ItemAdded_NoUpdate()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, -1, -2, -3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsTrue(source1.SetEquals(source2, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            source1.Add(1);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void SetEqualsComparer_ObservableSource1ItemRemoved_NoUpdate()
        {
            var update = false;
            var source1 = new NotifyCollection<int>() { -1, 1, -2, -3 };
            var source2 = new NotifyCollection<int>() { 1, 2, 2, 3 };

            var test = Observable.Expression(() => source1.SetEquals(source2, new AbsoluteValueComparer()));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value);
            Assert.IsTrue(source1.SetEquals(source2, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            source1.Remove(1);

            Assert.IsFalse(update);
        }
    }
}
