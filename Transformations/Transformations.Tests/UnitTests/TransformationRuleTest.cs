using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations;
using NMF.Transformations.Tests;
using NMF.Tests;
using NMF.Utilities;
using System.Linq;
using System.Collections.Generic;
using NMF.Transformations.Core;

namespace NMF.Transformations.Tests.UnitTests
{
    internal class TestRuleT1 : TransformationRuleBase<string, string>
    {
        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            return new MockComputation(input, this, context);
        }

        public new byte OutputDelayLevel
        {
            get
            {
                return base.OutputDelayLevel;
            }
            set
            {
                base.OutputDelayLevel = value;
            }
        }

        public new byte TransformationDelayLevel
        {
            get
            {
                return base.TransformationDelayLevel;
            }
            set
            {
                base.TransformationDelayLevel = value;
            }
        }

        public Computation CreateComputation(IComputationContext context, string input)
        {
            return CreateComputation(new object[] { input }, context);
        }
    }
    internal class TestRuleT2 : TransformationRuleBase<string, string, string>
    {
        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            return new MockComputation(input, this, context);
        }

        public new byte OutputDelayLevel
        {
            get
            {
                return base.OutputDelayLevel;
            }
            set
            {
                base.OutputDelayLevel = value;
            }
        }

        public new byte TransformationDelayLevel
        {
            get
            {
                return base.TransformationDelayLevel;
            }
            set
            {
                base.TransformationDelayLevel = value;
            }
        }

        public Computation CreateComputation(IComputationContext context, string input1, string input2)
        {
            return CreateComputation(new object[] { input1, input2 }, context);
        }
    }
    internal class TestRuleTN : TransformationRuleBase<string>
    {
        private static Type[] types = new Type[] { typeof(string), typeof(string), typeof(string) };

        public override Type[] InputType
        {
            get { return types; }
        }

        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            return new MockComputation(input, this, context);
        }

        public new byte OutputDelayLevel
        {
            get
            {
                return base.OutputDelayLevel;
            }
            set
            {
                base.OutputDelayLevel = value;
            }
        }

        public new byte TransformationDelayLevel
        {
            get
            {
                return base.TransformationDelayLevel;
            }
            set
            {
                base.TransformationDelayLevel = value;
            }
        }

        public Computation CreateComputation(IComputationContext context, params object[] inputs)
        {
            return CreateComputation(inputs, context);
        }
    }
    internal class VoidTestRuleT1 : InPlaceTransformationRuleBase<string>
    {
        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            return new MockComputation(input, this, context);
        }
    }
    internal class VoidTestRuleT2 : InPlaceTransformationRuleBase<string, string>
    {
        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            return new MockComputation(input, this, context);
        }
    }
    internal class OtherRuleT1 : TransformationRuleBase<Dummy, Dummy>
    {
        public override Computation CreateComputation(object[] input, IComputationContext context)
        {
            return new MockComputation(input, this, context);
        }
    }

    [TestClass]
    public class TransformationRuleTest
    {

        private TestRuleT1 ruleT1;
        private TestRuleT2 ruleT2;
        private OtherRuleT1 ruleDependentT1;
        private OtherRuleT2 ruleDependentT2;
        private MockTransformation transformation;
        private MockContext context;

        [TestInitialize]
        public void InitTestContext()
        {
            ruleT1 = new TestRuleT1();
            ruleT2 = new TestRuleT2();
            ruleDependentT1 = new OtherRuleT1();
            ruleDependentT2 = new OtherRuleT2();
            transformation = new MockTransformation(ruleT1, ruleT2, ruleDependentT1, ruleDependentT2);
            transformation.Initialize();
            context = new MockContext(transformation);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_Call1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.Call(ruleDependentT1, s => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT1, "Test"));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Call1_Exception1()
        {
            ruleT1.Call(null as OtherRuleT1, s => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Call1_Exception2()
        {
            ruleT1.Call(ruleDependentT1, null as Func<string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_Call1_Exception3()
        {
            ruleT1.Call(ruleDependentT1, s => new Dummy(), null as Action<string, Dummy>);

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_Call2()
        {
            bool selectorCalled = false;
            ruleT1.Call(ruleDependentT1, s => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            dependency.Selector(context.Computations.Add(ruleT1, "Test"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Call2_Exception1()
        {
            ruleT1.Call(null as OtherRuleT1, s => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Call2_Exception2()
        {
            ruleT1.Call(ruleDependentT1, null as Func<string, Dummy>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_Call3()
        {
            bool persistorCalled = false;
            ruleT1.Call(ruleT1, (s1, s2) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation);
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation);
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNotNull(dependency.Persistor);

            dependency.Persistor(null, null);

            Assert.IsTrue(persistorCalled);

            var input = new object[] { "a" };
            Assert.IsTrue(input.ArrayEquals(dependency.Selector(new MockComputation(input, ruleT1, context))));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Call3_Exception1()
        {
            ruleT1.Call(null as TestRuleT1, (s1, s2) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationRule_Call3_Exception2()
        {
            ruleT1.Call(ruleDependentT1, (s1, s2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_Call4()
        {
            bool selectorCalled = false;
            bool filterCalled = false;
            bool filterResult = true;
            ruleT1.Call(ruleDependentT1, s => { selectorCalled = true; return new Dummy(); }, s => { filterCalled = true; return filterResult; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            dependency.Selector(new MockComputation(new object[] { "a" }, ruleT1, context));

            var c = context.Computations.Add(ruleT1, "a");
            Assert.IsTrue(dependency.Filter(c));
            filterResult = false;
            Assert.IsFalse(dependency.Filter(c));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(filterCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Call4_Exception1()
        {
            ruleT1.Call(ruleDependentT1, null as Func<string, Dummy>, s => true);
        }

        [TestMethod]
        public void Transformations_TransformationRule_Call4_Exception2()
        {
            ruleT1.Call(ruleDependentT1, s => new Dummy(), null as Predicate<string>);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Filter);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_Call5()
        {
            bool filterResult = false;
            ruleT1.Call(ruleT1, s => filterResult);

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation);
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation);
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            var input = new object[] { "a" };
            var c = new MockComputation(input, ruleT1, context);
            Assert.IsTrue(input.ArrayEquals(dependency.Selector(c)));

            Assert.IsNotNull(dependency.Filter);

            Assert.IsFalse(dependency.Filter(c));
            filterResult = true;
            Assert.IsTrue(dependency.Filter(c));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Call5_Exception1()
        {
            ruleT1.Call(null as TestRuleT1, s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationRule_Call5_Exception2()
        {
            ruleT1.Call(ruleDependentT1, s => true);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_Call6()
        {
            ruleT1.Call(ruleT1);

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation);
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation);
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            var input = new object[] { "a" };
            Assert.IsTrue(input.ArrayEquals(dependency.Selector(new MockComputation(input, ruleT1, context))));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Call6_Exception1()
        {
            ruleT1.Call(null as TestRuleT1);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_Call7()
        {
            bool selectorCalled = false;
            bool filterResult = false;
            ruleT1.Call(ruleDependentT1, s => { selectorCalled = true; return new Dummy(); }, s => filterResult);

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Filter);

            var c = context.Computations.Add(ruleT1, "Test");

            dependency.Selector(c);

            Assert.IsTrue(selectorCalled);

            filterResult = false;
            Assert.IsFalse(dependency.Filter(c));
            filterResult = true;
            Assert.IsTrue(dependency.Filter(c));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Call7_Exception1()
        {
            ruleT1.Call(null as OtherRuleT1, s => new Dummy(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Call7_Exception2()
        {
            ruleT1.Call(ruleDependentT1, null as Func<string, Dummy>, s => true);
        }

        [TestMethod]
        public void Transformations_TransformationRule_Call7_Exception3()
        {
            ruleT1.Call(ruleDependentT1, s => new Dummy(), null as Predicate<string>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallByType<Dummy, Dummy>(s => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a" }, ruleT1, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallByType1_Exception1()
        {
            ruleT1.CallByType<Dummy, Dummy>(null as Func<string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_CallByType1_Exception2()
        {
            ruleT1.CallByType<Dummy, Dummy>(s => new Dummy(), null as Action<string, Dummy>);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallByType2()
        {
            bool selectorCalled = false;
            ruleT1.CallByType<Dummy>(s => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a" }, ruleT1, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallByType2_Exception1()
        {
            ruleT1.CallByType<Dummy>(null as Func<string, Dummy>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallByType3()
        {
            bool persistorCalled = false;
            ruleT1.CallByType<string, string>((s1, s2) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation);
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation);
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNotNull(dependency.Persistor);

            dependency.Persistor(null, null);

            Assert.IsTrue(persistorCalled);

            var input = new object[] { "a" };
            Assert.IsTrue(input.ArrayEquals(dependency.Selector(new MockComputation(input, ruleT1, context))));
        }

        [TestMethod]
        public void Transformations_TransformationRule_CallByType3_Exception1()
        {
            ruleT1.CallByType<string, string>(null as Action<string, string>);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallFor1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallFor<Dummy, Dummy>(g => { selectorCalled = true; return ""; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

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
        public void Transformations_TransformationRule_CallFor1_Exception1()
        {
            ruleT1.CallFor<Dummy, Dummy>(null as Func<Dummy, string>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_CallFor1_Exception2()
        {
            ruleT1.CallFor<Dummy, Dummy>(g => "a", null as Action<string, Dummy>);

            var dependency = ruleDependentT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallFor2()
        {
            bool selectorCalled = false;
            ruleT1.CallFor<Dummy>(g => { selectorCalled = true; return ""; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleDependentT1, new Dummy()));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallFor2_Exception1()
        {
            ruleT1.CallFor<Dummy>(null as Func<Dummy, string>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallForEach1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallForEach<Dummy, Dummy>(g => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleDependentT1, new Dummy()));
            
            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallForEach1_Exception1()
        {
            ruleT1.CallForEach<Dummy, Dummy>(null as Func<Dummy, IEnumerable<string>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_CallForEach1_Exception2()
        {
            ruleT1.CallForEach<Dummy, Dummy>(g => Enumerable.Empty<string>(), null as Action<Dummy,IEnumerable<string>>);

            var dependency = ruleDependentT1.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallForEach2()
        {
            bool selectorCalled = false;
            ruleT1.CallForEach<Dummy>(g => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleDependentT1, new Dummy()));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallForEach2_Exception1()
        {
            ruleT1.CallForEach<Dummy>(null as Func<Dummy, IEnumerable<string>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallForEach3()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallForEach(ruleDependentT1, g => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleDependentT1, new Dummy()));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallForEach3_Exception1()
        {
            ruleT1.CallForEach(ruleDependentT1, null as Func<Dummy, IEnumerable<string>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_CallForEach3_Exception2()
        {
            ruleT1.CallForEach(ruleDependentT1, g => Enumerable.Empty<string>(), null as Action<Dummy, IEnumerable<string>>);

            var dependency = ruleDependentT1.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallForEach4()
        {
            bool selectorCalled = false;
            ruleT1.CallForEach(ruleDependentT1, g => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleDependentT1, new Dummy()));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallForEach4_Exception1()
        {
            ruleT1.CallForEach(ruleDependentT1, null as Func<Dummy, IEnumerable<string>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallFor3()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallFor(ruleDependentT1, g => { selectorCalled = true; return ""; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleDependentT1, new Dummy()));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallFor3_Exception1()
        {
            ruleT1.CallFor(ruleDependentT1, null as Func<Dummy, string>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_CallFor3_Exception2()
        {
            ruleT1.CallFor(ruleDependentT1, g => "a", null as Action<string, Dummy>);

            var dependency = ruleDependentT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallFor4()
        {
            bool selectorCalled = false;
            ruleT1.CallFor(ruleDependentT1, g => { selectorCalled = true; return ""; });

            Assert.AreEqual(1, ruleDependentT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleDependentT1, new Dummy()));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallFor4_Exception1()
        {
            ruleT1.CallFor(ruleDependentT1, null as Func<Dummy, string>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallMany1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallMany(ruleDependentT1, g => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT1, "a"));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallMany1_Exception1()
        {
            ruleT1.CallMany(ruleDependentT1, null as Func<string, IEnumerable<Dummy>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_CallMany1_Exception2()
        {
            ruleT1.CallMany(ruleDependentT1, s => Enumerable.Empty<Dummy>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallMany2()
        {
            bool selectorCalled = false;
            ruleT1.CallMany(ruleDependentT1, g => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleT1, "a"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallMany2_Exception1()
        {
            ruleT1.CallMany(ruleDependentT1, null as Func<string, IEnumerable<Dummy>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallManyByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallManyByType<Dummy, Dummy>(g => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT1, "a"));
        
            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallManyByType1_Exception1()
        {
            ruleT1.CallManyByType<Dummy, Dummy>(null as Func<string, IEnumerable<Dummy>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_CallManyByType1_Exception2()
        {
            ruleT1.CallManyByType<Dummy, Dummy>(s => Enumerable.Empty<Dummy>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_CallManyByType2()
        {
            bool selectorCalled = false;
            ruleT1.CallManyByType<Dummy>(g => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleT1, "a"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallManyByType2_Exception1()
        {
            ruleT1.CallManyByType<Dummy>(null as Func<string, IEnumerable<Dummy>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_InputType()
        {
            Assert.IsNotNull(ruleT1.InputType);
            Assert.AreEqual(1, ruleT1.InputType.Length);
            Assert.AreEqual(typeof(string), ruleT1.InputType[0]);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_OutputType()
        {
            Assert.IsNotNull(ruleT1.OutputType);
            Assert.AreEqual(typeof(string), ruleT1.OutputType);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_Require1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.Require(ruleDependentT1, s => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT1, "a"));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Require1_Exception1()
        {
            ruleT1.Require(null as OtherRuleT1, s => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Require1_Exception2()
        {
            ruleT1.Require(ruleDependentT1, null as Func<string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_Require1_Exception3()
        {
            ruleT1.Require(ruleDependentT1, s => new Dummy(), null as Action<string, Dummy>);

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_Require2()
        {
            bool selectorCalled = false;
            ruleT1.Require(ruleDependentT1, s => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            dependency.Selector(context.Computations.Add(ruleT1, "a"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Require2_Exception1()
        {
            ruleT1.Require(null as OtherRuleT1, s => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Require2_Exception2()
        {
            ruleT1.Require(ruleDependentT1, null as Func<string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_Require3()
        {
            bool persistorCalled = false;
            ruleT1.Require(ruleT1, (s1, s2) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation);
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation);
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNotNull(dependency.Persistor);

            dependency.Persistor(null, null);

            Assert.IsTrue(persistorCalled);

            var input = new object[] { "a" };
            Assert.IsTrue(input.ArrayEquals(dependency.Selector(new MockComputation(input, ruleT1, context))));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Require3_Exception1()
        {
            ruleT1.Require(null as TestRuleT1, (s1, s2) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationRule_Require3_Exception2()
        {
            ruleT1.Require(ruleDependentT1, (s1, s2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_Require4()
        {
            bool selectorCalled = false;
            bool filterCalled = false;
            bool filterResult = true;
            ruleT1.Require(ruleDependentT1, s => { selectorCalled = true; return new Dummy(); }, s => { filterCalled = true; return filterResult; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            dependency.Selector(context.Computations.Add(ruleT1, "a"));

            var c = context.Computations.Add(ruleT1, "a");
            Assert.IsTrue(dependency.Filter(c));
            filterResult = false;
            Assert.IsFalse(dependency.Filter(c));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(filterCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Require4_Exception1()
        {
            ruleT1.Require(ruleDependentT1, null as Func<string, Dummy>, s => true);
        }

        [TestMethod]
        public void Transformations_TransformationRule_Require4_Exception2()
        {
            ruleT1.Require(ruleDependentT1, s => new Dummy(), null as Predicate<string>);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Filter);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_Require5()
        {
            bool filterResult = false;
            ruleT1.Require(ruleT1, s => filterResult);

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation);
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation);
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            var input = new object[] { "a" };
            var c = new MockComputation(input, ruleT1, context);
            Assert.IsTrue(input.ArrayEquals(dependency.Selector(c)));

            Assert.IsNotNull(dependency.Filter);

            Assert.IsFalse(dependency.Filter(c));
            filterResult = true;
            Assert.IsTrue(dependency.Filter(c));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Require5_Exception1()
        {
            ruleT1.Require(null as TestRuleT1, s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Transformations_TransformationRule_Require5_Exception2()
        {
            ruleT1.Require(ruleDependentT1, s => true);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_Require6()
        {
            ruleT1.Require(ruleT1);

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation);
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation);
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            var input = new object[] { "a" };
            Assert.IsTrue(input.ArrayEquals(dependency.Selector(new MockComputation(input, ruleT1, context))));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Require6_Exception1()
        {
            ruleT1.Require(null as TestRuleT1);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_Require7()
        {
            bool selectorRequireed = false;
            bool filterResult = false;
            ruleT1.Require(ruleDependentT1, s => { selectorRequireed = true; return new Dummy(); }, s => filterResult);

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Filter);

            var c = context.Computations.Add(ruleT1, "Test");

            dependency.Selector(c);

            Assert.IsTrue(selectorRequireed);

            filterResult = false;
            Assert.IsFalse(dependency.Filter(c));
            filterResult = true;
            Assert.IsTrue(dependency.Filter(c));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Require7_Exception1()
        {
            ruleT1.Require(null as OtherRuleT1, s => new Dummy(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_Require7_Exception2()
        {
            ruleT1.Require(ruleDependentT1, null as Func<string, Dummy>, s => true);
        }

        [TestMethod]
        public void Transformations_TransformationRule_Require7_Exception3()
        {
            ruleT1.Require(ruleDependentT1, s => new Dummy(), null as Predicate<string>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_RequireByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.RequireByType<Dummy, Dummy>(s => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT1, "a"));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_RequireByType1_Exception1()
        {
            ruleT1.RequireByType<Dummy, Dummy>(null as Func<string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_RequireByType1_Exception2()
        {
            ruleT1.RequireByType<Dummy, Dummy>(s => new Dummy(), null as Action<string, Dummy>);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_RequireByType2()
        {
            bool selectorCalled = false;
            ruleT1.RequireByType<Dummy>(s => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleT1, "a"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_RequireByType2_Exception1()
        {
            ruleT1.RequireByType<Dummy>(null as Func<string, Dummy>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_RequireByType3()
        {
            bool persistorCalled = false;
            ruleT1.RequireByType<string, string>((s1, s2) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation);
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation);
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNotNull(dependency.Persistor);

            dependency.Persistor(null, null);

            Assert.IsTrue(persistorCalled);

            var input = new object[] { "a" };
            Assert.IsTrue(input.ArrayEquals(dependency.Selector(new MockComputation(input, ruleT1, context))));
        }

        [TestMethod]
        public void Transformations_TransformationRule_RequireByType3_Exception1()
        {
            ruleT1.RequireByType<string, string>(null as Action<string, string>);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_RequireMany1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.RequireMany(ruleDependentT1, g => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT1, "a"));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_RequireMany1_Exception1()
        {
            ruleT1.RequireMany(ruleDependentT1, null as Func<string, IEnumerable<Dummy>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_RequireMany1_Exception2()
        {
            ruleT1.RequireMany(ruleDependentT1, s => Enumerable.Empty<Dummy>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_RequireMany2()
        {
            bool selectorCalled = false;
            ruleT1.RequireMany(ruleDependentT1, g => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleT1, "a"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_RequireMany2_Exception1()
        {
            ruleT1.RequireMany(ruleDependentT1, null as Func<string, IEnumerable<Dummy>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_RequireManyByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.RequireManyByType<Dummy, Dummy>(g => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT1, "a"));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_RequireManyByType1_Exception1()
        {
            ruleT1.RequireManyByType<Dummy, Dummy>(null as Func<string, IEnumerable<Dummy>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_RequireManyByType1_Exception2()
        {
            ruleT1.RequireManyByType<Dummy, Dummy>(s => Enumerable.Empty<Dummy>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_RequireManyByType2()
        {
            bool selectorCalled = false;
            ruleT1.RequireManyByType<Dummy>(g => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleT1, "a"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_RequireManyByType2_Exception1()
        {
            ruleT1.RequireManyByType<Dummy>(null as Func<string, IEnumerable<Dummy>>);
        }



        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_Call1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.Call(ruleDependentT2, s => { selectorCalled = true; return new Dummy(); },
                s => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Call1_Exception1()
        {
            ruleT1.Call(null as OtherRuleT2, s => new Dummy(), s => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Call1_Exception2()
        {
            ruleT1.Call(ruleDependentT2, null as Func<string, Dummy>, s => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Call1_Exception3()
        {
            ruleT1.Call(ruleDependentT2, s => new Dummy(), null as Func<string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_Call1_Exception4()
        {
            ruleT1.Call(ruleDependentT2, s => new Dummy(), s => new Dummy(), null as Action<string, Dummy>);

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_Call2()
        {
            bool selectorCalled = false;
            ruleT1.Call<Dummy, Dummy>(ruleDependentT2, s => { selectorCalled = true; return new Dummy(); }, s => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Call2_Exception1()
        {
            ruleT1.Call(null as OtherRuleT2, s => new Dummy(), s => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Call2_Exception2()
        {
            ruleT1.Call(ruleDependentT2, null as Func<string, Dummy>, s => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Call2_Exception3()
        {
            ruleT1.Call(ruleDependentT2, s => new Dummy(), null as Func<string, Dummy>);
        }



        [TestMethod]
        public void Transformations_TransformationRule_T2_Call3()
        {
            bool selectorCalled = false;
            bool filterCalled = false;
            bool filterResult = true;
            ruleT1.Call(ruleDependentT2, s => { selectorCalled = true; return new Dummy(); },
                s => { selectorCalled = true; return new Dummy(); }, s => { filterCalled = true; return filterResult; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            dependency.Selector(new MockComputation(new object[] { "a" }, ruleT1, context));

            var c = context.Computations.Add(ruleT1, "a");
            Assert.IsTrue(dependency.Filter(c));
            filterResult = false;
            Assert.IsFalse(dependency.Filter(c));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(filterCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Call3_Exception1()
        {
            ruleT1.Call(ruleDependentT2, null as Func<string, Dummy>, s => new Dummy(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Call3_Exception2()
        {
            ruleT1.Call(ruleDependentT2, s => new Dummy(), null as Func<string, Dummy>, s => true);
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_Call3_Exception3()
        {
            ruleT1.Call(ruleDependentT2, s => new Dummy(), s => new Dummy(), null as Predicate<string>);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Filter);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallByType<Dummy, Dummy, Dummy>(s => { selectorCalled = true; return new Dummy(); }, s => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_CallByType1_Exception1()
        {
            ruleT1.CallByType<Dummy, Dummy, Dummy>(null as Func<string, Dummy>, s => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_CallByType1_Exception2()
        {
            ruleT1.CallByType<Dummy, Dummy, Dummy>(s => new Dummy(), null as Func<string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_CallByType1_Exception3()
        {
            ruleT1.CallByType<Dummy, Dummy, Dummy>(s => new Dummy(), s => new Dummy(), null as Action<string, Dummy>);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallByType2()
        {
            bool selectorCalled = false;
            ruleT1.CallByType<Dummy, Dummy>(s => { selectorCalled = true; return new Dummy(); }, s => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_CallByType2_Exception1()
        {
            ruleT1.CallByType<Dummy, Dummy>(null as Func<string, Dummy>, s => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_CallByType2_Exception2()
        {
            ruleT1.CallByType<Dummy, Dummy>(s => new Dummy(), null as Func<string, Dummy>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallFor1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallFor<Dummy, Dummy, Dummy>((g1, g2) => { selectorCalled = true; return ""; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

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
        public void Transformations_TransformationRule_T2_CallFor1_Exception1()
        {
            ruleT1.CallFor<Dummy, Dummy, Dummy>(null as Func<Dummy, Dummy, string>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_CallFor1_Exception2()
        {
            ruleT1.CallFor<Dummy, Dummy, Dummy>((d1, d2) => "a", null as Action<string, Dummy>);

            var dependency = ruleDependentT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallFor2()
        {
            bool selectorCalled = false;
            ruleT1.CallFor<Dummy, Dummy>((g1, g2) => { selectorCalled = true; return ""; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { new Dummy(), new Dummy() }, ruleDependentT2, context));

            Assert.IsTrue(selectorCalled);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_CallFor2_Exception1()
        {
            ruleT1.CallFor<Dummy, Dummy>(null as Func<Dummy, Dummy, string>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallForEach1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallForEach<Dummy, Dummy, Dummy>((g1, g2) => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

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
        public void Transformations_TransformationRule_T2_CallForEach1_Exception1()
        {
            ruleT1.CallForEach<Dummy, Dummy, Dummy>(null as Func<Dummy, Dummy, IEnumerable<string>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_CallForEach1_Exception2()
        {
            ruleT1.CallForEach<Dummy, Dummy, Dummy>((d1, d2) => Enumerable.Empty<string>(), null as Action<Dummy, IEnumerable<string>>);

            var dependency = ruleDependentT2.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallForEach2()
        {
            bool selectorCalled = false;
            ruleT1.CallForEach<Dummy, Dummy>((g1, g2) => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { new Dummy(), new Dummy() }, ruleDependentT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_CallForEach2_Exception1()
        {
            ruleT1.CallForEach<Dummy, Dummy>(null as Func<Dummy, Dummy, IEnumerable<string>>);
        }


        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallFor3()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallFor(ruleDependentT2, (g1, g2) => { selectorCalled = true; return ""; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

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
        public void Transformations_TransformationRule_T2_CallFor3_Exception1()
        {
            ruleT1.CallFor(ruleDependentT2, null as Func<Dummy, Dummy, string>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_CallFor3_Exception2()
        {
            ruleT1.CallFor(ruleDependentT2, (d1, d2) => "a", null as Action<string, Dummy>);

            var dependency = ruleDependentT2.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallFor4()
        {
            bool selectorCalled = false;
            ruleT1.CallFor(ruleDependentT2, (g1, g2) => { selectorCalled = true; return ""; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { new Dummy(), new Dummy() }, ruleDependentT2, context));

            Assert.IsTrue(selectorCalled);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_CallFor4_Exception1()
        {
            ruleT1.CallFor(ruleDependentT2, null as Func<Dummy, Dummy, string>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallForEach3()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallForEach(ruleDependentT2, (g1, g2) => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

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
        public void Transformations_TransformationRule_T2_CallForEach3_Exception1()
        {
            ruleT1.CallForEach(ruleDependentT2, null as Func<Dummy, Dummy, IEnumerable<string>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_CallForEach3_Exception2()
        {
            ruleT1.CallForEach(ruleDependentT2, (d1, d2) => Enumerable.Empty<string>(), null as Action<Dummy, IEnumerable<string>>);

            var dependency = ruleDependentT2.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallForEach4()
        {
            bool selectorCalled = false;
            ruleT1.CallForEach(ruleDependentT2, (g1, g2) => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleDependentT2.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleDependentT2.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleDependentT2, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { new Dummy(), new Dummy() }, ruleDependentT2, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_CallForEach4_Exception1()
        {
            ruleT1.CallForEach(ruleDependentT2, null as Func<Dummy, Dummy, IEnumerable<string>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallMany1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallMany(ruleDependentT2, s => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_CallMany1_Exception1()
        {
            ruleT1.CallMany(ruleDependentT2, null as Func<string, IEnumerable<Tuple<Dummy, Dummy>>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_CallMany1_Exception2()
        {
            ruleT1.CallMany(ruleDependentT2, s => Enumerable.Empty<Tuple<Dummy, Dummy>>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallMany2()
        {
            bool selectorCalled = false;
            ruleT1.CallMany(ruleDependentT2, s => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_CallMany2_Exception1()
        {
            ruleT1.CallMany(ruleDependentT2, null as Func<string, IEnumerable<Tuple<Dummy, Dummy>>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallManyByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.CallManyByType<Dummy, Dummy, Dummy>(s => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_CallManyByType1_Exception1()
        {
            ruleT1.CallManyByType<Dummy, Dummy, Dummy>(null as Func<string, IEnumerable<Tuple<Dummy, Dummy>>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_CallManyByType1_Exception2()
        {
            ruleT1.CallManyByType<Dummy, Dummy, Dummy>(s => Enumerable.Empty<Tuple<Dummy, Dummy>>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_CallManyByType2()
        {
            bool selectorCalled = false;
            ruleT1.CallManyByType<Dummy, Dummy>(s => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_CallManyByType2_Exception1()
        {
            ruleT1.CallManyByType<Dummy, Dummy>(null as Func<string, IEnumerable<Tuple<Dummy, Dummy>>>);
        }

        [TestMethod]
        public void Transformations_TransformationRule_CallOutputSensitive1()
        {
            var dummy = new Dummy();

            ruleT1.CallOutputSensitive(ruleDependentT1, (s, o) =>
            {
                Assert.AreEqual("a", s);
                Assert.AreEqual("b", o);
                return dummy;
            });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsTrue(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            var selected = dependency.Selector(new MockComputation(new object[] { "a" }, ruleT1, context, "b"));

            Assert.IsNotNull(selected);
            Assert.AreEqual(1, selected.Length);
            Assert.AreEqual(dummy, selected[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallOutputSensitive1_Exception1()
        {
            ruleT1.CallOutputSensitive(null as OtherRuleT1, (s, o) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallOutputSensitive1_Exception2()
        {
            ruleT1.CallOutputSensitive(ruleDependentT1, null as Func<string, string, Dummy>);
        }

        [TestMethod]
        public void Transformations_TransformationRule_CallOutputSensitive2()
        {
            var dummy1 = new Dummy();
            var dummy2 = new Dummy();

            ruleT1.CallOutputSensitive(ruleDependentT2, (s, o) =>
            {
                Assert.AreEqual("a", s);
                Assert.AreEqual("c", o);
                return dummy1;
            }, (s, o) =>
            {
                Assert.AreEqual("a", s);
                Assert.AreEqual("c", o);
                return dummy2;
            });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsTrue(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            var selected = dependency.Selector(new MockComputation(new object[] { "a" }, ruleT1, context, "c"));

            Assert.IsNotNull(selected);
            Assert.AreEqual(2, selected.Length);
            Assert.AreEqual(dummy1, selected[0]);
            Assert.AreEqual(dummy2, selected[1]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallOutputSensitive2_Exception1()
        {
            ruleT1.CallOutputSensitive(null as OtherRuleT2, (s, o) => new Dummy(), (s,o) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallOutputSensitive2_Exception2()
        {
            ruleT1.CallOutputSensitive(ruleDependentT2, null as Func<string, string, Dummy>, (s,o) => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallOutputSensitive2_Exception3()
        {
            ruleT1.CallOutputSensitive(ruleDependentT2, (s,o) => new Dummy(), null as Func<string, string, Dummy>);
        }

        [TestMethod]
        public void Transformations_TransformationRule_CallManyOutputSensitive1()
        {
            bool selectorCalled = false;

            ruleT1.CallManyOutputSensitive(ruleDependentT1, (s, o) =>
            {
                Assert.AreEqual("a", s);
                Assert.AreEqual("c", o);
                selectorCalled = true;
                return Enumerable.Empty<Dummy>();
            });

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT1, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsTrue(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a" }, ruleT1, context, "c")).AssertEmpty();

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallManyOutputSensitive1_Exception1()
        {
            ruleT1.CallManyOutputSensitive<Dummy>(null as OtherRuleT1, (s, o) => null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallManyOutputSensitive1_Exception2()
        {
            ruleT1.CallManyOutputSensitive(ruleDependentT1, null as Func<string, string, IEnumerable<Dummy>>);
        }

        [TestMethod]
        public void Transformations_TransformationRule_CallManyOutputSensitive2()
        {
            bool selectorCalled = false;

            ruleT1.CallManyOutputSensitive(ruleDependentT2, (s, o) =>
            {
                Assert.AreEqual("a", s);
                Assert.AreEqual("c", o);
                selectorCalled = true;
                return Enumerable.Empty<Tuple<Dummy, Dummy>>();
            });

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsFalse(dependency.ExecuteBefore);

            Assert.IsTrue(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(new MockComputation(new object[] { "a" }, ruleT1, context, "c")).AssertEmpty();

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallManyOutputSensitive2_Exception1()
        {
            ruleT1.CallManyOutputSensitive<Dummy, Dummy>(null as OtherRuleT2, (s, o) => null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_CallManyOutputSensitive2_Exception2()
        {
            ruleT1.CallManyOutputSensitive(ruleDependentT2, null as Func<string, string, IEnumerable<Tuple<Dummy, Dummy>>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_Require1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.Require(ruleDependentT2, s => { selectorCalled = true; return new Dummy(); }, s => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a" }, ruleT1, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Require1_Exception1()
        {
            ruleT1.Require(null as OtherRuleT2, s => new Dummy(), s => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Require1_Exception2()
        {
            ruleT1.Require(ruleDependentT2, null as Func<string, Dummy>, s => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Require1_Exception3()
        {
            ruleT1.Require(ruleDependentT2, s => new Dummy(), null as Func<string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_Require2()
        {
            bool selectorCalled = false;
            ruleT1.Require<Dummy, Dummy>(ruleDependentT2, s => { selectorCalled = true; return new Dummy(); }, s => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            dependency.Selector(new MockComputation(new object[] { "a" }, ruleT1, context));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Require2_Exception1()
        {
            ruleT1.Require(null as OtherRuleT2, s => new Dummy(), s => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Require2_Exception2()
        {
            ruleT1.Require(ruleDependentT2, null as Func<string, Dummy>, s => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Require2_Exception3()
        {
            ruleT1.Require(ruleDependentT2, s => new Dummy(), null as Func<string, Dummy>, (s, h) => { });
        }


        [TestMethod]
        public void Transformations_TransformationRule_T2_Require3()
        {
            bool selectorCalled = false;
            bool filterCalled = false;
            bool filterResult = true;
            ruleT1.Require(ruleDependentT2, s => { selectorCalled = true; return new Dummy(); },
                s => { selectorCalled = true; return new Dummy(); }, s => { filterCalled = true; return filterResult; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Selector);
            Assert.IsNull(dependency.Persistor);

            dependency.Selector(new MockComputation(new object[] { "a" }, ruleT1, context));

            var c = context.Computations.Add(ruleT1, "a");
            Assert.IsTrue(dependency.Filter(c));
            filterResult = false;
            Assert.IsFalse(dependency.Filter(c));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(filterCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Require3_Exception1()
        {
            ruleT1.Require(ruleDependentT2, null as Func<string, Dummy>, s => new Dummy(), s => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_Require3_Exception2()
        {
            ruleT1.Require(ruleDependentT2, s => new Dummy(), null as Func<string, Dummy>, s => true);
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_Require3_Exception3()
        {
            ruleT1.Require(ruleDependentT2, s => new Dummy(), s => new Dummy(), null as Predicate<string>);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Filter);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_RequireByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.RequireByType<Dummy, Dummy, Dummy>(s => { selectorCalled = true; return new Dummy(); }, s => { selectorCalled = true; return new Dummy(); },
                (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_RequireByType1_Exception1()
        {
            ruleT1.RequireByType<Dummy, Dummy, Dummy>(null as Func<string, Dummy>, s => new Dummy(), (s, h) => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_RequireByType1_Exception2()
        {
            ruleT1.RequireByType<Dummy, Dummy, Dummy>(s => new Dummy(), null as Func<string, Dummy>, (s, h) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_RequireByType1_Exception3()
        {
            ruleT1.RequireByType<Dummy, Dummy, Dummy>(s => new Dummy(), s => new Dummy(), null as Action<string, Dummy>);

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_RequireByType2()
        {
            bool selectorCalled = false;
            ruleT1.RequireByType<Dummy, Dummy>(s => { selectorCalled = true; return new Dummy(); }, s => { selectorCalled = true; return new Dummy(); });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as SingleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_RequireByType2_Exception1()
        {
            ruleT1.RequireByType<Dummy, Dummy>(null as Func<string, Dummy>, s => new Dummy());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_RequireByType2_Exception2()
        {
            ruleT1.RequireByType<Dummy, Dummy>(s => new Dummy(), null as Func<string, Dummy>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_RequireMany1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.RequireMany(ruleDependentT2, s => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_RequireMany1_Exception1()
        {
            ruleT1.RequireMany(ruleDependentT2, null as Func<string, IEnumerable<Tuple<Dummy, Dummy>>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_RequireMany1_Exception2()
        {
            ruleT1.RequireMany(ruleDependentT2, s => Enumerable.Empty<Tuple<Dummy, Dummy>>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_RequireMany2()
        {
            bool selectorCalled = false;
            ruleT1.RequireMany(ruleDependentT2, s => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count, "Falsche Anzahl Dependencies gesetzt");

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_RequireMany2_Exception1()
        {
            ruleT1.RequireMany(ruleDependentT2, null as Func<string, IEnumerable<Tuple<Dummy, Dummy>>>);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_RequireManyByType1()
        {
            bool selectorCalled = false;
            bool persistorCalled = false;
            ruleT1.RequireManyByType<Dummy, Dummy, Dummy>(s => { selectorCalled = true; return null; }, (s, h) => { persistorCalled = true; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNotNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Persistor(null, null);
            dependency.Selector(new MockComputation(new object[] { "a" }, ruleT1, context));

            Assert.IsTrue(selectorCalled);
            Assert.IsTrue(persistorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_RequireManyByType1_Exception1()
        {
            ruleT1.RequireManyByType<Dummy, Dummy, Dummy>(null as Func<string, IEnumerable<Tuple<Dummy, Dummy>>>, (o1, o2) => { });
        }

        [TestMethod]
        public void Transformations_TransformationRule_T2_RequireManyByType1_Exception2()
        {
            ruleT1.RequireManyByType<Dummy, Dummy, Dummy>(s => Enumerable.Empty<Tuple<Dummy, Dummy>>(), null as Action<string, IEnumerable<Dummy>>);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.IsNotNull(dependency);
            Assert.IsNotNull(dependency.Persistor);
        }

        [TestMethod]
        [TestCategory("TransformationRule T1")]
        public void Transformations_TransformationRule_T2_RequireManyByType2()
        {
            bool selectorCalled = false;
            ruleT1.RequireManyByType<Dummy, Dummy>(s => { selectorCalled = true; return null; });

            Assert.AreEqual(1, ruleT1.Dependencies.Count);

            var dependency = ruleT1.Dependencies.First() as MultipleDependency;

            Assert.AreEqual(ruleT1, dependency.BaseTransformation, "Die BaseTransformation ist falsch gesetzt");
            Assert.AreEqual(ruleDependentT2, dependency.DependencyTransformation, "Die DependencyTransformation ist falsch gesetzt");
            Assert.IsTrue(dependency.ExecuteBefore);

            Assert.IsFalse(dependency.NeedOutput);
            Assert.IsNull(dependency.Persistor);
            Assert.IsNotNull(dependency.Selector);

            dependency.Selector(context.Computations.Add(ruleT2, "a", "b"));

            Assert.IsTrue(selectorCalled);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_TransformationRule_T2_RequireManyByType2_Exception1()
        {
            ruleT1.RequireManyByType<Dummy, Dummy>(null as Func<string, IEnumerable<Tuple<Dummy, Dummy>>>);
        }
        
    }
}
