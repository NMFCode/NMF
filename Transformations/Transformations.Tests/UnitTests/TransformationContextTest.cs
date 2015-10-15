using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations.Tests;
using NMF.Transformations;
using NMF.Transformations.Core;

namespace NMF.Transformations.Tests.UnitTests
{
    public class TestDependency : ITransformationRuleDependency
    {
        public TestDependency(bool executeBefore)
        {
            IsHandled = false;
            this.executeBefore = executeBefore;
        }

        public bool IsHandled { get; private set; }
        private bool executeBefore;

        public void HandleDependency(Computation computation)
        {
            IsHandled = true;
        }

        public bool ExecuteBefore
        {
            get { return executeBefore; }
        }
    }

    public class TestOutputRuleT1 : TransformationRule<string, string>
    {
        public bool OutputCreated { get; set; }
        public bool Transformed { get; set; }

        public override string CreateOutput(string input, ITransformationContext context)
        {
            OutputCreated = true;
            return input;
        }

        public override void Transform(string input, string output, ITransformationContext context)
        {
            Transformed = true;
        }

        public void SetOutputDelayLevel(byte level)
        {
            OutputDelayLevel = level;
        }
    }

    public class TestOutputRuleT2 : TransformationRule<string, string, string>
    {
        public bool Transformed { get; set; }
        public bool OutputCreated { get; set; }

        public override string CreateOutput(string input, string input2, ITransformationContext context)
        {
            OutputCreated = true;
            return input;
        }

        public override void Transform(string input1, string input2, string output, ITransformationContext context)
        {
            Transformed = true;
        }

        public void SetOutputDelayLevel(byte level)
        {
            OutputDelayLevel = level;
        }
    }


    [TestClass]
    public class TransformationContextTest
    {

        private TestOutputRuleT1 ruleT1;
        private TestOutputRuleT2 ruleT2;
        private OtherRuleT1 ruleDependent;
        private MockTransformation transformation;
        private TransformationContext context;


        [TestInitialize]
        public void InitTestContext()
        {
            ruleT1 = new TestOutputRuleT1();
            ruleT2 = new TestOutputRuleT2();
            ruleDependent = new OtherRuleT1();
            transformation = new MockTransformation(ruleT1, ruleT2, ruleDependent);
            transformation.Initialize();
            context = new TransformationContext(transformation);
        }

        [TestMethod]
        public void Transformations_TransformationContext_ExecuteBefore1()
        {
            var dep = new TestDependency(true);
            ruleT1.Dependencies.Add(dep);

            var comp = context.CallTransformation(ruleT1, "a");

            Assert.IsTrue(dep.IsHandled);
        }

        [TestMethod]
        public void Transformations_TransformationContext_ExecuteBefore2()
        {
            var dep = new TestDependency(true);
            ruleT2.Dependencies.Add(dep);

            var comp = context.CallTransformation(ruleT2, "a", "b");

            Assert.IsTrue(dep.IsHandled);
        }

        [TestMethod]
        public void Transformations_TransformationContext_ExecuteAfter1()
        {
            var dep = new TestDependency(false);
            ruleT1.Dependencies.Add(dep);

            var comp = context.CallTransformation(ruleT1, "a");
            context.CallPendingDependencies();

            Assert.IsTrue(dep.IsHandled);
        }

        [TestMethod]
        public void Transformations_TransformationContext_ExecuteAfter2()
        {
            var dep = new TestDependency(false);
            ruleT2.Dependencies.Add(dep);

            var comp = context.CallTransformation(ruleT2, "a", "b");
            context.CallPendingDependencies();

            Assert.IsTrue(dep.IsHandled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationContext_Ctor_Exception()
        {
            context = new TransformationContext(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationContext_CallTransformation_NoTransformationRule()
        {
            context.CallTransformation(null, null, new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationContext_CallTransformation_NoInput()
        {
            context.CallTransformation(ruleT1, null);
        }
    }
}
