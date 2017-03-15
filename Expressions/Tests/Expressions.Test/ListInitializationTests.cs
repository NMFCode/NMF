using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class ListInitializationTests
    {
        [TestMethod]
        public void ListInit_NoObservable_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<string>("foo");

            var test = Observable.Expression(() => new List<string>() { dummy.Item });

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value.Contains("foo"));
            Assert.IsFalse(update);

            dummy.Item = "bar";

            Assert.IsFalse(update);
            Assert.AreEqual(1, test.Value.Count);
            Assert.IsFalse(test.Value.Contains("bar"));
        }

        [TestMethod]
        public void ListInit_Observable_NoUpdate()
        {
            var update = false;
            var dummy = new ObservableDummy<string>("foo");

            var test = Observable.Expression(() => new List<string>() { dummy.Item });

            test.ValueChanged += (o, e) => update = true;

            Assert.IsTrue(test.Value.Contains("foo"));
            Assert.IsFalse(update);

            dummy.Item = "bar";

            Assert.IsFalse(update);
            Assert.AreEqual(1, test.Value.Count);
            Assert.IsTrue(test.Value.Contains("bar"));
        }

        [TestMethod]
        public void ListInit_CustomListNoObservable_NoUpdate()
        {
            var update = false;
            var dummy = new Dummy<string>("foo");

            var test = Observable.Expression(() => new ListFake() { dummy.Item });

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("foo", test.Value.Value);
            Assert.IsFalse(update);

            dummy.Item = "bar";

            Assert.IsFalse(update);
            Assert.AreEqual("foo", test.Value.Value);
        }

        [TestMethod]
        public void ListInit_CustomListObservable_NoUpdate()
        {
            var update = false;
            var dummy = new ObservableDummy<string>("foo");

            var test = Observable.Expression(() => new ListFake() { dummy.Item });

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual("foo", test.Value.Value);
            Assert.IsFalse(update);

            dummy.Item = "bar";

            Assert.IsFalse(update);
            Assert.AreEqual("bar", test.Value.Value);
        }

        private class ListFake : IEnumerable
        {
            public string Value { get; set; }

            public void Add(string value)
            {
                Value += value;
            }

            public bool Remove(string value)
            {
                var index = Value.IndexOf(value);
                if (index < 0)
                {
                    return false;
                }
                string first = index > 0 ? Value.Substring(0, index) : string.Empty;
                string last = index + value.Length < Value.Length - 1 ? Value.Substring(index + value.Length) : string.Empty;
                Value = first + last;
                return true;
            }

            public IEnumerator GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
