using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMF.Expressions;
using System.Collections.Generic;
using NMF.Collections.ObjectModel;

namespace NMF.Analyses.Connectivity.Tests
{
    public abstract class ConnectivityTests
    {
        public abstract Func<bool> CreateCheck(IEnumerableExpression<Node> nodes, Node start, Node target);

        [TestMethod]
        public void TestDirectConnections()
        {
            var a = new Node { Name = "A" };
            var b = new Node { Name = "B" };
            a.Edges.Add(b);
            b.Edges.Add(a);

            var nodes = new ObservableList<Node> { a, b };
            var checker = CreateCheck(nodes, a, b);

            Assert.IsTrue(checker());
            a.Edges.Clear();
            b.Edges.Clear();
            Assert.IsFalse(checker());
            a.Edges.Add(b);
            b.Edges.Add(a);
            Assert.IsTrue(checker());
            a.Edges.Clear();
            b.Edges.Clear();
            Assert.IsFalse(checker());
        }

        [TestMethod]
        public void TestIndirectConnections()
        {
            var a = new Node { Name = "A" };
            var b = new Node { Name = "B" };
            var c = new Node { Name = "C" };
            a.Edges.Add(b);
            b.Edges.Add(a);
            b.Edges.Add(c);
            c.Edges.Add(b);

            var nodes = new ObservableList<Node> { a, b, c };
            var checker = CreateCheck(nodes, a, c);

            Assert.IsTrue(checker());
            a.Edges.Clear();
            b.Edges.Remove(a);
            Assert.IsFalse(checker());
            a.Edges.Add(b);
            b.Edges.Add(a);
            Assert.IsTrue(checker());
            a.Edges.Clear();
            b.Edges.Clear();
            Assert.IsFalse(checker());
        }

        [TestMethod]
        public void TestCircularConnections()
        {
            var a = new Node { Name = "A" };
            var b = new Node { Name = "B" };
            var c = new Node { Name = "C" };
            var d = new Node { Name = "D" };
            a.Edges.Add(b);
            a.Edges.Add(c);
            a.Edges.Add(d);
            b.Edges.Add(a);
            b.Edges.Add(c);
            b.Edges.Add(d);
            c.Edges.Add(a);
            c.Edges.Add(b);
            c.Edges.Add(d);
            d.Edges.Add(a);
            d.Edges.Add(b);
            d.Edges.Add(c);

            var nodes = new ObservableList<Node> { a, b, c, d };
            var checker = CreateCheck(nodes, a, d);

            Assert.IsTrue(checker());
            a.Edges.Remove(c);
            c.Edges.Remove(a);
            Assert.IsTrue(checker());
            a.Edges.Remove(d);
            d.Edges.Remove(a);
            Assert.IsTrue(checker());
            b.Edges.Remove(d);
            d.Edges.Remove(b);
            Assert.IsTrue(checker());
            c.Edges.Remove(d);
            d.Edges.Remove(c);
            Assert.IsFalse(checker());
        }
    }

    [TestClass]
    public class HolmBatchConnectivityTests : ConnectivityTests
    {
        public override Func<bool> CreateCheck(IEnumerableExpression<Node> nodes, Node start, Node target)
        {
            return () => (new HolmConnectivity<Node>(n => n.Edges, false, nodes).AreConnected(start, target));
        }
    }

    [TestClass]
    public class HolmIncConnectivityTests : ConnectivityTests
    {
        public override Func<bool> CreateCheck(IEnumerableExpression<Node> nodes, Node start, Node target)
        {
            var holm = new HolmConnectivity<Node>(n => n.Edges, true, nodes);
            var inc = Observable.Expression(() => holm.AreConnected(start, target));
            return () => inc.Value;
        }
    }

    [TestClass]
    public class UnionFindConnectivityTests : ConnectivityTests
    {
        public override Func<bool> CreateCheck(IEnumerableExpression<Node> nodes, Node start, Node target)
        {
            return () => (new UnionFind<Node>(n => n.Edges, nodes).AreConnected(start, target));
        }
    }


}
