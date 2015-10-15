using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations.Tests;
using System.Collections.Generic;
using NMF.Transformations;
using System.Linq;
using NMF.Transformations.Core;
using NMF.Utilities;

namespace NMF.Transformations.Tests.UnitTests
{
    public class Dummy { }

    [TestClass]
    public class MultipleDependencyTest
    {

        private TestRuleT1 ruleT1;
        private OtherRuleT1 ruleDependent;
        private MockTransformation transformation;
        private MockContext context;
        private MultipleDependency dependency;

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

            dependency = new MultipleDependency();

            dependency.BaseTransformation = ruleT1;
            dependency.DependencyTransformation = ruleDependent;
            dependency.ExecuteBefore = true;

            c_Test = new MockComputation(new object[] { "a" }, ruleT1, context, "b");
            c_Dependent = new MockComputation(new object[] { new Dummy() }, ruleDependent, context, new Dummy());

            context.Computations.Add(c_Test);
            context.Computations.Add(c_Dependent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_MultipleDependency_Handle_Null()
        {
            dependency.HandleDependency(null);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_1()
        {
            Test_MultipleDependency(false, false, false, false, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_2()
        {
            Test_MultipleDependency(false, false, false, false, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_3()
        {
            Test_MultipleDependency(false, false, false, true, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_4()
        {
            Test_MultipleDependency(false, false, false, true, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_5()
        {
            Test_MultipleDependency(false, false, true, false, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_6()
        {
            Test_MultipleDependency(false, false, true, false, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_7()
        {
            Test_MultipleDependency(false, false, true, true, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_8()
        {
            Test_MultipleDependency(false, false, true, true, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_9()
        {
            Test_MultipleDependency(false, true, false, false, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_10()
        {
            Test_MultipleDependency(false, true, false, false, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_11()
        {
            Test_MultipleDependency(false, true, false, true, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_12()
        {
            Test_MultipleDependency(false, true, false, true, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_13()
        {
            Test_MultipleDependency(false, true, true, false, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_14()
        {
            Test_MultipleDependency(false, true, true, false, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_15()
        {
            Test_MultipleDependency(false, true, true, true, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_16()
        {
            Test_MultipleDependency(false, true, true, true, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_17()
        {
            Test_MultipleDependency(true, false, false, false, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_18()
        {
            Test_MultipleDependency(true, false, false, false, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_19()
        {
            Test_MultipleDependency(true, false, false, true, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_20()
        {
            Test_MultipleDependency(true, false, false, true, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_21()
        {
            Test_MultipleDependency(true, false, true, false, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_22()
        {
            Test_MultipleDependency(true, false, true, false, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_23()
        {
            Test_MultipleDependency(true, false, true, true, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_24()
        {
            Test_MultipleDependency(true, false, true, true, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_25()
        {
            Test_MultipleDependency(true, true, false, false, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_26()
        {
            Test_MultipleDependency(true, true, false, false, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_27()
        {
            Test_MultipleDependency(true, true, false, true, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_28()
        {
            Test_MultipleDependency(true, true, false, true, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_29()
        {
            Test_MultipleDependency(true, true, true, false, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_30()
        {
            Test_MultipleDependency(true, true, true, false, true);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_31()
        {
            Test_MultipleDependency(true, true, true, true, false);
        }

        [TestMethod]
        public void Transformations_MultipleDependency_Handle_32()
        {
            Test_MultipleDependency(true, true, true, true, true);
        }

        public void Test_MultipleDependency(bool filter, bool persistor, bool needOutput, bool delay, bool targetDelay)
        {
            bool selectorCalled = false;
            bool filterCalled = false;
            bool persistorCalled = false;
            var target = new Dummy();
            var targetResult = new Dummy();
            Computation c_Target = null;

            dependency.Selector = comp =>
            {
                selectorCalled = true;
                var l = new List<object[]>();
                var output = new object[] { target };
                l.Add(output);
                return l;
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
                    Assert.IsNotNull(t);
                    var en = t.GetEnumerator();
                    Assert.IsTrue(en.MoveNext());
                    Assert.AreEqual(targetResult, en.Current);
                    Assert.IsFalse(en.MoveNext());
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
                c_Target = context.Computations.Add(ruleDependent, target);
                c_Target.DelayOutput(new OutputDelay() { DelayLevel = 1 });
            }
            else
            {
                c_Target = context.Computations.Add(ruleDependent, target, targetResult);
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
                    Assert.IsFalse(persistorCalled, "The persistor has been called too early!");

                    c_Target.InitializeOutput(targetResult);
                }
                Assert.IsTrue(persistorCalled, "The persistor has not been called");
            }
        }
    }
}
