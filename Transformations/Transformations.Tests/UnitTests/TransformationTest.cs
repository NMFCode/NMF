using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations.Tests;
using NMF.Transformations;
using System.Linq;
using NMF.Utilities;
using System.Collections.Generic;

using NMF.Tests;
using NMF.Transformations.Core;

namespace NMF.Transformations.Tests.UnitTests
{
    [TestClass]
    public class TransformationTest
    {
        private TestRuleT1 ruleT1;
        private TestRuleT2 ruleT2;
        private TestRuleTN ruleTN;

        private BaseTestRuleT1 baseT1;
        private BaseTestRuleT2 baseT2;
        private BaseTestRuleTN baseTN;

        private class TestTransformation : Transformation
        {
            private TransformationTest test;

            public TestTransformation(TransformationTest test)
            {
                this.test = test;
            }

            protected override System.Collections.Generic.IEnumerable<GeneralTransformationRule> CreateRules()
            {
                yield return test.ruleT1;
                yield return test.ruleT2;
                yield return test.ruleTN;

                yield return test.baseT1;
                yield return test.baseT2;
                yield return test.baseTN;
            }
        }

        private TestTransformation transformation;

        [TestInitialize]
        public void InitTestContext()
        {
            ruleT1 = new TestRuleT1();
            ruleT2 = new TestRuleT2();
            ruleTN = new TestRuleTN();

            baseT1 = new BaseTestRuleT1();
            baseT2 = new BaseTestRuleT2();
            baseTN = new BaseTestRuleTN();

            transformation = new TestTransformation(this);
        }

        [TestMethod]
        public void Transformations_Transformation_Initialize()
        {
            transformation.Initialize();

            Assert.IsTrue(transformation.IsInitialized);
            Assert.IsTrue(transformation.IsRulesRegistered);

            Assert.IsTrue(transformation.Rules.Contains(ruleT1));
            Assert.IsTrue(transformation.Rules.Contains(ruleT2));
            Assert.IsTrue(transformation.Rules.Contains(ruleTN));
            Assert.IsTrue(transformation.Rules.Contains(baseT1));
            Assert.IsTrue(transformation.Rules.Contains(baseT2));
            Assert.IsTrue(transformation.Rules.Contains(baseTN));

            Assert.AreEqual(6, transformation.Rules.Count());
        }

        [TestMethod]
        public void Transformations_Transformation_GetRuleForRuleType()
        {
            transformation.Initialize();

            Assert.AreEqual(ruleT1, transformation.GetRuleForRuleType(typeof(TestRuleT1)));
            Assert.AreEqual(ruleT2, transformation.GetRuleForRuleType(typeof(TestRuleT2)));
            Assert.AreEqual(ruleTN, transformation.GetRuleForRuleType(typeof(TestRuleTN)));
            Assert.AreEqual(baseT1, transformation.GetRuleForRuleType(typeof(BaseTestRuleT1)));
            Assert.AreEqual(baseT2, transformation.GetRuleForRuleType(typeof(BaseTestRuleT2)));
            Assert.AreEqual(baseTN, transformation.GetRuleForRuleType(typeof(BaseTestRuleTN)));
            Assert.AreEqual(null, transformation.GetRuleForRuleType(typeof(OtherRuleT1)));
        }

        [TestMethod]
        public void Transformations_Transformation_GetRulesForRuleType()
        {
            transformation.Initialize();

            transformation.GetRulesForRuleType(typeof(TestRuleT1)).AssertContainsOnly(ruleT1);
            transformation.GetRulesForRuleType(typeof(TestRuleT2)).AssertContainsOnly(ruleT2);
            transformation.GetRulesForRuleType(typeof(TestRuleTN)).AssertContainsOnly(ruleTN);
            transformation.GetRulesForRuleType(typeof(BaseTestRuleT1)).AssertContainsOnly(baseT1);
            transformation.GetRulesForRuleType(typeof(BaseTestRuleT2)).AssertContainsOnly(baseT2);
            transformation.GetRulesForRuleType(typeof(BaseTestRuleTN)).AssertContainsOnly(baseTN);
            Assert.IsTrue(transformation.GetRulesForRuleType(typeof(OtherRuleT1)).IsNullOrEmpty());
        }

        [TestMethod]
        public void Transformations_Transformation_GetRuleForTypeSignature()
        {
            transformation.Initialize();

            Assert.AreEqual(ruleT1, transformation.GetRuleForTypeSignature(new Type[] { typeof(string) }, typeof(string)));
            Assert.AreEqual(ruleT2, transformation.GetRuleForTypeSignature(new Type[] { typeof(string), typeof(string) }, typeof(string)));
            Assert.AreEqual(ruleTN, transformation.GetRuleForTypeSignature(new Type[] { typeof(string), typeof(string), typeof(string) }, typeof(string)));
            Assert.AreEqual(null, transformation.GetRuleForTypeSignature(new Type[] { typeof(string), typeof(string), typeof(string), typeof(string) }, typeof(string)));
            Assert.AreEqual(null, transformation.GetRuleForTypeSignature(new Type[] { typeof(string) }, typeof(Dummy)));
            Assert.AreEqual(ruleT1, transformation.GetRuleForTypeSignature(new Type[] { typeof(string) }, typeof(object)));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Transformation_GetRuleForTypeSignature_NoInputTypes()
        {
            transformation.Initialize();

            transformation.GetRuleForTypeSignature(null, typeof(void));
        }

        [TestMethod]
        public void Transformations_Transformation_GetRuleForTypeSignature_NoOutputType()
        {
            transformation.Initialize();

            Assert.IsNull(transformation.GetRuleForTypeSignature(new Type[] { typeof(string) }, null));
        }

        [TestMethod]
        public void Transformations_Transformation_GetRulesForTypeSignature()
        {
            transformation.Initialize();

            transformation.GetRulesForTypeSignature(new Type[] { typeof(string) }, typeof(string)).AssertContainsOnly(ruleT1);
            transformation.GetRulesForTypeSignature(new Type[] { typeof(string), typeof(string) }, typeof(string)).AssertContainsOnly(ruleT2);
            transformation.GetRulesForTypeSignature(new Type[] { typeof(string), typeof(string), typeof(string) }, typeof(string)).AssertContainsOnly(ruleTN);
            transformation.GetRulesForTypeSignature(new Type[] { typeof(string), typeof(string), typeof(string), typeof(string) }, typeof(string)).AssertEmpty();
            transformation.GetRulesForTypeSignature(new Type[] { typeof(string) }, typeof(Dummy)).AssertEmpty();
            transformation.GetRulesForTypeSignature(new Type[] { typeof(string) }, typeof(object)).AssertContainsOnly(ruleT1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Transformation_GetRulesForTypeSignature_NoInputTypes()
        {
            transformation.Initialize();

            transformation.GetRulesForTypeSignature(null, typeof(void));
        }

        [TestMethod]
        public void Transformations_Transformation_GetRulesForTypeSignature_NoOutputType()
        {
            transformation.Initialize();

            transformation.GetRulesForTypeSignature(new Type[] { typeof(string) }, null).AssertEmpty();
        }

        [TestMethod]
        public void Transformations_Transformation_MaxOutputDelay_0()
        {
            transformation.Initialize();

            Assert.AreEqual(0, transformation.MaxOutputDelay);
        }

        [TestMethod]
        public void Transformations_Transformation_MaxOutputDelay_1()
        {
            ruleT1.OutputDelayLevel = 1;

            transformation.Initialize();

            Assert.AreEqual(1, transformation.MaxOutputDelay);
        }

        [TestMethod]
        public void Transformations_Transformation_MaxOutputDelay_2()
        {
            ruleT1.OutputDelayLevel = 1;
            ruleT2.OutputDelayLevel = 2;

            transformation.Initialize();

            Assert.AreEqual(2, transformation.MaxOutputDelay);
        }

        [TestMethod]
        public void Transformations_Transformation_MaxOutputDelay_3()
        {
            ruleT1.OutputDelayLevel = 1;
            ruleT2.OutputDelayLevel = 2;
            ruleTN.OutputDelayLevel = 3;

            transformation.Initialize();

            Assert.AreEqual(3, transformation.MaxOutputDelay);
        }

        [TestMethod]
        public void Transformations_Transformation_MaxTransformationDelay_0()
        {
            transformation.Initialize();

            Assert.AreEqual(0, transformation.MaxTransformationDelay);
        }

        [TestMethod]
        public void Transformations_Transformation_MaxTransformationDelay_1()
        {
            ruleT1.TransformationDelayLevel = 1;

            transformation.Initialize();

            Assert.AreEqual(1, transformation.MaxTransformationDelay);
        }

        [TestMethod]
        public void Transformations_Transformation_MaxTransformationDelay_2()
        {
            ruleT1.TransformationDelayLevel = 1;
            ruleT2.TransformationDelayLevel = 2;

            transformation.Initialize();

            Assert.AreEqual(2, transformation.MaxTransformationDelay);
        }

        [TestMethod]
        public void Transformations_Transformation_MaxTransformationDelay_3()
        {
            ruleT1.TransformationDelayLevel = 1;
            ruleT2.TransformationDelayLevel = 2;
            ruleTN.TransformationDelayLevel = 3;

            transformation.Initialize();

            Assert.AreEqual(3, transformation.MaxTransformationDelay);
        }
    }
}
