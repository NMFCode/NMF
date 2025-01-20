using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Tests.Debug;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NMF.Models.Tests
{
    [TestClass]
    public class AncestorTests
    {
        private readonly Test t1 = new Test();
        private readonly Test t11 = new Test();
        private readonly Test t12 = new Test();
        private readonly Test t111 = new Test();
        private readonly Test t112 = new Test();

        [TestInitialize]
        public void Setup()
        {
            t1.Invalids.Clear();
            t1.Invalids.Add(t11);
            t1.Invalids.Add(t12);
            t11.Invalids.Clear();
            t11.Invalids.Add(t111);
            t11.Invalids.Add(t112);
            t12.Invalids.Clear();
        }

        [TestMethod]
        public void Ancestors_NonIncremental_CorrectResults()
        {
            var expected = new List<IModelElement>();
            CollectionAssert.AreEquivalent(t1.Ancestors().ToList(), expected);
            expected.Add(t1);
            CollectionAssert.AreEquivalent(t11.Ancestors().ToList(), expected);
            CollectionAssert.AreEquivalent(t12.Ancestors().ToList(), expected);
            expected.Add(t11);
            CollectionAssert.AreEquivalent(t111.Ancestors().ToList(), expected);
            CollectionAssert.AreEquivalent(t112.Ancestors().ToList(), expected);
        }

        [TestMethod]
        public void Ancestors_IncrementalNoUpdate_CorrectResults()
        {
            var expected = new List<IModelElement>();
            CollectionAssert.AreEquivalent(t1.Ancestors().AsNotifiable().ToList(), expected);
            expected.Add(t1);
            CollectionAssert.AreEquivalent(t11.Ancestors().AsNotifiable().ToList(), expected);
            CollectionAssert.AreEquivalent(t12.Ancestors().AsNotifiable().ToList(), expected);
            expected.Add(t11);
            CollectionAssert.AreEquivalent(t111.Ancestors().AsNotifiable().ToList(), expected);
            CollectionAssert.AreEquivalent(t112.Ancestors().AsNotifiable().ToList(), expected);
        }

        [TestMethod]
        public void Ancestors_IncrementalMoveDown_CorrectResults()
        {
            var t1_ancestors = t1.Ancestors().AsNotifiable();
            var t11_ancestors = t11.Ancestors().AsNotifiable();
            var t12_ancestors = t12.Ancestors().AsNotifiable();
            var t111_ancestors = t111.Ancestors().AsNotifiable();
            var t112_ancestors = t112.Ancestors().AsNotifiable();

            var handlersCalled = 0;
            NotifyCollectionChangedEventHandler noChanges = (o, e) =>
            {
                throw new InvalidOperationException();
            };
            NotifyCollectionChangedEventHandler t12Added = (o, e) =>
            {
                var is_t11 = o == t11_ancestors;
                var is_t111 = o == t111_ancestors;
                var is_t112 = o == t112_ancestors;

                handlersCalled++;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewItems.Count);
                Assert.AreEqual(t12, e.NewItems[0]);
            };
            t1_ancestors.CollectionChanged += noChanges;
            t12_ancestors.CollectionChanged += noChanges;
            t11_ancestors.CollectionChanged += t12Added;
            t111_ancestors.CollectionChanged += t12Added;
            t112_ancestors.CollectionChanged += t12Added;

            Assert.AreEqual(0, handlersCalled);

            t12.Invalids.Add(t11);

            Assert.AreEqual(3, handlersCalled);

            var expected = new List<IModelElement>() { t1, t12 };
            CollectionAssert.AreEquivalent(t11_ancestors.ToList(), expected);
            expected.Add(t11);
            CollectionAssert.AreEquivalent(t111_ancestors.ToList(), expected);
            CollectionAssert.AreEquivalent(t112_ancestors.ToList(), expected);

            t12_ancestors.CollectionChanged -= noChanges;
            t111_ancestors.CollectionChanged -= t12Added;
            t112_ancestors.CollectionChanged -= t12Added;
            t11_ancestors.CollectionChanged -= t12Added;

            NotifyCollectionChangedEventHandler t1Removed = (o, e) =>
            {
                var is_t11 = o == t11_ancestors;
                var is_t12 = o == t12_ancestors;
                var is_t111 = o == t111_ancestors;
                var is_t112 = o == t112_ancestors;

                handlersCalled++;
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(t1, e.OldItems[0]);
            };
            t12_ancestors.CollectionChanged += t1Removed;
            t11_ancestors.CollectionChanged += t1Removed;
            t111_ancestors.CollectionChanged += t1Removed;
            t112_ancestors.CollectionChanged += t1Removed;

            t1.Invalids.Remove(t12);

            Assert.AreEqual(7, handlersCalled);
        }


        [TestMethod]
        public void Ancestors_IncrementalMoveUp_CorrectResults()
        {
            var t1_ancestors = t1.Ancestors().AsNotifiable();
            var t11_ancestors = t11.Ancestors().AsNotifiable();
            var t12_ancestors = t12.Ancestors().AsNotifiable();
            var t111_ancestors = t111.Ancestors().AsNotifiable();
            var t112_ancestors = t112.Ancestors().AsNotifiable();

            var handlersCalled = 0;
            NotifyCollectionChangedEventHandler noChanges = (o, e) =>
            {
                throw new InvalidOperationException();
            };
            NotifyCollectionChangedEventHandler t11Removed = (o, e) =>
            {
                var is_t111 = o == t111_ancestors;

                handlersCalled++;
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(t11, e.OldItems[0]);
            };
            t1_ancestors.CollectionChanged += noChanges;
            t12_ancestors.CollectionChanged += noChanges;
            t11_ancestors.CollectionChanged += noChanges;
            t111_ancestors.CollectionChanged += t11Removed;
            t112_ancestors.CollectionChanged += noChanges;

            Assert.AreEqual(0, handlersCalled);

            t1.Invalids.Add(t111);

            Assert.AreEqual(1, handlersCalled);

            var expected = new List<IModelElement>() { t1 };
            CollectionAssert.AreEquivalent(t11_ancestors.ToList(), expected);
            CollectionAssert.AreEquivalent(t111_ancestors.ToList(), expected);
            expected.Add(t11);
            CollectionAssert.AreEquivalent(t112_ancestors.ToList(), expected);

            t111_ancestors.CollectionChanged -= t11Removed;
            t111_ancestors.CollectionChanged += noChanges;
            NotifyCollectionChangedEventHandler t1Removed = (o, e) =>
            {
                var is_t11 = o == t11_ancestors;
                var is_t112 = o == t112_ancestors;

                handlersCalled++;
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(1, e.OldItems.Count);
                Assert.AreEqual(t1, e.OldItems[0]);
            };

            t11_ancestors.CollectionChanged -= noChanges;
            t112_ancestors.CollectionChanged -= noChanges;
            t11_ancestors.CollectionChanged += t1Removed;
            t112_ancestors.CollectionChanged += t1Removed;

            t1.Invalids.Remove(t11);

            Assert.AreEqual(3, handlersCalled);
        }
    }
}
