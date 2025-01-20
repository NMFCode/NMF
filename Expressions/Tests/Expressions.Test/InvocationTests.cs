using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class InvocationTests
    {
        [TestMethod]
        public void Test_Invocation_Works_Correctly()
        {
            var add = new Func<int, int>(i => i + 1);
            var dummy = new ObservableDummy<int>(0);
            var changed = false;

            var test = Observable.Expression(() => add(dummy.Item));
            test.ValueChanged += (o, e) =>
            {
                changed = true;
            };

            Assert.AreEqual(1, test.Value);
            Assert.IsFalse(changed);

            dummy.Item = 41;

            Assert.IsTrue(changed);
            Assert.AreEqual(42, test.Value);
        }

        [TestMethod]
        public void Test_Invocation_WorksWhenFuncChanges()
        {
            var dummy = new ObservableDummy<Func<int, int>>(i => i + 1);
            var changed = false;

            var test = Observable.Expression(() => dummy.Item(43));
            test.ValueChanged += (o, e) =>
            {
                changed = true;
            };

            Assert.AreEqual(44, test.Value);
            Assert.IsFalse(changed);

            dummy.Item = i => i - 1;

            Assert.IsTrue(changed);
            Assert.AreEqual(42, test.Value);
        }

        public static int MethodWithProxy(int value)
        {
            return value;
        }
    }
}
