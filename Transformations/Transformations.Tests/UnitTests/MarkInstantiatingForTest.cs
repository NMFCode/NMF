using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations;
using NMF.Transformations.Tests;
using NMF.Transformations.Core;

namespace NMF.Transformations.Tests.UnitTests
{

    internal class BaseTestRuleT1 : TransformationRule<object, object>
    {
    }

    internal class BaseTestRuleT2 : TransformationRule<object, object, object>
    {
    }

    internal class BaseTestRuleTN : TransformationRule<object>
    {
        private static Type[] inputs = new Type[] { typeof(object), typeof(object), typeof(object) };

        public override Type[] InputType
        {
            get { return inputs; }
        }
    }



    [TestClass]
    public class MarkInstantiatingForTest
    {

        private TestRuleT1 ruleT1;
        private TestRuleT2 ruleT2;
        private TestRuleTN ruleTN;

        private BaseTestRuleT1 baseT1;
        private BaseTestRuleT2 baseT2;
        private BaseTestRuleTN baseTN;

        private OtherRuleT1 otherRuleT1;
        private OtherRuleT2 otherRuleT2;
        private OtherRuleTN otherRuleTN;

        private MockTransformation transformation;
        private MockContext context;

        private MockComputation c_a;
        private MockComputation c_ab;
        private MockComputation c_abc;

        private MockComputation c_other1;
        private MockComputation c_other2;
        private MockComputation c_otherN;

        [TestInitialize]
        public void InitTestContext()
        {
            ruleT1 = new TestRuleT1();
            ruleT2 = new TestRuleT2();
            ruleTN = new TestRuleTN();

            baseT1 = new BaseTestRuleT1();
            baseT2 = new BaseTestRuleT2();
            baseTN = new BaseTestRuleTN();

            otherRuleT1 = new OtherRuleT1();
            otherRuleT2 = new OtherRuleT2();
            otherRuleTN = new OtherRuleTN();

            transformation = new MockTransformation(
                ruleT1, ruleT2, ruleTN, 
                baseT1, baseT2, baseTN,
                otherRuleT1, otherRuleT2, otherRuleTN);
            transformation.Initialize();

            context = new MockContext(transformation);

            c_a = context.Computations.Add(ruleT1, "a");
            c_ab = context.Computations.Add(ruleT2, "a", "b");
            c_abc = context.Computations.Add(ruleTN, new object[] { "a", "b", "c" });

            c_other1 = context.Computations.Add(otherRuleT1, new Dummy());
            c_other2 = context.Computations.Add(otherRuleT2, new Dummy(), new Dummy());
            c_otherN = context.Computations.Add(otherRuleTN, new object[] { new Dummy(), new Dummy(), new Dummy() });
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationRule_MarkInstantiatingFor_Exception1()
        {
            ruleT1.MarkInstantiatingFor(baseT2);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationRule_MarkInstantiatingFor_Exception2()
        {
            ruleT1.MarkInstantiatingFor(baseTN);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_MarkInstantiatingFor1()
        {
            ruleT1.MarkInstantiatingFor(baseT1);

            Assert.AreEqual(1, baseT1.Children.Count);
            Assert.AreEqual(baseT1, ruleT1.BaseRule);

            Assert.IsTrue(baseT1.Children.Contains(ruleT1));

            var filter = ruleT1.BaseFilter;

            Assert.IsNull(filter);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_MarkInstantiatingFor2()
        {
            bool selectorCalled = false;
            ruleT1.MarkInstantiatingFor(baseT1, s => { selectorCalled = true; return true; });

            Assert.AreEqual(1, baseT1.Children.Count);
            Assert.AreEqual(baseT1, ruleT1.BaseRule);

            Assert.IsTrue(baseT1.Children.Contains(ruleT1));

            var filter = ruleT1.BaseFilter;

            Assert.IsNotNull(filter);

            Assert.IsTrue(ruleT1.IsInstantiating(c_a));
            Assert.IsFalse(ruleT1.IsInstantiating(c_ab));

            Assert.IsTrue(selectorCalled);

            selectorCalled = false;

            Assert.IsFalse(ruleT1.IsInstantiating(c_abc));
            Assert.IsFalse(ruleT1.IsInstantiating(c_other1));
            Assert.IsFalse(ruleT1.IsInstantiating(c_other2));
            Assert.IsFalse(ruleT1.IsInstantiating(c_otherN));

            Assert.IsFalse(selectorCalled);

        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_MarkInstantiatingFor3()
        {
            bool selectorCalled = false;
            ruleT1.MarkInstantiatingFor(baseT1, s => { selectorCalled = true; return false; });

            Assert.AreEqual(1, baseT1.Children.Count);
            Assert.AreEqual(baseT1, ruleT1.BaseRule);

            Assert.IsTrue(baseT1.Children.Contains(ruleT1));

            var filter = ruleT1.BaseFilter;

            Assert.IsNotNull(filter);

            Assert.IsFalse(ruleT1.IsInstantiating(c_a));
            Assert.IsFalse(ruleT1.IsInstantiating(c_ab));

            Assert.IsTrue(selectorCalled);

            selectorCalled = false;

            Assert.IsFalse(ruleT1.IsInstantiating(c_abc));
            Assert.IsFalse(ruleT1.IsInstantiating(c_other1));
            Assert.IsFalse(ruleT1.IsInstantiating(c_other2));
            Assert.IsFalse(ruleT1.IsInstantiating(c_otherN));

            Assert.IsFalse(selectorCalled);
        }

        [TestMethod]
        public void Transformations_TransformationRule_MarkInstantiationFor4()
        {
            ruleT1.MarkInstantiatingFor(baseT1, null);

            Assert.AreEqual(1, baseT1.Children.Count);
            Assert.AreEqual(baseT1, ruleT1.BaseRule);

            Assert.IsTrue(baseT1.Children.Contains(ruleT1));

            var filter = ruleT1.BaseFilter;

            Assert.IsNull(filter);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_MarkInstantiatingFor5()
        {
            ruleT1.MarkInstantiatingFor<object, object>();

            Assert.AreEqual(1, baseT1.Children.Count);
            Assert.AreEqual(baseT1, ruleT1.BaseRule);

            Assert.IsTrue(baseT1.Children.Contains(ruleT1));

            var filter = ruleT1.BaseFilter;

            Assert.IsNull(filter);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_MarkInstantiatingFor6()
        {
            bool selectorCalled = false;
            ruleT1.MarkInstantiatingFor<object, object>(s => { selectorCalled = true; return true; });

            Assert.AreEqual(1, baseT1.Children.Count);
            Assert.AreEqual(baseT1, ruleT1.BaseRule);

            Assert.IsTrue(baseT1.Children.Contains(ruleT1));

            var filter = ruleT1.BaseFilter;

            Assert.IsNotNull(filter);

            Assert.IsTrue(filter(c_a));

            Assert.IsTrue(selectorCalled);

            selectorCalled = false;
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_MarkInstantiatingFor1()
        {
            ruleT2.MarkInstantiatingFor(baseT2);

            Assert.AreEqual(1, baseT2.Children.Count);
            Assert.AreEqual(baseT2, ruleT2.BaseRule);

            Assert.IsTrue(baseT2.Children.Contains(ruleT2));

            var filter = ruleT2.BaseFilter;

            Assert.IsNull(filter);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_MarkInstantiatingFor3()
        {
            bool selectorCalled = false;
            ruleT2.MarkInstantiatingFor(baseT2, (s1, s2) => { selectorCalled = true; return true; });

            Assert.AreEqual(1, baseT2.Children.Count);
            Assert.AreEqual(baseT2, ruleT2.BaseRule);

            Assert.IsTrue(baseT2.Children.Contains(ruleT2));

            var filter = ruleT2.BaseFilter;

            Assert.IsNotNull(filter);

            Assert.IsFalse(ruleT2.IsInstantiating(c_a));
            Assert.IsTrue(ruleT2.IsInstantiating(c_ab));

            Assert.IsTrue(selectorCalled);

            selectorCalled = false;

            Assert.IsFalse(ruleT2.IsInstantiating(c_abc));
            Assert.IsFalse(ruleT2.IsInstantiating(c_other1));
            Assert.IsFalse(ruleT2.IsInstantiating(c_other2));
            Assert.IsFalse(ruleT2.IsInstantiating(c_otherN));

            Assert.IsFalse(selectorCalled);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_MarkInstantiatingFor4()
        {
            bool selectorCalled = false;
            ruleT2.MarkInstantiatingFor(baseT2, (s1, s2) => { selectorCalled = true; return false; });

            Assert.AreEqual(1, baseT2.Children.Count);
            Assert.AreEqual(baseT2, ruleT2.BaseRule);

            Assert.IsTrue(baseT2.Children.Contains(ruleT2));

            var filter = ruleT2.BaseFilter;

            Assert.IsNotNull(filter);

            Assert.IsFalse(ruleT2.IsInstantiating(c_a));
            Assert.IsFalse(ruleT2.IsInstantiating(c_ab));

            Assert.IsTrue(selectorCalled);

            selectorCalled = false;

            Assert.IsFalse(ruleT2.IsInstantiating(c_abc));
            Assert.IsFalse(ruleT2.IsInstantiating(c_other1));
            Assert.IsFalse(ruleT2.IsInstantiating(c_other2));
            Assert.IsFalse(ruleT2.IsInstantiating(c_otherN));

            Assert.IsFalse(selectorCalled);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_MarkInstantiatingFor5()
        {
            ruleT2.MarkInstantiatingFor<object, object, object>();

            Assert.AreEqual(1, baseT2.Children.Count);
            Assert.AreEqual(baseT2, ruleT2.BaseRule);

            Assert.IsTrue(baseT2.Children.Contains(ruleT2));

            var filter = ruleT2.BaseFilter;

            Assert.IsNull(filter);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_MarkInstantiatingFor6()
        {
            bool selectorCalled = false;
            ruleT2.MarkInstantiatingFor<object, object, object>((s1, s2) => { selectorCalled = true; return true; });

            Assert.AreEqual(1, baseT2.Children.Count);
            Assert.AreEqual(baseT2, ruleT2.BaseRule);

            Assert.IsTrue(baseT2.Children.Contains(ruleT2));

            var filter = ruleT2.BaseFilter;

            Assert.IsNotNull(filter);

            Assert.IsFalse(filter(c_a));
            Assert.IsTrue(filter(c_ab));

            Assert.IsTrue(selectorCalled);

            selectorCalled = false;

            Assert.IsFalse(filter(c_abc));
            Assert.IsFalse(filter(c_other1));
            Assert.IsFalse(filter(c_other2));
            Assert.IsFalse(filter(c_otherN));

            Assert.IsFalse(selectorCalled);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationRuleT2_MarkInstantiatingFor_Exception1()
        {
            ruleT2.MarkInstantiatingFor(baseT1);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationRuleT2_MarkInstantiatingFor_Exception2()
        {
            ruleT2.MarkInstantiatingFor(baseTN);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationRuleT2_MarkInstantiatingFor_Exception3()
        {
            ruleT2.MarkInstantiatingFor(baseT1);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationRuleT2_MarkInstantiatingFor_Exception4()
        {
            ruleT2.MarkInstantiatingFor(baseTN);
        }

        [TestMethod]
        [TestCategory("TransformationRule TN")]
        public void Transformations_TransformationRuleTN_MarkInstantiatingFor1()
        {
            ruleTN.MarkInstantiatingFor(baseTN);

            Assert.AreEqual(1, baseTN.Children.Count);
            Assert.AreEqual(baseTN, ruleTN.BaseRule);

            Assert.IsTrue(baseTN.Children.Contains(ruleTN));

            var filter = ruleTN.BaseFilter;

            Assert.IsNull(filter);
        }

        [TestMethod]
        [TestCategory("TransformationRule TN")]
        public void Transformations_TransformationRuleTN_MarkInstantiatingFor3()
        {
            bool selectorCalled = false;
            ruleTN.MarkInstantiatingFor(baseTN, s => { selectorCalled = true; return true; });

            Assert.AreEqual(1, baseTN.Children.Count);
            Assert.AreEqual(baseTN, ruleTN.BaseRule);

            Assert.IsTrue(baseTN.Children.Contains(ruleTN));

            var filter = ruleTN.BaseFilter;

            Assert.IsNotNull(filter);

            Assert.IsFalse(ruleTN.IsInstantiating(c_a));
            Assert.IsFalse(ruleTN.IsInstantiating(c_ab));
            Assert.IsTrue(ruleTN.IsInstantiating(c_abc));

            Assert.IsTrue(selectorCalled);

            selectorCalled = false;

            Assert.IsFalse(ruleTN.IsInstantiating(c_other1));
            Assert.IsFalse(ruleTN.IsInstantiating(c_other2));
            Assert.IsFalse(ruleTN.IsInstantiating(c_otherN));

            Assert.IsFalse(selectorCalled);
        }

        [TestMethod]
        [TestCategory("TransformationRule TN")]
        public void Transformations_TransformationRuleTN_MarkInstantiatingFor4()
        {
            bool selectorCalled = false;
            ruleTN.MarkInstantiatingFor(baseTN, s => { selectorCalled = true; return false; });

            Assert.AreEqual(1, baseTN.Children.Count);
            Assert.AreEqual(baseTN, ruleTN.BaseRule);

            Assert.IsTrue(baseTN.Children.Contains(ruleTN));

            var filter = ruleTN.BaseFilter;

            Assert.IsNotNull(filter);

            Assert.IsFalse(ruleTN.IsInstantiating(c_abc));

            Assert.IsTrue(selectorCalled);

            selectorCalled = false;

            Assert.IsFalse(ruleTN.IsInstantiating(c_a));
            Assert.IsFalse(ruleTN.IsInstantiating(c_ab));
            Assert.IsFalse(ruleTN.IsInstantiating(c_other1));
            Assert.IsFalse(ruleTN.IsInstantiating(c_other2));
            Assert.IsFalse(ruleTN.IsInstantiating(c_otherN));

            Assert.IsFalse(selectorCalled);
        }
    }
}
