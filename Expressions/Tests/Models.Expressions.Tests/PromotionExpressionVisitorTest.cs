using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Tests.Railway;
using NMF.Expressions.Linq;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class PromotionExpressionVisitorTest
    {
        [TestMethod]
        public void Test_PromotionExpressionVisitor_CheckEntrySemaphore()
        {
            Expression<Func<Route, bool>> test = r => r.Entry != null && r.Entry.Signal == Signal.GO;

            var visitor = new PromotionExpressionVisitor();
            var visited = visitor.Visit(test);

            Assert.AreEqual(1, visitor.ExtractParameters.Count);
            ParameterAssertions.AssertOnlyParameters(visited, visitor.ExtractParameters.Single().Parameter);
            Assert.AreEqual(1, visitor.ParameterInfos.Count);
            Assert.AreEqual("Signal", visitor.ParameterInfos.Single().Value.Properties.Single());
        }

        [TestMethod]
        public void Test_PromotionExpressionVisitor_CheckSwitchPosition()
        {
            Expression<Func<SwitchPosition, bool>> test = swP => swP.Switch.CurrentPosition == swP.Position;

            var visitor = new PromotionExpressionVisitor();
            var visited = visitor.Visit(test);

            Assert.AreEqual(1, visitor.ExtractParameters.Count);
            ParameterAssertions.AssertOnlyParameters(visited, test.Parameters[0], visitor.ExtractParameters.Single().Parameter);
            Assert.AreEqual(2, visitor.ParameterInfos.Count);
            Assert.AreEqual("Position", visitor.ParameterInfos["swP"].Properties.Single());
            Assert.AreEqual("CurrentPosition", visitor.ParameterInfos[visitor.ExtractParameters.Single().Parameter.Name].Properties.Single());
        }

        [TestMethod]
        public void Test_PromotionExpressionVisitor_CheckSwitchPositionSensor()
        {
            Expression<Func<Route, SwitchPosition, bool>> test = (r, swP) => swP.Switch.Sensor != null && !r.DefinedBy.Contains(swP.Switch.Sensor);

            var visitor = new PromotionExpressionVisitor();
            var visited = visitor.Visit(test);

            Assert.AreEqual(1, visitor.ExtractParameters.Count);
            ParameterAssertions.AssertOnlyParameters(visited, test.Parameters[0], visitor.ExtractParameters.Single().Parameter);
            Assert.AreEqual(2, visitor.ParameterInfos.Count);
            Assert.AreEqual("DefinedBy", visitor.ParameterInfos["r"].Properties.Single());
            Assert.AreEqual("Sensor", visitor.ParameterInfos[visitor.ExtractParameters.Single().Parameter.Name].Properties.Single());
        }

        [TestMethod]
        public void Test_PromotionExpressionVisitor_SwitchSet()
        {
            Expression<Func<RailwayContainer, IEnumerableExpression<SwitchPosition>>> test = rc =>
                from route in rc.Routes
                where route.Entry != null && route.Entry.Signal == Signal.GO
                from swP in route.Follows.OfType<SwitchPosition>()
                where swP.Switch.CurrentPosition != swP.Position
                select swP;

            var visitor = new PromotionExpressionVisitor();
            var visited = visitor.Visit(test);

            Assert.AreEqual(1, visitor.ExtractParameters.Count);
            Assert.AreEqual(visitor.ExtractParameters.Single().Parameter, (visited as LambdaExpression).Body);
            Assert.AreEqual(visitor.ExtractParameters.Single().Value, test.Body);
        }

        [TestMethod]
        public void Test_PromotionExpressionVisitor_PosLength()
        {
            Expression<Func<RailwayContainer, IEnumerableExpression<ISegment>>> test = rc =>
                from seg in rc.Invalids.OfType<Segment>()
                where seg.Length <= 0
                select seg;

            var visitor = new PromotionExpressionVisitor();
            var visited = visitor.Visit(test);

            Assert.Fail();
        }
    }
}
