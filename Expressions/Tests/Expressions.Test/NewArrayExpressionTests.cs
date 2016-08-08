using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class NewArrayExpressionTests
    {
        [TestMethod]
        public void ArrayCreation_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<int>() { Item = 23 };

            var test = new NotifyValue<int[]>(() => new int[dummy.Item]);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, (test.Value as int[]).Length);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ArrayCreation_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<int>() { Item = 23 };

            var test = new NotifyValue<int[]>(() => new int[dummy.Item]);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23, (e.OldValue as int[]).Length);
                Assert.AreEqual(42, (e.NewValue as int[]).Length);
            };

            Assert.AreEqual(23, (test.Value as int[]).Length);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void ArrayInitialization_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<int>() { Item = 23 };

            var test = new NotifyValue<int[]>(() => new int[] { dummy.Item });

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, (test.Value as int[])[0]);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ArrayInitialization_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<int>() { Item = 23 };

            var test = new NotifyValue<int[]>(() => new int[] { dummy.Item });

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(42, (e.NewValue as int[])[0]);
            };

            Assert.AreEqual(23, (test.Value as int[])[0]);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
        }
    }
}
