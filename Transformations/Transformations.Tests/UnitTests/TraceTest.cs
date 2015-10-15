using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Transformations;
using NMF.Transformations.Core;
using System.Collections.Generic;
using NMF.Transformations.Tests;
using NMF.Utilities;
using System.Linq;

using NMF.Tests;

namespace NMF.Transformations.Tests.UnitTests
{
    [TestClass]
    public class TransformationContextTraceTest : TraceTest
    {
        protected override ITransformationContext CreateContext(Transformation transformation)
        {
            return new TransformationContext(transformation);
        }
    }

    [TestClass]
    public class TraceTest
    {

        private TestRuleT1 ruleT1;
        private TestRuleT2 ruleT2;
        private TestRuleTN ruleTN;

        private OtherRuleT1 ruleDependent;

        private Computation c_a;
        private Computation c_b;
        private Computation c_ab;
        private Computation c_bc;
        private Computation c_abc;
        private Computation c_bcd;

        private MockTransformation transformation;
        private ITransformationContext context;
        private ITransformationTrace trace;


        [TestInitialize]
        public void InitTestContext()
        {
            ruleT1 = new TestRuleT1();
            ruleT2 = new TestRuleT2();
            ruleTN = new TestRuleTN();

            ruleDependent = new OtherRuleT1();
            transformation = new MockTransformation(ruleT1, ruleT2, ruleTN, ruleDependent);
            transformation.Initialize();
            context = CreateContext(transformation);
            trace = context.Trace;

            c_a = context.CallTransformation(ruleT1, new object[] { "a" });
            c_b = context.CallTransformation(ruleT1, new object[] { "b" });
            c_ab = context.CallTransformation(ruleT2, new object[] { "a", "b" });
            c_bc = context.CallTransformation(ruleT2, new object[] { "b", "c" });
            c_abc = context.CallTransformation(ruleTN, new object[] { "a", "b", "c" });
            c_bcd = context.CallTransformation(ruleTN, new object[] { "b", "c", "d" });

            c_a.InitializeOutput("b");
            c_b.InitializeOutput(null);
            c_ab.InitializeOutput("c");
            c_bc.InitializeOutput(null);
            c_abc.InitializeOutput("d");
            c_bcd.InitializeOutput(null);
        }

        protected virtual ITransformationContext CreateContext(Transformation transformation)
        {
            return new MockContext(transformation);
        }

        [TestMethod]
        public void Transformations_AbstractTrace_TraceIn()
        {
            Assert.AreEqual(c_a, trace.TraceIn(ruleT1, "a").FirstOrDefault());
            Assert.AreEqual(c_b, trace.TraceIn(ruleT1, "b").FirstOrDefault());
            Assert.AreEqual(c_ab, trace.TraceIn(ruleT2, "a", "b").FirstOrDefault());
            Assert.AreEqual(c_bc, trace.TraceIn(ruleT2, "b", "c").FirstOrDefault());
            Assert.AreEqual(c_abc, trace.TraceIn(ruleTN, "a", "b", "c").FirstOrDefault());
            Assert.AreEqual(c_bcd, trace.TraceIn(ruleTN, "b", "c", "d").FirstOrDefault());

            Assert.AreEqual(null, trace.TraceIn(ruleT1, "c").FirstOrDefault());
            Assert.AreEqual(null, trace.TraceIn(ruleT2, "a", "a").FirstOrDefault());
            Assert.AreEqual(null, trace.TraceIn(ruleTN, "a", "a", "a").FirstOrDefault());

            Assert.AreEqual(null, trace.TraceIn(ruleT2, "a").FirstOrDefault());
            Assert.AreEqual(null, trace.TraceIn(ruleT2, "b").FirstOrDefault());
            Assert.AreEqual(null, trace.TraceIn(ruleTN, "a", "b").FirstOrDefault());
            Assert.AreEqual(null, trace.TraceIn(ruleTN, "b", "c").FirstOrDefault());
            Assert.AreEqual(null, trace.TraceIn(ruleT1, "a", "b", "c").FirstOrDefault());
            Assert.AreEqual(null, trace.TraceIn(ruleT1, "b", "c", "d").FirstOrDefault());

            Assert.AreEqual(null, trace.TraceIn(ruleTN, "a").FirstOrDefault());
            Assert.AreEqual(null, trace.TraceIn(ruleTN, "b").FirstOrDefault());
            Assert.AreEqual(null, trace.TraceIn(ruleT1, "a", "b").FirstOrDefault());
            Assert.AreEqual(null, trace.TraceIn(ruleT1, "b", "c").FirstOrDefault());
            Assert.AreEqual(null, trace.TraceIn(ruleT2, "a", "b", "c").FirstOrDefault());
            Assert.AreEqual(null, trace.TraceIn(ruleT2, "b", "c", "d").FirstOrDefault());
        }

        [TestMethod]
        public void Transformations_AbstractTrace_Trace()
        {
            trace.Trace("a").AssertContainsOnly(c_a);
            trace.Trace("b").AssertContainsOnly(c_b);
            trace.Trace("a", "b").AssertContainsOnly(c_ab);
            trace.Trace("b", "c").AssertContainsOnly(c_bc);
            trace.Trace("a", "b", "c").AssertContainsOnly(c_abc);
            trace.Trace("b", "c", "d").AssertContainsOnly(c_bcd);

            trace.Trace("c").AssertEmpty();
            trace.Trace("a", "a").AssertEmpty();
        }

        [TestMethod]
        public void Transformations_AbstractTrace_Resolve1()
        {
            Assert.AreEqual("b", trace.ResolveIn(ruleT1, new object[] { "a" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleT1, new object[] { "b" }));
            Assert.AreEqual("c", trace.ResolveIn(ruleT2, new object[] { "a", "b" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleT2, new object[] { "b", "c" }));
            Assert.AreEqual("d", trace.ResolveIn(ruleTN, new object[] { "a", "b", "c" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleTN, new object[] { "b", "c", "d" }));

            Assert.AreEqual(null, trace.ResolveIn(ruleT2, new object[] { "a" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleT2, new object[] { "b" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleTN, new object[] { "a", "b" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleTN, new object[] { "b", "c" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleT1, new object[] { "a", "b", "c" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleT1, new object[] { "b", "c", "d" }));

            Assert.AreEqual(null, trace.ResolveIn(ruleTN, new object[] { "a" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleTN, new object[] { "b" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleT1, new object[] { "a", "b" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleT1, new object[] { "b", "c" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleT2, new object[] { "a", "b", "c" }));
            Assert.AreEqual(null, trace.ResolveIn(ruleT2, new object[] { "b", "c", "d" }));
        }

        [TestMethod]
        public void Transformations_AbstractTrace_Resolve2()
        {
            Assert.AreEqual("b", trace.ResolveIn<string, string>(ruleT1, "a"));
            Assert.AreEqual(null, trace.ResolveIn<string, string>(ruleT1, "b"));
            Assert.AreEqual(null, trace.ResolveIn<string, string>(ruleT1, "c"));

            Assert.AreEqual("b", trace.Resolve<string, string>("a"));
            Assert.AreEqual(null, trace.Resolve<string, string>("b"));
            Assert.AreEqual(null, trace.Resolve<string, string>("c"));
        }

        [TestMethod]
        public void Transformations_AbstractTrace_Resolve3()
        {
            Assert.AreEqual("c", trace.ResolveIn<string, string, string>(ruleT2, "a", "b"));
            Assert.AreEqual(null, trace.ResolveIn<string, string, string>(ruleT2, "b", "c"));
            Assert.AreEqual(null, trace.ResolveIn<string, string, string>(ruleT2, "c", "d"));

            Assert.AreEqual("c", trace.Resolve<string, string, string>("a", "b"));
            Assert.AreEqual(null, trace.Resolve<string, string, string>("b", "c"));
            Assert.AreEqual(null, trace.Resolve<string, string, string>("c", "d"));
        }

        [TestMethod]
        public void Transformations_AbstractTrace_Resolve4()
        {
            Assert.AreEqual("d", trace.ResolveIn<string>(ruleTN, new object[] { "a", "b", "c" }));
            Assert.AreEqual(null, trace.ResolveIn<string>(ruleTN, new object[] { "b", "c", "d" }));
            Assert.AreEqual(null, trace.ResolveIn<string>(ruleTN, new object[] { "c", "d", "e" }));

            Assert.AreEqual("d", trace.Resolve<string>(new object[] { "a", "b", "c" }));
            Assert.AreEqual(null, trace.Resolve<string>(new object[] { "b", "c", "d" }));
            Assert.AreEqual(null, trace.Resolve<string>(new object[] { "c", "d", "e" }));
        }


        [TestMethod]
        public void Transformations_AbstractTrace_TraceWhere1()
        {
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere(ruleT1, (object[] o) => object.Equals(o[0], "a")), c_a);
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere(ruleT1, (object[] o) => object.Equals(o[0], "b")), c_b);
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere(ruleT2, (object[] o) => object.Equals(o[0], "a") && object.Equals(o[1], "b")), c_ab);
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere(ruleT2, (object[] o) => object.Equals(o[0], "b") && object.Equals(o[1], "c")), c_bc);
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere(ruleTN, (object[] o) => object.Equals(o[0], "a") && object.Equals(o[1], "b") && object.Equals(o[2], "c")), c_abc);
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere(ruleTN, (object[] o) => object.Equals(o[0], "b") && object.Equals(o[1], "c") && object.Equals(o[2], "d")), c_bcd);
        }

        [TestMethod]
        public void Transformations_AbstractTrace_TraceWhere2()
        {
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere<string>(ruleT1, filter: s => s == "a"), c_a);
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere<string>(ruleT1, filter: s => s == "b"), c_b);
            Assert.IsTrue(trace.TraceInWhere<string>(ruleT1, filter: s => s == "c").IsNullOrEmpty());

            AssertExtensions.AssertContainsOnly(trace.TraceInWhere<string>(ruleT1, s => s == "a"), c_a);
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere<string>(ruleT1, s => s == "b"), c_b);
            Assert.IsTrue(trace.TraceInWhere<string>(ruleT1, s => s == "c").IsNullOrEmpty());
        }

        [TestMethod]
        public void Transformations_AbstractTrace_TraceWhere3()
        {
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere<string, string>(ruleT2, (s1, s2) => s1 == "a" && s2 == "b"), c_ab);
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere<string, string>(ruleT2, (s1, s2) => s1 == "b" && s2 == "c"), c_bc);
            Assert.IsTrue(trace.TraceInWhere<string, string>(ruleT2, (s1, s2) => s1 == "c" && s2 == "d").IsNullOrEmpty());

            AssertExtensions.AssertContainsOnly(trace.TraceInWhere<string, string>(ruleT2, (s1, s2) => s1 == "a" && s2 == "b"), c_ab);
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere<string, string>(ruleT2, (s1, s2) => s1 == "b" && s2 == "c"), c_bc);
            Assert.IsTrue(trace.TraceInWhere<string, string>(ruleT2, (s1, s2) => s1 == "c" && s2 == "d").IsNullOrEmpty());
        }

        [TestMethod]
        public void Transformations_AbstractTrace_TraceWhere4()
        {
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere(ruleTN, o => o.ArrayEquals(new object[] { "a", "b", "c" })), c_abc);
            AssertExtensions.AssertContainsOnly(trace.TraceInWhere(ruleTN, o => o.ArrayEquals(new object[] { "b", "c", "d" })), c_bcd);
            Assert.IsTrue(trace.TraceInWhere(ruleTN, o => o.ArrayEquals(new object[] { "c", "d", "e" })).IsNullOrEmpty());
        }


        [TestMethod]
        public void Transformations_AbstractTrace_ResolveWhere1()
        {
            AssertExtensions.AssertContainsOnly(trace.ResolveInWhere(ruleT1, s => s == "a"), "b");
            AssertExtensions.AssertContainsOnly(trace.ResolveInWhere(ruleT1, s => s == "b"), null);
            trace.ResolveInWhere(ruleT1, o => true).AssertContainsOnly("b", null);
            trace.ResolveInWhere(ruleT1, null as Predicate<string>).AssertContainsOnly("b", null);
            AssertExtensions.AssertContainsOnly(trace.ResolveInWhere(ruleT2, (s1,s2) => s1 == "a" && s2 == "b"), "c");
            AssertExtensions.AssertContainsOnly(trace.ResolveInWhere(ruleT2, (s1,s2) => s1 == "b" && s2 == "c"), null);
            trace.ResolveInWhere(ruleT2, o => true).AssertContainsOnly("c", null);
            trace.ResolveInWhere(ruleT2, null as Func<string, string, bool>).AssertContainsOnly("c", null);
            AssertExtensions.AssertContainsOnly(trace.ResolveInWhere(ruleTN, o => object.Equals(o[0], "a") && object.Equals(o[1], "b") && object.Equals(o[2], "c")), "d");
            AssertExtensions.AssertContainsOnly(trace.ResolveInWhere(ruleTN, o => object.Equals(o[0], "b") && object.Equals(o[1], "c") && object.Equals(o[2], "d")), null);
            trace.ResolveInWhere(ruleTN, o => true).AssertContainsOnly("d", null);
            trace.ResolveInWhere(ruleTN, null as Predicate<object[]>).AssertContainsOnly("d", null);
        }

        [TestMethod]
        public void Transformations_AbstractTrace_ResolveWhere2()
        {
            AssertExtensions.AssertContainsOnly(trace.ResolveInWhere<string>(ruleTN, o => o.ArrayEquals(new object[] { "a", "b", "c" })), "d");
            AssertExtensions.AssertContainsOnly(trace.ResolveInWhere<string>(ruleTN, o => o.ArrayEquals(new object[] { "b", "c", "d" })), null);
            trace.ResolveInWhere<string>(ruleTN, o => o.ArrayEquals(new object[] { "c", "d", "e" })).AssertEmpty();

            var types = new Type[] { typeof(string), typeof(string), typeof(string) };
            AssertExtensions.AssertContainsOnly(trace.ResolveWhere<string>(types, o => o.ArrayEquals(new object[] { "a", "b", "c" })), "d");
            AssertExtensions.AssertContainsOnly(trace.ResolveWhere<string>(types, o => o.ArrayEquals(new object[] { "b", "c", "d" })), null);
            trace.ResolveWhere<string>(types, o => o.ArrayEquals(new object[] { "c", "d", "e" })).AssertEmpty();

            trace.ResolveWhere<string, string>(s => s == "a").AssertContainsOnly("b");
            trace.ResolveWhere<string, string>(s => s == "b").AssertContainsOnly(null);
            trace.ResolveWhere<string, string>(null).AssertContainsOnly("b", null);

            trace.ResolveWhere<string, string, string>((s1, s2) => s1 == "a" && s2 == "b").AssertContainsOnly("c");
            trace.ResolveWhere<string, string, string>((s1, s2) => s1 == "b" && s2 == "c").AssertContainsOnly(null);
            trace.ResolveWhere<string, string, string>(null).AssertContainsOnly("c", null);
        }

        [TestMethod]
        public void Transformations_AbstractTrace_ResolveMany1()
        {
            var list = new List<string>();
            list.Add("a");
            AssertExtensions.AssertContainsOnly(trace.ResolveManyIn<string, string>(ruleT1, list), "b");
            AssertExtensions.AssertContainsOnly(trace.ResolveMany<string, string>(list), "b");

            list.Clear();
            list.Add("b");
            AssertExtensions.AssertContainsOnly(trace.ResolveManyIn<string, string>(ruleT1, list), null);
            AssertExtensions.AssertContainsOnly(trace.ResolveMany<string, string>(list), null);
            list.Clear();
            list.Add("c");
            Assert.IsTrue(trace.ResolveManyIn<string, string>(ruleT1, list).IsNullOrEmpty());
            Assert.IsTrue(trace.ResolveManyIn<string, string>(ruleT1, list).IsNullOrEmpty());

            list.Add("a");
            list.Add("b");
            var collection = trace.ResolveManyIn<string, string>(ruleT1, list);

            Assert.AreEqual(2, collection.Count());
            Assert.IsTrue(collection.Contains("b"));
            Assert.IsTrue(collection.Contains(null));
        }

        [TestMethod]
        public void Transformations_AbstractTrace_ResolveMany2()
        {
            trace.ResolveManyIn(ruleT1, null as IEnumerable<string>).AssertEmpty();
            trace.ResolveMany<string, string>(null as IEnumerable<string>).AssertEmpty();
        }

        [TestMethod]
        public void Transformations_AbstractTrace_FindAll1()
        {
            trace.FindInWhere(ruleT1, s => true).AssertContainsOnly("b", null);
            trace.FindInWhere(ruleT1, null).AssertContainsOnly("b", null);
            trace.FindInWhere(ruleT1, s => false).AssertEmpty();
            trace.FindAllIn(ruleT1).AssertContainsOnly("b", null);

            trace.FindWhere<string, string>(s => true).AssertContainsOnly("b", null);
            trace.FindAll<string, string>().AssertContainsOnly("b", null);
            trace.FindWhere<string, string>(s => false).AssertEmpty();
            trace.FindWhere<string, string>(null).AssertContainsOnly("b", null);
        }

        [TestMethod]
        public void Transformations_AbstractTrace_FindAll2()
        {
            trace.FindInWhere(ruleT2, s => true).AssertContainsOnly("c", null);
            trace.FindInWhere(ruleT2, null).AssertContainsOnly("c", null);
            trace.FindInWhere(ruleT2, s => false).AssertEmpty();
            trace.FindAllIn(ruleT2).AssertContainsOnly("c", null);

            trace.FindWhere<string, string, string>(s => true).AssertContainsOnly("c", null);
            trace.FindWhere<string, string, string>(null as Predicate<string>).AssertContainsOnly("c", null);
            trace.FindWhere<string, string, string>(s => false).AssertEmpty();
            trace.FindAll<string, string, string>().AssertContainsOnly("c", null);
        }

        [TestMethod]
        public void Transformations_AbstractTrace_FindAll3()
        {
            trace.FindInWhere(ruleTN, s => true).AssertContainsOnly("d", null);
            trace.FindInWhere(ruleTN, null).AssertContainsOnly("d", null);
            trace.FindInWhere(ruleTN, s => false).AssertEmpty();
            trace.FindAllIn(ruleTN).AssertContainsOnly("d", null);

            Type[] types = { typeof(string), typeof(string), typeof(string) };
            trace.FindWhere<string>(types, s => true).AssertContainsOnly("d", null);
            trace.FindWhere<string>(types, null).AssertContainsOnly("d", null);
            trace.FindWhere<string>(types, s => false).AssertEmpty();
            trace.FindAll<string>(types).AssertContainsOnly("d", null);
        }

        [TestMethod]
        public void Transformations_AbstractTrace_TraceAll()
        {
            trace.TraceAllIn(ruleT1).AssertContainsOnly(c_a, c_b);
            trace.TraceAllIn(ruleT2).AssertContainsOnly(c_ab, c_bc);
            trace.TraceAllIn(ruleTN).AssertContainsOnly(c_abc, c_bcd);

            trace.TraceInWhere(ruleT1, s => true).AssertContainsOnly(c_a, c_b);
            trace.TraceInWhere(ruleT2, (s1,s2) => true).AssertContainsOnly(c_ab, c_bc);
            trace.TraceInWhere(ruleTN, strings => true).AssertContainsOnly(c_abc, c_bcd);

            trace.TraceInWhere(ruleT1, null).AssertContainsOnly(c_a, c_b);
            trace.TraceInWhere(ruleT2, null).AssertContainsOnly(c_ab, c_bc);
            trace.TraceInWhere(ruleTN, null).AssertContainsOnly(c_abc, c_bcd);

            trace.TraceInWhere(ruleT1, s => false).AssertEmpty();
            trace.TraceInWhere(ruleT2, (s1, s2) => false).AssertEmpty();
            trace.TraceInWhere(ruleTN, strings => false).AssertEmpty();
        }

        [TestMethod]
        public void Transformations_AbstractTrace_TraceAllTransformationOutputs()
        {
            trace.FindAllIn(ruleT1).AssertContainsOnly("b", null);
            trace.FindAllIn(ruleT2).AssertContainsOnly("c", null);
            trace.FindAllIn(ruleTN).AssertContainsOnly("d", null);

            trace.ResolveInWhere(ruleT2, (s1,s2) => true).AssertContainsOnly("c", null);
            trace.ResolveInWhere(ruleTN, strings => true).AssertContainsOnly("d", null);

            trace.ResolveInWhere(ruleT2, null).AssertContainsOnly("c", null);
            trace.ResolveInWhere(ruleTN, null).AssertContainsOnly("d", null);

            trace.ResolveInWhere(ruleT2, (s1, s2) => false).AssertEmpty();
            trace.ResolveInWhere(ruleTN, strings => false).AssertEmpty();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_AbstractTrace_TraceIn_NoTransformationRule()
        {
            trace.TraceIn(null, "a");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_AbstractTrace_TraceAllIn_NoTransformationRule()
        {
            trace.TraceAllIn(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_AbstractTrace_TraceManyIn_NoTransformationRule()
        {
            trace.TraceManyIn(null, Enumerable.Empty<object[]>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_AbstractTrace_PublishEntry_Null()
        {
            trace.PublishEntry(null);
        }

        [TestMethod]
        public void Transformations_AbstractTrace_PublishEntry_NewEntry()
        {
            var dummy = new Dummy();
            var traceEntry = new TraceEntry<Dummy, Dummy>(ruleDependent,dummy, dummy);

            Assert.IsFalse(trace.TraceIn(ruleDependent, dummy).Contains(traceEntry));

            trace.PublishEntry(traceEntry);

            Assert.IsTrue(trace.TraceIn(ruleDependent, dummy).Contains(traceEntry));
        }

        [TestMethod]
        public void Transformations_AbstractTrace_PublishEntry_ExistingEntry()
        {
            trace.PublishEntry(c_a);

            trace.TraceIn(ruleT1, "a").AssertContainsOnly(c_a);
        }

        [TestMethod]
        public void Transformations_AbstractTrace_PublishEntry_ReInsertEntry()
        {
            trace.RevokeEntry(c_a);
            trace.PublishEntry(c_a);

            trace.TraceIn(ruleT1, "a").AssertContainsOnly(c_a);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_AbstractTrace_RevokeEntry_Null()
        {
            trace.RevokeEntry(null);
        }

        [TestMethod]
        public void Transformations_AbstractTrace_RevokeEntry_NewEntry()
        {
            var dummy = new Dummy();
            var traceEntry = new TraceEntry<Dummy, Dummy>(ruleDependent, dummy, dummy);

            Assert.IsFalse(trace.TraceIn(ruleDependent, dummy).Contains(traceEntry));

            trace.RevokeEntry(traceEntry);

            Assert.IsFalse(trace.TraceIn(ruleDependent, dummy).Contains(traceEntry));
        }

        [TestMethod]
        public void Transformations_AbstractTrace_RevokeEntry_ExistingEntry()
        {
            trace.RevokeEntry(c_a);

            trace.TraceIn(ruleT1, "a").AssertEmpty();
        }

        [TestMethod]
        public void Transformations_AbstractTrace_PublishEntry_RevokePublished()
        {
            var dummy = new Dummy();
            var traceEntry = new TraceEntry<Dummy, Dummy>(ruleDependent, dummy, dummy);

            Assert.IsFalse(trace.TraceIn(ruleDependent, dummy).Contains(traceEntry));

            trace.PublishEntry(traceEntry);
            trace.RevokeEntry(traceEntry);

            Assert.IsFalse(trace.TraceIn(ruleDependent, dummy).Contains(traceEntry));
        }

    }
}
