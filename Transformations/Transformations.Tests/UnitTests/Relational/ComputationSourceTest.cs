using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using NMF.Transformations.Linq;

using NMF.Tests;
using NMF.Transformations.Core;

namespace NMF.Transformations.Tests.UnitTests.Relational
{
    [TestClass]
    public class ComputationSourceTest
    {
        private Transformation transformation;
        private ITransformationEngineContext context;
        private TestRuleT1 ruleT1;
        private TestRuleT2 ruleT2;
        private VoidTestRuleT1 voidT1;
        private VoidTestRuleT2 voidT2;

        [TestInitialize]
        public void Initialize()
        {
            ruleT1 = new TestRuleT1();
            ruleT2 = new TestRuleT2();
            voidT1 = new VoidTestRuleT1();
            voidT2 = new VoidTestRuleT2();

            transformation = new MockTransformation(ruleT1, ruleT2, voidT1, voidT2);
            context = new TransformationContext(transformation);
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT1_1()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = ruleT1.ToComputationSource(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((TransformationComputationWrapper<string, string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT1_2()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = ruleT1.ToComputationSource(context, false);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput, ((TransformationComputationWrapper<string, string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT1_3()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = ruleT1.ToComputationSource(context, true);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput, ((TransformationComputationWrapper<string, string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>());
            var c = context.CallTransformation(ruleT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>(c), new TransformationComputationWrapper<string, string>());
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT1_4()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = ruleT1.ToComputationSource(context, comp => true);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput, ((TransformationComputationWrapper<string, string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT1_5()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = ruleT1.ToComputationSource(context, comp => false);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput, ((TransformationComputationWrapper<string, string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT1, "a");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertEmpty();
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT1_6()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = ruleT1.ToComputationSource(context, true, comp => false);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput, ((TransformationComputationWrapper<string, string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>());
            var c = context.CallTransformation(ruleT1, "a");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT1__Exception1()
        {
            (null as TransformationRuleBase<string, string>).ToComputationSource(context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT1__Exception2()
        {
            (null as TransformationRuleBase<string, string>).ToComputationSource(context, c => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT1__Exception3()
        {
            (null as TransformationRuleBase<string, string>).ToComputationSource(context, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT1__Exception4()
        {
            (null as TransformationRuleBase<string, string>).ToComputationSource(context, true, c => true);
        }


        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT2_1()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = ruleT2.ToComputationSource(context);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput1, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT2_2()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = ruleT2.ToComputationSource(context, false);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput1, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT2_3()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = ruleT2.ToComputationSource(context, true);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput1, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>());
            var c = context.CallTransformation(ruleT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>(c), new TransformationComputationWrapper<string, string, string>());
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT2_4()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = ruleT2.ToComputationSource(context, comp => true);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput1, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT2_5()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = ruleT2.ToComputationSource(context, comp => false);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput1, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT2, "a", "b");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertEmpty();
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT2_6()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = ruleT2.ToComputationSource(context, true, comp => false);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput1, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>());
            var c = context.CallTransformation(ruleT2, "a", "b");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT2__Exception1()
        {
            (null as TransformationRuleBase<string, string, string>).ToComputationSource(context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT2__Exception2()
        {
            (null as TransformationRuleBase<string, string, string>).ToComputationSource(context, c => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT2__Exception3()
        {
            (null as TransformationRuleBase<string, string, string>).ToComputationSource(context, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT2__Exception4()
        {
            (null as TransformationRuleBase<string, string, string>).ToComputationSource(context, true, c => true);
        }


        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT1_1()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = voidT1.ToComputationSource(context);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput, ((InPlaceComputationWrapper<string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT1_2()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = voidT1.ToComputationSource(context, false);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput, ((InPlaceComputationWrapper<string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT1_3()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = voidT1.ToComputationSource(context, true);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput, ((InPlaceComputationWrapper<string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new InPlaceComputationWrapper<string>());
            var c = context.CallTransformation(voidT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string>(c), new InPlaceComputationWrapper<string>());
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT1_4()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = voidT1.ToComputationSource(context, comp => true);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput, ((InPlaceComputationWrapper<string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT1_5()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = voidT1.ToComputationSource(context, comp => false);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput, ((InPlaceComputationWrapper<string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT1, "a");
            Assert.IsFalse(nextItemCalled);
            source.AssertEmpty();
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT1_6()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = voidT1.ToComputationSource(context, true, comp => false);
            source.CollectionChanged += (o, e) =>
            {
                Assert.AreEqual(expectedInput, ((InPlaceComputationWrapper<string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new InPlaceComputationWrapper<string>());
            var c = context.CallTransformation(voidT1, "a");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT1__Exception1()
        {
            (null as InPlaceTransformationRuleBase<string>).ToComputationSource(context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT1__Exception2()
        {
            (null as InPlaceTransformationRuleBase<string>).ToComputationSource(context, c => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT1__Exception3()
        {
            (null as InPlaceTransformationRuleBase<string>).ToComputationSource(context, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT1__Exception4()
        {
            (null as InPlaceTransformationRuleBase<string>).ToComputationSource(context, true, c => true);
        }


        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT2_1()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = voidT2.ToComputationSource(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT2_2()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = voidT2.ToComputationSource(context, false);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT2_3()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = voidT2.ToComputationSource(context, true);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>());
            var c = context.CallTransformation(voidT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>(c), new InPlaceComputationWrapper<string, string>());
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT2_4()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = voidT2.ToComputationSource(context, comp => true);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT2_5()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = voidT2.ToComputationSource(context, comp => false);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT2, "a", "b");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertEmpty();
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT2_6()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = voidT2.ToComputationSource(context, true, comp => false);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>());
            var c = context.CallTransformation(voidT2, "a", "b");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT2__Exception1()
        {
            (null as InPlaceTransformationRuleBase<string, string>).ToComputationSource(context);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT2__Exception2()
        {
            (null as InPlaceTransformationRuleBase<string, string>).ToComputationSource(context, c => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT2__Exception3()
        {
            (null as InPlaceTransformationRuleBase<string, string>).ToComputationSource(context, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT2__Exception4()
        {
            (null as InPlaceTransformationRuleBase<string, string>).ToComputationSource(context, true, c => true);
        }


        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT1_7()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = ruleT1.ToComputationSource()(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((TransformationComputationWrapper<string, string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT1_8()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = ruleT1.ToComputationSource(false)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((TransformationComputationWrapper<string, string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT1_9()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = ruleT1.ToComputationSource(true)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((TransformationComputationWrapper<string, string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>());
            var c = context.CallTransformation(ruleT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>(c), new TransformationComputationWrapper<string, string>());
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT1_10()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = ruleT1.ToComputationSource(comp => true)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((TransformationComputationWrapper<string, string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT1_11()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = ruleT1.ToComputationSource(comp => false)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((TransformationComputationWrapper<string, string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT1, "a");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertEmpty();
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT1_12()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = ruleT1.ToComputationSource(true, comp => false)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((TransformationComputationWrapper<string, string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>());
            var c = context.CallTransformation(ruleT1, "a");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT1__Exception5()
        {
            (null as TransformationRuleBase<string, string>).ToComputationSource();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT1__Exception6()
        {
            (null as TransformationRuleBase<string, string>).ToComputationSource(c => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT1__Exception7()
        {
            (null as TransformationRuleBase<string, string>).ToComputationSource(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT1__Exception8()
        {
            (null as TransformationRuleBase<string, string>).ToComputationSource(true, c => true);
        }


        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT2_7()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = ruleT2.ToComputationSource()(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT2_8()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = ruleT2.ToComputationSource(false)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT2_9()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = ruleT2.ToComputationSource(true)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>());
            var c = context.CallTransformation(ruleT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>(c), new TransformationComputationWrapper<string, string, string>());
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT2_10()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = ruleT2.ToComputationSource(comp => true)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT2_11()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = ruleT2.ToComputationSource(comp => false)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(ruleT2, "a", "b");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertEmpty();
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceT2_12()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = ruleT2.ToComputationSource(true, comp => false)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((TransformationComputationWrapper<string, string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>());
            var c = context.CallTransformation(ruleT2, "a", "b");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertContainsOnly(new TransformationComputationWrapper<string, string, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT2__Exception5()
        {
            (null as TransformationRuleBase<string, string, string>).ToComputationSource();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT2__Exception6()
        {
            (null as TransformationRuleBase<string, string, string>).ToComputationSource(c => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT2__Exception7()
        {
            (null as TransformationRuleBase<string, string, string>).ToComputationSource(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceT2__Exception8()
        {
            (null as TransformationRuleBase<string, string, string>).ToComputationSource(true, c => true);
        }


        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT1_7()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = voidT1.ToComputationSource()(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((InPlaceComputationWrapper<string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT1_8()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = voidT1.ToComputationSource(false)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((InPlaceComputationWrapper<string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT1_9()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = voidT1.ToComputationSource(true)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((InPlaceComputationWrapper<string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new InPlaceComputationWrapper<string>());
            var c = context.CallTransformation(voidT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string>(c), new InPlaceComputationWrapper<string>());
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT1_10()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = voidT1.ToComputationSource(comp => true)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((InPlaceComputationWrapper<string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT1, "a");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT1_11()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = voidT1.ToComputationSource(comp => false)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((InPlaceComputationWrapper<string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT1, "a");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertEmpty();
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT1_12()
        {
            bool nextItemCalled = false;
            string expectedInput = "a";
            var source = voidT1.ToComputationSource(true, comp => false)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput, ((InPlaceComputationWrapper<string>)e.NewItems[0]).Input);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new InPlaceComputationWrapper<string>());
            var c = context.CallTransformation(voidT1, "a");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT1__Exception5()
        {
            (null as InPlaceTransformationRuleBase<string>).ToComputationSource();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT1__Exception6()
        {
            (null as InPlaceTransformationRuleBase<string>).ToComputationSource(c => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT1__Exception7()
        {
            (null as InPlaceTransformationRuleBase<string>).ToComputationSource(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT1__Exception8()
        {
            (null as InPlaceTransformationRuleBase<string>).ToComputationSource(true, c => true);
        }


        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT2_7()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = voidT2.ToComputationSource()(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT2_8()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = voidT2.ToComputationSource(false)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT2_9()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = voidT2.ToComputationSource(true)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>());
            var c = context.CallTransformation(voidT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>(c), new InPlaceComputationWrapper<string, string>());
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT2_10()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = voidT2.ToComputationSource(comp => true)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT2, "a", "b");
            context.ExecutePending();
            Assert.IsTrue(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>(c));
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT2_11()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = voidT2.ToComputationSource(comp => false)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertEmpty();
            var c = context.CallTransformation(voidT2, "a", "b");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertEmpty();
        }

        [TestMethod]
        public void Transformations_Relational_ToComputationSourceVT2_12()
        {
            bool nextItemCalled = false;
            string expectedInput1 = "a";
            string expectedInput2 = "b";
            var source = voidT2.ToComputationSource(true, comp => false)(context);
            source.CollectionChanged += (o,e) =>
            {
                Assert.AreEqual(expectedInput1, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input1);
                Assert.AreEqual(expectedInput2, ((InPlaceComputationWrapper<string, string>)e.NewItems[0]).Input2);
                nextItemCalled = true;
            };

            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>());
            var c = context.CallTransformation(voidT2, "a", "b");
            context.ExecutePending();
            Assert.IsFalse(nextItemCalled);
            source.AssertContainsOnly(new InPlaceComputationWrapper<string, string>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT2__Exception5()
        {
            (null as InPlaceTransformationRuleBase<string, string>).ToComputationSource();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT2__Exception6()
        {
            (null as InPlaceTransformationRuleBase<string, string>).ToComputationSource(c => true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT2__Exception7()
        {
            (null as InPlaceTransformationRuleBase<string, string>).ToComputationSource(true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Transformations_Relational_ToComputationSourceVT2__Exception8()
        {
            (null as InPlaceTransformationRuleBase<string, string>).ToComputationSource(true, c => true);
        }
    }
}
