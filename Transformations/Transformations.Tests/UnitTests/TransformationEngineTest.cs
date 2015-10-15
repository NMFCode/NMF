using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

using NMF.Tests;
using NMF.Transformations.Core;

namespace NMF.Transformations.Tests.UnitTests
{
    public class TestPattern : ITransformationPattern, ITransformationPatternContext
    {
        public bool Finished { get; set; }
        public ITransformationContext Context { get; set; }

        public void Finish()
        {
            Finished = true;
        }

        ITransformationPatternContext ITransformationPattern.CreatePattern(ITransformationContext context)
        {
            Context = context;
            return this;
        }

        public void Begin()
        {
        }
    }



    [TestClass]
    public class TransformationEngineTest
    {
        private Transformation transformation;
        private TransformationContext context;
        private TestRuleT1 ruleT1;
        private TestRuleT2 ruleT2;
        private TestRuleTN ruleTN;

        private TestPattern pattern;

        [TestInitialize]
        public void Initialize()
        {
            ruleT1 = new TestRuleT1();
            ruleT2 = new TestRuleT2();
            ruleTN = new TestRuleTN();

            transformation = new MockTransformation(ruleT1, ruleT2, ruleTN);

            pattern = new TestPattern();

            transformation.Patterns.Add(pattern);

            context = new TransformationContext(transformation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_ProcessT1_1()
        {
            TransformationEngine.Process<string>("a", null as Transformation);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_ProcessT1_2()
        {
            TransformationEngine.Process<string>("a", transformation);
            Assert.IsTrue(pattern.Finished);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_ProcessManyT1_1()
        {
            TransformationEngine.ProcessMany<string>(Enumerable.Empty<string>(), null as Transformation);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_ProcessManyT1_2()
        {
            TransformationEngine.ProcessMany<string>(Enumerable.Empty<string>(), transformation);
            Assert.IsTrue(pattern.Finished);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_ProcessT1_3()
        {
            TransformationEngine.Process<string>("a", null as TransformationContext);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_ProcessT1_4()
        {
            TransformationEngine.Process<string>("a", context);
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_ProcessManyT1_3()
        {
            TransformationEngine.ProcessMany<string>(Enumerable.Empty<string>(), null as TransformationContext);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_ProcessManyT1_4()
        {
            TransformationEngine.ProcessMany<string>(Enumerable.Empty<string>(), context);
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_ProcessT1_5()
        {
            TransformationEngine.Process<string>("a", context, ruleT1);
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationEngine_ProcessT1_6()
        {
            TransformationEngine.Process<string>("a", context, new TestRuleT1());
        }

        [TestMethod]
        public void Transformations_TransformationEngine_ProcessManyT1_5()
        {
            TransformationEngine.ProcessMany<string>(Enumerable.Empty<string>(), context, ruleT1);
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationEngine_ProcessManyT1_6()
        {
            TransformationEngine.ProcessMany<string>(Enumerable.Empty<string>(), context, new TestRuleT1());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_TransformT1_1()
        {
            TransformationEngine.Transform<string, string>("a", null as Transformation);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_TransformT1_2()
        {
            TransformationEngine.Transform<string, string>("a", transformation).AssertNull();
            Assert.IsTrue(pattern.Finished);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_TransformManyT1_1()
        {
            TransformationEngine.TransformMany<string, string>(Enumerable.Empty<string>(), null as Transformation);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_TransformManyT1_2()
        {
            TransformationEngine.TransformMany<string, string>(Enumerable.Empty<string>(), transformation).AssertEmpty();
            Assert.IsTrue(pattern.Finished);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_TransformT1_3()
        {
            TransformationEngine.Transform<string, string>("a", null as TransformationContext);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_TransformT1_4()
        {
            TransformationEngine.Transform<string, string>("a", context).AssertNull();
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_TransformManyT1_3()
        {
            TransformationEngine.TransformMany<string, string>(Enumerable.Empty<string>(), null as TransformationContext);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_TransformManyT1_4()
        {
            TransformationEngine.TransformMany<string, string>(Enumerable.Empty<string>(), context).AssertEmpty();
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_TransformT1_5()
        {
            TransformationEngine.Transform<string, string>("a", context, ruleT1).AssertNull();
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationEngine_TransformT1_6()
        {
            TransformationEngine.Transform<string, string>("a", context, new TestRuleT1()).AssertNull();
        }

        [TestMethod]
        public void Transformations_TransformationEngine_TransformManyT1_5()
        {
            TransformationEngine.TransformMany<string, string>(Enumerable.Empty<string>(), context, ruleT1).AssertEmpty();
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationEngine_TransformManyT1_6()
        {
            TransformationEngine.TransformMany<string, string>(Enumerable.Empty<string>(), context, new TestRuleT1()).AssertNull();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_ProcessT2_1()
        {
            TransformationEngine.Process<string, string>("a", "a", null as Transformation);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_ProcessT2_2()
        {
            TransformationEngine.Process<string, string>("a", "a", transformation);
            Assert.IsTrue(pattern.Finished);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_ProcessManyT2_1()
        {
            TransformationEngine.ProcessMany<string, string>(Enumerable.Empty<Tuple<string, string>>(), null as Transformation);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_ProcessManyT2_2()
        {
            TransformationEngine.ProcessMany<string, string>(Enumerable.Empty<Tuple<string, string>>(), transformation);
            Assert.IsTrue(pattern.Finished);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_ProcessT2_3()
        {
            TransformationEngine.Process<string, string>("a", "a", null as TransformationContext);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_ProcessT2_4()
        {
            TransformationEngine.Process<string, string>("a", "a", context);
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_ProcessManyT2_3()
        {
            TransformationEngine.ProcessMany<string, string>(Enumerable.Empty<Tuple<string, string>>(), null as TransformationContext);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_ProcessManyT2_4()
        {
            TransformationEngine.ProcessMany<string, string>(Enumerable.Empty<Tuple<string, string>>(), context);
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_ProcessT2_5()
        {
            TransformationEngine.Process<string, string>("a", "a", context, ruleT2);
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationEngine_ProcessT2_6()
        {
            TransformationEngine.Process<string, string>("a", "a", context, new TestRuleT2());
        }

        [TestMethod]
        public void Transformations_TransformationEngine_ProcessManyT2_5()
        {
            TransformationEngine.ProcessMany<string, string>(Enumerable.Empty<Tuple<string, string>>(), context, ruleT2);
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationEngine_ProcessManyT2_6()
        {
            TransformationEngine.ProcessMany<string, string>(Enumerable.Empty<Tuple<string, string>>(), context, new TestRuleT2());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_TransformT2_1()
        {
            TransformationEngine.Transform<string, string, string>("a", "a", null as Transformation);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_TransformT2_2()
        {
            TransformationEngine.Transform<string, string, string>("a", "a", transformation).AssertNull();
            Assert.IsTrue(pattern.Finished);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_TransformManyT2_1()
        {
            TransformationEngine.TransformMany<string, string, string>(Enumerable.Empty<Tuple<string, string>>(), null as Transformation);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_TransformManyT2_2()
        {
            TransformationEngine.TransformMany<string, string, string>(Enumerable.Empty<Tuple<string, string>>(), transformation).AssertEmpty();
            Assert.IsTrue(pattern.Finished);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_TransformT2_3()
        {
            TransformationEngine.Transform<string, string, string>("a", "a", null as TransformationContext);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_TransformT2_4()
        {
            TransformationEngine.Transform<string, string, string>("a", "a", context).AssertNull();
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationEngine_TransformManyT2_3()
        {
            TransformationEngine.TransformMany<string, string, string>(Enumerable.Empty<Tuple<string, string>>(), null as TransformationContext);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_TransformManyT2_4()
        {
            TransformationEngine.TransformMany<string, string, string>(Enumerable.Empty<Tuple<string, string>>(), context).AssertEmpty();
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        public void Transformations_TransformationEngine_TransformT2_5()
        {
            TransformationEngine.Transform<string, string, string>("a", "a", context, ruleT2).AssertNull();
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationEngine_TransformT2_6()
        {
            TransformationEngine.Transform<string, string, string>("a", "a", context, new TestRuleT2()).AssertNull();
        }

        [TestMethod]
        public void Transformations_TransformationEngine_TransformManyT2_5()
        {
            TransformationEngine.TransformMany<string, string, string>(Enumerable.Empty<Tuple<string, string>>(), context, ruleT2).AssertEmpty();
            Assert.IsTrue(pattern.Finished);
            Assert.AreEqual(context, pattern.Context);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationEngine_TransformManyT2_6()
        {
            TransformationEngine.TransformMany<string, string, string>(Enumerable.Empty<Tuple<string, string>>(), context, new TestRuleT2()).AssertNull();
        }
    }

}
