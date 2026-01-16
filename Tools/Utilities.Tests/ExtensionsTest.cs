using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Utilities.Tests
{
    [TestClass]
    public class ExtensionsTest
    {

        [TestMethod]
        public void Utilities_Extensions_AddRange1()
        {
            var list = new List<string>();
            list.AddRange<string>(null);
            Assert.AreEqual(0, list.Count);
            list.AddRange<string>(new string[] { "a", "b", "c" });
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("a", list[0]);
            Assert.AreEqual("b", list[1]);
            Assert.AreEqual("c", list[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Utilities_Extensions_AddRange2()
        {
            ICollection<string> coll = null;
            coll.AddRange(null);
        }

        [TestMethod]
        public void Utilities_Extensions_RemoveRange1()
        {
            var list = new List<string>() { "a", "b", "c", "d", "e", "f" };
            list.RemoveRange(null);
            Assert.AreEqual(6, list.Count);
            list.RemoveRange(new string[] { "d", "b", "c" });
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("a", list[0]);
            Assert.AreEqual("e", list[1]);
            Assert.AreEqual("f", list[2]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Utilities_Extensions_RemoveRange2()
        {
            ICollection<string> coll = null;
            coll.RemoveRange(null);
        }

        [TestMethod]
        public void Utilities_Extensions_RemoveRange3()
        {
            var list = new List<string>() { "a", "b", "c", "d", "e", "f" };
            list.RemoveRange(list);
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void Utilities_Extensions_AddRange4()
        {
            var list = new HashSet<string>() { "a", "b", "c", "d", "e", "f" };
            list.RemoveRange(null);
            Assert.AreEqual(6, list.Count);
            list.RemoveRange(new string[] { "d", "b", "c" });
            Assert.AreEqual(3, list.Count);
            Assert.IsTrue(list.Contains("a"));
            Assert.IsTrue(list.Contains("e"));
            Assert.IsTrue(list.Contains("f"));
        }

        [TestMethod]
        public void Utilities_Extensions_PairWithConstant1()
        {
            var list = new List<string>() { "a", "b", "c" };
            var en = list.PairWithConstant(42).GetEnumerator();
            Assert.IsTrue(en.MoveNext());
            Assert.IsNotNull(en.Current);
            Assert.AreEqual("a", en.Current.Item1);
            Assert.AreEqual(42, en.Current.Item2);
            Assert.IsTrue(en.MoveNext());
            Assert.IsNotNull(en.Current);
            Assert.AreEqual("b", en.Current.Item1);
            Assert.AreEqual(42, en.Current.Item2);
            Assert.IsTrue(en.MoveNext());
            Assert.IsNotNull(en.Current);
            Assert.AreEqual("c", en.Current.Item1);
            Assert.AreEqual(42, en.Current.Item2);
            Assert.IsFalse(en.MoveNext());
        }

        [TestMethod]
        public void Utilities_Extensions_PairWithConstant2()
        {
            IEnumerable<string> collection = null;
            var en = collection.PairWithConstant(42).GetEnumerator();
            Assert.IsFalse(en.MoveNext());
        }

        [TestMethod]
        public void Utilities_Extensions_PairWithConstant()
        {
            var par1 = new object();
            var par2 = new object();

            var tuple = par1.PairWith(par2);

            Assert.AreEqual(par1, tuple.Item1);
            Assert.AreEqual(par2, tuple.Item2);
        }

        [TestMethod]
        public void Utilities_Extensions_IsInstanceArrayOfType1()
        {
            Type[] types = { typeof(string), typeof(ExtensionsTest), typeof(int) };
            object[] objects = { "a", this, 42 };

            Assert.IsTrue(types.IsInstanceArrayOfType(objects));
            objects[0] = "b";
            Assert.IsTrue(types.IsInstanceArrayOfType(objects));
            objects[1] = 42.0;
            Assert.IsFalse(types.IsInstanceArrayOfType(objects));
            objects[1] = null;
            Assert.IsFalse(types.IsInstanceArrayOfType(objects));

            Assert.IsFalse(types.IsInstanceArrayOfType(null));
            Assert.IsFalse(types.IsInstanceArrayOfType(new object[] { "a", this }));
            Assert.IsTrue(Type.EmptyTypes.IsInstanceArrayOfType(new object[] { }));

            Type[] nullTypes = null;
            Assert.IsFalse(nullTypes.IsInstanceArrayOfType(null));
            Assert.IsFalse(nullTypes.IsInstanceArrayOfType(objects));
        }

        [TestMethod]
        public void Utilities_Extensions_IsInstanceArrayOfType2()
        {
            Type[] types = { typeof(string), typeof(ExtensionsTest), typeof(int) };
            object[] objects = { "a", this, 42 };

            Assert.IsTrue(types.IsInstanceArrayOfType(objects, false));
            objects[0] = "b";
            Assert.IsTrue(types.IsInstanceArrayOfType(objects, false));
            objects[1] = 42.0;
            Assert.IsFalse(types.IsInstanceArrayOfType(objects, false));
            objects[1] = null;
            Assert.IsFalse(types.IsInstanceArrayOfType(objects, false));

            Assert.IsFalse(types.IsInstanceArrayOfType(null, false));
            Assert.IsFalse(types.IsInstanceArrayOfType(new object[] { "a", this }, false));
            Assert.IsTrue(Type.EmptyTypes.IsInstanceArrayOfType(new object[] { }, false));

            Type[] nullTypes = null;
            Assert.IsFalse(nullTypes.IsInstanceArrayOfType(null, false));
            Assert.IsFalse(nullTypes.IsInstanceArrayOfType(objects, false));
        }

        [TestMethod]
        public void Utilities_Extensions_IsInstanceArrayOfType3()
        {
            Type[] types = { typeof(string), typeof(ExtensionsTest), typeof(int) };
            object[] objects = { "a", this, 42 };

            Assert.IsTrue(types.IsInstanceArrayOfType(objects, true));
            objects[0] = "b";
            Assert.IsTrue(types.IsInstanceArrayOfType(objects, true));
            objects[1] = 42.0;
            Assert.IsFalse(types.IsInstanceArrayOfType(objects, true));
            objects[1] = null;
            Assert.IsTrue(types.IsInstanceArrayOfType(objects, true));

            Assert.IsFalse(types.IsInstanceArrayOfType(null, true));
            Assert.IsFalse(types.IsInstanceArrayOfType(new object[] { "a", this }, true));
            Assert.IsTrue(Type.EmptyTypes.IsInstanceArrayOfType(new object[] { }, true));

            Type[] nullTypes = null;
            Assert.IsFalse(nullTypes.IsInstanceArrayOfType(null, true));
            Assert.IsFalse(nullTypes.IsInstanceArrayOfType(objects, true));
        }


        [TestMethod]
        public void Utilities_Extensions_IsAssignableArrayFrom()
        {
            Type[] types = { typeof(object), typeof(ExtensionsTest), typeof(int) };
            Type[] objects = { typeof(string), typeof(ExtensionsTest), typeof(int) };

            Assert.IsTrue(types.IsAssignableArrayFrom(objects));
            objects[0] = typeof(StringComparer);
            Assert.IsTrue(types.IsAssignableArrayFrom(objects));
            objects[2] = typeof(double);
            Assert.IsFalse(types.IsAssignableArrayFrom(objects));
            objects[2] = null;
            Assert.IsFalse(types.IsAssignableArrayFrom(objects));

            Assert.IsFalse(types.IsAssignableArrayFrom(null));
            Assert.IsFalse(types.IsAssignableArrayFrom(new Type[] { typeof(string), typeof(ExtensionsTest) }));
            Assert.IsTrue(Type.EmptyTypes.IsAssignableArrayFrom(Type.EmptyTypes));

            Type[] nullTypes = null;
            Assert.IsFalse(nullTypes.IsAssignableArrayFrom(null));
            Assert.IsFalse(nullTypes.IsAssignableArrayFrom(objects));
        }

        [TestMethod]
        public void Utilities_Extensions_ArrayEquals()
        {
            char[] array1 = "Hello World".ToCharArray();
            char[] array2 = "Hallo Welt!".ToCharArray();

            Assert.IsTrue(array1.ArrayEquals(array1));
            Assert.IsTrue(array2.ArrayEquals(array2));
            Assert.IsFalse(array1.ArrayEquals(array2));
            Assert.IsTrue(array1.ArrayEquals("Hello World".ToCharArray()));
            Assert.IsFalse(array1.ArrayEquals("Hello World!".ToCharArray()));
            Assert.IsFalse(array1.ArrayEquals(null));

            char[] nullArray = null;
            Assert.IsFalse(nullArray.ArrayEquals(array1));
        }

        [TestMethod]
        public void Utilities_Extensions_GetTypes()
        {
            object[] objects = { "a", new ExtensionsTest(), 42 , null};

            var types = objects.GetTypes();

            Assert.AreEqual(4, types.Length);
            Assert.AreEqual(typeof(string), types[0]);
            Assert.AreEqual(typeof(ExtensionsTest), types[1]);
            Assert.AreEqual(typeof(int), types[2]);
            Assert.IsNull(types[3]);

            object[] nulls = null;

            types = nulls.GetTypes();
            Assert.IsNull(types);
        }

        [TestMethod]
        public void Utilities_Extensions_IsNullOrEmpty()
        {
            var list = new List<string>();
            var enumerable = list.Select(s => s.Length);
            Assert.IsTrue(Extensions.IsNullOrEmpty<string>(null));
            Assert.IsTrue(list.IsNullOrEmpty());
            Assert.IsTrue(enumerable.IsNullOrEmpty());
            list.Add("a");
            Assert.IsFalse(list.IsNullOrEmpty());
            Assert.IsFalse(enumerable.IsNullOrEmpty());
        }

        [TestMethod]
        public void Utilities_Extensions_ToCamelCase()
        {
            Assert.AreEqual(null, (null as string).ToCamelCase());
            Assert.AreEqual("", "".ToCamelCase());
            Assert.AreEqual("helloWorld", "Hello World!".ToCamelCase());
            Assert.AreEqual("parameter", "parameter".ToCamelCase());
            Assert.AreEqual("isUnique", "Is unique?".ToCamelCase());
            Assert.AreEqual("anYway", "AnYway".ToCamelCase());
            Assert.AreEqual("_hello", "_Hello".ToCamelCase());
            Assert.AreEqual("nMF", "NMF".ToCamelCase());
            Assert.AreEqual("_setMag", "_setMag".ToCamelCase());
        }

        [TestMethod]
        public void Utilities_Extensions_ToPascalCase()
        {
            Assert.AreEqual(null, (null as string).ToPascalCase());
            Assert.AreEqual("", "".ToPascalCase());
            Assert.AreEqual("HelloWorld", "Hello World!".ToPascalCase());
            Assert.AreEqual("Parameter", "parameter".ToPascalCase());
            Assert.AreEqual("IsUnique", "Is unique?".ToPascalCase());
            Assert.AreEqual("AnYway", "AnYway".ToPascalCase());
            Assert.AreEqual("_Hello", "_Hello".ToPascalCase());
            Assert.AreEqual("NMF", "NMF".ToPascalCase());
            Assert.AreEqual("_SetMag", "_setMag".ToPascalCase());
        }

        [TestMethod]
        public void Utilities_Extensions_ToSupersetToSubset()
        {
            var list1 = new List<string>();
            var list2 = new List<string>();
            var set1 = new HashSet<string>();
            var set2 = new HashSet<string>();

            Assert.IsTrue(list1.IsSubsetOf(list2));
            Assert.IsTrue(set1.IsSubsetOf<string>(set2));
            Assert.IsTrue(list2.IsSupersetOf(list1));
            Assert.IsTrue(set2.IsSupersetOf<string>(set1));

            list2.Add("a");
            set2.Add("a");

            Assert.IsTrue(list1.IsSubsetOf(list2));
            Assert.IsTrue(set1.IsSubsetOf<string>(set2));
            Assert.IsTrue(list2.IsSupersetOf(list1));
            Assert.IsTrue(set2.IsSupersetOf<string>(set1));

            list1.Add("a"); list1.Add("b");
            set1.Add("a"); set1.Add("b");

            Assert.IsFalse(list1.IsSubsetOf(list2));
            Assert.IsFalse(set1.IsSubsetOf<string>(set2));
            Assert.IsFalse(list2.IsSupersetOf(list1));
            Assert.IsFalse(set2.IsSupersetOf<string>(set1));

            Assert.IsTrue(Extensions.IsSubsetOf(null, set1));
            Assert.IsFalse(Extensions.IsSupersetOf(null, set1));
            Assert.IsFalse(list1.IsSubsetOf(null));
            Assert.IsTrue(list1.IsSupersetOf(null));
        }

        [TestMethod]
        public void Utilities_Extensions_Except()
        {
            var list = new List<string>() { "a", "b", "c", "d" };

            AssertSequence(list.Except("a"), "b", "c", "d");
            AssertSequence(list.Except("b"), "a", "c", "d");
            AssertSequence(list.Except(null as string), "a", "b", "c", "d");
            AssertSequence(list.Except("e"), "a", "b", "c", "d");
            AssertSequence((null as IEnumerable<string>).Except("a"));
        }

        [TestMethod]
        public void Utilities_Extensions_IntersectsWith1()
        {
            var list1 = new List<string>() { "a", "b", "c" };
            var list2 = new List<string>() { "c", "d", "e" };
            var enumerable = list1 as IEnumerable<string>;

            Assert.IsTrue(list1.IntersectsWith(list2));
            Assert.IsFalse(list1.IntersectsWith(list2.Except("c")));
            Assert.IsTrue(enumerable.IntersectsWith(list2));
            Assert.IsFalse(enumerable.IntersectsWith(list2.Except("c")));
            Assert.IsFalse(list1.Except("c").IntersectsWith(list2));
            Assert.IsTrue(list1.Except("a").IntersectsWith(list2.Except("e")));

            list1.Remove("c");
            Assert.IsFalse(list1.IntersectsWith(list2));
            Assert.IsFalse(enumerable.IntersectsWith(list2));

            Assert.IsFalse(list1.IntersectsWith(null));
            Assert.IsFalse(enumerable.IntersectsWith(null));
            Assert.IsFalse(Extensions.IntersectsWith(null, list1));
            Assert.IsFalse(Extensions.IntersectsWith(null, enumerable));
        }

        [TestMethod]
        public void Utilities_Extensions_SetEquals()
        {
            var list = new List<string>() { "a", "b", "c", "d" };

            Assert.IsTrue(list.SetEquals(list));
            Assert.IsTrue(list.SetEquals(list.Where(s => true)));
            Assert.IsFalse(list.SetEquals(list.Except("a")));
            Assert.IsFalse(list.SetEquals(null));
            Assert.IsFalse(Extensions.SetEquals(null, list));
            Assert.IsFalse(list.SetEquals(new string[] { "a", "b", "c", "e" }));

            list.Clear();
            Assert.IsTrue(list.SetEquals(null));
            Assert.IsTrue(Extensions.SetEquals(null, list));
        }

        private void AssertSequence<T>(IEnumerable<T> underTest, params T[] expected)
        {
            var en = underTest.GetEnumerator();
            if (expected != null)
            {
                for (int i = 0; i < expected.Length; i++)
                {
                    Assert.IsTrue(en.MoveNext());
                    Assert.AreEqual(expected[i], en.Current);
                }
            }
            Assert.IsFalse(en.MoveNext());
        }
    }

    internal class State
    {
        public List<State> Successors { get; private set; }

        public State()
        {
            Successors = new List<State>();
        }

        public State Sucessor(State successor)
        {
            Successors.Add(successor);
            return this;
        }
    }
}