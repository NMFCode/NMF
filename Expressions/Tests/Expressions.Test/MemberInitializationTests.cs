using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class MemberInitializationTests
    {
        [TestMethod]
        public void MemberInitialization_NoObservable_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<int>(23);

            var test = Observable.Expression(() => new Dummy<int>() { Item = dummy.Item });

            test.ValueChanged += (o,e) => update = true;

            Assert.AreEqual(23, (test.Value as Dummy<int>).Item);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
            Assert.AreNotSame(dummy, test.Value);
            Assert.AreEqual(23, (test.Value as Dummy<int>).Item);
        }

        [TestMethod]
        public void MemberInitialization_Observable_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<int>(23);

            var test = Observable.Expression(() => new Dummy<int>() { Item = dummy.Item });

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, (test.Value as Dummy<int>).Item);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);

            Assert.AreEqual(42, (test.Value as Dummy<int>).Item);
            Assert.AreNotSame(dummy, test.Value);
        }
    }
}
