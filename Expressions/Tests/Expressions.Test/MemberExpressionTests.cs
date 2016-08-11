using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class MemberExpressionTests
    {
        [TestMethod]
        public void MemberExpression_NoObservableSourceMemberChanges_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<int>() { Item = 23 };

            var test = Observable.Expression<int>(() => dummy.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MemberExpression_ObservableSourceMemberChanges_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<int>() { Item = 23 };

            var test = Observable.Expression<int>(() => dummy.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23, e.OldValue);
                Assert.AreEqual(42, e.NewValue);
            };

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void MemberExpression_StaticMember_CorrectResult()
        {
            var test = Observable.Expression(() => EventArgs.Empty);

            Assert.AreSame(EventArgs.Empty, test.Value);
        }

        [TestMethod]
        public void MemberExpression_NoObservableSourceTargetChanges_NoUpdates()
        {
            var update = false;
            var dummy = new Dummy<Dummy<int>>() { Item = new Dummy<int>(23) };

            var test = Observable.Expression<int>(() => dummy.Item.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = new Dummy<int>(42);

            Assert.IsFalse(update);
        }

        [TestMethod]
        public void MemberExpression_ObservableSourceTargetChanges_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<Dummy<int>>() { Item = new Dummy<int>(23) };

            var test = Observable.Expression<int>(() => dummy.Item.Item);

            test.ValueChanged += (o, e) =>
            {
                update = true;
                Assert.AreEqual(23, e.OldValue);
                Assert.AreEqual(42, e.NewValue);
            };

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item = new Dummy<int>(42);

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void MemberExpression_ObservableSourceObservableTargetChanges_Update()
        {
            var update = false;
            var dummy = new ObservableDummy<Dummy<int>>() { Item = new ObservableDummy<int>(23) };

            var test = Observable.Expression<int>(() => dummy.Item.Item);

            test.ValueChanged += (o, e) => update = true;

            Assert.AreEqual(23, test.Value);
            Assert.IsFalse(update);

            dummy.Item.Item = 42;

            Assert.IsTrue(update);
            Assert.AreEqual(42, test.Value);
            update = false;

            dummy.Item = new ObservableDummy<int>(42);

            Assert.IsFalse(update);
            Assert.AreEqual(42, test.Value);

            dummy.Item.Item = 23;

            Assert.IsTrue(update);
            Assert.AreEqual(23, test.Value);
        }
    }
}
