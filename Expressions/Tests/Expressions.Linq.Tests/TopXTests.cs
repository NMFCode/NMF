using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using NMF.Expressions.Test;

namespace NMF.Expressions.Linq.Tests
{
    [TestClass]
    public class TopXTests
    {
        [TestMethod]
        public void TopX_NoObservable_CorrectResult()
        {
            var collection = new NotifyCollection<Dummy<int>>();
            var top1 = new Dummy<int>(43);
            var top2 = new Dummy<int>(42);
            var top3 = new Dummy<int>(30);
            collection.Add(top1);
            collection.Add(new Dummy<int>(5));
            collection.Add(new Dummy<int>(7));
            collection.Add(new Dummy<int>(1));
            collection.Add(new Dummy<int>(27));
            collection.Add(new Dummy<int>(25));
            collection.Add(new Dummy<int>(13));
            collection.Add(new Dummy<int>(17));
            collection.Add(new Dummy<int>(7));
            collection.Add(top2);
            collection.Add(top3);
            collection.Add(new Dummy<int>(23));
            collection.Add(new Dummy<int>(6));

            var top = collection.TopX(3, d => d.Item);
            Assert.AreEqual(top1, top[0]);
            Assert.AreEqual(top2, top[1]);
            Assert.AreEqual(top3, top[2]);
            Assert.AreEqual(3, top.Length);
        }

        [TestMethod]
        public void TopX_NoChanges_CorrectResult()
        {
            var collection = new NotifyCollection<Dummy<int>>();
            var top1 = new Dummy<int>(43);
            var top2 = new Dummy<int>(42);
            var top3 = new Dummy<int>(30);
            collection.Add(top1);
            collection.Add(new Dummy<int>(5));
            collection.Add(new Dummy<int>(7));
            collection.Add(new Dummy<int>(1));
            collection.Add(new Dummy<int>(27));
            collection.Add(new Dummy<int>(25));
            collection.Add(new Dummy<int>(13));
            collection.Add(new Dummy<int>(17));
            collection.Add(new Dummy<int>(7));
            collection.Add(top2);
            collection.Add(top3);
            collection.Add(new Dummy<int>(23));
            collection.Add(new Dummy<int>(6));

            var topEx = Observable.Expression(() => collection.TopX(3, d => d.Item));
            var top = topEx.Value;
            Assert.AreEqual(top1, top[0]);
            Assert.AreEqual(top2, top[1]);
            Assert.AreEqual(top3, top[2]);
            Assert.AreEqual(3, top.Length);
        }


        [TestMethod]
        public void TopX_Insert_CorrectResult()
        {
            var collection = new NotifyCollection<Dummy<int>>();
            var top1 = new Dummy<int>(43);
            var top2 = new Dummy<int>(42);
            var top3 = new Dummy<int>(30);
            collection.Add(top1);
            collection.Add(top2);
            collection.Add(top3);
            collection.Add(new Dummy<int>(23));
            collection.Add(new Dummy<int>(6));

            var topEx = Observable.Expression(() => collection.TopX(3, d => d.Item));
            var top = topEx.Value;

            Assert.AreEqual(top1, top[0]);
            Assert.AreEqual(top2, top[1]);
            Assert.AreEqual(top3, top[2]);
            Assert.AreEqual(3, top.Length);

            var changed = false;

            topEx.ValueChanged += (o, e) =>
            {
                Assert.IsNotNull(e.OldValue);
                Assert.IsNotNull(e.NewValue);
                changed = true;
            };

            collection.Add(new Dummy<int>(5));

            Assert.AreEqual(top, topEx.Value);
            Assert.IsFalse(changed);

            collection.Add(new Dummy<int>(42));

            Assert.IsTrue(changed);
            Assert.AreEqual(43, topEx.Value[0].Item);
            Assert.AreEqual(42, topEx.Value[1].Item);
            Assert.AreEqual(42, topEx.Value[2].Item);
        }

        [TestMethod]
        public void TopX_EmptyMultipleAdd_CorrectResult()
        {
            var collection = new NotifyCollection<Dummy<int>>();
            var top1 = new Dummy<int>(43);
            var top2 = new Dummy<int>(42);
            var top3 = new Dummy<int>(30);

            var changed = false;
            var topEx = Observable.Expression(() => collection.TopX(3, d => d.Item));
            topEx.ValueChanged += (sender, args) => changed = true;

            var top = topEx.Value;
            Assert.AreEqual(0, top.Length);

            collection.Add(top2);
            top = topEx.Value;

            Assert.IsTrue(changed);
            Assert.AreEqual(top2, top[0]);
            Assert.AreEqual(1, top.Length);

            changed = false;
            collection.Add(top1);
            collection.Add(top3);
            top = topEx.Value;

            Assert.IsTrue(changed);
            Assert.AreEqual(top1, top[0]);
            Assert.AreEqual(top2, top[1]);
            Assert.AreEqual(top3, top[2]);
            Assert.AreEqual(3, top.Length);

            changed = false;
            collection.Add(new Dummy<int>(22));

            Assert.IsFalse(changed);
            Assert.AreEqual(top, topEx.Value);

            var newTop3 = new Dummy<int>((top3.Item + top2.Item) / 2);
            collection.Add(newTop3);
            top = topEx.Value;

            Assert.IsTrue(changed);
            Assert.AreEqual(top1, top[0]);
            Assert.AreEqual(top2, top[1]);
            Assert.AreEqual(newTop3, top[2]);
            Assert.AreEqual(3, top.Length);
        }

        [TestMethod]
        public void TopX_Remove_CorrectResult()
        {
            var collection = new NotifyCollection<Dummy<int>>();
            var top1 = new Dummy<int>(43);
            var top2 = new Dummy<int>(42);
            var top3 = new Dummy<int>(30);
            collection.Add(top1);
            collection.Add(top2);
            collection.Add(top3);
            collection.Add(new Dummy<int>(23));
            collection.Add(new Dummy<int>(6));

            var topEx = Observable.Expression(() => collection.TopX(3, d => d.Item));
            var top = topEx.Value;

            Assert.AreEqual(top1, top[0]);
            Assert.AreEqual(top2, top[1]);
            Assert.AreEqual(top3, top[2]);
            Assert.AreEqual(3, top.Length);

            var changed = false;

            topEx.ValueChanged += (o, e) =>
            {
                Assert.IsNotNull(e.OldValue);
                Assert.IsNotNull(e.NewValue);
                changed = true;
            };

            collection.RemoveAt(3);

            Assert.AreEqual(top, topEx.Value);
            Assert.IsFalse(changed);

            collection.Remove(top1);

            Assert.IsTrue(changed);
            Assert.AreEqual(42, topEx.Value[0].Item);
            Assert.AreEqual(30, topEx.Value[1].Item);
            Assert.AreEqual(6, topEx.Value[2].Item);
        }


        [TestMethod]
        public void TopX_Clear_CorrectResult()
        {
            var collection = new NotifyCollection<Dummy<int>>();
            var top1 = new Dummy<int>(43);
            var top2 = new Dummy<int>(42);
            var top3 = new Dummy<int>(30);
            collection.Add(top1);
            collection.Add(top2);
            collection.Add(top3);
            collection.Add(new Dummy<int>(23));
            collection.Add(new Dummy<int>(6));

            var topEx = Observable.Expression(() => collection.TopX(3, d => d.Item));
            var top = topEx.Value;

            Assert.AreEqual(top1, top[0]);
            Assert.AreEqual(top2, top[1]);
            Assert.AreEqual(top3, top[2]);
            Assert.AreEqual(3, top.Length);

            var changed = false;

            topEx.ValueChanged += (o, e) =>
            {
                Assert.IsNotNull(e.OldValue);
                Assert.IsNotNull(e.NewValue);
                changed = true;
            };

            collection.Clear();

            Assert.IsTrue(changed);
            Assert.AreEqual(0, topEx.Value.Length);
        }

        [TestMethod]
        public void TopX_Change_CorrectResult()
        {
            var collection = new NotifyCollection<Dummy<int>>();
            var top1 = new ObservableDummy<int>(43);
            var top2 = new ObservableDummy<int>(42);
            var top3 = new ObservableDummy<int>(3);
            collection.Add(top1);
            collection.Add(top2);
            collection.Add(top3);
            collection.Add(new Dummy<int>(23));
            collection.Add(new Dummy<int>(6));

            var topEx = Observable.Expression(() => collection.TopX(3, d => d.Item));
            var top = topEx.Value;

            Assert.AreEqual(top1, top[0]);
            Assert.AreEqual(top2, top[1]);
            Assert.AreEqual(23, top[2].Item);
            Assert.AreEqual(3, top.Length);

            var changed = false;

            topEx.ValueChanged += (o, e) =>
            {
                Assert.IsNotNull(e.OldValue);
                Assert.IsNotNull(e.NewValue);
                changed = true;
            };

            top3.Item = 12;

            Assert.AreEqual(top, topEx.Value);
            Assert.IsFalse(changed);

            top3.Item = 42;

            Assert.IsTrue(changed);
            Assert.AreEqual(43, topEx.Value[0].Item);
            Assert.AreEqual(42, topEx.Value[1].Item);
            Assert.AreEqual(42, topEx.Value[2].Item);
        }
    }
}
