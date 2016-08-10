using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class ChangeAwareDictionaryTests
    {
        [TestMethod]
        public void ChangeAwareDictionary_AsNotifiable_Tracks_Items()
        {
            var dict = new ChangeAwareDictionary<string, int>();
            dict["Foo"] = 8;

            var fooTracker = dict.AsNotifiable("Foo");
            var barTracker = dict.AsNotifiable("Bar");

            var fooChanged = false;
            var fooOldValue = 8;
            var fooNewValue = 15;

            fooTracker.ValueChanged += (o, e) =>
            {
                fooChanged = true;
                Assert.AreEqual(fooOldValue, e.OldValue);
                Assert.AreEqual(fooNewValue, e.NewValue);
            };

            var barChanged = false;
            var barOldValue = 0;
            var barNewValue = 42;

            barTracker.ValueChanged += (o, e) =>
            {
                barChanged = true;
                Assert.AreEqual(barOldValue, e.OldValue);
                Assert.AreEqual(barNewValue, e.NewValue);
            };

            dict["Foo"] = 15;

            Assert.IsTrue(fooChanged);
            Assert.IsFalse(barChanged);
            Assert.AreEqual(15, dict["Foo"]);

            fooChanged = false;

            dict["Bar"] = 42;

            Assert.IsTrue(barChanged);
            Assert.IsFalse(fooChanged);
            Assert.AreEqual(42, dict["Bar"]);
        }

        [TestMethod]
        public void ChangeAwareDictionary_Incrementalization_Tracks_Items()
        {
            var dict = new ChangeAwareDictionary<string, int>();
            dict["Foo"] = 8;

            var fooTracker = Observable.Expression(() => dict["Foo"]);
            var barTracker = Observable.Expression(() => dict["Bar"]);

            var fooChanged = false;
            var fooOldValue = 8;
            var fooNewValue = 15;

            fooTracker.ValueChanged += (o, e) =>
            {
                fooChanged = true;
                Assert.AreEqual(fooOldValue, e.OldValue);
                Assert.AreEqual(fooNewValue, e.NewValue);
            };

            var barChanged = false;
            var barOldValue = 0;
            var barNewValue = 42;

            barTracker.ValueChanged += (o, e) =>
            {
                barChanged = true;
                Assert.AreEqual(barOldValue, e.OldValue);
                Assert.AreEqual(barNewValue, e.NewValue);
            };

            dict["Foo"] = 15;

            Assert.IsTrue(fooChanged);
            Assert.IsFalse(barChanged);
            Assert.AreEqual(15, dict["Foo"]);

            fooChanged = false;

            dict["Bar"] = 42;

            Assert.IsTrue(barChanged);
            Assert.IsFalse(fooChanged);
            Assert.AreEqual(42, dict["Bar"]);
        }

        [TestMethod]
        public void ChangeAwareDictionary_ReturnSameNotifiable()
        {
            var dict = new ChangeAwareDictionary<string, int>();

            var first = dict.AsNotifiable("Foo");
            var second = Observable.Expression(() => dict["Foo"]);

            Assert.AreSame(first, second);
        }

        [TestMethod]
        public void ChangeAwareDictionary_Forget()
        {
            var dict = new ChangeAwareDictionary<string, int>();
            dict["Foo"] = 42;

            var second = Observable.Expression(() => dict["Foo"]);
            dict.Forget("Foo");
            var first = dict.AsNotifiable("Foo");

            Assert.AreNotSame(first, second);
            Assert.AreNotEqual(first.Value, second.Value);
        }
    }
}
