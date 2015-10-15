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
        public void ArrayCreation_Observable_NoUpdatesWhenDetached()
        {
            var update = false;
            var dummy = new ObservableDummy<int>() { Item = 23 };

            var test = new NotifyValue<int[]>(() => new int[dummy.Item]);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, (test.Value as int[]).Length);
            Assert.IsFalse(update);

            test.Detach();

            dummy.Item = 42;

            Assert.IsFalse(update);

            test.Attach();

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value.Length);
            update = false;

            dummy.Item = 2;

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

        [TestMethod]
        public void ArrayInitialization_Observable_NoUpdatesWhenDetached()
        {
            var update = false;
            var dummy = new ObservableDummy<int>() { Item = 23 };

            var test = new NotifyValue<int[]>(() => new int[] { dummy.Item });

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, (test.Value as int[])[0]);
            Assert.IsFalse(update);

            test.Detach();

            dummy.Item = 42;

            Assert.IsFalse(update);

            test.Attach();

            Assert.AreEqual(42, test.Value[0]);
            update = true;

            dummy.Item = 1;

            Assert.AreEqual(1, test.Value[0]);
        }
    }
}
