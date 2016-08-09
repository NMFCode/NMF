using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Utilities;
using System.Collections.Generic;
using System.Collections;

namespace NMF.Utilities.Tests
{
    [TestClass]
    public class ItemEqualityComparerTest
    {
        [TestMethod]
        public void ItemEqualityComparer_Equals1()
        {
            AssertEqual(CreateArray("a", "b"), CreateArray("a", "b"));
            AssertEqual(CreateArray("a"), CreateArray("a"));
            AssertEqual(CreateArray(1), CreateArray(1));
            AssertEqual(CreateArray(1, 2), CreateArray(1, 2));
        }

        [TestMethod]
        public void ItemEqualityComparer_Equals2()
        {
            AssertNotEqual(CreateArray("a"), CreateArray("a", "b"));
            AssertNotEqual(CreateArray("a", "b"), CreateArray("a", "b", "c"));
            AssertNotEqual(CreateArray(1, 2), CreateArray(1, 2, 3));
            AssertNotEqual(CreateArray(1), CreateArray(1, 2));
        }

        [TestMethod]
        public void ItemEqualityComparer_Equals3()
        {
            AssertNotEqual(CreateArray("a"), CreateArray("b"));
            AssertNotEqual(CreateArray(1), CreateArray(2));
            AssertNotEqual(CreateArray("a", "b"), CreateArray("b", "c"));
            AssertNotEqual(CreateArray(1, 2), CreateArray(2, 3));
        }

        [TestMethod]
        public void ItemEqualityComparer_Equals4()
        {
            AssertNotEqual(CreateArray("a"), null);
            AssertNotEqual(CreateArray(1), null);
            AssertEqual(null as object[], null);
        }

        private T[] CreateArray<T>(params T[] items)
        {
            return items;
        }

        private void AssertEqual<T>(T[] left, T[] right)
        {
            Assert.IsTrue(ItemEqualityComparer<T>.Instance.Equals(left, right));
            Assert.IsTrue(ItemEqualityComparer<T>.Instance.Equals(right, left));
            Assert.AreEqual(ItemEqualityComparer<T>.Instance.GetHashCode(left), ItemEqualityComparer<T>.Instance.GetHashCode(right));
        }

        private void AssertNotEqual<T>(T[] left, T[] right)
        {
            Assert.IsFalse(ItemEqualityComparer<T>.Instance.Equals(left, right));
            Assert.IsFalse(ItemEqualityComparer<T>.Instance.Equals(right, left));
        }
    }
}
