using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Linq;
using NMF.Tests;
using NMF.Transformations.Core;

namespace NMF.Transformations.Tests.UnitTests
{
    [TestClass]
    public class DelayTest
    {
        private TestOutputRuleT1 ruleT1;
        private TestOutputRuleT2 ruleT2;
        private TestOutputRuleT1 otherT1;
        private TestOutputRuleT2 otherT2;
        
        private MockTransformation transformation;
        private TransformationContext context;


        [TestInitialize]
        public void InitTestContext()
        {
            ruleT1 = new TestOutputRuleT1();
            ruleT2 = new TestOutputRuleT2();
            otherT1 = new TestOutputRuleT1();
            otherT2 = new TestOutputRuleT2();

            transformation = new MockTransformation(ruleT1, ruleT2, otherT1, otherT2);
            context = new TransformationContext(transformation);
        }

        [TestMethod]
        public void Transformations_TransformationContext_DelayedSinglePersistorT1_1()
        {
            bool persistorCalled = false;
            ruleT1.SetOutputDelayLevel(2);
            ruleT1.Call(otherT1, persistor: (s1, s2) =>
            {
                Assert.AreEqual(s1, s2);
                persistorCalled = true;
            });

            context.CallTransformation(ruleT1, "a");
            context.CallPendingDependencies();

            Assert.IsFalse(ruleT1.OutputCreated);
            Assert.IsFalse(persistorCalled);
            Assert.IsTrue(otherT1.OutputCreated);

            context.CreateDelayedOutputs();

            Assert.IsTrue(ruleT1.OutputCreated);
            Assert.IsTrue(persistorCalled);
            Assert.IsTrue(otherT1.OutputCreated);
        }

        [TestMethod]
        public void Transformations_TransformationContext_DelayedSinglePersistorT1_2()
        {
            bool persistorCalled = false;
            ruleT1.SetOutputDelayLevel(2);
            ruleT1.Require(otherT1, persistor: (s1, s2) =>
            {
                Assert.AreEqual(s1, s2);
                persistorCalled = true;
            });

            context.CallTransformation(ruleT1, "a");

            Assert.IsFalse(ruleT1.OutputCreated);
            Assert.IsFalse(persistorCalled);
            Assert.IsTrue(otherT1.OutputCreated);

            context.CreateDelayedOutputs();

            Assert.IsTrue(ruleT1.OutputCreated);
            Assert.IsTrue(persistorCalled);
            Assert.IsTrue(otherT1.OutputCreated);
        }

        [TestMethod]
        public void Transformations_TransformationContext_DelayedSinglePersistorT2_1()
        {
            bool persistorCalled = false;
            ruleT2.SetOutputDelayLevel(2);
            ruleT2.Call(otherT2, persistor: (s1, s2) =>
            {
                Assert.AreEqual(s1, s2);
                persistorCalled = true;
            });

            context.CallTransformation(ruleT2, "a", "b");
            context.CallPendingDependencies();

            Assert.IsFalse(ruleT2.OutputCreated);
            Assert.IsFalse(persistorCalled);
            Assert.IsTrue(otherT2.OutputCreated);

            context.CreateDelayedOutputs();

            Assert.IsTrue(ruleT2.OutputCreated);
            Assert.IsTrue(persistorCalled);
            Assert.IsTrue(otherT2.OutputCreated);
        }

        [TestMethod]
        public void Transformations_TransformationContext_DelayedSinglePersistorT2_2()
        {
            bool persistorCalled = false;
            ruleT2.SetOutputDelayLevel(2);
            ruleT2.Require(otherT2, persistor: (s1, s2) =>
            {
                Assert.AreEqual(s1, s2);
                persistorCalled = true;
            });

            context.CallTransformation(ruleT2, "a", "b");

            Assert.IsFalse(ruleT2.OutputCreated);
            Assert.IsFalse(persistorCalled);
            Assert.IsTrue(otherT2.OutputCreated);

            context.CreateDelayedOutputs();

            Assert.IsTrue(ruleT2.OutputCreated);
            Assert.IsTrue(persistorCalled);
            Assert.IsTrue(otherT2.OutputCreated);
        }

        [TestMethod]
        public void Transformations_TransformationContext_DelayedMultiplePersistorT1_1()
        {
            bool persistorCalled = false;
            ruleT1.SetOutputDelayLevel(2);
            ruleT1.CallMany(otherT1, selector: s => Enumerable.Repeat(s, 3), persistor: (s, strings) =>
            {
                strings.AssertSequence("a", "a", "a");
                persistorCalled = true;
            });

            context.CallTransformation(ruleT1, "a");
            context.CallPendingDependencies();

            Assert.IsFalse(ruleT1.OutputCreated);
            Assert.IsFalse(persistorCalled);
            Assert.IsTrue(otherT1.OutputCreated);

            context.CreateDelayedOutputs();

            Assert.IsTrue(ruleT1.OutputCreated);
            Assert.IsTrue(persistorCalled);
            Assert.IsTrue(otherT1.OutputCreated);
        }

        [TestMethod]
        public void Transformations_TransformationContext_DelayedMultiplePersistorT1_2()
        {
            bool persistorCalled = false;
            ruleT1.SetOutputDelayLevel(2);
            ruleT1.RequireMany(otherT1, selector: s => Enumerable.Repeat(s, 3), persistor: (s, strings) =>
            {
                strings.AssertSequence("a", "a", "a");
                persistorCalled = true;
            });

            context.CallTransformation(ruleT1, "a");

            Assert.IsFalse(ruleT1.OutputCreated);
            Assert.IsFalse(persistorCalled);
            Assert.IsTrue(otherT1.OutputCreated);

            context.CreateDelayedOutputs();

            Assert.IsTrue(ruleT1.OutputCreated);
            Assert.IsTrue(persistorCalled);
            Assert.IsTrue(otherT1.OutputCreated);
        }

        [TestMethod]
        public void Transformations_TransformationContext_DelayedMultiplePersistorT2_1()
        {
            bool persistorCalled = false;
            ruleT2.SetOutputDelayLevel(2);
            ruleT2.CallMany(otherT2, selector: (s1,s2) => Enumerable.Repeat(Tuple.Create(s1, s2), 3), persistor: (s, strings) =>
            {
                Assert.AreEqual(3, strings.Count());
                persistorCalled = true;
            });

            context.CallTransformation(ruleT2, "a", "b");
            context.CallPendingDependencies();

            Assert.IsFalse(ruleT2.OutputCreated);
            Assert.IsFalse(persistorCalled);
            Assert.IsTrue(otherT2.OutputCreated);

            context.CreateDelayedOutputs();

            Assert.IsTrue(ruleT2.OutputCreated);
            Assert.IsTrue(persistorCalled);
            Assert.IsTrue(otherT2.OutputCreated);
        }

        [TestMethod]
        public void Transformations_TransformationContext_DelayedMultiplePersistorT2_2()
        {
            bool persistorCalled = false;
            ruleT2.SetOutputDelayLevel(2);
            ruleT2.RequireMany(otherT2, selector: (s1, s2) => Enumerable.Repeat(Tuple.Create(s1, s2), 3), persistor: (s, strings) =>
            {
                Assert.AreEqual(3, strings.Count());
                persistorCalled = true;
            });

            context.CallTransformation(ruleT2, "a", "b");

            Assert.IsFalse(ruleT2.OutputCreated);
            Assert.IsFalse(persistorCalled);
            Assert.IsTrue(otherT2.OutputCreated);

            context.CreateDelayedOutputs();

            Assert.IsTrue(ruleT2.OutputCreated);
            Assert.IsTrue(persistorCalled);
            Assert.IsTrue(otherT2.OutputCreated);
        }
    }
}
