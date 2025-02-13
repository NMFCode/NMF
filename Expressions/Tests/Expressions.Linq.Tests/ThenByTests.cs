﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using NMF.Expressions.Test;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class ThenByTests
    {
        [TestMethod]
        public void ThenBy_NoObservableSequenceItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new OrderableList<int>();
            coll.Sequences.Add(new List<int>() { 6, 4, 5 });
            var list = new List<int>() { 3, 1, 2 };
            coll.Sequences.Add(list);

            var test = coll.ThenBy(i => i);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence(4, 5, 6, 1, 2, 3);
            Assert.IsFalse(update);
            Assert.AreEqual(6, test.Sequences.Count());

            list.Add(0);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ThenBy_ObservableSequenceNewItemAdded_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            coll.Sequences.Add(new List<int>() { 6, 4, 5 });
            var list = new ObservableCollection<int>() { 3, 1, 2 };
            coll.Sequences.Add(list);

            var test = coll.ThenBy(i => i);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(0, e.NewItems[0]);
                Assert.IsNull(e.OldItems);
                update = true;
            };

            test.AssertSequence(4, 5, 6, 1, 2, 3);
            Assert.IsFalse(update);
            Assert.AreEqual(6, test.Sequences.Count());

            list.Add(0);

            Assert.IsTrue(update);
            test.AssertSequence(4, 5, 6, 0, 1, 2, 3);
            Assert.AreEqual(7, test.Sequences.Count());
            foreach (var sequence in test.Sequences)
            {
                Assert.AreEqual(1, sequence.Count());
            }
        }

        [TestMethod]
        public void ThenBy_ObservableSequenceExistingItemAdded_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            coll.Sequences.Add(new List<int>() { 6, 4, 5 });
            var list = new ObservableCollection<int>() { 3, 1, 2 };
            coll.Sequences.Add(list);

            var test = coll.ThenBy(i => i);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewItems[0]);
                Assert.IsNull(e.OldItems);
                update = true;
            };

            test.AssertSequence(4, 5, 6, 1, 2, 3);
            Assert.IsFalse(update);
            Assert.AreEqual(6, test.Sequences.Count());
            
            list.Add(1);

            Assert.IsTrue(update);
            test.AssertSequence(4, 5, 6, 1, 1, 2, 3);
            Assert.AreEqual(6, test.Sequences.Count());
        }

        [TestMethod]
        public void ThenBy_NoObservableSequenceItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new OrderableList<int>();
            coll.Sequences.Add(new List<int>() { 6, 4, 5 });
            var list = new List<int>() { 3, 1, 2 };
            coll.Sequences.Add(list);

            var test = coll.ThenBy(i => i);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence(4, 5, 6, 1, 2, 3);
            Assert.IsFalse(update);
            Assert.AreEqual(6, test.Sequences.Count());

            list.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ThenBy_ObservableSequenceLastItemRemoved_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            coll.Sequences.Add(new List<int>() { 6, 4, 5 });
            var list = new ObservableCollection<int>() { 3, 1, 1, 2 };
            coll.Sequences.Add(list);

            var test = coll.ThenBy(i => i);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(2, e.OldItems[0]);
                Assert.IsNull(e.NewItems);
                update = true;
            };

            test.AssertSequence(4, 5, 6, 1, 1, 2, 3);
            Assert.IsFalse(update);
            Assert.AreEqual(6, test.Sequences.Count());

            list.Remove(2);

            Assert.IsTrue(update);
            test.AssertSequence(4, 5, 6, 1, 1, 3);
            Assert.AreEqual(5, test.Sequences.Count());
        }

        [TestMethod]
        public void ThenBy_ObservableSequenceDoubleRemoved_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            coll.Sequences.Add(new List<int>() { 6, 4, 5 });
            var list = new ObservableCollection<int>() { 3, 1, 1, 2 };
            coll.Sequences.Add(list);

            var test = coll.ThenBy(i => i);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(1, e.OldItems[0]);
                Assert.IsNull(e.NewItems);
                update = true;
            };

            test.AssertSequence(4, 5, 6, 1, 1, 2, 3);
            Assert.IsFalse(update);
            Assert.AreEqual(6, test.Sequences.Count());
            
            list.Remove(1);

            Assert.IsTrue(update);
            test.AssertSequence(4, 5, 6, 1, 2, 3);
            Assert.AreEqual(6, test.Sequences.Count());
        }

        [TestMethod]
        public void ThenBy_NoObservableItemSelectorChanges_NoUpdate()
        {
            var update = false;
            var coll = new OrderableList<Dummy<int>>();
            var dummy = new Dummy<int>[6];
            for (int i = 0; i < 6; i++)
            {
                dummy[i] = new Dummy<int>(i);
            }
            coll.Sequences.Add(new List<Dummy<int>>() { dummy[3], dummy[4], dummy[5] });
            coll.Sequences.Add(new List<Dummy<int>>() { dummy[0], dummy[1], dummy[2] });

            var test = coll.ThenBy(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
            };

            test.AssertSequence(dummy[3], dummy[4], dummy[5], dummy[0], dummy[1], dummy[2]);
            Assert.IsFalse(update);

            dummy[0].Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ThenBy_ObservableItemSelectorChanges_Update()
        {
            var update = false;
            var coll = new OrderableList<Dummy<int>>();
            var dummy = new Dummy<int>[6];
            for (int i = 0; i < 6; i++)
            {
                dummy[i] = new ObservableDummy<int>(i);
            }
            coll.Sequences.Add(new List<Dummy<int>>() { dummy[3], dummy[4], dummy[5] });
            coll.Sequences.Add(new List<Dummy<int>>() { dummy[0], dummy[1], dummy[2] });

            var test = coll.ThenBy(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreSame(dummy[0], e.NewItems[0]);
                Assert.AreSame(dummy[0], e.OldItems[0]);
            };

            test.AssertSequence(dummy[3], dummy[4], dummy[5], dummy[0], dummy[1], dummy[2]);
            Assert.IsFalse(update);

            dummy[0].Item = 3;

            Assert.IsTrue(update);
            test.AssertSequence(dummy[3], dummy[4], dummy[5], dummy[1], dummy[2], dummy[0]);
        }

        [TestMethod]
        public void ThenBy_SequenceAdded_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            coll.Sequences.Add(new List<int>() { 3, 1, 2 });

            var test = coll.ThenBy(i => i);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.IsTrue(e.NewItems.Contains(4));
                Assert.IsTrue(e.NewItems.Contains(5));
                Assert.IsTrue(e.NewItems.Contains(6));
                Assert.AreEqual(3, e.NewItems.Count);
                Assert.IsNull(e.OldItems);
            };

            test.AssertSequence(1, 2, 3);
            Assert.IsFalse(update);

            coll.Sequences.Add(new List<int>() { 6, 4, 5});

            Assert.IsTrue(update);
            test.AssertSequence(1, 2, 3, 4, 5, 6);
        }

        [TestMethod]
        public void ThenBy_SequenceRemoved_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            coll.Sequences.Add(new List<int>() { 3, 1, 2 });
            var list = new List<int>() { 4, 5, 6 };
            coll.Sequences.Add(list);

            var test = coll.ThenBy(i => i);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.IsTrue(e.OldItems.Contains(4));
                Assert.IsTrue(e.OldItems.Contains(5));
                Assert.IsTrue(e.OldItems.Contains(6));
                Assert.AreEqual(3, e.OldItems.Count);
                Assert.IsNull(e.NewItems);
            };

            test.AssertSequence(1, 2, 3, 4, 5, 6);
            Assert.IsFalse(update);

            coll.Sequences.Remove(list);

            Assert.IsTrue(update);
            test.AssertSequence(1, 2, 3);
        }

        [TestMethod]
        public void ThenBy_SequenceReset_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            coll.Sequences.Add(new List<int>() { 3, 1, 2 });

            var test = coll.ThenBy(i => i);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);
                update = true;
            };

            coll.Sequences.Clear();

            Assert.IsTrue(update);
        }

        [TestMethod]
        public void ThenByDescending_NoObservableSequenceItemAdded_NoUpdate()
        {
            var update = false;
            var coll = new OrderableList<int>();
            var list = new List<int>() { 3, 1, 2 };
            coll.Sequences.Add(list);
            coll.Sequences.Add(new List<int>() { 6, 4, 5 });

            var test = coll.ThenByDescending(i => i);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence(3, 2, 1, 6, 5, 4);
            Assert.IsFalse(update);
            Assert.AreEqual(6, test.Sequences.Count());

            list.Add(0);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ThenByDescending_ObservableSequenceNewItemAdded_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            var list = new ObservableCollection<int>() { 3, 1, 2 };
            coll.Sequences.Add(list);
            coll.Sequences.Add(new List<int>() { 6, 4, 5 });

            var test = coll.ThenByDescending(i => i);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(0, e.NewItems[0]);
                Assert.IsNull(e.OldItems);
                update = true;
            };
            
            test.AssertSequence(3, 2, 1, 6, 5, 4);
            Assert.IsFalse(update);
            Assert.AreEqual(6, test.Sequences.Count());

            list.Add(0);

            Assert.IsTrue(update);
            test.AssertSequence(3, 2, 1, 0, 6, 5, 4);
            Assert.AreEqual(7, test.Sequences.Count());
            foreach (var sequence in test.Sequences)
            {
                Assert.AreEqual(1, sequence.Count());
            }
        }

        [TestMethod]
        public void ThenByDescending_ObservableSequenceExistingItemAdded_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            var list = new ObservableCollection<int>() { 3, 1, 2 };
            coll.Sequences.Add(list);
            coll.Sequences.Add(new List<int>() { 6, 4, 5 });

            var test = coll.ThenByDescending(i => i);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(1, e.NewItems[0]);
                Assert.IsNull(e.OldItems);
                update = true;
            };
            
            test.AssertSequence(3, 2, 1, 6, 5, 4);
            Assert.IsFalse(update);
            Assert.AreEqual(6, test.Sequences.Count());
            
            list.Add(1);

            Assert.IsTrue(update);
            test.AssertSequence(3, 2, 1, 1, 6, 5, 4);
            Assert.AreEqual(6, test.Sequences.Count());
        }

        [TestMethod]
        public void ThenByDescending_NoObservableSequenceItemRemoved_NoUpdate()
        {
            var update = false;
            var coll = new OrderableList<int>();
            var list = new List<int>() { 3, 1, 2 };
            coll.Sequences.Add(list);
            coll.Sequences.Add(new List<int>() { 6, 4, 5 });

            var test = coll.ThenByDescending(i => i);

            test.CollectionChanged += (o, e) => update = true;

            test.AssertSequence(3, 2, 1, 6, 5, 4);
            Assert.IsFalse(update);
            Assert.AreEqual(6, test.Sequences.Count());

            list.Remove(2);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ThenByDescending_ObservableSequenceLastItemRemoved_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            var list = new ObservableCollection<int>() { 3, 1, 1, 2 };
            coll.Sequences.Add(list);
            coll.Sequences.Add(new List<int>() { 6, 4, 5 });

            var test = coll.ThenByDescending(i => i);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(2, e.OldItems[0]);
                Assert.IsNull(e.NewItems);
                update = true;
            };
            
            test.AssertSequence(3, 2, 1, 1, 6, 5, 4);
            Assert.IsFalse(update);
            Assert.AreEqual(6, test.Sequences.Count());

            list.Remove(2);

            Assert.IsTrue(update);
            test.AssertSequence(3, 1, 1, 6, 5, 4);
            Assert.AreEqual(5, test.Sequences.Count());
        }

        [TestMethod]
        public void ThenByDescending_ObservableSequenceDoubleRemoved_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            var list = new ObservableCollection<int>() { 3, 1, 1, 2 };
            coll.Sequences.Add(list);
            coll.Sequences.Add(new List<int>() { 6, 4, 5 });

            var test = coll.ThenByDescending(i => i);

            test.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.AreEqual(1, e.OldItems[0]);
                Assert.IsNull(e.NewItems);
                update = true;
            };
            
            test.AssertSequence(3, 2, 1, 1, 6, 5, 4);
            Assert.IsFalse(update);
            Assert.AreEqual(6, test.Sequences.Count());
            
            list.Remove(1);

            Assert.IsTrue(update);
            test.AssertSequence(3, 2, 1, 6, 5, 4);
            Assert.AreEqual(6, test.Sequences.Count());
        }

        [TestMethod]
        public void ThenByDescending_NoObservableItemSelectorChanges_NoUpdate()
        {
            var update = false;
            var coll = new OrderableList<Dummy<int>>();
            var dummy = new Dummy<int>[6];
            for (int i = 0; i < 6; i++)
            {
                dummy[i] = new Dummy<int>(i);
            }
            coll.Sequences.Add(new List<Dummy<int>>() { dummy[0], dummy[1], dummy[2] });
            coll.Sequences.Add(new List<Dummy<int>>() { dummy[3], dummy[4], dummy[5] });

            var test = coll.ThenByDescending(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
            };

            test.AssertSequence(dummy[2], dummy[1], dummy[0], dummy[5], dummy[4], dummy[3]);
            Assert.IsFalse(update);

            dummy[0].Item = 3;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void ThenByDescending_ObservableItemSelectorChanges_Update()
        {
            var update = false;
            var coll = new OrderableList<Dummy<int>>();
            var dummy = new Dummy<int>[6];
            for (int i = 0; i < 6; i++)
            {
                dummy[i] = new ObservableDummy<int>(i);
            }
            coll.Sequences.Add(new List<Dummy<int>>() { dummy[0], dummy[1], dummy[2] });
            coll.Sequences.Add(new List<Dummy<int>>() { dummy[3], dummy[4], dummy[5] });

            var test = coll.ThenByDescending(d => d.Item);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreSame(dummy[0], e.NewItems[0]);
                Assert.AreSame(dummy[0], e.OldItems[0]);
            };

            test.AssertSequence(dummy[2], dummy[1], dummy[0], dummy[5], dummy[4], dummy[3]);
            Assert.IsFalse(update);

            dummy[0].Item = 3;

            Assert.IsTrue(update);
            test.AssertSequence(dummy[0], dummy[2], dummy[1], dummy[5], dummy[4], dummy[3]);
        }

        [TestMethod]
        public void ThenByDescending_SequenceAdded_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            coll.Sequences.Add(new List<int>() { 3, 1, 2 });

            var test = coll.ThenByDescending(i => i);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.IsTrue(e.NewItems.Contains(4));
                Assert.IsTrue(e.NewItems.Contains(5));
                Assert.IsTrue(e.NewItems.Contains(6));
                Assert.AreEqual(3, e.NewItems.Count);
                Assert.IsNull(e.OldItems);
            };

            test.AssertSequence(3, 2, 1);
            Assert.IsFalse(update);

            coll.Sequences.Add(new List<int>() { 6, 4, 5 });

            Assert.IsTrue(update);
            test.AssertSequence(3, 2, 1, 6, 5, 4);
        }

        [TestMethod]
        public void ThenByDescending_SequenceRemoved_Update()
        {
            var update = false;
            var coll = new OrderableList<int>();
            coll.Sequences.Add(new List<int>() { 3, 1, 2 });
            var list = new List<int>() { 4, 5, 6 };
            coll.Sequences.Add(list);

            var test = coll.ThenByDescending(i => i);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Remove, e.Action);
                Assert.IsTrue(e.OldItems.Contains(4));
                Assert.IsTrue(e.OldItems.Contains(5));
                Assert.IsTrue(e.OldItems.Contains(6));
                Assert.AreEqual(3, e.OldItems.Count);
                Assert.IsNull(e.NewItems);
            };

            test.AssertSequence(3, 2, 1, 6, 5, 4);
            Assert.IsFalse(update);

            coll.Sequences.Remove(list);

            Assert.IsTrue(update);
            test.AssertSequence(3, 2, 1);
        }

        [TestMethod]
        public void OrderByThenBy_OrderByUpdate_Updates()
        {
            var update = false;
            var dummy = new ObservableDummy<int>(42);
            var dummy2 = new Dummy<int>(23);
            var coll = new ObservableCollection<Dummy<int>>
            {
                dummy,
                dummy2
            };

            var test = coll.WithUpdates().OrderBy(d => d.Item).ThenBy(d => 0);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Move, e.Action);
                Assert.AreEqual(dummy, e.NewItems[0]);
                Assert.AreEqual(dummy, e.OldItems[0]);
            };

            test.AssertSequence(dummy2, dummy);
            Assert.IsFalse(update);

            dummy.Item = 4;

            Assert.IsTrue(update);
            test.AssertSequence(dummy, dummy2);
        }

        [TestMethod]
        public void OrderByThenBy_OrderByAdded_Updates()
        {
            var update = false;
            var dummy = new Dummy<int>(42);
            var dummy2 = new Dummy<int>(23);
            var coll = new ObservableCollection<Dummy<int>>
            {
                dummy
            };

            var test = coll.WithUpdates().OrderBy(d => d.Item).ThenBy(d => 0);

            test.CollectionChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(dummy2, e.NewItems[0]);
            };

            test.AssertSequence(dummy);
            Assert.IsFalse(update);

            coll.Add(dummy2);

            Assert.IsTrue(update);
            test.AssertSequence(dummy2, dummy);
        }
    }
}
