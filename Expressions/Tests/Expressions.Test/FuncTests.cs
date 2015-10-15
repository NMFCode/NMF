using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NMF.Expressions.Test
{
    [TestClass]
    public class FuncTests
    {
        [TestMethod]
        public void FuncT1_Invoke_ReturnsCorrectResult()
        {
            var test = Observable.Func((object o1) => "42");
            var result = test.Observe(null);
            Assert.AreEqual("42", result.Value);
        }

        [TestMethod]
        public void FuncT2_Invoke_ReturnsCorrectResult()
        {
            var test = Observable.Func((object o1, object o2) => "42");
            var result = test.Observe(null, null);
            Assert.AreEqual("42", result.Value);
        }

        [TestMethod]
        public void FuncT3_Invoke_ReturnsCorrectResult()
        {
            var test = Observable.Func((object o1, object o2, object o3) => "42");
            var result = test.Observe(null, null, null);
            Assert.AreEqual("42", result.Value);
        }

        [TestMethod]
        public void FuncT4_Invoke_ReturnsCorrectResult()
        {
            var test = Observable.Func((object o1, object o2, object o3, object o4) => "42");
            var result = test.Observe(null, null, null, null);
            Assert.AreEqual("42", result.Value);
        }

        [TestMethod]
        public void FuncT5_Invoke_ReturnsCorrectResult()
        {
            var test = Observable.Func((object o1, object o2, object o3, object o4, object o5) => "42");
            var result = test.Observe(null, null, null, null, null);
            Assert.AreEqual("42", result.Value);
        }

        [TestMethod]
        public void FuncT6_Invoke_ReturnsCorrectResult()
        {
            var test = Observable.Func((object o1, object o2, object o3, object o4, object o5, object o6) => "42");
            var result = test.Observe(null, null, null, null, null, null);
            Assert.AreEqual("42", result.Value);
        }

        [TestMethod]
        public void FuncT7_Invoke_ReturnsCorrectResult()
        {
            var test = Observable.Func((object o1, object o2, object o3, object o4, object o5, object o6, object o7) => "42");
            var result = test.Observe(null, null, null, null, null, null, null);
            Assert.AreEqual("42", result.Value);
        }

        [TestMethod]
        public void FuncT8_Invoke_ReturnsCorrectResult()
        {
            var test = Observable.Func((object o1, object o2, object o3, object o4, object o5, object o6, object o7, object o8) => "42");
            var result = test.Observe(null, null, null, null, null, null, null, null);
            Assert.AreEqual("42", result.Value);
        }

        [TestMethod]
        public void FuncT9_Invoke_ReturnsCorrectResult()
        {
            var test = Observable.Func((object o1, object o2, object o3, object o4, object o5, object o6, object o7, object o8, object o9) => "42");
            var result = test.Observe(null, null, null, null, null, null, null, null, null);
            Assert.AreEqual("42", result.Value);
        }
    }
}
