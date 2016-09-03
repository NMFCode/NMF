using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NMF.Transformations.Linq;
using NMF.Expressions;
using NMF.Expressions.Linq;

using System.Collections.Generic;

using NMF.Tests;
using System.Linq.Expressions;
using System.Collections.Specialized;

namespace NMF.Transformations.Tests.UnitTests.Relational
{
    [TestClass]
    public class RelationalTest
    {
        private NotifyCollection<string> source = new NotifyCollection<string>();
        private Func<int, INotifyEnumerable<string>> sourceFunc;

        [TestInitialize]
        public void TestInit()
        {
            sourceFunc = i => source;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_Where_Exception1()
        {
            var result = source.Where(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_Where_Exception2()
        {
            var result = (null as INotifyEnumerable<string>).Where(s => true);
        }

        [TestMethod]
        public void Transformations_Relational_Select1()
        {
            bool nextItemCalled = false;
            int expectedResult = 1;

            var select = source.Select(s => s.Length);
            select.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
                Assert.AreEqual(expectedResult, e.NewItems[0]);
                nextItemCalled = true;
            };

            select.AssertEmpty();

            source.Add("a");

            Assert.IsTrue(nextItemCalled);
            select.AssertContainsOnly(1);

            nextItemCalled = false;
            expectedResult = 2;

            source.Add("aa");
            Assert.IsTrue(nextItemCalled);
            select.AssertContainsOnly(1, 2);

            nextItemCalled = false;
            expectedResult = 0;

            select.Successors.Clear();

            source.Add("");

            Assert.IsFalse(nextItemCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_Select_Exception1()
        {
            var result = source.Select(null as Expression<Func<string, GenericParameterHelper>>);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_Select_Exception2()
        {
            var result = (null as INotifyEnumerable<string>).Select(s => new GenericParameterHelper());
        }
    }
}
