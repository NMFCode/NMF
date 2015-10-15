using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations.Tests;
using NMF.Transformations;
using System.Collections.Generic;
using System.Collections;
using NMF.Transformations.Core;

namespace NMF.Transformations.Tests.UnitTests
{
    [TestClass]
    public class GeneralTransformationRuleTest
    {

        private TestRuleT1 ruleT1;
        private OtherRuleT1 ruleDependent;
        private MockTransformation transformation;
        private MockContext context;


        [TestInitialize]
        public void InitTestContext()
        {
            ruleT1 = new TestRuleT1();
            ruleDependent = new OtherRuleT1();
            transformation = new MockTransformation(ruleT1, ruleDependent);
            transformation.Initialize();
            context = new MockContext(transformation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_GeneralTransformationRule_Depend_Exception1()
        {
            ruleT1.Depend(null, c => c.CreateInputArray(), null, null, true, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_GeneralTransformationRule_Depend_Exception2()
        {
            ruleT1.Depend(null, null, ruleT1, null, true, false);
        }

        [TestMethod]
        public void Transformations_GeneralTransformationRule_Depend()
        {
            Predicate<Computation> filter = c => true;
            Func<Computation, object[]> selector = c => c.CreateInputArray();
            Action<object, object> persistor = (a,b) => {};
            ruleT1.Depend(filter, selector, ruleDependent, persistor, true, false);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;
            Assert.AreEqual(ruleT1, dependency.BaseTransformation);
            Assert.AreEqual(ruleDependent, dependency.DependencyTransformation);
            Assert.AreEqual(true, dependency.ExecuteBefore);
            Assert.AreEqual(filter, dependency.Filter);
            Assert.AreEqual(false, dependency.NeedOutput);
            Assert.AreEqual(persistor, dependency.Persistor);
            Assert.AreEqual(selector, dependency.Selector);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_GeneralTransformationRule_DependMany_Exception1()
        {
            ruleT1.DependMany(null, c => null, null, null, true, false);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_GeneralTransformationRule_DependMany_Exception2()
        {
            ruleT1.DependMany(null, null, ruleT1, null, true, false);
        }


        [TestMethod]
        public void Transformations_GeneralTransformationRule_DependMany()
        {
            Predicate<Computation> filter = c => true;
            Func<Computation, IEnumerable<object[]>> selector = c => null;
            Action<object, IEnumerable> persistor = (a, b) => { };
            ruleT1.DependMany(filter, selector, ruleDependent, persistor, true, false);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;
            Assert.AreEqual(ruleT1, dependency.BaseTransformation);
            Assert.AreEqual(ruleDependent, dependency.DependencyTransformation);
            Assert.AreEqual(true, dependency.ExecuteBefore);
            Assert.AreEqual(filter, dependency.Filter);
            Assert.AreEqual(false, dependency.NeedOutput);
            Assert.AreEqual(persistor, dependency.Persistor);
            Assert.AreEqual(selector, dependency.Selector);
        }

        [TestMethod]
        public void Transformations_GeneralTransformationRule_Call1()
        {
            ruleT1.Call(ruleT1);

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies[0] as SingleDependency;
            Assert.AreEqual(ruleT1, dependency.BaseTransformation);
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation);
            Assert.AreEqual(false, dependency.ExecuteBefore);
            Assert.AreEqual(false, dependency.NeedOutput);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_GeneralTransformationRule_Call_MustInherit()
        {
            ruleT1.Call(ruleDependent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_GeneralTransformationRule_Call_NoRule()
        {
            ruleT1.Call(null);
        }

        [TestMethod]
        public void Transformations_GeneralTransformationRule_Require1()
        {
            ruleT1.Require(ruleT1);

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies[0] as SingleDependency;
            Assert.AreEqual(ruleT1, dependency.BaseTransformation);
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation);
            Assert.AreEqual(true, dependency.ExecuteBefore);
            Assert.AreEqual(false, dependency.NeedOutput);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_GeneralTransformationRule_Require_MustInherit()
        {
            ruleT1.Require(ruleDependent);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_GeneralTransformationRule_Require_NoRule()
        {
            ruleT1.Require(null);
        }

        [TestMethod]
        public void Transformations_GeneralTransformationRule_HasCompliantInput_Null()
        {
            Assert.IsFalse(ruleT1.HasCompliantInput(null));
        }

        [TestMethod]
        public void Transformations_GeneralTransformationRule_HasCompliantInput_NoCompliance()
        {
            Assert.IsFalse(ruleT1.HasCompliantInput(context.Computations.Add(ruleDependent, new Dummy())));
        }

        [TestMethod]
        public void Transformations_GeneralTransformationRule_IsInstantiating_Null()
        {
            Assert.IsFalse(ruleT1.HasCompliantInput(null));
        }

        [TestMethod]
        public void Transformations_GeneralTransformationRule_IsInstantiating_NoCompliance()
        {
            Assert.IsFalse(ruleT1.IsInstantiating(context.Computations.Add(ruleDependent, new Dummy())));
        }
    }
}
