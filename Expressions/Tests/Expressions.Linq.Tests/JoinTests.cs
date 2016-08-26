using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Test;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class JoinTests
    {
        [TestMethod]
        public void Join_NoObservableSource1ItemAdded_NoUpdate()
        {
            var update = false;

            var source1 = CreateFilterSource();
            var source2 = CreateStringSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source1.Add(new Pair<int, bool>(4, true));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Join_ObservableSource1ItemAdded_Update()
        {
            var update = false;

            var source1 = CreateObservableFilterSource();
            var source2 = CreateStringSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2, e.NewItems.Count);
                Assert.IsTrue(e.NewItems.Contains("Foo"));
                Assert.IsTrue(e.NewItems.Contains("Bar"));
                Assert.IsNull(e.OldItems);
            };

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source1.Add(new Pair<int, bool>(4, true));

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
            Assert.IsTrue(test.Contains("Bar"));
        }

        [TestMethod]
        public void Join_NoObservableSource1ItemRemoved_NoUpdate()
        {
            var update = false;

            var source1 = CreateFilterSource();
            var source2 = CreateStringSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source1.Remove(source1[1]);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Join_ObservableSource1ItemRemoved_Update()
        {
            var update = false;

            var source1 = CreateObservableFilterSource();
            var source2 = CreateStringSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.IsTrue(e.OldItems.Contains(null));
                Assert.IsNull(e.NewItems);
            };

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source1.Remove(source1[2]);

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("42"));
        }

        [TestMethod]
        public void Join_NoObservableSource2ItemAdded_NoUpdate()
        {
            var update = false;

            var source1 = CreateStringSource();
            var source2 = CreateFilterSource();

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source2.Add(new Pair<int, bool>(4, true));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Join_ObservableSource2ItemAdded_Update()
        {
            var update = false;

            var source1 = CreateStringSource();
            var source2 = CreateObservableFilterSource();

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2, e.NewItems.Count);
                Assert.IsTrue(e.NewItems.Contains("Foo"));
                Assert.IsTrue(e.NewItems.Contains("Bar"));
                Assert.IsNull(e.OldItems);
            };

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source2.Add(new Pair<int, bool>(4, true));

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
            Assert.IsTrue(test.Contains("Bar"));
        }

        [TestMethod]
        public void Join_NoObservableSource2ItemRemoved_NoUpdate()
        {
            var update = false;

            var source1 = CreateStringSource();
            var source2 = CreateFilterSource();

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source2.Remove(source2[1]);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Join_ObservableSource2ItemRemoved_Update()
        {
            var update = false;

            var source1 = CreateStringSource();
            var source2 = CreateObservableFilterSource();

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.IsTrue(e.OldItems.Contains(null));
                Assert.IsNull(e.NewItems);
            };

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source2.Remove(source2[2]);

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("42"));
        }

        [TestMethod]
        public void Join_NoObservableOuterKey_NoUpdate()
        {
            var update = false;
            var item = new Pair<int, string>(3, "Foo");
            var source1 = new List<Pair<int, string>>() { item };
            var source2 = CreateFilterSource();

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsFalse(update);

            item.Item1 = 1;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Join_ObservableOuterKey_Update()
        {
            var update = false;
            var item = new ObservablePair<int, string>(3, "Foo");
            var source1 = new List<Pair<int, string>>() { item };
            var source2 = CreateFilterSource();

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.IsNull(e.OldItems[0]);
                Assert.AreEqual("Foo", e.NewItems[0]);
                update = true;
            };

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsFalse(update);

            item.Item1 = 1;

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
        }

        [TestMethod]
        public void Join_NoObservableInnerKey_NoUpdate()
        {
            var update = false;
            var item = new Pair<int, string>(3, "Foo");
            var source2 = new List<Pair<int, string>>() { item };
            var source1 = CreateFilterSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsFalse(update);

            item.Item1 = 1;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Join_ObservableInnerKey_Update()
        {
            var update = false;
            var item = new ObservablePair<int, string>(3, "Foo");
            var source2 = new List<Pair<int, string>>() { item };
            var source1 = CreateFilterSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.IsNull(e.OldItems[0]);
                Assert.AreEqual("Foo", e.NewItems[0]);
                update = true;
            };

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsFalse(update);

            item.Item1 = 1;

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
        }

        [TestMethod]
        public void Join_NoObservableOuterResult_NoUpdate()
        {
            var update = false;
            var item = new Pair<int, string>(1, "Foo");
            var source1 = new List<Pair<int, string>>() { item };
            var source2 = CreateFilterSource();

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
            Assert.IsFalse(update);

            item.Item2 = "Bar";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Join_ObservableOuterResult_Update()
        {
            var update = false;
            var item = new ObservablePair<int, string>(1, "Foo");
            var source1 = new List<Pair<int, string>>() { item };
            var source2 = CreateFilterSource();

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreEqual("Foo", e.OldItems[0]);
                Assert.AreEqual("Bar", e.NewItems[0]);
                update = true;
            };

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
            Assert.IsFalse(update);

            item.Item2 = "Bar";

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Bar"));
        }

        [TestMethod]
        public void Join_NoObservableInnerResult_NoUpdate()
        {
            var update = false;
            var item = new Pair<int, string>(1, "Foo");
            var source2 = new List<Pair<int, string>>() { item };
            var source1 = CreateFilterSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
            Assert.IsFalse(update);

            item.Item2 = "Bar";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Join_ObservableInnerResult_Update()
        {
            var update = false;
            var item = new ObservablePair<int, string>(1, "Foo");
            var source2 = new List<Pair<int, string>>() { item };
            var source1 = CreateFilterSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreEqual("Foo", e.OldItems[0]);
                Assert.AreEqual("Bar", e.NewItems[0]);
                update = true;
            };

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
            Assert.IsFalse(update);

            item.Item2 = "Bar";

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Bar"));
        }

        [TestMethod]
        public void JoinComparer_NoObservableSource1ItemAdded_NoUpdate()
        {
            var update = false;

            var source1 = CreateFilterSource(-1);
            var source2 = CreateStringSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source1.Add(new Pair<int, bool>(4, true));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void JoinComparer_ObservableSource1ItemAdded_Update()
        {
            var update = false;

            var source1 = CreateObservableFilterSource(-1);
            var source2 = CreateStringSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2, e.NewItems.Count);
                Assert.IsTrue(e.NewItems.Contains("Foo"));
                Assert.IsTrue(e.NewItems.Contains("Bar"));
                Assert.IsNull(e.OldItems);
            };

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source1.Add(new Pair<int, bool>(4, true));

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
            Assert.IsTrue(test.Contains("Bar"));
        }

        [TestMethod]
        public void JoinComparer_NoObservableSource1ItemRemoved_NoUpdate()
        {
            var update = false;

            var source1 = CreateFilterSource(-1);
            var source2 = CreateStringSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source1.Remove(source1[1]);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void JoinComparer_ObservableSource1ItemRemoved_Update()
        {
            var update = false;

            var source1 = CreateObservableFilterSource(-1);
            var source2 = CreateStringSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.IsTrue(e.OldItems.Contains(null));
                Assert.IsNull(e.NewItems);
            };

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source1.Remove(source1[2]);

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("42"));
        }

        [TestMethod]
        public void JoinComparer_NoObservableSource2ItemAdded_NoUpdate()
        {
            var update = false;

            var source1 = CreateStringSource();
            var source2 = CreateFilterSource(-1);

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source2.Add(new Pair<int, bool>(4, true));

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void JoinComparer_ObservableSource2ItemAdded_Update()
        {
            var update = false;

            var source1 = CreateStringSource();
            var source2 = CreateObservableFilterSource(-1);

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(2, e.NewItems.Count);
                Assert.IsTrue(e.NewItems.Contains("Foo"));
                Assert.IsTrue(e.NewItems.Contains("Bar"));
                Assert.IsNull(e.OldItems);
            };

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source2.Add(new Pair<int, bool>(4, true));

            Assert.IsTrue(update);
            Assert.AreEqual(4, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
            Assert.IsTrue(test.Contains("Bar"));
        }

        [TestMethod]
        public void JoinComparer_NoObservableSource2ItemRemoved_NoUpdate()
        {
            var update = false;

            var source1 = CreateStringSource();
            var source2 = CreateFilterSource(-1);

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source2.Remove(source2[1]);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void JoinComparer_ObservableSource2ItemRemoved_Update()
        {
            var update = false;

            var source1 = CreateStringSource();
            var source2 = CreateObservableFilterSource(-1);

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.IsTrue(e.OldItems.Contains(null));
                Assert.IsNull(e.NewItems);
            };

            Assert.AreEqual(2, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsTrue(test.Contains("42"));
            Assert.IsFalse(update);

            source2.Remove(source2[2]);

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("42"));
        }

        [TestMethod]
        public void JoinComparer_NoObservableOuterKey_NoUpdate()
        {
            var update = false;
            var item = new Pair<int, string>(3, "Foo");
            var source1 = new List<Pair<int, string>>() { item };
            var source2 = CreateFilterSource(-1);

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsFalse(update);

            item.Item1 = 1;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void JoinComparer_ObservableOuterKey_Update()
        {
            var update = false;
            var item = new ObservablePair<int, string>(3, "Foo");
            var source1 = new List<Pair<int, string>>() { item };
            var source2 = CreateFilterSource(-1);

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.IsNull(e.OldItems[0]);
                Assert.AreEqual("Foo", e.NewItems[0]);
                update = true;
            };

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsFalse(update);

            item.Item1 = 1;

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
        }

        [TestMethod]
        public void JoinComparer_NoObservableInnerKey_NoUpdate()
        {
            var update = false;
            var item = new Pair<int, string>(3, "Foo");
            var source2 = new List<Pair<int, string>>() { item };
            var source1 = CreateFilterSource(-1);

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsFalse(update);

            item.Item1 = 1;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void JoinComparer_ObservableInnerKey_Update()
        {
            var update = false;
            var item = new ObservablePair<int, string>(3, "Foo");
            var source2 = new List<Pair<int, string>>() { item };
            var source1 = CreateFilterSource(-1);

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.IsNull(e.OldItems[0]);
                Assert.AreEqual("Foo", e.NewItems[0]);
                update = true;
            };

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains(null));
            Assert.IsFalse(update);

            item.Item1 = 1;

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
        }

        [TestMethod]
        public void JoinComparer_NoObservableOuterResult_NoUpdate()
        {
            var update = false;
            var item = new Pair<int, string>(1, "Foo");
            var source1 = new List<Pair<int, string>>() { item };
            var source2 = CreateFilterSource(-1);

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
            Assert.IsFalse(update);

            item.Item2 = "Bar";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void JoinComparer_ObservableOuterResult_Update()
        {
            var update = false;
            var item = new ObservablePair<int, string>(1, "Foo");
            var source1 = new List<Pair<int, string>>() { item };
            var source2 = CreateFilterSource(-1);

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreEqual("Foo", e.OldItems[0]);
                Assert.AreEqual("Bar", e.NewItems[0]);
                update = true;
            };

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
            Assert.IsFalse(update);

            item.Item2 = "Bar";

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Bar"));
        }

        [TestMethod]
        public void JoinComparer_NoObservableInnerResult_NoUpdate()
        {
            var update = false;
            var item = new Pair<int, string>(1, "Foo");
            var source2 = new List<Pair<int, string>>() { item };
            var source1 = CreateFilterSource(-1);

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
            Assert.IsFalse(update);

            item.Item2 = "Bar";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void JoinComparer_ObservableInnerResult_Update()
        {
            var update = false;
            var item = new ObservablePair<int, string>(1, "Foo");
            var source2 = new List<Pair<int, string>>() { item };
            var source1 = CreateFilterSource(-1);

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null,
                new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreEqual("Foo", e.OldItems[0]);
                Assert.AreEqual("Bar", e.NewItems[0]);
                update = true;
            };

            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Foo"));
            Assert.IsFalse(update);

            item.Item2 = "Bar";

            Assert.IsTrue(update);
            Assert.AreEqual(1, test.Count());
            Assert.IsTrue(test.Contains("Bar"));
        }

        [TestMethod]
        public void Join_NoObservableSource1Reset_NoUpdate()
        {
            var update = false;
            var source1 = CreateFilterSource();
            var source2 = CreateStringSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) => update = true;

            source1.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Join_ObservableSource1Reset_Update()
        {
            var update = false;
            var source1 = CreateObservableFilterSource();
            var source2 = CreateStringSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
                update = true;
            };

            source1.Clear();

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Count());
        }

        [TestMethod]
        public void Join_NoObservableSource2Reset_NoUpdate()
        {
            var update = false;
            var source1 = CreateFilterSource();
            var source2 = CreateStringSource();

            var test = source1.WithUpdates().Join(source2,
                filter => filter.Item1,
                content => content.Item1,
                (filter, content) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) => update = true;

            source2.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Join_ObservableSource2Reset_Update()
        {
            var update = false;
            var source1 = CreateStringSource();
            var source2 = CreateObservableFilterSource();

            var test = source1.WithUpdates().Join(source2,
                content => content.Item1,
                filter => filter.Item1,
                (content, filter) => filter.Item2 ? content.Item2 : null);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
                update = true;
            };

            source2.Clear();

            Assert.IsTrue(update);
            Assert.AreEqual(0, test.Count());
        }

        private List<Pair<int, bool>> CreateFilterSource(int multiplier = 1)
        {
            return new List<Pair<int, bool>>()
            {
                new Pair<int, bool>(1 * multiplier, true),
                new Pair<int, bool>(2 * multiplier, true),
                new Pair<int, bool>(2 * multiplier, false),
                new Pair<int, bool>(3 * multiplier, false)
            };
        }

        private List<Pair<int, string>> CreateStringSource()
        {
            return new List<Pair<int, string>>()
            {
                new Pair<int, string>(2, "42"),
                new Pair<int, string>(4, "Foo"),
                new Pair<int, string>(4, "Bar")
            };
        }

        private NotifyCollection<Pair<int, bool>> CreateObservableFilterSource(int multiplier = 1)
        {
            return new NotifyCollection<Pair<int, bool>>()
            {
                new Pair<int, bool>(1 * multiplier, true),
                new Pair<int, bool>(2 * multiplier, true),
                new Pair<int, bool>(2 * multiplier, false),
                new Pair<int, bool>(3 * multiplier, false)
            };
        }
    }
}
