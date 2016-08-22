using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class EqualsTests
    {
        [TestMethod]
        public void Equals_Class_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<string>() { Item = "42" };

            var test = new ObservingFunc<Dummy<string>, bool>(d => d.Item == "42");

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = "23";

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Equals_Class_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<string>() { Item = "42" };

            var test = new ObservingFunc<Dummy<string>, bool>(d => d.Item == "42");

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = "23";

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void Equals_Int_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<int>() { Item = 42 };

            var test = new ObservingFunc<Dummy<int>, bool>(d => d.Item == 42);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 23;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void Equals_Int_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<int>() { Item = 42 };

            var test = new ObservingFunc<Dummy<int>, bool>(d => d.Item == 42);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 23;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void NotEquals_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<string>() { Item = "42" };

            var test = new ObservingFunc<Dummy<string>, bool>(d => d.Item != "42");

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsFalse(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = "23";

            Assert.IsFalse(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void NotEquals_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<string>() { Item = "42" };

            var test = new ObservingFunc<Dummy<string>, bool>(d => d.Item != "42");

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsFalse(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = "23";

            Assert.IsTrue(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void TypeIs_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<object>() { Item = "42" };

            var test = new ObservingFunc<Dummy<object>, bool>(d => d.Item is string);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 42;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void TypeIs_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<object>() { Item = "42" };

            var test = new ObservingFunc<Dummy<object>, bool>(d => d.Item is string);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.IsTrue(result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 42;

            Assert.IsFalse(result.Value);
            Assert.IsTrue(updated);
        }

        [TestMethod]
        public void TypeAs_NoObservable_NoUpdates()
        {
            var updated = false;
            var dummy = new Dummy<object>() { Item = "42" };

            var test = new ObservingFunc<Dummy<object>, string>(d => d.Item as string);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.AreEqual("42", result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 42;

            Assert.AreEqual("42", result.Value);
            Assert.IsFalse(updated);
        }

        [TestMethod]
        public void TypeAs_Observable_Update()
        {
            var updated = false;
            var dummy = new ObservableDummy<object>() { Item = "42" };

            var test = new ObservingFunc<Dummy<object>, string>(d => d.Item as string);

            var result = test.Observe(dummy);
            result.Successors.Add(null);

            result.ValueChanged += (o, e) => updated = true;

            Assert.AreEqual("42", result.Value);
            Assert.IsFalse(updated);

            dummy.Item = 42;

            Assert.IsTrue(updated);
            Assert.IsNull(result.Value);
        }
    }
}
