using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

using NMF.Collections.Generic;
using NMF.Collections.ObjectModel;

namespace NMF.Collections.Test
{
    public abstract class ReadOnlyTest
    {
        private ICollection<string> collection;

        protected abstract ICollection<string> CreateCollection();

        [TestInitialize]
        public virtual void Initialize()
        {
            collection = CreateCollection();
        }

        [TestMethod]
        public void Collections_Readonly_IsReadonly()
        {
            Assert.IsTrue(collection.IsReadOnly);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Collections_Readonly_Add()
        {
            collection.Add("a");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Collections_Readonly_Remove()
        {
            collection.Remove("a");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Collections_Readonly_Clear()
        {
            collection.Clear();
        }
    }

    public abstract class ReadOnlySetTest : ReadOnlyTest
    {
        private ISet<string> set;

        protected sealed override ICollection<string> CreateCollection()
        {
            return set;
        }

        protected abstract ISet<string> CreateSet();

        [TestInitialize]
        public override void Initialize()
        {
            set = CreateSet();
            base.Initialize();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Collections_Readonly_IntersectWith()
        {
            set.IntersectWith(Enumerable.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Collections_Readonly_UnionWith()
        {
            set.UnionWith(Enumerable.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Collections_Readonly_ExceptWith()
        {
            set.ExceptWith(Enumerable.Empty<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Collections_Readonly_SymmetricExceptWith()
        {
            set.SymmetricExceptWith(Enumerable.Empty<string>());
        }
    }

    [TestClass]
    public class TestReadOnlyListSelection_ReadOnly : ReadOnlyTest
    {
        protected override ICollection<string> CreateCollection()
        {
            return new ReadOnlyListSelection<string, string>(new List<string>(), s => s);
        }
    }

    [TestClass]
    public class TestReadOnlyOrderedSet_ReadOnly : ReadOnlySetTest
    {

        protected override ISet<string> CreateSet()
        {
            return (new OrderedSet<string>()).AsReadOnly();
        }
    }

    [TestClass]
    public class TestObservableReadOnlyOrderedSet_ReadOnly : ReadOnlySetTest
    {

        protected override ISet<string> CreateSet()
        {
            return new ObservableReadOnlyOrderedSet<string>(new ObservableOrderedSet<string>());
        }
    }

}
