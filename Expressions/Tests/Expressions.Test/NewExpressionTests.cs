using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class NewExpressionTests
    {
        [TestMethod]
        public void New_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<int>() { Item = 23 };

            var test = new NotifyValue<Dummy<int>>(() => new Dummy<int>(dummy.Item));

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, (test.Value as Dummy<int>).Item);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void New_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<int>() { Item = 23 };

            var test = new NotifyValue<Dummy<int>>(() => new Dummy<int>(dummy.Item));

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23, (e.OldValue as Dummy<int>).Item);
                Assert.AreEqual(42, (e.NewValue as Dummy<int>).Item);
            };

            Assert.AreEqual(23, (test.Value as Dummy<int>).Item);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
        }
    }
}
