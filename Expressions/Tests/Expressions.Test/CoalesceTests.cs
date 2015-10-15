using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class CoalesceTests
    {
        [TestMethod]
        public void Coalesce_NoObservableSource_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<string>();

            var test = Observable.Expression(() => dummy.Item ?? "42");

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("42", test.Value);
            Assert.IsFalse(update);

            dummy.Item = "23";

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Coalesce_ObservableSource_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<string>();

            var test = Observable.Expression(() => dummy.Item ?? "42");

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual("42", e.OldValue);
                Assert.AreEqual("23", e.NewValue);
            };

            Assert.AreEqual("42", test.Value);
            Assert.IsFalse(update);

            dummy.Item = "23";

            Assert.IsTrue(update);
            Assert.AreEqual("23", test.Value);
        }
    }
}
