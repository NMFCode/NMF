using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations.Tests;
using NMF.Transformations;
using NMF.Tests;
using NMF.Utilities;
using System.Collections.Generic;
using NMF.Transformations.Core;

namespace NMF.Transformations.Tests.UnitTests
{
    internal class OtherRuleT2 : TransformationRuleBase<Dummy, Dummy, Dummy>
    {
        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            return new MockComputation(input, this, context);
        }
    }

    internal class OtherRuleTN : TransformationRuleBase<Dummy>
    {
        private static Type[] inputs = { typeof(Dummy), typeof(Dummy), typeof(Dummy) };

        public override Type[] InputType
        {
            get { return inputs; }
        }

        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            return new MockComputation(input, this, context);
        }
    }


    [TestClass]
    public class TransformationRuleT2Test
    {
        private TestRuleT2 ruleT2;
        private OtherRuleT1 ruleDependentT1;
        private OtherRuleT2 ruleDependentT2;
        private MockTransformation transformation;
        private MockContext context;

        [TestInitialize]
        public void InitTestContext()
        {
            ruleT2 = new TestRuleT2();
            ruleDependentT1 = new OtherRuleT1();
            ruleDependentT2 = new OtherRuleT2();
            transformation = new MockTransformation(ruleT2, ruleDependentT1, ruleDependentT2);
            transformation.Initialize();
            context = new MockContext(transformation);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_Call1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.Call(ruleDependentT2, (s1, s2) => { selectorCalled = true; return new Dummy(); },
                (s1, s2) => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, new ComputationContext(context)));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Call1_Exception1()
        {
            ruleT2.Call(null as OtherRuleT2, (s1, s2) => new Dummy(), (s1, s2) => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Call1_Exception2()
        {
            ruleT2.Call(ruleDependentT2, null as Func<string, string, Dummy>, (s1, s2) => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Call1_Exception3()
        {
            ruleT2.Call(ruleDependentT2, (s1, s2) => new Dummy(), null as Func<string, string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_Call1_Exception4()
        {
            ruleT2.Call(ruleDependentT2, (s1, s2) => new Dummy(), (s1, s2) => new Dummy(), null as Action<string, Dummy>);

            Assert.AreEqual(1, ruleT2.Dependencies.Count);

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_Call2()
        {
            bool selectorCalled = false;
            ruleT2.Call<Dummy, Dummy>(ruleDependentT2, (s1, s2) => { selectorCalled = true; return new Dummy(); }, (s1, s2) => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Call2_Exception1()
        {
            ruleT2.Call(null as OtherRuleT2, (s1, s2) => new Dummy(), (s1, s2) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Call2_Exception2()
        {
            ruleT2.Call(ruleDependentT2, null as Func<string, string, Dummy>, (s1, s2) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Call2_Exception3()
        {
            ruleT2.Call(ruleDependentT2, (s1, s2) => new Dummy(), null as Func<string, string, Dummy>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRuleT2_Call3()
        {
            bool persistorCalled = false;
            ruleT2.Call(ruleT2, (s1, s2) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation);
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation);
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNotNull(dependency.Persistor);

            dependency.Persistor(null, null);

            Assert.IsTrue(persistorCalled);

            var input = new object[] { "a", "b" };
            Assert.IsTrue(input.ArrayEquals(dependency.Selector(new MockComputation(input, ruleT2, context))));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Call3_Exception1()
        {
            ruleT2.Call(null as TestRuleT2, (s1, s2) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationRuleT2_Call3_Exception2()
        {
            ruleT2.Call(ruleDependentT2, (s1, s2) => { });
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallByType<Dummy, Dummy, Dummy>((s1, s2) => { selectorCalled = true; return new Dummy(); }, (s1, s2) => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallByType1_Exception1()
        {
            ruleT2.CallByType<Dummy, Dummy, Dummy>(null as Func<string, string, Dummy>, (s1, s2) => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallByType1_Exception2()
        {
            ruleT2.CallByType<Dummy, Dummy, Dummy>((s1, s2) => new Dummy(), null as Func<string, string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_CallByType1_Exception3()
        {
            ruleT2.CallByType<Dummy, Dummy, Dummy>((s1, s2) => new Dummy(), (s1, s2) => new Dummy(), null as Action<string, Dummy>);

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallByType2()
        {
            bool selectorCalled = false;
            ruleT2.CallByType<Dummy, Dummy>((s1, s2) => { selectorCalled = true; return new Dummy(); }, (s1, s2) => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallByType2_Exception1()
        {
            ruleT2.CallByType<Dummy, Dummy>(null as Func<string, string, Dummy>, (s1, s2) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallByType2_Exception2()
        {
            ruleT2.CallByType<Dummy, Dummy>((s1, s2) => new Dummy(), null as Func<string, string, Dummy>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRuleT2_CallByType3()
        {
            bool persistorCalled = false;
            ruleT2.CallByType<string, string, string>((s1, s2) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation);
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation);
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNotNull(dependency.Persistor);

            dependency.Persistor(null, null);

            Assert.IsTrue(persistorCalled);

            var input = new object[] { "a", "b" };
            Assert.IsTrue(input.ArrayEquals(dependency.Selector(new MockComputation(input, ruleT2, context))));
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_CallByType3_Exception1()
        {
            ruleT2.CallByType<string, string, string>(null as Action<string, string>);

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallFor1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallFor<Dummy, Dummy, Dummy>((g1, g2) => { selectorCalled = true; return ""; }, (g1, g2) => { selectorCalled = true; return ""; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { new Dummy(), new Dummy() }, ruleDependentT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallFor1_Exception1()
        {
            ruleT2.CallFor<Dummy, Dummy, Dummy>(null as Func<Dummy, Dummy, string>, (d1, d2) => "a", (o1, o2) => { });
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallFor1_Exception2()
        {
            ruleT2.CallFor<Dummy, Dummy, Dummy>((d1, d2) => "a", null as Func<Dummy, Dummy, string>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_CallFor1_Exception3()
        {
            ruleT2.CallFor<Dummy, Dummy, Dummy>((d1, d2) => "a", (d1, d2) => "b", null as Action<string, Dummy>);

            var dependency = ruleDependentT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallFor2()
        {
            bool selectorCalled = false;
            ruleT2.CallFor<Dummy, Dummy>((g1, g2) => { selectorCalled = true; return ""; }, (g1, g2) => { selectorCalled = true; return ""; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { new Dummy(), new Dummy() }, ruleDependentT2, context));

            Assert.IsTrue(selectorCalled);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallFor2_Exception1()
        {
            ruleT2.CallFor<Dummy, Dummy>(null as Func<Dummy, Dummy, string>, (d1, d2) => "a");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallFor2_Exception2()
        {
            ruleT2.CallFor<Dummy, Dummy>((d1, d2) => "a", null as Func<Dummy, Dummy, string>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallForEach1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallForEach<Dummy, Dummy, Dummy>((g1, g2) => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { new Dummy(), new Dummy() }, ruleDependentT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallForEach1_Exception1()
        {
            ruleT2.CallForEach<Dummy, Dummy, Dummy>(null as Func<Dummy, Dummy, IEnumerable<Tuple<string, string>>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_CallForEach1_Exception2()
        {
            ruleT2.CallForEach<Dummy, Dummy, Dummy>((d1, d2) => Enumerable.Empty<Tuple<string, string>>(), null as Action<Dummy, IEnumerable<string>>);

            var dependency = ruleDependentT2.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallForEach2()
        {
            bool selectorCalled = false;
            ruleT2.CallForEach<Dummy, Dummy>((g1, g2) => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { new Dummy(), new Dummy() }, ruleDependentT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallForEach2_Exception1()
        {
            ruleT2.CallForEach<Dummy, Dummy>(null as Func<Dummy, Dummy, IEnumerable<Tuple<string, string>>>);
        }


        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallFor3()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallFor(ruleDependentT2, (g1, g2) => { selectorCalled = true; return ""; }, (g1, g2) => { selectorCalled = true; return ""; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { new Dummy(), new Dummy() }, ruleDependentT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallFor3_Exception1()
        {
            ruleT2.CallFor(ruleDependentT2, null as Func<Dummy, Dummy, string>, (d1, d2) => "a", (o1, o2) => { });
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallFor3_Exception2()
        {
            ruleT2.CallFor(ruleDependentT2, (d1, d2) => "a", null as Func<Dummy, Dummy, string>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_CallFor3_Exception3()
        {
            ruleT2.CallFor(ruleDependentT2, (d1, d2) => "a", (d1, d2) => "b", null as Action<string, Dummy>);

            var dependency = ruleDependentT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallFor4()
        {
            bool selectorCalled = false;
            ruleT2.CallFor(ruleDependentT2, (g1, g2) => { selectorCalled = true; return ""; }, (g1, g2) => { selectorCalled = true; return ""; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { new Dummy(), new Dummy() }, ruleDependentT2, context));

            Assert.IsTrue(selectorCalled);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallFor4_Exception1()
        {
            ruleT2.CallFor(ruleDependentT2, null as Func<Dummy, Dummy, string>, (d1, d2) => "a");
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallFor4_Exception2()
        {
            ruleT2.CallFor(ruleDependentT2, (d1, d2) => "a", null as Func<Dummy, Dummy, string>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallForEach3()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallForEach(ruleDependentT2, (g1, g2) => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { new Dummy(), new Dummy() }, ruleDependentT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallForEach3_Exception1()
        {
            ruleT2.CallForEach(ruleDependentT2, null as Func<Dummy, Dummy, IEnumerable<Tuple<string, string>>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_CallForEach3_Exception2()
        {
            ruleT2.CallForEach(ruleDependentT2, (d1, d2) => Enumerable.Empty<Tuple<string, string>>(), null as Action<Dummy, IEnumerable<string>>);

            var dependency = ruleDependentT2.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallForEach4()
        {
            bool selectorCalled = false;
            ruleT2.CallForEach(ruleDependentT2, (g1, g2) => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { new Dummy(), new Dummy() }, ruleDependentT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallForEach4_Exception1()
        {
            ruleT2.CallForEach(ruleDependentT2, null as Func<Dummy, Dummy, IEnumerable<Tuple<string, string>>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallMany1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallMany(ruleDependentT2, (g1, g2) => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallMany1_Exception1()
        {
            ruleT2.CallMany(ruleDependentT2, null as Func<string, string, IEnumerable<Tuple<Dummy, Dummy>>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_CallMany1_Exception2()
        {
            ruleT2.CallMany(ruleDependentT2, (s1, s2) => Enumerable.Empty<Tuple<Dummy, Dummy>>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallMany2()
        {
            bool selectorCalled = false;
            ruleT2.CallMany(ruleDependentT2, (g1, g2) => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallMany2_Exception1()
        {
            ruleT2.CallMany(ruleDependentT2, null as Func<string, string, IEnumerable<Tuple<Dummy, Dummy>>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallManyByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallManyByType<Dummy, Dummy, Dummy>((g1, g2) => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallManyByType1_Exception1()
        {
            ruleT2.CallManyByType<Dummy, Dummy, Dummy>(null as Func<string, string, IEnumerable<Tuple<Dummy, Dummy>>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_CallManyByType1_Exception2()
        {
            ruleT2.CallManyByType<Dummy, Dummy, Dummy>((s1, s2) => Enumerable.Empty<Tuple<Dummy, Dummy>>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_CallManyByType2()
        {
            bool selectorCalled = false;
            ruleT2.CallManyByType<Dummy, Dummy>((g1, g2) => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallManyByType2_Exception1()
        {
            ruleT2.CallManyByType<Dummy, Dummy>(null as Func<string, string, IEnumerable<Tuple<Dummy, Dummy>>>);
        }


        [TestMethod]
        public void Transformations_TransformationRuleT2_CallOutputSensitive1()
        {
            var dummy = new Dummy();

            ruleT2.CallOutputSensitive(ruleDependentT1, (s1, s2, o) =>
            {
                Assert.AreEqual("a", s1);
                Assert.AreEqual("b", s2);
                Assert.AreEqual("c", o);
                return dummy;
            });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsTrue(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            var selected = dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context, "c"));

            Assert.IsNotNull(selected);
            Assert.AreEqual(1, selected.Length);
            Assert.AreEqual(dummy, selected[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallOutputSensitive1_Exception1()
        {
            ruleT2.CallOutputSensitive(null as OtherRuleT1, (s1, s2, o) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallOutputSensitive1_Exception2()
        {
            ruleT2.CallOutputSensitive(ruleDependentT1, null as Func<string, string, string, Dummy>);
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_CallOutputSensitive2()
        {
            var dummy1 = new Dummy();
            var dummy2 = new Dummy();

            ruleT2.CallOutputSensitive(ruleDependentT2, (s1, s2, o) =>
            {
                Assert.AreEqual("a", s1);
                Assert.AreEqual("b", s2);
                Assert.AreEqual("c", o);
                return dummy1;
            }, (s1, s2, o) =>
            {
                Assert.AreEqual("a", s1);
                Assert.AreEqual("b", s2);
                Assert.AreEqual("c", o);
                return dummy2;
            });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsTrue(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            var selected = dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context, "c"));

            Assert.IsNotNull(selected);
            Assert.AreEqual(2, selected.Length);
            Assert.AreEqual(dummy1, selected[0]);
            Assert.AreEqual(dummy2, selected[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallOutputSensitive2_Exception1()
        {
            ruleT2.CallOutputSensitive(null as OtherRuleT2, (s1, s2, o) => new Dummy(), (s1, s2, o) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallOutputSensitive2_Exception2()
        {
            ruleT2.CallOutputSensitive(ruleDependentT2, null as Func<string, string, string, Dummy>, (s1, s2, o) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallOutputSensitive2_Exception3()
        {
            ruleT2.CallOutputSensitive(ruleDependentT2, (s1, s2, o) => new Dummy(), null as Func<string, string, string, Dummy>);
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_CallManyOutputSensitive1()
        {
            bool selectorCalled = false;

            ruleT2.CallManyOutputSensitive(ruleDependentT1, (s1, s2, o) =>
            {
                Assert.AreEqual("a", s1);
                Assert.AreEqual("b", s2);
                Assert.AreEqual("c", o);
                selectorCalled = true;
                return Enumerable.Empty<Dummy>();
            });

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsTrue(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context, "c")).AssertEmpty();

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallManyOutputSensitive1_Exception1()
        {
            ruleT2.CallManyOutputSensitive<Dummy>(null as OtherRuleT1, (s1, s2, o) => null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallManyOutputSensitive1_Exception2()
        {
            ruleT2.CallManyOutputSensitive(ruleDependentT1, null as Func<string, string, string, IEnumerable<Dummy>>);
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_CallManyOutputSensitive2()
        {
            bool selectorCalled = false;

            ruleT2.CallManyOutputSensitive(ruleDependentT2, (s1, s2, o) =>
            {
                Assert.AreEqual("a", s1);
                Assert.AreEqual("b", s2);
                Assert.AreEqual("c", o);
                selectorCalled = true;
                return Enumerable.Empty<Tuple<Dummy, Dummy>>();
            });

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsTrue(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context, "c")).AssertEmpty();

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallManyOutputSensitive2_Exception1()
        {
            ruleT2.CallManyOutputSensitive<Dummy, Dummy>(null as OtherRuleT2, (s1, s2, o) => null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_CallManyOutputSensitive2_Exception2()
        {
            ruleT2.CallManyOutputSensitive(ruleDependentT2, null as Func<string, string, string, IEnumerable<Tuple<Dummy, Dummy>>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_InputType()
        {
            Assert.IsNotNull(ruleT2.InputType);
            Assert.AreEqual(2, ruleT2.InputType.Length);
            Assert.AreEqual(typeof(string), ruleT2.InputType[0]);
            Assert.AreEqual(typeof(string), ruleT2.InputType[1]);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_OutputType()
        {
            Assert.IsNotNull(ruleT2.OutputType);
            Assert.AreEqual(typeof(string), ruleT2.OutputType);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_Require1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.Require(ruleDependentT2, (s1, s2) => { selectorCalled = true; return new Dummy(); }, (s1, s2) => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Require1_Exception1()
        {
            ruleT2.Require(null as OtherRuleT2, (s1, s2) => new Dummy(), (s1, s2) => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Require1_Exception2()
        {
            ruleT2.Require(ruleDependentT2, null as Func<string, string, Dummy>, (s1, s2) => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Require1_Exception3()
        {
            ruleT2.Require(ruleDependentT2, (s1, s2) => new Dummy(), null as Func<string, string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_Require2()
        {
            bool selectorCalled = false;
            ruleT2.Require<Dummy, Dummy>(ruleDependentT2, (s1, s2) => { selectorCalled = true; return new Dummy(); }, (s1, s2) => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Require2_Exception1()
        {
            ruleT2.Require(null as OtherRuleT2, (s1, s2) => new Dummy(), (s1, s2) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Require2_Exception2()
        {
            ruleT2.Require(ruleDependentT2, null as Func<string, string, Dummy>, (s1, s2) => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Require2_Exception3()
        {
            ruleT2.Require(ruleDependentT2, (s1, s2) => new Dummy(), null as Func<string, string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRuleT2_Require3()
        {
            bool persistorCalled = false;
            ruleT2.Require(ruleT2, (s1, s2) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation);
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation);
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNotNull(dependency.Persistor);

            dependency.Persistor(null, null);

            Assert.IsTrue(persistorCalled);

            var input = new object[] { "a", "b" };
            Assert.IsTrue(input.ArrayEquals(dependency.Selector(new MockComputation(input, ruleT2, context))));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_Require3_Exception1()
        {
            ruleT2.Require(null as TestRuleT2, (s1, s2) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationRuleT2_Require3_Exception2()
        {
            ruleT2.Require(ruleDependentT2, (s1, s2) => { });
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_RequireByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.RequireByType<Dummy, Dummy, Dummy>((s1, s2) => { selectorCalled = true; return new Dummy(); }, (s1, s2) => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_RequireByType1_Exception1()
        {
            ruleT2.RequireByType<Dummy, Dummy, Dummy>(null as Func<string, string, Dummy>, (s1, s2) => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_RequireByType1_Exception2()
        {
            ruleT2.RequireByType<Dummy, Dummy, Dummy>((s1, s2) => new Dummy(), null as Func<string, string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_RequireByType1_Exception3()
        {
            ruleT2.RequireByType<Dummy, Dummy, Dummy>((s1, s2) => new Dummy(), (s1, s2) => new Dummy(), null as Action<string, Dummy>);

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_RequireByType2()
        {
            bool selectorCalled = false;
            ruleT2.RequireByType<Dummy, Dummy>((s1, s2) => { selectorCalled = true; return new Dummy(); }, (s1, s2) => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_RequireByType2_Exception1()
        {
            ruleT2.RequireByType<Dummy, Dummy>(null as Func<string, string, Dummy>, (s1, s2) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_RequireByType2_Exception2()
        {
            ruleT2.RequireByType<Dummy, Dummy>((s1, s2) => new Dummy(), null as Func<string, string, Dummy>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRuleT2_RequireByType3()
        {
            bool persistorCalled = false;
            ruleT2.RequireByType<string, string, string>((s1, s2) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation);
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation);
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNotNull(dependency.Persistor);

            dependency.Persistor(null, null);

            Assert.IsTrue(persistorCalled);

            var input = new object[] { "a", "b" };
            Assert.IsTrue(input.ArrayEquals(dependency.Selector(new MockComputation(input, ruleT2, context))));
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_RequireByType3_Exception1()
        {
            ruleT2.RequireByType<string, string, string>(null as Action<string, string>);

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_RequireMany1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.RequireMany(ruleDependentT2, (s1, s2) => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_RequireMany1_Exception1()
        {
            ruleT2.RequireMany(ruleDependentT2, null as Func<string, string, IEnumerable<Tuple<Dummy, Dummy>>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_RequireMany1_Exception2()
        {
            ruleT2.RequireMany(ruleDependentT2, (s1, s2) => Enumerable.Empty<Tuple<Dummy, Dummy>>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_RequireMany2()
        {
            bool selectorCalled = false;
            ruleT2.RequireMany(ruleDependentT2, (s1, s2) => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_RequireMany2_Exception1()
        {
            ruleT2.RequireMany(ruleDependentT2, null as Func<string, string, IEnumerable<Tuple<Dummy, Dummy>>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_RequireManyByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.RequireManyByType<Dummy, Dummy, Dummy>((s1, s2) => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_RequireManyByType1_Exception1()
        {
            ruleT2.RequireManyByType<Dummy, Dummy, Dummy>(null as Func<string, string, IEnumerable<Tuple<Dummy, Dummy>>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_RequireManyByType1_Exception2()
        {
            ruleT2.RequireManyByType<Dummy, Dummy, Dummy>((s1, s2) => Enumerable.Empty<Tuple<Dummy, Dummy>>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_RequireManyByType2()
        {
            bool selectorCalled = false;
            ruleT2.RequireManyByType<Dummy, Dummy>((s1, s2) => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_RequireManyByType2_Exception1()
        {
            ruleT2.RequireManyByType<Dummy, Dummy>(null as Func<string, string, IEnumerable<Tuple<Dummy, Dummy>>>);
        }


        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_Call1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.Call(ruleDependentT1, (s1, s2) => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_Call1_Exception1()
        {
            ruleT2.Call(null as OtherRuleT1, (s1, s2) => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_Call1_Exception2()
        {
            ruleT2.Call(ruleDependentT1, null as Func<string, string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_T1_Call1_Exception3()
        {
            ruleT2.Call(ruleDependentT1, (s1, s2) => new Dummy(), null as Action<string, Dummy>);

            Assert.AreEqual(1, ruleT2.Dependencies.Count);

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_Call2()
        {
            bool selectorCalled = false;
            ruleT2.Call(ruleDependentT1, (s1, s2) => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_Call2_Exception1()
        {
            ruleT2.Call(null as OtherRuleT1, (s1, s2) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_Call2_Exception2()
        {
            ruleT2.Call(ruleDependentT1, null as Func<string, string, Dummy>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallByType<Dummy, Dummy>((s1, s2) => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallByType1_Exception1()
        {
            ruleT2.CallByType<Dummy, Dummy>(null as Func<string, string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_T1_CallByType1_Exception2()
        {
            ruleT2.CallByType<Dummy, Dummy>((s1, s2) => new Dummy(), null as Action<string, Dummy>);

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallByType2()
        {
            bool selectorCalled = false;
            ruleT2.CallByType<Dummy>((s1, s2) => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallByType2_Exception1()
        {
            ruleT2.CallByType<Dummy>(null as Func<string, string, Dummy>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallFor1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallFor<Dummy, Dummy>(s => { selectorCalled = true; return ""; }, s => { selectorCalled = true; return ""; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { new Dummy() }, ruleDependentT1, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallFor1_Exception1()
        {
            ruleT2.CallFor<Dummy, Dummy>(null as Func<Dummy, string>, d => "a", (o1, o2) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallFor1_Exception2()
        {
            ruleT2.CallFor<Dummy, Dummy>(d => "a", null as Func<Dummy, string>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_T1_CallFor1_Exception3()
        {
            ruleT2.CallFor<Dummy, Dummy>(d => "a", d => "b", null as Action<string, Dummy>);

            var dependency = ruleDependentT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallFor2()
        {
            bool selectorCalled = false;
            ruleT2.CallFor<Dummy>(g => { selectorCalled = true; return "a"; }, d => { selectorCalled = true; return "b"; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { new Dummy() }, ruleDependentT1, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallFor2_Exception1()
        {
            ruleT2.CallFor<Dummy>(null as Func<Dummy, string>, d => "b");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallFor2_Exception2()
        {
            ruleT2.CallFor<Dummy>(d => "a", null as Func<Dummy, string>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallForEach1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallForEach<Dummy, Dummy>(d => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { new Dummy() }, ruleDependentT1, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallForEach1_Exception1()
        {
            ruleT2.CallForEach<Dummy, Dummy>(null as Func<Dummy, IEnumerable<Tuple<string, string>>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_T1_CallForEach1_Exception2()
        {
            ruleT2.CallForEach<Dummy, Dummy>(g => Enumerable.Empty<Tuple<string, string>>(), null as Action<Dummy, IEnumerable<string>>);

            var dependency = ruleDependentT1.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallForEach2()
        {
            bool selectorCalled = false;
            ruleT2.CallForEach<Dummy>(d => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { new Dummy() }, ruleDependentT1, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallForEach2_Exception1()
        {
            ruleT2.CallForEach<Dummy>(null as Func<Dummy, IEnumerable<Tuple<string, string>>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallFor3()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallFor(ruleDependentT1, d => { selectorCalled = true; return "a"; }, d => { selectorCalled = true; return "b"; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { new Dummy() }, ruleDependentT1, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallFor3_Exception1()
        {
            ruleT2.CallFor(ruleDependentT1, null as Func<Dummy, string>, d => "b", (o1, o2) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallFor3_Exception2()
        {
            ruleT2.CallFor(ruleDependentT1, d => "a", null as Func<Dummy, string>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_T1_CallFor3_Exception3()
        {
            ruleT2.CallFor(ruleDependentT1, d => "a", d => "b", null as Action<string, Dummy>);

            var dependency = ruleDependentT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallFor4()
        {
            bool selectorCalled = false;
            ruleT2.CallFor(ruleDependentT1, d => { selectorCalled = true; return "a"; }, d => { selectorCalled = true; return "b"; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { new Dummy() }, ruleDependentT1, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallFor4_Exception1()
        {
            ruleT2.CallFor(ruleDependentT1, null as Func<Dummy, string>, d => "b");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallFor4_Exception2()
        {
            ruleT2.CallFor(ruleDependentT1, d => "a", null as Func<Dummy, string>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallForEach3()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallForEach(ruleDependentT1, g => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { new Dummy() }, ruleDependentT1, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallForEach3_Exception1()
        {
            ruleT2.CallForEach(ruleDependentT1, null as Func<Dummy, IEnumerable<Tuple<string, string>>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_T1_CallForEach3_Exception2()
        {
            ruleT2.CallForEach(ruleDependentT1, g => Enumerable.Empty<Tuple<string, string>>(), null as Action<Dummy, IEnumerable<string>>);

            var dependency = ruleDependentT1.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallForEach4()
        {
            bool selectorCalled = false;
            ruleT2.CallForEach(ruleDependentT1, g => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { new Dummy() }, ruleDependentT1, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallForEach4_Exception1()
        {
            ruleT2.CallForEach(ruleDependentT1, null as Func<Dummy, IEnumerable<Tuple<string, string>>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallMany1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallMany(ruleDependentT1, (d1, d2) => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallMany1_Exception1()
        {
            ruleT2.CallMany(ruleDependentT1, null as Func<string, string, IEnumerable<Dummy>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_T1_CallMany1_Exception2()
        {
            ruleT2.CallMany(ruleDependentT1, (d1, d2) => Enumerable.Empty<Dummy>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallMany2()
        {
            bool selectorCalled = false;
            ruleT2.CallMany(ruleDependentT1, (d1, d2) => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallMany2_Exception1()
        {
            ruleT2.CallMany(ruleDependentT1, null as Func<string, string, IEnumerable<Dummy>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallManyByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.CallManyByType<Dummy, Dummy>((d1, d2) => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallManyByType1_Exception1()
        {
            ruleT2.CallManyByType<Dummy, Dummy>(null as Func<string, string, IEnumerable<Dummy>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_T1_CallManyByType1_Exception2()
        {
            ruleT2.CallManyByType<Dummy, Dummy>((s1, s2) => Enumerable.Empty<Dummy>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_CallManyByType2()
        {
            bool selectorCalled = false;
            ruleT2.CallManyByType<Dummy>((s1, s2) => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_CallManyByType2_Exception1()
        {
            ruleT2.CallManyByType<Dummy>(null as Func<string, string, IEnumerable<Dummy>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_Require1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.Require(ruleDependentT1, (s1, s2) => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_Require1_Exception1()
        {
            ruleT2.Require(null as OtherRuleT1, (s1, s2) => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_Require1_Exception2()
        {
            ruleT2.Require(ruleDependentT1, null as Func<string, string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_T1_Require1_Exception3()
        {
            ruleT2.Require(ruleDependentT1, (s1, s2) => new Dummy(), null as Action<string, Dummy>);

            Assert.AreEqual(1, ruleT2.Dependencies.Count);

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_Require2()
        {
            bool selectorCalled = false;
            ruleT2.Require(ruleDependentT1, (s1, s2) => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_Require2_Exception1()
        {
            ruleT2.Require(null as OtherRuleT1, (s1, s2) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_Require2_Exception2()
        {
            ruleT2.Require(ruleDependentT1, null as Func<string, string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_RequireByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.RequireByType<Dummy, Dummy>((s1, s2) => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_RequireByType1_Exception1()
        {
            ruleT2.RequireByType<Dummy, Dummy>(null as Func<string, string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_T1_RequireByType1_Exception2()
        {
            ruleT2.RequireByType<Dummy, Dummy>((s1, s2) => new Dummy(), null as Action<string, Dummy>);

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_RequireByType2()
        {
            bool selectorCalled = false;
            ruleT2.RequireByType<Dummy>((s1, s2) => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_RequireByType2_Exception1()
        {
            ruleT2.RequireByType<Dummy>(null as Func<string, string, Dummy>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_RequireMany1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.RequireMany(ruleDependentT1, (s1, s2) => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_RequireMany1_Exception1()
        {
            ruleT2.RequireMany(ruleDependentT1, null as Func<string, string, IEnumerable<Dummy>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_T1_RequireMany1_Exception2()
        {
            ruleT2.RequireMany(ruleDependentT1, (s1, s2) => Enumerable.Empty<Dummy>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_RequireMany2()
        {
            bool selectorCalled = false;
            ruleT2.RequireMany(ruleDependentT1, (s1, s2) => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_RequireMany2_Exception1()
        {
            ruleT2.RequireMany(ruleDependentT1, null as Func<string, string, IEnumerable<Dummy>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_RequireManyByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT2.RequireManyByType<Dummy, Dummy>((s1, s2) => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_RequireManyByType1_Exception1()
        {
            ruleT2.RequireManyByType<Dummy, Dummy>(null as Func<string, string, IEnumerable<Dummy>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRuleT2_T1_RequireManyByType1_Exception2()
        {
            ruleT2.RequireManyByType<Dummy, Dummy>((s1, s2) => Enumerable.Empty<Dummy>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T2")]
        public void Transformations_TransformationRuleT2_T1_RequireManyByType2()
        {
            bool selectorCalled = false;
            ruleT2.RequireManyByType<Dummy>((s1, s2) => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT2.Dependencies.Count);

            var dependency = ruleT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a", "b" }, ruleT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRuleT2_T1_RequireManyByType2_Exception1()
        {
            ruleT2.RequireManyByType<Dummy>(null as Func<string, string, IEnumerable<Dummy>>);
        }
    }
}
