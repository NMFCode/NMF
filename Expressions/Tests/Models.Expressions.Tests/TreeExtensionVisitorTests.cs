using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Models.Tests.Railway;
using NMF.Expressions.Linq;
using System.Linq.Expressions;

namespace NMF.Expressions.Tests
{
    [TestClass]
    public class TreeExtensionVisitorTests
    {
        [TestMethod]
        public void TreeExtensionVisitor_SwitchSet()
        {
            Expression<Func<RailwayContainer, IEnumerableExpression<SwitchPosition>>> func = 
                rc =>
                from route in rc.Routes
                where route.Entry != null && route.Entry.Signal == Signal.GO
                from swP in route.Follows.OfType<SwitchPosition>()
                where swP.Switch.CurrentPosition != swP.Position
                select swP;

            var visitor = new TreeExtensionExpressionVisitor();
            var visited = visitor.Visit(func.Body);

            Assert.AreSame(func.Body, visited);

            var parameters = visitor.ListParameters();
            Assert.AreEqual(1, parameters.Count);
            CollectionAssert.Contains(parameters, func.Parameters[0]);
        }
    }
}
