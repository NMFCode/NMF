using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;

using NMF.Tests;

namespace NMF.Collections.Test
{
    public abstract class CollectionTest<T>
    {
        protected ICollection<T> ReadCollection { get; private set; }
        protected ICollection<T> WriteCollection { get; private set; }

        protected abstract ICollection<T> CreateWriteCollection();
        protected abstract bool AllowDuplicateEntries { get; }
        protected abstract bool PreservesOrder { get; }

        protected virtual ICollection<T> CreateReadCollection()
        {
            return WriteCollection;
        }

        protected abstract T CreateItem1();
        protected abstract T CreateItem2();
        protected abstract T CreateItem3();

        [TestInitialize]
        public virtual void Initialize()
        {
            WriteCollection = CreateWriteCollection();
            ReadCollection = CreateReadCollection();
        }

        [TestMethod]
        public void Collections_Add()
        {
            ReadCollection.AssertEmpty();

            var item = CreateItem1();

            WriteCollection.Add(item);

            ReadCollection.AssertContainsOnly(item);

            WriteCollection.Add(item);

            if (AllowDuplicateEntries)
            {
                ReadCollection.AssertSequence(item, item);
            }
            else
            {
                ReadCollection.AssertContainsOnly(item);
            }

            WriteCollection.Remove(item);

            if (AllowDuplicateEntries)
            {
                ReadCollection.AssertContainsOnly(item);
            }
            else
            {
                ReadCollection.AssertEmpty();
            }
        }

        [TestMethod]
        public void Collections_Order()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();

            WriteCollection.Add(item1);
            WriteCollection.Add(item2);

            if (PreservesOrder)
            {
                ReadCollection.AssertSequence(item1, item2);
            }
            else
            {
                ReadCollection.AssertContainsOnly(item1, item2);
            }
        }

        [TestMethod]
        public void Collections_Clear()
        {
            var item1 = CreateItem1();
            var item2 = CreateItem2();

            WriteCollection.Add(item1);
            WriteCollection.Add(item2);

            WriteCollection.Clear();

            ReadCollection.AssertEmpty();
        }

        [TestMethod]
        public void Collections_CopyTo()
        {
            WriteCollection.Add(CreateItem1());
            WriteCollection.Add(CreateItem2());
            WriteCollection.Add(CreateItem3());
            WriteCollection.Add(CreateItem2());

            T[] array = new T[ReadCollection.Count];
            ReadCollection.CopyTo(array, 0);

            if (PreservesOrder)
            {
                ReadCollection.AssertSequence(array);
            }
            else
            {
                ReadCollection.AssertContainsOnly(array);
            }
        }
    }

    public abstract class StringCollectionTest : CollectionTest<string>
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


    [TestClass]
    public class TestOrderedSetCollection_ICollection : StringListTest
    {
        protected override IList<string> CreateWriteList()
        {
            return new OrderedSet<string>();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }

        protected override bool PreservesOrder
        {
            get { return true; }
        }
    }

    [TestClass]
    public class TestObservableOrderedSet_ICollection : StringListTest
    {
        protected override IList<string> CreateWriteList()
        {
            return new ObservableOrderedSet<string>();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }

        protected override bool PreservesOrder
        {
            get { return true; }
        }
    }

    [TestClass]
    public class TestObservableSet_ICollection : StringCollectionTest
    {
        protected override ICollection<string> CreateWriteCollection()
        {
            return new ObservableSet<string>();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }

        protected override bool PreservesOrder
        {
            get { return false; }
        }
    }

    [TestClass]
    public class TestObservableReadOnlyOrderedSet_ICollection : StringListTest
    {

        protected override IList<string> CreateWriteList()
        {
            return new ObservableOrderedSet<string>();
        }

        protected override IList<string> CreateReadList()
        {
            return ((ObservableOrderedSet<string>)WriteList).AsReadOnly();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }

        protected override bool PreservesOrder
        {
            get { return true; }
        }
    }

    [TestClass]
    public class TestReadonlyOrderedSet_ICollection : StringListTest
    {

        protected override IList<string> CreateWriteList()
        {
            return new OrderedSet<string>();
        }

        protected override IList<string> CreateReadList()
        {
            return ((OrderedSet<string>)WriteList).AsReadOnly();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }

        protected override bool PreservesOrder
        {
            get { return true; }
        }
    }

    [TestClass]
    public class TestReadOnlyListSelection_ICollection : StringListTest
    {

        protected override IList<string> CreateWriteList()
        {
            return new List<string>();
        }

        protected override IList<string> CreateReadList()
        {
            return new ReadOnlyListSelection<string, string>(WriteList as IList<string>, s => s);
        }

        protected override bool AllowDuplicateEntries
        {
            get { return true; }
        }

        protected override bool PreservesOrder
        {
            get { return true; }
        }
    }

    [TestClass]
    public class TestOppositeSet_ICollection : StringCollectionTest
    {

        protected override ICollection<string> CreateWriteCollection()
        {
            return new TestOppositeSet();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }

        protected override bool PreservesOrder
        {
            get { return false; }
        }
    }

    [TestClass]
    public class TestOppositeOrderedSet_ICollection : StringListTest
    {

        protected override IList<string> CreateWriteList()
        {
            return new TestOppositeOrderedSet();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }

        protected override bool PreservesOrder
        {
            get { return true; }
        }
    }

    [TestClass]
    public class TestOppositeList_ICollection : StringListTest
    {

        protected override IList<string> CreateWriteList()
        {
            return new TestOppositeList();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return true; }
        }

        protected override bool PreservesOrder
        {
            get { return true; }
        }
    }

    [TestClass]
    public class TestObservableOppositeSet_ICollection : StringCollectionTest
    {

        protected override ICollection<string> CreateWriteCollection()
        {
            return new TestObservableOppositeSet();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }

        protected override bool PreservesOrder
        {
            get { return false; }
        }
    }

    [TestClass]
    public class TestObservableOppositeOrderedSet_ICollection : StringListTest
    {

        protected override IList<string> CreateWriteList()
        {
            return new TestObservableOppositeOrderedSet();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return false; }
        }

        protected override bool PreservesOrder
        {
            get { return true; }
        }
    }

    [TestClass]
    public class TestObservableOppositeList_ICollection : StringListTest
    {

        protected override IList<string> CreateWriteList()
        {
            return new TestObservableOppositeList();
        }

        protected override bool AllowDuplicateEntries
        {
            get { return true; }
        }

        protected override bool PreservesOrder
        {
            get { return true; }
        }
    }
}
