using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class ConditionalExpressionTests
    {
        [TestMethod]
        public void ConditionalExpression_NoObservableTest_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<bool>() { Item = false };

            var test = new ObservingFunc<Dummy<bool>, string>(d => d.Item ? "42" : "23");

            var result = test.Observe(dummy);

            result.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("23", result.Value);
            Assert.IsFalse(update);

            dummy.Item = true;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ConditionalExpression_ObservableTest_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<bool>() { Item = false };

            var test = new ObservingFunc<Dummy<bool>, string>(d => d.Item ? "42" : "23");

            var result = test.Observe(dummy);

            result.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("23", e.OldValue);
                Assert.AreEqual("42", e.NewValue);
            };

            Assert.AreEqual("23", result.Value);
            Assert.IsFalse(update);

            dummy.Item = true;

            Assert.IsTrue(update);
            Assert.AreEqual("42", result.Value);
        }
        
        [TestMethod]
        public void ConditionalExpression_NoObservableTruePart_NoUpdates()
        {
            var update = false;
            var dummy = new ObservableDummy<bool>() { Item = true };
            var dummy2 = new Dummy<string>() { Item = "23" };

            var test = new ObservingFunc<Dummy<bool>, string>(d => d.Item ? dummy2.Item : "23");

            var result = test.Observe(dummy);

            result.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("23", result.Value);
            Assert.IsFalse(update);

            dummy2.Item = "42";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ConditionalExpression_ObservableTruePart_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<bool>() { Item = true };
            var dummy2 = new ObservableDummy<string>() { Item = "23" };

            var test = new ObservingFunc<Dummy<bool>, string>(d => d.Item ? dummy2.Item : "23");

            var result = test.Observe(dummy);

            result.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("23", e.OldValue);
                Assert.AreEqual("42", e.NewValue);
            };

            Assert.AreEqual("23", result.Value);
            Assert.IsFalse(update);

            dummy2.Item = "42";

            Assert.IsTrue(update);
            Assert.AreEqual("42", result.Value);
        }
        
        [TestMethod]
        public void ConditionalExpression_NoObservableFalsePart_NoUpdates()
        {
            var update = false;
            var dummy = new ObservableDummy<bool>() { Item = false };
            var dummy2 = new Dummy<string>() { Item = "23" };

            var test = new ObservingFunc<Dummy<bool>, string>(d => d.Item ? "23" : dummy2.Item);

            var result = test.Observe(dummy);

            result.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("23", result.Value);
            Assert.IsFalse(update);

            dummy2.Item = "42";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ConditionalExpression_ObservableFalsePart_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<bool>() { Item = false };
            var dummy2 = new ObservableDummy<string>() { Item = "23" };

            var test = new ObservingFunc<Dummy<bool>, string>(d => d.Item ? "23" : dummy2.Item);

            var result = test.Observe(dummy);

            result.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("23", e.OldValue);
                Assert.AreEqual("42", e.NewValue);
            };

            Assert.AreEqual("23", result.Value);
            Assert.IsFalse(update);

            dummy2.Item = "42";

            Assert.IsTrue(update);
            Assert.AreEqual("42", result.Value);
        }
    }
}
