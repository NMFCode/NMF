using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;

using NMF.Tests;
using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;
using System.Collections.Generic;

namespace NMF.Collections.Test
{
    public abstract class ListTest<T> : CollectionTest<T>
    {
        public IList<T> WriteList { get; private set; }
        public IList<T> ReadList { get; set; }

        protected virtual IList<T> CreateReadList() { return WriteList; }
        protected abstract IList<T> CreateWriteList();

        [TestInitialize]
        public override void Initialize()
        {
            WriteList = CreateWriteList();
            ReadList = CreateReadList();

            base.Initialize();
        }

        protected override ICollection<T> CreateReadCollection()
        {
            return ReadList;
        }

        protected override ICollection<T> CreateWriteCollection()
        {
            return WriteList;
        }

        [TestMethod]
        public void Collections_List_Insert()
        {
            ReadList.AssertEmpty();

            var item1 = CreateItem1();
            var item2 = CreateItem2();

            WriteList.Add(item1);
            WriteList.Insert(0, item2);

            ReadList.AssertSequence(item2, item1);
        }

        [TestMethod]
        public void Collections_List_IndexOf()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();

            WriteList.Add(item1);
            WriteList.Add(item2);

            Assert.AreEqual(0, ReadList.IndexOf(item1));
            Assert.AreEqual(1, ReadList.IndexOf(item2));
            Assert.AreEqual(-1, ReadList.IndexOf(item3));
        }

        [TestMethod]
        public void Collections_List_SetItem()
        {

            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();

            WriteList.Add(item1);
            WriteList.Add(item2);

            WriteList[1] = item3;

            Assert.AreEqual(item1, ReadList[0]);
            Assert.AreEqual(item3, ReadList[1]);
            Assert.IsFalse(ReadList.Contains(item2));
            ReadList.AssertSequence(item1, item3);
        }

        [TestMethod]
        public void Collections_List_Order()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();

            WriteList.Add(item1);
            WriteList.Add(item2);

            Assert.AreEqual(item1, ReadList[0]);
            Assert.AreEqual(item2, ReadList[1]);

            ReadList.AssertSequence(item1, item2);
        }
    }
    
    public abstract class StringListTest : ListTest<string>
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
    }

    public abstract class IListTest
    {
        private IList collection;

        protected abstract IList CreateCollection();
        protected abstract bool AllowDuplicateEntries { get; }

        protected abstract object CreateItem1();
        protected abstract object CreateItem2();
        protected abstract object CreateItem3();

        [TestInitialize]
        public void Initialize()
        {
            collection = CreateCollection();
        }

        [TestMethod]
        public void Collections_IList_Add()
        {
            collection.AssertEmpty();

            var item = CreateItem1();

            collection.Add(item);

            collection.AssertContainsOnly(item);
            Assert.AreEqual(item, collection[0]);

            collection.Add(item);

            if (AllowDuplicateEntries)
            {
                collection.AssertSequence(item, item);
                Assert.AreEqual(item, collection[1]);
            }
            else
            {
                collection.AssertContainsOnly(item);
            }

            collection.Remove(item);

            if (AllowDuplicateEntries)
            {
                collection.AssertContainsOnly(item);
            }
            else
            {
                collection.AssertEmpty();
            }
        }


        [TestMethod]
        public void Collections_IList_Insert()
        {
            collection.AssertEmpty();

            var item1 = CreateItem1();
            var item2 = CreateItem2();

            collection.Add(item1);
            collection.Insert(0, item2);

            collection.AssertSequence(item2, item1);
        }

        [TestMethod]
        public void Collections_IList_Remove()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();

            collection.Add(item1);
            collection.Add(item2);
            collection.Add(item3);

            collection.Remove(item2);

            Assert.AreEqual(item1, collection[0]);
            Assert.AreEqual(item3, collection[1]);
            collection.AssertSequence(item1, item3);
        }

        [TestMethod]
        public void Collections_IList_IndexOf()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();

            collection.Add(item1);
            collection.Add(item2);

            Assert.AreEqual(0, collection.IndexOf(item1));
            Assert.AreEqual(1, collection.IndexOf(item2));
            Assert.AreEqual(-1, collection.IndexOf(item3));
        }

        [TestMethod]
        public void Collections_IList_SetItem()
        {
            
            var item1 = CreateItem1();
            var item2 = CreateItem2();
            var item3 = CreateItem3();

            collection.Add(item1);
            collection.Add(item2);

            collection[1] = item3;

            Assert.AreEqual(item1, collection[0]);
            Assert.AreEqual(item3, collection[1]);
            Assert.IsFalse(collection.Contains(item2));
            collection.AssertSequence(item1, item3);
        }

        [TestMethod]
        public void Collections_IList_Order()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();

            collection.Add(item1);
            collection.Add(item2);

            Assert.AreEqual(item1, collection[0]);
            Assert.AreEqual(item2, collection[1]);

            collection.AssertSequence(item1, item2);
        }

        [TestMethod]
        public void Collections_IList_Clear()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();

            collection.Add(item1);
            collection.Add(item2);

            collection.Clear();

            collection.AssertEmpty();
        }

        [TestMethod]
        public void Collections_IList_CopyTo()
        {
            collection.Add(CreateItem1());
            collection.Add(CreateItem2());
            collection.Add(CreateItem3());
            collection.Add(CreateItem2());

            object[] array = new object[collection.Count];
            collection.CopyTo(array, 0);
            collection.AssertSequence(array);
        }
    }

    public abstract class IListStringTest : IListTest
    {
        protected override object CreateItem1()
        {
            return "a";
        }

        protected override object CreateItem2()
        {
            return "b";
        }

        protected override object CreateItem3()
        {
            return "c";
        }
    }



    [TestClass]
    public class TestOrderedSetCollection_IList : IListStringTest
    {
        protected override IList CreateCollection()
        {
            return new OrderedSet<string>();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }
    }

    [TestClass]
    public class TestOrderedSetCollection_List : StringListTest
    {
        protected override bool AllowDuplicateEntries => false;

        protected override bool PreservesOrder => true;

        protected override IList<string> CreateWriteList()
        {
            return new OrderedSet<string>();
        }
    }

    [TestClass]
    public class TestObservableOrderedSet_IList : IListStringTest
    {
        protected override IList CreateCollection()
        {
            return new ObservableOrderedSet<string>();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }
    }

    [TestClass]
    public class TestOppositeList_IList : IListStringTest
    {

        protected override IList CreateCollection()
        {
            return new TestOppositeList();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return true; }
        }
    }

    [TestClass]
    public class TestObservableOppositeList_IList : IListStringTest
    {
        protected override IList CreateCollection()
        {
            return new TestObservableOppositeList();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return true; }
        }
    }

    [TestClass]
    public class TestOppositeOrderedSet_IList : IListStringTest
    {
        protected override IList CreateCollection()
        {
            return new TestOppositeOrderedSet();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }
    }

    [TestClass]
    public class TestObservableOppositeOrderedSet_IList : IListStringTest
    {
        protected override IList CreateCollection()
        {
            return new TestObservableOppositeOrderedSet();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }
    }
}
