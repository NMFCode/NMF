using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using NMF.Tests;
using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;

namespace NMF.Collections.Test
{
    public abstract class SetTest<T>
    {
        public ISet<T> ReadSet { get; set; }
        public ISet<T> WriteSet { get; set; }

        protected abstract ISet<T> CreateWriteSet();
        protected virtual ISet<T> CreateReadSet() { return WriteSet; }

        protected abstract T CreateItem1();
        protected abstract T CreateItem2();
        protected abstract T CreateItem3();
        protected abstract T CreateItem4();

        [TestInitialize]
        public void Initialize()
        {
            WriteSet = CreateWriteSet();
            ReadSet = CreateReadSet();
        }

        [TestMethod]
        public void Collections_Set_Add()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();

            Assert.IsTrue(WriteSet.Add(item1));
            Assert.IsTrue(WriteSet.Add(item2));
            Assert.IsFalse(WriteSet.Add(item1));

            ReadSet.AssertContainsOnly(item1, item2);
        }

        [TestMethod]
        public void Collections_Set_Remove()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();

            WriteSet.Add(item1);

            Assert.IsFalse(WriteSet.Remove(item2));
            Assert.IsTrue(WriteSet.Remove(item1));

            ReadSet.AssertEmpty();
        }

        [TestMethod]
        public void Collections_Set_ExceptWith()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();

            var list = new List<T>() { item2, item3 };

            WriteSet.Add(item1);
            WriteSet.Add(item2);

            WriteSet.ExceptWith(list);

            ReadSet.AssertContainsOnly(item1);

            WriteSet.ExceptWith(WriteSet);

            ReadSet.AssertEmpty();
        }

        [TestMethod]
        public void Collections_Set_IntersectWith()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();

            var list = new List<T>() { item2, item3 };

            WriteSet.Add(item1);
            WriteSet.Add(item2);

            WriteSet.IntersectWith(list);

            ReadSet.AssertContainsOnly(item2);

            WriteSet.IntersectWith(ReadSet);

            ReadSet.AssertContainsOnly(item2);
        }


        [TestMethod]
        public void Collections_Set_UnionWith()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();

            var list = new List<T>() { item2, item3 };

            WriteSet.Add(item1);
            WriteSet.Add(item2);

            WriteSet.UnionWith(list);

            ReadSet.AssertContainsOnly(item1, item2, item3);

            WriteSet.UnionWith(ReadSet);

            ReadSet.AssertContainsOnly(item1, item2, item3);
        }

        [TestMethod]
        public void Collections_Set_SymmetricExceptWith()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();

            var list = new List<T>() { item2, item3 };

            WriteSet.Add(item1);
            WriteSet.Add(item2);

            WriteSet.SymmetricExceptWith(list);

            ReadSet.AssertContainsOnly(item1, item3);

            WriteSet.SymmetricExceptWith(WriteSet);

            ReadSet.AssertEmpty();
        }

        [TestMethod]
        public void Collections_Set_IsProperSubsetOf()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();
            var item4 = CreateItem4();

            var list = new List<T>() { item1, item2, item3 };

            Assert.IsTrue(ReadSet.IsProperSubsetOf(list));
            WriteSet.Add(item1);
            WriteSet.Add(item2);
            Assert.IsTrue(ReadSet.IsProperSubsetOf(list));
            WriteSet.Add(item3);
            Assert.IsFalse(ReadSet.IsProperSubsetOf(list));
            WriteSet.Remove(item3);
            WriteSet.Add(item4);
            Assert.IsFalse(ReadSet.IsProperSubsetOf(list));
            WriteSet.Clear();
            WriteSet.Add(item4);
            Assert.IsFalse(ReadSet.IsProperSubsetOf(list));
            Assert.IsFalse(ReadSet.IsProperSubsetOf(ReadSet));
        }

        [TestMethod]
        public void Collections_Set_IsSubsetOf()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();
            var item4 = CreateItem4();

            var list = new List<T>() { item1, item2, item3 };

            Assert.IsTrue(ReadSet.IsSubsetOf(list));
            WriteSet.Add(item1);
            WriteSet.Add(item2);
            Assert.IsTrue(ReadSet.IsSubsetOf(list));
            WriteSet.Add(item3);
            Assert.IsTrue(ReadSet.IsSubsetOf(list));
            WriteSet.Remove(item3);
            WriteSet.Add(item4);
            Assert.IsFalse(ReadSet.IsSubsetOf(list));
            WriteSet.Clear();
            WriteSet.Add(item4);
            Assert.IsFalse(ReadSet.IsSubsetOf(list));
            Assert.IsTrue(ReadSet.IsSubsetOf(ReadSet));
        }

        [TestMethod]
        public void Collections_Set_IsProperSupersetOf()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();
            var item4 = CreateItem4();

            var list = new List<T>() { item1, item2 };

            Assert.IsFalse(ReadSet.IsProperSupersetOf(list));
            WriteSet.Add(item1);
            WriteSet.Add(item2);
            Assert.IsFalse(ReadSet.IsProperSupersetOf(list));
            WriteSet.Add(item3);
            Assert.IsTrue(ReadSet.IsProperSupersetOf(list));
            WriteSet.Remove(item2);
            WriteSet.Add(item4);
            Assert.IsFalse(ReadSet.IsProperSupersetOf(list));
            WriteSet.Clear();
            WriteSet.Add(item4);
            Assert.IsFalse(ReadSet.IsProperSupersetOf(list));
            Assert.IsFalse(ReadSet.IsProperSupersetOf(ReadSet));
        }

        [TestMethod]
        public void Collections_Set_IsSupersetOf()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();
            var item4 = CreateItem4();

            var list = new List<T>() { item1, item2 };

            Assert.IsFalse(ReadSet.IsSupersetOf(list));
            WriteSet.Add(item1);
            WriteSet.Add(item2);
            Assert.IsTrue(ReadSet.IsSupersetOf(list));
            WriteSet.Add(item3);
            Assert.IsTrue(ReadSet.IsSupersetOf(list));
            WriteSet.Remove(item2);
            WriteSet.Add(item4);
            Assert.IsFalse(ReadSet.IsSupersetOf(list));
            WriteSet.Clear();
            WriteSet.Add(item4);
            Assert.IsFalse(ReadSet.IsSupersetOf(list));
            Assert.IsTrue(ReadSet.IsSupersetOf(ReadSet));
        }

        [TestMethod]
        public void Collections_Set_Overlaps()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();
            var item4 = CreateItem4();

            var list = new List<T>() { item1, item2 };

            Assert.IsFalse(ReadSet.Overlaps(list));
            WriteSet.Add(item1);
            WriteSet.Add(item2);
            Assert.IsTrue(ReadSet.Overlaps(list));
            WriteSet.Add(item3);
            Assert.IsTrue(ReadSet.Overlaps(list));
            WriteSet.Remove(item2);
            WriteSet.Add(item4);
            Assert.IsTrue(ReadSet.Overlaps(list));
            WriteSet.Clear();
            WriteSet.Add(item4);
            Assert.IsFalse(ReadSet.Overlaps(list));
            Assert.IsTrue(ReadSet.Overlaps(ReadSet));
        }

        [TestMethod]
        public void Collections_Set_SetEquals()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();
            var item4 = CreateItem4();

            var list = new List<T>() { item1, item2 };

            Assert.IsFalse(ReadSet.SetEquals(list));
            WriteSet.Add(item1);
            WriteSet.Add(item2);
            Assert.IsTrue(ReadSet.SetEquals(list));
            WriteSet.Add(item3);
            Assert.IsFalse(ReadSet.SetEquals(list));
            WriteSet.Remove(item2);
            WriteSet.Add(item4);
            Assert.IsFalse(ReadSet.SetEquals(list));
            WriteSet.Clear();
            WriteSet.Add(item4);
            Assert.IsFalse(ReadSet.SetEquals(list));
            Assert.IsTrue(ReadSet.SetEquals(ReadSet));
        }
    }

    public abstract class StringSetTest : SetTest<string>
    {
        protected override string CreateItem1()
        {
            return "a";
        }

        protected override string CreateItem2()
        {
            return "b";
        }

        protected override string CreateItem3()
        {
            return "c";
        }

        protected override string CreateItem4()
        {
            return "d";
        }
    }

    [TestClass]
    public class TestOrderedSet_Set : StringSetTest
    {
        protected override ISet<string> CreateWriteSet()
        {
            return new OrderedSet<string>();
        }
    }

    [TestClass]
    public class TestObservableOrderedSet_Set : StringSetTest
    {
        protected override ISet<string> CreateWriteSet()
        {
            return new ObservableOrderedSet<string>();
        }
    }

    [TestClass]
    public class TestObservableSet_Set : StringSetTest
    {
        protected override ISet<string> CreateWriteSet()
        {
            return new ObservableSet<string>();
        }
    }

    [TestClass]
    public class TestReadOnlyOrderedSet_Set : StringSetTest
    {
        protected override ISet<string> CreateWriteSet()
        {
            return new OrderedSet<string>();
        }

        protected override ISet<string> CreateReadSet()
        {
            return ((OrderedSet<string>)WriteSet).AsReadOnly();
        }
    }

    [TestClass]
    public class TestObservableReadOnlyOrderedSet_Set : StringSetTest
    {
        protected override ISet<string> CreateWriteSet()
        {
            return new ObservableOrderedSet<string>();
        }

        protected override ISet<string> CreateReadSet()
        {
            return new ObservableReadOnlyOrderedSet<string>(WriteSet as ObservableOrderedSet<string>);
        }
    }

    [TestClass]
    public class TestOppositeSet_Set : StringSetTest
    {

        protected override ISet<string> CreateWriteSet()
        {
            return new TestOppositeSet();
        }
    }

    [TestClass]
    public class TestOppositeOrderedSet_Set : StringSetTest
    {
        protected override ISet<string> CreateWriteSet()
        {
            return new TestOppositeOrderedSet();
        }
    }

    [TestClass]
    public class TestObservableOppositeSet_Set : StringSetTest
    {

        protected override ISet<string> CreateWriteSet()
        {
            return new TestObservableOppositeSet();
        }
    }

    [TestClass]
    public class TestObservableOppositeOrderedSet_Set : StringSetTest
    {
        protected override ISet<string> CreateWriteSet()
        {
            return new TestObservableOppositeOrderedSet();
        }
    }

}
