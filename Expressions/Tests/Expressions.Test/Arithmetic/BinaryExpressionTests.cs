using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class BinaryExpressionTests
    {
        [TestMethod]
        public void Add_String_NoObservable_NoUpdate()
        {
            var updated = false;
            var dummy = new Dummy<string>() { Item = "5" };

            var result = Observable.Expression(() => dummy.Item + "2");

            result.ValueChanged += (o, e) => updated = true;

            Assert.AreEqual("52", result.Value);
            Assert.IsFalse(updated);

            dummy.Item = "4";

            Assert.AreEqual("52", result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Add_String_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<string>() { Item = "5" };

            var result = Observable.Expression(() => dummy.Item + "2");

            result.ValueChanged += (o, e) =>
            {
                Assert.AreEqual("52", e.OldValue);
                updated = true;
            };

            Assert.AreEqual("52", result.Value);
            Assert.IsFalse(updated);

            dummy.Item = "4";

            Assert.IsTrue(updated);
            Assert.AreEqual("42", result.Value);
        }

        [TestMethod]
        public void And_Boolean_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<bool>() { Item = true };

            var result = Observable.Expression(() => dummy.Item & true);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = false;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void And_Boolean_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<bool>() { Item = true };

            var result = Observable.Expression(() => dummy.Item & true);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = false;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void AndAlso_Boolean_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<bool>() { Item = true };

            var result = Observable.Expression(() => dummy.Item && true);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = false;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void AndAlso_Boolean_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<bool>() { Item = true };

            var result = Observable.Expression(() => dummy.Item && true);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = false;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void Or_Boolean_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<bool>() { Item = true };

            var result = Observable.Expression(() => dummy.Item | false);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = false;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Or_Boolean_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<bool>() { Item = true };

            var result = Observable.Expression(() => dummy.Item | false);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = false;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void Xor_Boolean_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<bool>() { Item = true };

            var result = Observable.Expression(() => dummy.Item ^ false);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = false;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Xor_Boolean_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<bool>() { Item = true };

            var test = new ObservingFunc<Dummy<bool>, bool>(d => d.Item ^ false);

            var result = test.Observe(dummy);
            result.Successors.SetDummy();

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = false;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void OrElse_Boolean_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<bool>() { Item = true };

            var test = new ObservingFunc<Dummy<bool>, bool>(d => d.Item || false);

            var result = test.Observe(dummy);
            result.Successors.SetDummy();

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = false;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void OrElse_Boolean_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<bool>() { Item = true };

            var test = new ObservingFunc<Dummy<bool>, bool>(d => d.Item || false);

            var result = test.Observe(dummy);
            result.Successors.SetDummy();

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = false;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }
    }
}
