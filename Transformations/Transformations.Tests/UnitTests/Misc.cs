using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations.Core;

namespace NMF.Transformations.Tests.UnitTests
{
    [TestClass]
    public class Misc
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_AbstractTransformationRuleT1_CreateOutput_ThrowsInvalidOperationException()
        {
            var rule = new TestAbstractRuleT1();
            var transformation = new MockTransformation(rule);
            var context = new MockContext(transformation);
            rule.CreateOutput(new object(), context);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_AbstractTransformationRuleT2_CreateOutput_ThrowsInvalidOperationException()
        {
            var rule = new TestAbstractRuleT2();
            var transformation = new MockTransformation(rule);
            var context = new MockContext(transformation);
            rule.CreateOutput(new object(), new object(), context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationContextTrace_Constructor()
        {
            TestTransformationContext.CreateTestInstance();
        }

        private class TestAbstractRuleT1 : AbstractTransformationRule<object, object> { }
        private class TestAbstractRuleT2 : AbstractTransformationRule<object, object, object> { }

        private class TestTransformationContext : TransformationContext
        {
            private TestTransformationContext() : base(null) { }

            public static AbstractTrace CreateTestInstance()
            {
                return new TestTraceClass();
            }

            private class TestTraceClass : TransformationContext.TransformationContextTrace
            {
                public TestTraceClass() : base(null) { }
            }
        }
    }
}
