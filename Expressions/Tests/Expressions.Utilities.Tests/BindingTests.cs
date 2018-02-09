using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions;
using NMF.Expressions.Test;
using System;
using System.Collections.Generic;
using System.Text;

namespace Expressions.Utilities.Tests
{
    [TestClass]
    public class BindingTests
    {
        [TestMethod]
        public void TestBindings()
        {
            var dummy = new ObservableDummy<int>(42);
            var binding = Binding<Dummy<int>>.Create(d => dummy.Item, d => d.Item);

            var testDummy = new Dummy<int>();
            var b = binding.Bind(testDummy);

            Assert.AreEqual(42, testDummy.Item);

            dummy.Item = 23;

            Assert.AreEqual(23, testDummy.Item);

            b.Dispose();

            dummy.Item = 42;

            Assert.AreEqual(23, testDummy.Item);
        }
    }
}
