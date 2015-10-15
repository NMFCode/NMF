using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class FirstOrDefaultTests
    {
        [TestMethod]
        public void FirstOrDefault_NoObservableSourceFirstItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<string>();

            var test = Observable.Expression(() => coll.WithUpdates().FirstOrDefault());

            test.ValueChanged += (o, e) => update = true;

            Assert.IsNull(test.Value);
            Assert.IsFalse(update);

            coll.Add("42");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void FirstOrDefault_ObservableSourceFirstItemAdded_Update()
        {
            var update = false;
            var coll = new NotifyCollection<string>();

            var test = Observable.Expression(() => coll.FirstOrDefault());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("42", e.NewValue);
                Assert.IsNull(e.OldValue);
            };

            Assert.IsNull(test.Value);
            Assert.IsFalse(update);

            coll.Add("42");

            Assert.IsTrue(update);
            Assert.AreEqual("42", test.Value);
        }

        [TestMethod]
        public void FirstOrDefault_ObservableSourceNewFirstItemAdded_Update()
        {
            var update = false;
            var coll = new NotifyCollection<string>() { "23" };

            var test = Observable.Expression(() => coll.FirstOrDefault());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("42", e.NewValue);
                Assert.AreEqual("23", e.OldValue);
            };

            Assert.AreEqual("23", test.Value);
            Assert.IsFalse(update);

            coll.Insert(0, "42");

            Assert.IsTrue(update);
            Assert.AreEqual("42", test.Value);
        }

        [TestMethod]
        public void FirstOrDefault_ObservableSourceOtherItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new NotifyCollection<string>() { "23" };

            var test = Observable.Expression(() => coll.FirstOrDefault());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("23", test.Value);
            Assert.IsFalse(update);

            coll.Add("42");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void FirstOrDefault_NoObservableSourceFirstItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<string>() { "42" };

            var test = Observable.Expression(() => coll.WithUpdates().FirstOrDefault());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("42", test.Value);
            Assert.IsFalse(update);

            coll.Remove("42");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void FirstOrDefault_ObservableSourceFirstItemRemoved_Update()
        {
            var update = false;
            var coll = new NotifyCollection<string>() { "42" };

            var test = Observable.Expression(() => coll.FirstOrDefault());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("42", e.OldValue);
                Assert.IsNull(e.NewValue);
            };

            Assert.AreEqual("42", test.Value);
            Assert.IsFalse(update);

            coll.Remove("42");

            Assert.IsTrue(update);
            Assert.IsNull(test.Value);
        }

        [TestMethod]
        public void FirstOrDefault_ObservableSourceRemoveNewFirstItem_Update()
        {
            var update = false;
            var coll = new NotifyCollection<string>() { "42", "23" };

            var test = Observable.Expression(() => coll.FirstOrDefault());

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("42", e.OldValue);
                Assert.AreEqual("23", e.NewValue);
            };

            Assert.AreEqual("42", test.Value);
            Assert.IsFalse(update);

            coll.Remove("42");

            Assert.IsTrue(update);
            Assert.AreEqual("23", test.Value);
        }

        [TestMethod]
        public void FirstOrDefault_ObservableSourceOtherItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new NotifyCollection<string>() { "23", "42" };

            var test = Observable.Expression(() => coll.FirstOrDefault());

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("23", test.Value);
            Assert.IsFalse(update);

            coll.Remove("42");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void FirstOrDefault_ObservableSourceFirstItemAdded_NoUpdateWhenDetached()
        {
            var update = false;
            var coll = new NotifyCollection<string>();

            var test = Observable.Expression(() => coll.FirstOrDefault());

            test.ValueChanged += (o, e) => update = true;

            Assert.IsNull(test.Value);
            Assert.IsFalse(update);

            test.Detach();
            update = false;

            coll.Add("42");

            Assert.IsFalse(update);

            test.Attach();

            Assert.IsTrue(update);
            Assert.AreEqual("42", test.Value);
            update = false;

            coll.Insert(0, "23");

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void PredicateFirstOrDefault_NoObservableSourceFirstItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new List<string>() { "1" };

            var test = Observable.Expression(() => coll.WithUpdates().FirstOrDefault(s => s.Length > 1));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsNull(test.Value);
            Assert.IsFalse(update);

            coll.Add("42");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void PredicateFirstOrDefault_ObservableSourceFirstItemAdded_Update()
        {
            var update = false;
            var coll = new NotifyCollection<string>() { "1" };

            var test = Observable.Expression(() => coll.FirstOrDefault(s => s.Length > 1));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("42", e.NewValue);
                Assert.IsNull(e.OldValue);
            };

            Assert.IsNull(test.Value);
            Assert.IsFalse(update);

            coll.Add("42");

            Assert.IsTrue(update);
            Assert.AreEqual("42", test.Value);
        }

        [TestMethod]
        public void PredicateFirstOrDefault_ObservableSourceOtherItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new NotifyCollection<string>() { "1", "23" };

            var test = Observable.Expression(() => coll.FirstOrDefault(s => s.Length > 1));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("23", test.Value);
            Assert.IsFalse(update);

            coll.Add("42");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void PredicateFirstOrDefault_NoObservableSourceFirstItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new List<string>() { "1", "42" };

            var test = Observable.Expression(() => coll.WithUpdates().FirstOrDefault(s => s.Length > 1));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("42", test.Value);
            Assert.IsFalse(update);

            coll.Remove("42");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void PredicateFirstOrDefault_ObservableSourceFirstItemRemoved_Update()
        {
            var update = false;
            var coll = new NotifyCollection<string>() { "1", "42" };

            var test = Observable.Expression(() => coll.FirstOrDefault(s => s.Length > 1));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("42", e.OldValue);
                Assert.IsNull(e.NewValue);
            };

            Assert.AreEqual("42", test.Value);
            Assert.IsFalse(update);

            coll.Remove("42");

            Assert.IsTrue(update);
            Assert.IsNull(test.Value);
        }

        [TestMethod]
        public void PredicateFirstOrDefault_ObservableSourceRemoveNewFirstItem_Update()
        {
            var update = false;
            var coll = new NotifyCollection<string>() { "1", "42", "23" };

            var test = Observable.Expression(() => coll.FirstOrDefault(s => s.Length > 1));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("42", e.OldValue);
                Assert.AreEqual("23", e.NewValue);
            };

            Assert.AreEqual("42", test.Value);
            Assert.IsFalse(update);

            coll.Remove("42");

            Assert.IsTrue(update);
            Assert.AreEqual("23", test.Value);
        }

        [TestMethod]
        public void PredicateFirstOrDefault_ObservableSourceOtherItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new NotifyCollection<string>() { "1", "23", "42" };

            var test = Observable.Expression(() => coll.FirstOrDefault(s => s.Length > 1));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("23", test.Value);
            Assert.IsFalse(update);

            coll.Remove("42");

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void PredicateFirstOrDefault_ObservableSourceFirstItemAdded_NoUpdateWhenDetached()
        {
            var update = false;
            var coll = new NotifyCollection<string>() { "1" };

            var test = Observable.Expression(() => coll.FirstOrDefault(s => s.Length > 1));

            test.ValueChanged += (o, e) => update = true;

            Assert.IsNull(test.Value);
            Assert.IsFalse(update);

            test.Detach();
            update = false;

            coll.Add("42");

            Assert.IsFalse(update);

            test.Attach();

            Assert.IsTrue(update);
            Assert.AreEqual("42", test.Value);
            update = false;

            coll.Remove("42");

            Assert.IsTrue(update);
        }
    }
}
