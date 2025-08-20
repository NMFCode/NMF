using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Layout.Layered;
using NMF.Glsp.Contracts;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Point = NMF.Glsp.Protocol.Types.Point;

namespace NMF.Glsp.Processing.Layouting
{
    /// <summary>
    /// Denotes the base class for a layout service using the AGL library
    /// </summary>
    public abstract class AglLayoutService : ILayoutEngine
    {
        private const double DefaultWidth = 40;
        private const double DefaultHeight = 10;

        /// <inheritdoc />
        public void CalculateLayout(GGraph graph)
        {
            var g = new GeometryGraph();
            var lookup = new Dictionary<string, Node>();
            var edges = new List<GEdge>();

            ProcessElementChildren(graph, g, g.RootCluster, lookup, edges);
            foreach (var edge in edges)
            {
                Edge e = CreateEdge(lookup, edge);
                if (e != null) g.Edges.Add(e);
            }
            g.RootCluster.UserData = graph;

            if (lookup.Count == 0)
            {
                return;
            }

            g.RootCluster.BoundaryCurve = CurveFactory.CreateRectangle(g.BoundingBox);

            ProcessLayout(g);

            PostProcess(g.RootCluster);
            foreach (var edge in g.Edges)
            {
                PostProcess(edge);
            }
            graph.Position = new Point(0, 0);
        }

        /// <inheritdoc />
        public void CalculateLayout(IEnumerable<GElement> elements)
        {
            var g = new GeometryGraph();
            var lookup = new Dictionary<string, Node>();
            var edges = new List<GEdge>();

            foreach (var element in elements.OfType<GNode>())
            {
                ProcessElement(element, g, g.RootCluster, lookup, edges);
            }

            foreach (var edge in edges)
            {
                Edge e = CreateEdge(lookup, edge);
                if (e != null) g.Edges.Add(e);
            }

            g.RootCluster.BoundaryCurve = CurveFactory.CreateRectangle(g.BoundingBox);

            ProcessLayout(g);

            PostProcess(g.RootCluster);
            foreach (var edge in g.Edges)
            {
                PostProcess(edge);
            }
        }

        /// <summary>
        /// Processes the layout of the AGL graph
        /// </summary>
        /// <param name="g">the AGL graph</param>
        protected abstract void ProcessLayout(GeometryGraph g);

        private static Edge CreateEdge(Dictionary<string, Node> lookup, GEdge edge)
        {
            if (edge.SourceId != null
                && edge.TargetId != null
                && lookup.TryGetValue(edge.SourceId, out var sourceNode)
                && lookup.TryGetValue(edge.TargetId, out var targetNode))
            {
                var e = new Edge(sourceNode, targetNode);
                e.UserData = edge;

                foreach (var label in edge.Children.OfType<GLabel>().Where(l => l.Size != null))
                {
                    e.Labels.Add(new Label(label.Size.Value.Width, label.Size.Value.Height, e)
                    {
                        UserData = label,
                        PlacementOffset = label.EdgeLabelPlacement?.Position ?? 0.5
                    });
                }

                return e;
            }
            return null;
        }

        private void ProcessElementChildren(GElement element, GeometryGraph graph, Cluster cluster, Dictionary<string, Node> lookup, List<GEdge> edges)
        {
            foreach (var child in element.Children)
            {
                ProcessElement(child, graph, cluster, lookup, edges);
            }
        }

        private void ProcessElement(GElement child, GeometryGraph graph, Cluster cluster, Dictionary<string, Node> lookup, List<GEdge> edges)
        {
            if (child.Type == null)
            {
                return;
            }
            switch (child)
            {
                case GNode node:
                    var isNode = !child.Skeleton.LayoutStrategy.NeedsLayout;
                    if (isNode)
                    {
                        var n = new Node(CurveFactory.CreateRectangle(CalculateBoundingBox(node)));
                        n.UserData = child;
                        lookup.Add(child.Id, n);
                        graph.Nodes.Add(n);
                        cluster.AddChild(n);
                        ProcessElementChildren(child, graph, cluster, lookup, edges);
                    }
                    else
                    {
                        var c = new Cluster();
                        c.UserData = child;
                        c.BoundaryCurve = CurveFactory.CreateRectangle(CalculateBoundingBox(node));
                        lookup.Add(child.Id, c);
                        cluster.AddChild(c);
                        graph.Nodes.Add(c);
                        ProcessElementChildren(child, graph, c, lookup, edges);
                    }
                    break;
                case GEdge edge:
                    edges.Add(edge);
                    break;
                case GLabel:
                    break;
                default:
                    ProcessElementChildren(child, graph, cluster, lookup, edges);
                    break;
            }
        }

        private void PostProcess(Cluster cluster)
        {
            foreach (var subCluster in cluster.Clusters)
            {
                PostProcess(subCluster);
            }
            foreach (var node in cluster.Nodes)
            {
                PostProcess(node);
            }
            PostProcess((Node)cluster);
        }

        private void PostProcess(Node node)
        {
            var element = (GElement)node.UserData;
            if (node.BoundaryCurve != null)
            {
                element.Position = new Point(node.Center.X - node.Width / 2, node.Center.Y - node.Height / 2);
                element.Size = new Dimension(node.Width, node.Height);
            }
            else
            {
                element.Position = ConvertPoint(node.BoundingBox.LeftTop);
                element.Size = new Dimension(node.BoundingBox.Width, node.BoundingBox.Height);
            }
        }

        private void PostProcess(Edge edge)
        {
            var gEdge = (GEdge)edge.UserData;
            gEdge.EdgeSourcePointX = edge.SourcePort.Location.X;
            gEdge.EdgeSourcePointY = edge.SourcePort.Location.Y;
            gEdge.EdgeTargetPointX = edge.TargetPort.Location.X;
            gEdge.EdgeTargetPointY = edge.TargetPort.Location.Y;

            if (edge.EdgeGeometry.HasWaypoints)
            {
                gEdge.RoutingPoints.Clear();
                foreach (var waypoint in edge.EdgeGeometry.Waypoints)
                {
                    gEdge.RoutingPoints.Add(ConvertPoint(waypoint));
                }
            }

            foreach (var label in edge.Labels)
            {
                var gLabel = (GLabel)label.UserData;
                gLabel.Position = ConvertPoint(label.BoundingBox.LeftTop);
            }
        }

        private Point ConvertPoint(Microsoft.Msagl.Core.Geometry.Point point)
        {
            return new Point(point.X, point.Y);
        }

        private Rectangle CalculateBoundingBox(GNode node)
        {
            var size = node.Size ?? new Dimension(DefaultWidth, DefaultHeight);
            var position = node.Position ?? new Point(0, 0);
            return new Rectangle(position.X, position.Y, position.X + size.Width, position.Y + size.Height);
        }
    }
}
