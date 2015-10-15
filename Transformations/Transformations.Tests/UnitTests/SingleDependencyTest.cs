using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations.Tests;
using NMF.Transformations;
using NMF.Transformations.Core;

using System.Linq;
using System.Collections.Generic;
using NMF.Utilities;

namespace NMF.Transformations.Tests.UnitTests
{
    [TestClass]
    public class SingleDependencyTest
    {

        private TestRuleT1 ruleT1;
        private OtherRuleT1 ruleDependent;
        private MockTransformation transformation;
        private MockContext context;
        private SingleDependency dependency;

        private MockComputation c_Test;
        private MockComputation c_Dependent;

        [TestInitialize]
        public void InitTestContext()
        {
            ruleT1 = new TestRuleT1();
            ruleDependent = new OtherRuleT1();
            transformation = new MockTransformation(ruleT1, ruleDependent);
            transformation.Initialize();
            context = new MockContext(transformation);

            dependency = new SingleDependency();

            dependency.BaseTransformation = ruleT1;
            dependency.DependencyTransformation = ruleDependent;
            dependency.ExecuteBefore = true;

            c_Test = new MockComputation(new object[] { "a" }, ruleT1, context, "b");
            c_Dependent = new MockComputation(new object[] { new Dummy() }, ruleDependent, context, null);

            context.Computations.Add(c_Test);
            context.Computations.Add(c_Dependent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_SingleDependency_Handle_Null()
        {
            dependency.HandleDependency(null);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_1()
        {
            Test_SingleDependency(false, false, false, false, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_2()
        {
            Test_SingleDependency(false, false, false, false, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_3()
        {
            Test_SingleDependency(false, false, false, true, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_4()
        {
            Test_SingleDependency(false, false, false, true, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_5()
        {
            Test_SingleDependency(false, false, true, false, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_6()
        {
            Test_SingleDependency(false, false, true, false, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_7()
        {
            Test_SingleDependency(false, false, true, true, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_8()
        {
            Test_SingleDependency(false, false, true, true, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_9()
        {
            Test_SingleDependency(false, true, false, false, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_10()
        {
            Test_SingleDependency(false, true, false, false, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_11()
        {
            Test_SingleDependency(false, true, false, true, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_12()
        {
            Test_SingleDependency(false, true, false, true, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_13()
        {
            Test_SingleDependency(false, true, true, false, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_14()
        {
            Test_SingleDependency(false, true, true, false, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_15()
        {
            Test_SingleDependency(false, true, true, true, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_16()
        {
            Test_SingleDependency(false, true, true, true, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_17()
        {
            Test_SingleDependency(true, false, false, false, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_18()
        {
            Test_SingleDependency(true, false, false, false, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_19()
        {
            Test_SingleDependency(true, false, false, true, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_20()
        {
            Test_SingleDependency(true, false, false, true, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_21()
        {
            Test_SingleDependency(true, false, true, false, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_22()
        {
            Test_SingleDependency(true, false, true, false, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_23()
        {
            Test_SingleDependency(true, false, true, true, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_24()
        {
            Test_SingleDependency(true, false, true, true, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_25()
        {
            Test_SingleDependency(true, true, false, false, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_26()
        {
            Test_SingleDependency(true, true, false, false, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_27()
        {
            Test_SingleDependency(true, true, false, true, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_28()
        {
            Test_SingleDependency(true, true, false, true, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_29()
        {
            Test_SingleDependency(true, true, true, false, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_30()
        {
            Test_SingleDependency(true, true, true, false, true);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_31()
        {
            Test_SingleDependency(true, true, true, true, false);
        }

        [TestMethod]
        public void Transformations_SingleDependency_Handle_32()
        {
            Test_SingleDependency(true, true, true, true, true);
        }

        private void Test_SingleDependency(bool filter, bool persistor, bool needOutput, bool delay, bool targetDelay)
        {
            bool selectorCalled = false;
            bool filterCalled = false;
            bool persistorCalled = false;
            Computation c = null;
            var target = new Dummy();
            var targetResult = new Dummy();

            dependency.Selector = comp =>
            {
                selectorCalled = true;
                return new object[] { target };
            };

            if (filter)
            {
                dependency.Filter = comp =>
                {
                    filterCalled = true;
                    return true;
                };
            }

            if (persistor)
            {
                dependency.Persistor = (p, t) =>
                {
                    Assert.AreEqual("b", p);
                    Assert.AreEqual(targetResult, t);
                    persistorCalled = true;
                };
            }

            dependency.NeedOutput = needOutput;

            if (delay)
            {
                c_Test.DelayOutput(new OutputDelay() { DelayLevel = 1 });
            }

            if (targetDelay)
            {
                c = context.Computations.Add(ruleDependent, target);
                c.DelayOutput(new OutputDelay() { DelayLevel = 1 });
            }
            else
            {
                targetResult = null;
            }

            dependency.HandleDependency(c_Test);

            if (delay && needOutput)
            {
                Assert.IsFalse(selectorCalled, "The selector has been called too early");
                Assert.IsFalse(filterCalled, "The filter has been called too early");
            }

            if (delay)
            {
                c_Test.InitializeOutput(c_Test.Output);
            }

            Assert.IsTrue(selectorCalled, "The selector has not been called");
            if (filter)
            {
                Assert.IsTrue(filterCalled, "The filter has not been called");
            }
            Assert.AreEqual(3, context.Computations.Count);

            if (persistor)
            {
                if (targetDelay)
                {
                    Assert.IsFalse(persistorCalled, "The persistor has been called too early");

                    c.InitializeOutput(targetResult);
                }
                Assert.IsTrue(persistorCalled, "The persistor has not been called");
            }
        }
    }
}
