using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions.Test;

namespace NMF.Expressions.Linq.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class RecursiveTest
    {
        private static readonly ObservingFunc<int, int> fac = new ObservingFunc<int, int>(n => n >= 1 ? n * Fac(n - 1) : 1);

        [ObservableProxy(typeof(RecursiveTest), "FacProxy", isRecursive: true)]
        public static int Fac(int n)
        {
            return fac.Evaluate(n);
        }

        public static INotifyValue<int> FacProxy(INotifyValue<int> n)
        {
            return fac.Observe(n);
        }

        [TestMethod]
        public void RecursiveFac()
        {
            var dummy = new ObservableDummy<int>(4);
            var test = Observable.Expression(() => Fac(dummy.Item));
            var resultChanged = false;

            Assert.AreEqual(24, test.Value);

            test.ValueChanged += (o, e) =>
            {
                resultChanged = true;
                Assert.AreEqual(24, e.OldValue);
            };

            dummy.Item = 5;

            Assert.IsTrue(resultChanged);
            Assert.AreEqual(120, test.Value);
        }
    }
}
