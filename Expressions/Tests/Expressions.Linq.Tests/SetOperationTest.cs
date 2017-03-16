using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace NMF.Expressions.Linq.Tests
{
    public abstract class SetOperationTest
    {
        protected abstract bool ShouldBeInResult(bool inCollection1, bool inCollection2);

        protected abstract INotifyEnumerable<T> Operate<T>(INotifyEnumerable<T> source, IEnumerable<T> source2);

        protected abstract INotifyEnumerable<T> Operate<T>(INotifyEnumerable<T> source, IEnumerable<T> source2, IEqualityComparer<T> comparer);
        
        protected void SetOperation_NoObervableSource1ItemAdded_NoUpdate()
        {
            var update = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2);

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
            Assert.IsFalse(update);

            coll1.Add(4);

            Assert.IsFalse(update);
        }
        
        protected void SetOperation_ObservableSource1ItemAddedUnrelated_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2);

            var shouldUpdate = ShouldBeInResult(true, false);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(5, e.NewItems[0]);
                Assert.IsNull(e.OldItems);
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
            Assert.IsFalse(update);

            coll1.Add(5);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
            Assert.AreEqual(test.Contains(5), shouldUpdate);
        }
        
        protected void SetOperation_ObservableSource1ItemAddedFromSource2_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2);

            var shouldBeContainedBefore = ShouldBeInResult(false, true);
            var shouldBeContainedAfter = ShouldBeInResult(true, true);
            var shouldUpdate = shouldBeContainedBefore != shouldBeContainedAfter;

            test.CollectionChanged += (o, e) =>
            {
                if (shouldBeContainedBefore)
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                    Assert.AreEqual(4, e.OldItems[0]);
                    Assert.IsNull(e.NewItems);
                }
                else
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                    Assert.AreEqual(4, e.NewItems[0]);
                    Assert.IsNull(e.OldItems);
                }
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
            Assert.IsFalse(update);

            coll1.Add(4);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(4));
        }
        
        protected void SetOperation_ObservableSource2ItemAddedFromSource1_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2.WithUpdates());

            var shouldBeContainedBefore = ShouldBeInResult(true, false);
            var shouldBeContainedAfter = ShouldBeInResult(true, true);
            var shouldUpdate = shouldBeContainedBefore != shouldBeContainedAfter;

            test.CollectionChanged += (o, e) =>
            {
                if (shouldBeContainedBefore)
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                    Assert.AreEqual(1, e.OldItems[0]);
                    Assert.IsNull(e.NewItems);
                }
                else
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                    Assert.AreEqual(1, e.NewItems[0]);
                    Assert.IsNull(e.OldItems);
                }
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
            Assert.IsFalse(update);

            coll2.Add(1);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
        }
        
        protected void SetOperation_ObservableSource2ItemAddedUnrelated_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2.WithUpdates());

            var shouldUpdate = ShouldBeInResult(false, true);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(5, e.NewItems[0]);
                Assert.IsNull(e.OldItems);
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
            Assert.IsFalse(update);

            coll2.Add(5);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
            Assert.AreEqual(shouldUpdate, test.Contains(5));
        }
        
        protected void SetOperation_NoObervableSource1ItemRemoved_NoUpdate()
        {
            var update = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2);

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
            Assert.IsFalse(update);

            coll1.Remove(1);

            Assert.IsFalse(update);
        }
        
        protected void SetOperation_ObservableSource1ItemRemovedUnrelated_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2);

            var shouldUpdate = ShouldBeInResult(true, false);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(1, e.OldItems[0]);
                Assert.IsNull(e.NewItems);
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
            Assert.IsFalse(update);

            coll1.Remove(1);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
        }
        
        protected void SetOperation_ObservableSource1ItemRemovedFromSource2_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(2, e.OldItems[0]);
                Assert.IsNull(e.NewItems);
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
            Assert.IsFalse(update);

            coll1.Remove(2);

            Assert.AreEqual(ShouldBeInResult(true, true) && !ShouldBeInResult(false, true), update);
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
        }
        
        protected void SetOperation_ObservableSource2ItemRemovedFromSource1_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2.WithUpdates());

            var shouldBeContainedBefore = ShouldBeInResult(true, true);
            var shouldBeContainedAfter = ShouldBeInResult(true, false);
            var shouldUpdate = shouldBeContainedBefore != shouldBeContainedAfter;

            test.CollectionChanged += (o, e) =>
            {
                if (shouldBeContainedBefore)
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                    Assert.AreEqual(2, e.OldItems[0]);
                    Assert.IsNull(e.NewItems);
                }
                else
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                    Assert.AreEqual(2, e.NewItems[0]);
                    Assert.IsNull(e.OldItems);
                }
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
            Assert.IsFalse(update);

            coll2.Remove(2);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
        }
        
        protected void SetOperation_ObservableSource2ItemRemovedUnrelated_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2.WithUpdates());

            var shouldUpdate = ShouldBeInResult(false, true);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(4, e.OldItems[0]);
                Assert.IsNull(e.NewItems);
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4));
            Assert.IsFalse(update);

            coll2.Remove(4);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3));
        }
        
        protected void SetOperationEqualityComparer_NoObervableSource1ItemAdded_NoUpdate()
        {
            var update = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { -2, -3, -4 };

            var test = Operate(coll1.WithUpdates(), coll2, new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll1.Add(4);

            Assert.IsFalse(update);
        }
        
        protected void SetOperationEqualityComparer_ObservableSource1ItemAddedUnrelated_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { -2, -3, -4 };

            var test = Operate(coll1.WithUpdates(), coll2, new AbsoluteValueComparer());

            var shouldUpdate = ShouldBeInResult(true, false);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(5, e.NewItems[0]);
                Assert.IsNull(e.OldItems);
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll1.Add(5);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
            Assert.AreEqual(test.Contains(5, new AbsoluteValueComparer()), shouldUpdate);
        }
        
        protected void SetOperationEqualityComparer_ObservableSource1ItemAddedFromSource2_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { -2, -3, -4 };

            var test = Operate(coll1.WithUpdates(), coll2, new AbsoluteValueComparer());

            var shouldBeContainedBefore = ShouldBeInResult(false, true);
            var shouldBeContainedAfter = ShouldBeInResult(true, true);
            var shouldUpdate = shouldBeContainedBefore != shouldBeContainedAfter;

            test.CollectionChanged += (o, e) =>
            {
                if (shouldBeContainedBefore)
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                    Assert.AreEqual(4, e.OldItems[0]);
                    Assert.IsNull(e.NewItems);
                }
                else
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                    Assert.AreEqual(4, e.NewItems[0]);
                    Assert.IsNull(e.OldItems);
                }
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll1.Add(4);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(4, new AbsoluteValueComparer()));
        }
        
        protected void SetOperationEqualityComparer_ObservableSource2ItemAddedFromSource1_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { -2, -3, -4 };

            var test = Operate(coll1.WithUpdates(), coll2.WithUpdates(), new AbsoluteValueComparer());

            var shouldBeContainedBefore = ShouldBeInResult(true, false);
            var shouldBeContainedAfter = ShouldBeInResult(true, true);
            var shouldUpdate = shouldBeContainedBefore != shouldBeContainedAfter;

            test.CollectionChanged += (o, e) =>
            {
                if (shouldBeContainedBefore)
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                    Assert.AreEqual(1, e.OldItems[0]);
                    Assert.IsNull(e.NewItems);
                }
                else
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                    Assert.AreEqual(1, e.NewItems[0]);
                    Assert.IsNull(e.OldItems);
                }
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll2.Add(1);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
        }
        
        protected void SetOperationEqualityComparer_ObservableSource2ItemAddedUnrelated_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { -2, -3, -4 };

            var test = Operate(coll1.WithUpdates(), coll2.WithUpdates(), new AbsoluteValueComparer());

            var shouldUpdate = ShouldBeInResult(false, true);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(5, e.NewItems[0]);
                Assert.IsNull(e.OldItems);
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll2.Add(5);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
            Assert.AreEqual(shouldUpdate, test.Contains(5));
        }
        
        protected void SetOperationEqualityComparer_NoObervableSource1ItemRemoved_NoUpdate()
        {
            var update = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { -2, -3, -4 };

            var test = Operate(coll1.WithUpdates(), coll2, new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) => update = true;

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll1.Remove(1);

            Assert.IsFalse(update);
        }
        
        protected void SetOperationEqualityComparer_ObservableSource1ItemRemovedUnrelated_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { -2, -3, -4 };

            var test = Operate(coll1.WithUpdates(), coll2, new AbsoluteValueComparer());

            var shouldUpdate = ShouldBeInResult(true, false);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(1, e.OldItems[0]);
                Assert.IsNull(e.NewItems);
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll1.Remove(1);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
        }
        
        protected void SetOperationEqualityComparer_ObservableSource1ItemRemovedFromSource2_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { -2, -3, -4 };

            var test = Operate(coll1.WithUpdates(), coll2, new AbsoluteValueComparer());

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(2, e.OldItems[0]);
                Assert.IsNull(e.NewItems);
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll1.Remove(2);

            Assert.AreEqual(ShouldBeInResult(true, true) && !ShouldBeInResult(false, true), update);
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
        }
        
        protected void SetOperationEqualityComparer_ObservableSource2ItemRemovedFromSource1_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { -2, -3, -4 };

            var test = Operate(coll1.WithUpdates(), coll2.WithUpdates(), new AbsoluteValueComparer());

            var shouldBeContainedBefore = ShouldBeInResult(true, true);
            var shouldBeContainedAfter = ShouldBeInResult(true, false);
            var shouldUpdate = shouldBeContainedBefore != shouldBeContainedAfter;

            test.CollectionChanged += (o, e) =>
            {
                if (shouldBeContainedBefore)
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                    Assert.AreEqual(2, e.OldItems[0]);
                    Assert.IsNull(e.NewItems);
                }
                else
                {
                    Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                    Assert.AreEqual(2, e.NewItems[0]);
                    Assert.IsNull(e.OldItems);
                }
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll2.Remove(-2);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
        }
        
        protected void SetOperationEqualityComparer_ObservableSource2ItemRemovedUnrelated_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { -2, -3, -4 };

            var test = Operate(coll1.WithUpdates(), coll2.WithUpdates(), new AbsoluteValueComparer());

            var shouldUpdate = ShouldBeInResult(false, true);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(-4, e.OldItems[0]);
                Assert.IsNull(e.NewItems);
                update = true;
            };

            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(false, true), test.Contains(4, new AbsoluteValueComparer()));
            Assert.IsFalse(update);

            coll2.Remove(-4);

            Assert.AreEqual(shouldUpdate, update);
            Assert.AreEqual(ShouldBeInResult(true, false), test.Contains(1, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(2, new AbsoluteValueComparer()));
            Assert.AreEqual(ShouldBeInResult(true, true), test.Contains(3, new AbsoluteValueComparer()));
        }
    }

    [TestClass]
    public class UnionTests : SetOperationTest
    {
        protected override bool ShouldBeInResult(bool inCollection1, bool inCollection2)
        {
            return inCollection1 || inCollection2;
        }

        protected override INotifyEnumerable<T> Operate<T>(INotifyEnumerable<T> source, IEnumerable<T> source2)
        {
            return source.Union(source2);
        }

        protected override INotifyEnumerable<T> Operate<T>(INotifyEnumerable<T> source, IEnumerable<T> source2, IEqualityComparer<T> comparer)
        {
            return source.Union(source2, comparer);
        }

        [TestMethod]
        public void Union_NoObervableSource1ItemAdded_NoUpdate()
        {
            SetOperation_NoObervableSource1ItemAdded_NoUpdate();
        }

        [TestMethod]
        public void Union_ObservableSource1ItemAddedUnrelated_Update()
        {
            SetOperation_ObservableSource1ItemAddedUnrelated_Update();
        }

        [TestMethod]
        public void Union_ObservableSource1ItemAddedFromSource2_Update()
        {
            SetOperation_ObservableSource1ItemAddedFromSource2_Update();
        }

        [TestMethod]
        public void Union_ObservableSource2ItemAddedFromSource1_Update()
        {
            SetOperation_ObservableSource2ItemAddedFromSource1_Update();
        }

        [TestMethod]
        public void Union_ObservableSource2ItemAddedUnrelated_Update()
        {
            SetOperation_ObservableSource2ItemAddedUnrelated_Update();
        }

        [TestMethod]
        public void Union_NoObervableSource1ItemRemoved_NoUpdate()
        {
            SetOperation_NoObervableSource1ItemRemoved_NoUpdate();
        }

        [TestMethod]
        public void Union_ObservableSource1ItemRemovedUnrelated_Update()
        {
            SetOperation_ObservableSource1ItemRemovedUnrelated_Update();
        }

        [TestMethod]
        public void Union_ObservableSource1ItemRemovedFromSource2_Update()
        {
            SetOperation_ObservableSource1ItemRemovedFromSource2_Update();
        }

        [TestMethod]
        public void Union_ObservableSource2ItemRemovedFromSource1_Update()
        {
            SetOperation_ObservableSource2ItemRemovedFromSource1_Update();
        }

        [TestMethod]
        public void Union_ObservableSource2ItemRemovedUnrelated_Update()
        {
            SetOperation_ObservableSource2ItemRemovedUnrelated_Update();
        }

        [TestMethod]
        public void UnionEqualityComparer_NoObervableSource1ItemAdded_NoUpdate()
        {
            SetOperationEqualityComparer_NoObervableSource1ItemAdded_NoUpdate();
        }

        [TestMethod]
        public void UnionEqualityComparer_ObservableSource1ItemAddedUnrelated_Update()
        {
            SetOperationEqualityComparer_ObservableSource1ItemAddedUnrelated_Update();
        }

        [TestMethod]
        public void UnionEqualityComparer_ObservableSource1ItemAddedFromSource2_Update()
        {
            SetOperationEqualityComparer_ObservableSource1ItemAddedFromSource2_Update();
        }

        [TestMethod]
        public void UnionEqualityComparer_ObservableSource2ItemAddedFromSource1_Update()
        {
            SetOperationEqualityComparer_ObservableSource2ItemAddedFromSource1_Update();
        }

        [TestMethod]
        public void UnionEqualityComparer_ObservableSource2ItemAddedUnrelated_Update()
        {
            SetOperationEqualityComparer_ObservableSource2ItemAddedUnrelated_Update();
        }

        [TestMethod]
        public void UnionEqualityComparer_NoObervableSource1ItemRemoved_NoUpdate()
        {
            SetOperationEqualityComparer_NoObervableSource1ItemRemoved_NoUpdate();
        }

        [TestMethod]
        public void UnionEqualityComparer_ObservableSource1ItemRemovedUnrelated_Update()
        {
            SetOperationEqualityComparer_ObservableSource1ItemRemovedUnrelated_Update();
        }

        [TestMethod]
        public void UnionEqualityComparer_ObservableSource1ItemRemovedFromSource2_Update()
        {
            SetOperationEqualityComparer_ObservableSource1ItemRemovedFromSource2_Update();
        }

        [TestMethod]
        public void UnionEqualityComparer_ObservableSource2ItemRemovedFromSource1_Update()
        {
            SetOperationEqualityComparer_ObservableSource2ItemRemovedFromSource1_Update();
        }

        [TestMethod]
        public void UnionEqualityComparer_ObservableSource2ItemRemovedUnrelated_Update()
        {
            SetOperationEqualityComparer_ObservableSource2ItemRemovedUnrelated_Update();
        }

        [TestMethod]
        public void Union_NoObservableSource1Reset_NoUpdate()
        {
            var update = false;

            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 2, 3, 4 };

            var test = coll1.WithUpdates().Union(coll2);

            test.CollectionChanged += (o, e) => update = true;

            coll1.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Union_ObservableSource1Reset_Update()
        {
            var reset = false;

            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 2, 3, 4 };

            var test = coll1.WithUpdates().Union(coll2);

            test.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    reset = true;
                }
            };

            coll1.Clear();

            Assert.IsTrue(reset);
            Assert.AreEqual(3, test.Count());
            Assert.IsTrue(test.Contains(2));
            Assert.IsTrue(test.Contains(3));
            Assert.IsTrue(test.Contains(4));
        }

        [TestMethod]
        public void Union_NoObservableSource2Reset_NoUpdate()
        {
            var update = false;

            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 2, 3, 4 };

            var test = coll1.WithUpdates().Union(coll2);

            test.CollectionChanged += (o, e) => update = true;

            coll2.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Union_ObservableSource2Reset_Update()
        {
            var reset = false;

            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 2, 3, 4 };

            var test = coll1.WithUpdates().Union(coll2.WithUpdates());

            test.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    reset = true;
                }
            };

            coll2.Clear();

            Assert.IsTrue(reset);
            Assert.AreEqual(3, test.Count());
            Assert.IsTrue(test.Contains(1));
            Assert.IsTrue(test.Contains(2));
            Assert.IsTrue(test.Contains(3));
        }
    }

    [TestClass]
    public class ExceptTests : SetOperationTest
    {
        protected override bool ShouldBeInResult(bool inCollection1, bool inCollection2)
        {
            return inCollection1 && !inCollection2;
        }

        protected override INotifyEnumerable<T> Operate<T>(INotifyEnumerable<T> source, IEnumerable<T> source2)
        {
            return source.Except(source2);
        }

        protected override INotifyEnumerable<T> Operate<T>(INotifyEnumerable<T> source, IEnumerable<T> source2, IEqualityComparer<T> comparer)
        {
            return source.Except(source2, comparer);
        }

        [TestMethod]
        public void Except_NoObervableSource1ItemAdded_NoUpdate()
        {
            SetOperation_NoObervableSource1ItemAdded_NoUpdate();
        }

        [TestMethod]
        public void Except_ObservableSource1ItemAddedUnrelated_Update()
        {
            SetOperation_ObservableSource1ItemAddedUnrelated_Update();
        }

        [TestMethod]
        public void Except_ObservableSource1ItemAddedFromSource2_Update()
        {
            SetOperation_ObservableSource1ItemAddedFromSource2_Update();
        }

        [TestMethod]
        public void Except_ObservableSource2ItemAddedFromSource1_Update()
        {
            SetOperation_ObservableSource2ItemAddedFromSource1_Update();
        }

        [TestMethod]
        public void Except_ObservableSource2ItemAddedUnrelated_Update()
        {
            SetOperation_ObservableSource2ItemAddedUnrelated_Update();
        }

        [TestMethod]
        public void Except_NoObervableSource1ItemRemoved_NoUpdate()
        {
            SetOperation_NoObervableSource1ItemRemoved_NoUpdate();
        }

        [TestMethod]
        public void Except_ObservableSource1ItemRemovedUnrelated_Update()
        {
            SetOperation_ObservableSource1ItemRemovedUnrelated_Update();
        }

        [TestMethod]
        public void Except_ObservableSource1ItemRemovedFromSource2_Update()
        {
            SetOperation_ObservableSource1ItemRemovedFromSource2_Update();
        }

        [TestMethod]
        public void Except_ObservableSource2ItemRemovedFromSource1_Update()
        {
            SetOperation_ObservableSource2ItemRemovedFromSource1_Update();
        }

        [TestMethod]
        public void Except_ObservableSource2ItemRemovedUnrelated_Update()
        {
            SetOperation_ObservableSource2ItemRemovedUnrelated_Update();
        }

        [TestMethod]
        public void ExceptEqualityComparer_NoObervableSource1ItemAdded_NoUpdate()
        {
            SetOperationEqualityComparer_NoObervableSource1ItemAdded_NoUpdate();
        }

        [TestMethod]
        public void ExceptEqualityComparer_ObservableSource1ItemAddedUnrelated_Update()
        {
            SetOperationEqualityComparer_ObservableSource1ItemAddedUnrelated_Update();
        }

        [TestMethod]
        public void ExceptEqualityComparer_ObservableSource1ItemAddedFromSource2_Update()
        {
            SetOperationEqualityComparer_ObservableSource1ItemAddedFromSource2_Update();
        }

        [TestMethod]
        public void ExceptEqualityComparer_ObservableSource2ItemAddedFromSource1_Update()
        {
            SetOperationEqualityComparer_ObservableSource2ItemAddedFromSource1_Update();
        }

        [TestMethod]
        public void ExceptEqualityComparer_ObservableSource2ItemAddedUnrelated_Update()
        {
            SetOperationEqualityComparer_ObservableSource2ItemAddedUnrelated_Update();
        }

        [TestMethod]
        public void ExceptEqualityComparer_NoObervableSource1ItemRemoved_NoUpdate()
        {
            SetOperationEqualityComparer_NoObervableSource1ItemRemoved_NoUpdate();
        }

        [TestMethod]
        public void ExceptEqualityComparer_ObservableSource1ItemRemovedUnrelated_Update()
        {
            SetOperationEqualityComparer_ObservableSource1ItemRemovedUnrelated_Update();
        }

        [TestMethod]
        public void ExceptEqualityComparer_ObservableSource1ItemRemovedFromSource2_Update()
        {
            SetOperationEqualityComparer_ObservableSource1ItemRemovedFromSource2_Update();
        }

        [TestMethod]
        public void ExceptEqualityComparer_ObservableSource2ItemRemovedFromSource1_Update()
        {
            SetOperationEqualityComparer_ObservableSource2ItemRemovedFromSource1_Update();
        }

        [TestMethod]
        public void ExceptEqualityComparer_ObservableSource2ItemRemovedUnrelated_Update()
        {
            SetOperationEqualityComparer_ObservableSource2ItemRemovedUnrelated_Update();
        }

        [TestMethod]
        public void Except_NoObservableSource2Reset_NoUpdate()
        {
            var update = false;

            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 2, 3, 4 };

            var test = coll1.WithUpdates().Except(coll2);

            test.CollectionChanged += (o, e) => update = true;

            coll2.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Except_ObservableSource2Reset_Update()
        {
            var update = false;

            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 2, 3, 4 };

            var test = coll1.WithUpdates().Except(coll2.WithUpdates());

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
                update = true;
            };

            coll2.Clear();

            Assert.IsTrue(update);
            Assert.AreEqual(3, test.Count());
            Assert.IsTrue(test.Contains(1));
            Assert.IsTrue(test.Contains(2));
            Assert.IsTrue(test.Contains(3));
        }

        [TestMethod]
        public void Except_NoObservableSource1Reset_NoUpdate()
        {
            var update = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2);

            test.CollectionChanged += (o, e) => update = true;

            coll1.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Except_ObservableSource1Reset_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
                update = true;
            };

            coll1.Clear();

            Assert.IsTrue(update);
        }
    }

    [TestClass]
    public class IntersectTests : SetOperationTest
    {
        protected override bool ShouldBeInResult(bool inCollection1, bool inCollection2)
        {
            return inCollection1 && inCollection2;
        }

        protected override INotifyEnumerable<T> Operate<T>(INotifyEnumerable<T> source, IEnumerable<T> source2)
        {
            return source.Intersect(source2);
        }

        protected override INotifyEnumerable<T> Operate<T>(INotifyEnumerable<T> source, IEnumerable<T> source2, IEqualityComparer<T> comparer)
        {
            return source.Intersect(source2, comparer);
        }

        [TestMethod]
        public void Intersect_NoObervableSource1ItemAdded_NoUpdate()
        {
            SetOperation_NoObervableSource1ItemAdded_NoUpdate();
        }

        [TestMethod]
        public void Intersect_ObservableSource1ItemAddedUnrelated_Update()
        {
            SetOperation_ObservableSource1ItemAddedUnrelated_Update();
        }

        [TestMethod]
        public void Intersect_ObservableSource1ItemAddedFromSource2_Update()
        {
            SetOperation_ObservableSource1ItemAddedFromSource2_Update();
        }

        [TestMethod]
        public void Intersect_ObservableSource2ItemAddedFromSource1_Update()
        {
            SetOperation_ObservableSource2ItemAddedFromSource1_Update();
        }

        [TestMethod]
        public void Intersect_ObservableSource2ItemAddedUnrelated_Update()
        {
            SetOperation_ObservableSource2ItemAddedUnrelated_Update();
        }

        [TestMethod]
        public void Intersect_NoObervableSource1ItemRemoved_NoUpdate()
        {
            SetOperation_NoObervableSource1ItemRemoved_NoUpdate();
        }

        [TestMethod]
        public void Intersect_ObservableSource1ItemRemovedUnrelated_Update()
        {
            SetOperation_ObservableSource1ItemRemovedUnrelated_Update();
        }

        [TestMethod]
        public void Intersect_ObservableSource1ItemRemovedFromSource2_Update()
        {
            SetOperation_ObservableSource1ItemRemovedFromSource2_Update();
        }

        [TestMethod]
        public void Intersect_ObservableSource2ItemRemovedFromSource1_Update()
        {
            SetOperation_ObservableSource2ItemRemovedFromSource1_Update();
        }

        [TestMethod]
        public void Intersect_ObservableSource2ItemRemovedUnrelated_Update()
        {
            SetOperation_ObservableSource2ItemRemovedUnrelated_Update();
        }

        [TestMethod]
        public void IntersectEqualityComparer_NoObervableSource1ItemAdded_NoUpdate()
        {
            SetOperationEqualityComparer_NoObervableSource1ItemAdded_NoUpdate();
        }

        [TestMethod]
        public void IntersectEqualityComparer_ObservableSource1ItemAddedUnrelated_Update()
        {
            SetOperationEqualityComparer_ObservableSource1ItemAddedUnrelated_Update();
        }

        [TestMethod]
        public void IntersectEqualityComparer_ObservableSource1ItemAddedFromSource2_Update()
        {
            SetOperationEqualityComparer_ObservableSource1ItemAddedFromSource2_Update();
        }

        [TestMethod]
        public void IntersectEqualityComparer_ObservableSource2ItemAddedFromSource1_Update()
        {
            SetOperationEqualityComparer_ObservableSource2ItemAddedFromSource1_Update();
        }

        [TestMethod]
        public void IntersectEqualityComparer_ObservableSource2ItemAddedUnrelated_Update()
        {
            SetOperationEqualityComparer_ObservableSource2ItemAddedUnrelated_Update();
        }

        [TestMethod]
        public void IntersectEqualityComparer_NoObervableSource1ItemRemoved_NoUpdate()
        {
            SetOperationEqualityComparer_NoObervableSource1ItemRemoved_NoUpdate();
        }

        [TestMethod]
        public void IntersectEqualityComparer_ObservableSource1ItemRemovedUnrelated_Update()
        {
            SetOperationEqualityComparer_ObservableSource1ItemRemovedUnrelated_Update();
        }

        [TestMethod]
        public void IntersectEqualityComparer_ObservableSource1ItemRemovedFromSource2_Update()
        {
            SetOperationEqualityComparer_ObservableSource1ItemRemovedFromSource2_Update();
        }

        [TestMethod]
        public void IntersectEqualityComparer_ObservableSource2ItemRemovedFromSource1_Update()
        {
            SetOperationEqualityComparer_ObservableSource2ItemRemovedFromSource1_Update();
        }

        [TestMethod]
        public void IntersectEqualityComparer_ObservableSource2ItemRemovedUnrelated_Update()
        {
            SetOperationEqualityComparer_ObservableSource2ItemRemovedUnrelated_Update();
        }

        [TestMethod]
        public void Intersect_NoObservableSource2Reset_NoUpdate()
        {
            var update = false;

            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 2, 3, 4 };

            var test = coll1.WithUpdates().Intersect(coll2);

            test.CollectionChanged += (o, e) => update = true;

            coll2.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Intersect_ObservableSource2Reset_Update()
        {
            var update = false;

            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new ObservableCollection<int>() { 2, 3, 4 };

            var test = coll1.WithUpdates().Intersect(coll2.WithUpdates());

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
                update = true;
            };

            coll2.Clear();

            Assert.IsTrue(update);
            Assert.IsFalse(test.Any());
        }

        [TestMethod]
        public void Intersect_NoObservableSource1Reset_NoUpdate()
        {
            var update = false;
            var coll1 = new List<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2);

            test.CollectionChanged += (o, e) => update = true;

            coll1.Clear();

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void Intersect_ObservableSource1Reset_Update()
        {
            var update = false;
            var coll1 = new ObservableCollection<int>() { 1, 2, 3 };
            var coll2 = new List<int>() { 2, 3, 4 };

            var test = Operate(coll1.WithUpdates(), coll2);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
                update = true;
            };

            coll1.Clear();

            Assert.IsTrue(update);
        }
    }
}
