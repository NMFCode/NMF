using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Tests.Railway;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class ModelFuncExpressionVisitorTest
    {
        private struct TestTuple
        {
            public Route Route { get; set; }
            public int Severity { get; set; }
        }

        private struct TestTuple2
        {
            public TestTuple Inner { get; set; }
        }

        private static Route GetRoute(TestTuple t)
        {
            return t.Route;
        }

        private static ISemaphore GetEntry(Route route)
        {
            return route.Entry;
        }

        [TestMethod]
        public void Test_ModelFuncExpressionVisitor_Property()
        {
            Expression<Func<Route, bool>> test = r => r.Entry != null && r.Entry.Signal == Signal.GO;

            var visitor = new ModelFuncExpressionVisitor();
            var visited = visitor.Visit(test);

            Assert.AreEqual(0, visitor.ExtractParameters.Count);
            ParameterAssertions.AssertOnlyParameters(visited, test.Parameters[0]);
        }

        [TestMethod]
        public void Test_ModelFuncExpressionVisitor_Struct()
        {
            Expression<Func<TestTuple, bool>> test = t => t.Route.Entry != null && t.Route.Entry.Signal == Signal.GO;

            var visitor = new ModelFuncExpressionVisitor();
            var visited = visitor.Visit(test);

            Assert.AreNotSame(test, visited);
            Assert.AreEqual(1, visitor.ExtractParameters.Count);
            ParameterAssertions.AssertOnlyParameters(visited, visitor.ExtractParameters.Single().Parameter);

            Assert.AreEqual("t.Route", visitor.ExtractParameters.Single().Value.ToString());
        }

        [TestMethod]
        public void Test_ModelFuncExpressionVisitor_InnerStruct()
        {
            Expression<Func<TestTuple2, bool>> test = t => t.Inner.Route.Entry != null && t.Inner.Route.Entry.Signal == Signal.GO;

            var visitor = new ModelFuncExpressionVisitor();
            var visited = visitor.Visit(test);

            Assert.AreNotSame(test, visited);
            Assert.AreEqual(1, visitor.ExtractParameters.Count);
            ParameterAssertions.AssertOnlyParameters(visited, visitor.ExtractParameters.Single().Parameter);

            Assert.AreEqual("t.Inner.Route", visitor.ExtractParameters.Single().Value.ToString());
        }

        [TestMethod]
        public void Test_ModelFuncExpressionVisitor_ModelMethod()
        {
            Expression<Func<Route, bool>> test = r => GetEntry(r) != null && GetEntry(r).Signal == Signal.GO;

            var visitor = new ModelFuncExpressionVisitor();
            var visited = visitor.Visit(test);

            Assert.AreSame(test, visited);
            Assert.AreEqual(0, visitor.ExtractParameters.Count);
            ParameterAssertions.AssertOnlyParameters(visited, test.Parameters[0]);
        }

        [TestMethod]
        public void Test_ModelFuncExpressionVisitor_KeepParameterWhenNeeded()
        {
            Expression<Func<TestTuple, bool>> test = t => t.Route.Entry != null && t.Route.Entry.Signal == Signal.GO && t.Severity > 0;

            var visitor = new ModelFuncExpressionVisitor();
            var visited = visitor.Visit(test);

            Assert.AreNotSame(test, visited);
            Assert.AreEqual(1, visitor.ExtractParameters.Count);
            ParameterAssertions.AssertOnlyParameters(visited, test.Parameters[0], visitor.ExtractParameters.Single().Parameter);

            Assert.AreEqual("t.Route", visitor.ExtractParameters.Single().Value.ToString());
        }

        [TestMethod]
        public void Test_ModelFuncExpressionVisitor_NonModelMethod()
        {
            Expression<Func<TestTuple, bool>> test = t => GetRoute(t).Entry != null && GetRoute(t).Entry.Signal == Signal.GO;

            var visitor = new ModelFuncExpressionVisitor();
            var visited = visitor.Visit(test);

            Assert.AreNotSame(visited, test);
            Assert.AreEqual(1, visitor.ExtractParameters.Count);
            ParameterAssertions.AssertOnlyParameters(visited, visitor.ExtractParameters.Single().Parameter);

            Assert.AreEqual("GetRoute(t)", visitor.ExtractParameters.Single().Value.ToString());
        }

        [TestMethod]
        public void Test_ModelFuncExpressionVisitor_ModelNonModelMethod()
        {
            Expression<Func<TestTuple, bool>> test = t => GetEntry(GetRoute(t)) != null && GetEntry(GetRoute(t)).Signal == Signal.GO;

            var visitor = new ModelFuncExpressionVisitor();
            var visited = visitor.Visit(test);

            Assert.AreNotSame(visited, test);
            Assert.AreEqual(1, visitor.ExtractParameters.Count);
            ParameterAssertions.AssertOnlyParameters(visited, visitor.ExtractParameters.Single().Parameter);

            Assert.AreEqual("GetRoute(t)", visitor.ExtractParameters.Single().Value.ToString());
        }
    }
}
