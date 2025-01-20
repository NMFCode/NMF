using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

using NMF.Utilities;
using System.Collections;

namespace NMF.Tests
{

    public static class AssertExtensions
    {
        public static void AssertSequence<T>(this IEnumerable<T> collection, params T[] items)
        {
            if (items != null)
            {
                var en = collection.GetEnumerator();
                for (int i = 0; i < items.Length; i++)
                {
                    Assert.IsTrue(en.MoveNext());
                    Assert.AreEqual(items[i], en.Current);
                }
                Assert.IsFalse(en.MoveNext());
                var en2 = ((IEnumerable)collection).GetEnumerator();
                for (int i = 0; i < items.Length; i++)
                {
                    Assert.IsTrue(en2.MoveNext());
                    Assert.AreEqual(items[i], en2.Current);
                }
                Assert.IsFalse(en2.MoveNext());
            }
        }

        public static void AssertSequence(this IEnumerable collection, params object[] items)
        {
            if (items != null)
            {
                var en = collection.GetEnumerator();
                for (int i = 0; i < items.Length; i++)
                {
                    Assert.IsTrue(en.MoveNext());
                    Assert.AreEqual(items[i], en.Current);
                }
                Assert.IsFalse(en.MoveNext());
            }
        }

        public static void AssertContainsOnly<T>(this IEnumerable<T> collection, params T[] items)
        {
            if (items != null)
            {
                Assert.AreEqual(items.Length, collection.Count());
                foreach (var item in items)
                {
                    Assert.IsTrue(collection.Contains(item));
                }

                foreach (T item in (IEnumerable)items)
                {
                    Assert.IsTrue(collection.Contains(item));
                }
            }
        }


        public static void AssertContainsOnly(this IEnumerable collection, params object[] items)
        {
            if (items != null)
            {
                var counter = 0;
                foreach (var item in items)
                {
                    Assert.IsTrue(Contains(collection, item));
                    counter++;
                }
                Assert.AreEqual(items.Length, counter);
            }
        }

        private static bool Contains(IEnumerable collection, object item)
        {
            var en = collection.GetEnumerator();
            while (en.MoveNext())
            {
                if (en.Current == item) return true;
            }
            return false;
        }

        public static void AssertEmpty<T>(this IEnumerable<T> collection)
        {
            Assert.IsTrue(collection.IsNullOrEmpty());
            if (collection == null) return;
            var en = ((IEnumerable)collection).GetEnumerator();
            Assert.IsFalse(en.MoveNext());
        }

        public static void AssertEmpty(this IEnumerable collection)
        {
            if (collection == null) return;
            var en = collection.GetEnumerator();
            Assert.IsFalse(en.MoveNext());
        }

        public static void AssertNull<T>(this T item)
            where T : class
        {
            Assert.IsNull(item);
        }

        public static void AssertNotNull<T>(this T item)
            where T : class
        {
            Assert.IsNotNull(item);
        }
    }
}
