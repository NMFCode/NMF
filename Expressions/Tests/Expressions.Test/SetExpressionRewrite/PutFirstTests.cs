using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ex = NMF.Expressions.Linq.ExpressionExtensions;

namespace NMF.Expressions.Test.SetExpressionRewrite
{
    [TestClass]
    public class PutFirstTests
    {
        [TestMethod]
        public void PutFirstList_AddsNotExisting()
        {
            var list = new List<int>();
            Ex.PutFirst(list, 42);
            Assert.AreEqual(42, list.Single());
        }

        [TestMethod]
        public void PutFirstList_DoesNothingIfAlreadyFirst()
        {
            var list = new List<int>() { 42, 23 };
            Ex.PutFirst(list, 42);
            Assert.AreEqual(42, list[0]);
            Assert.AreEqual(23, list[1]);
        }

        [TestMethod]
        public void PutFirstList_SwapsExisting()
        {
            var list = new List<int>() { 23, 42 };
            Ex.PutFirst(list, 42);
            Assert.AreEqual(42, list[0]);
            Assert.AreEqual(23, list[1]);
        }

        [TestMethod]
        public void PutFirstList_ClearsIfNull()
        {
            var list = new List<string>() { "Foo", "Bar" };
            Ex.PutFirst(list, null);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void PutFirstCollection_AddsNotExisting()
        {
            var list = new TestCollection();
            Ex.PutFirst(list, "42");
            Assert.AreEqual("42", list.Single());
        }

        [TestMethod]
        public void PutFirstCollection_DoesNothingIfAlreadyFirst()
        {
            var list = new TestCollection() { "42", "23" };
            Ex.PutFirst(list, "42");
            Assert.AreEqual("42", list[0]);
            Assert.AreEqual("23", list[1]);
        }

        [TestMethod]
        public void PutFirstCollection_DeletesExisting()
        {
            var list = new TestCollection() { "23", "42" };
            Ex.PutFirst(list, "42");
            Assert.AreEqual("42", list[0]);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void PutFirstCollection_ClearsIfNull()
        {
            var list = new TestCollection() { "Foo", "Bar" };
            Ex.PutFirst(list, null);
            Assert.AreEqual(0, list.Count);
        }

        private class TestCollection : ICollection<string>
        {
            private readonly List<string> _list = new List<string>();

            public int Count => ((ICollection<string>)_list).Count;

            public bool IsReadOnly => ((ICollection<string>)_list).IsReadOnly;

            public void Add(string item)
            {
                ((ICollection<string>)_list).Add(item);
            }

            public void Clear()
            {
                ((ICollection<string>)_list).Clear();
            }

            public bool Contains(string item)
            {
                return ((ICollection<string>)_list).Contains(item);
            }

            public void CopyTo(string[] array, int arrayIndex)
            {
                ((ICollection<string>)_list).CopyTo(array, arrayIndex);
            }

            public IEnumerator<string> GetEnumerator()
            {
                return ((IEnumerable<string>)_list).GetEnumerator();
            }

            public string this[int index] => _list[index];

            public bool Remove(string item)
            {
                return ((ICollection<string>)_list).Remove(item);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return ((IEnumerable)_list).GetEnumerator();
            }
        }
    }
}
