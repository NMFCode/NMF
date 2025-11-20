using NMF.Glsp.Graph;
using NMF.Glsp.Language;
using NMF.Glsp.Notation;
using NMF.Glsp.Protocol.Types;
using NMF.Models;
using NMF.Utilities;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Linq;

namespace NMF.Glsp.Processing
{
    internal class GEdgeSkeleton<T> : GElementSkeleton<T>
    {
        public GEdgeSkeleton(EdgeDescriptor<T> elementDescriptor) : base(elementDescriptor)
        {
        }

        public EdgeHelper<T> Source { get; set; }

        public EdgeHelper<T> Target { get; set; }

        protected override GElement CreateElement(T input, ISkeletonTrace trace, ref INotationElement notation)
        {
            var traceResult = trace.ResolveElement(input, this);
            if (traceResult != null)
            {
                return traceResult;
            }

            var edgeNotation = notation as IEdge;

            var edge = new GEdge(edgeNotation?.Id);
            edge.IsManualLayout = edgeNotation != null;
            LoadNotation(input, ref notation, ref edgeNotation, edge);
            if (edgeNotation != null)
            {
                LoadRoutingPoints(edgeNotation, edge);
            }

            Source.SetSourceId(input, trace, edgeNotation, edge);
            Target.SetTargetId(input, trace, edgeNotation, edge);

            ConfigureRouterKind(edge);

            return edge;
        }

        private void ConfigureRouterKind(GEdge edge)
        {
            var edgeDescriptor = (EdgeDescriptor<T>)_elementDescriptor;
            switch (edgeDescriptor.RouterKind)
            {
                case RouterKind.Manhattan:
                    edge.Details.Add("routerKind", "manhattan");
                    break;
                case RouterKind.Bezier:
                    edge.Details.Add("routerKind", "bezier");
                    break;
            }
        }

        private void LoadRoutingPoints(IEdge edgeNotation, GEdge edge)
        {
            foreach (var routingPoint in edgeNotation.BendPoints)
            {
                edge.RoutingPoints.Add(new Point(routingPoint.X, routingPoint.Y));
            }
            edge.RoutingPoints.AsNotifiable().CollectionChanged += (s, e) => UpdateRoutingPoints(edgeNotation, e);
        }

        private static void LoadNotation(T input, ref INotationElement notation, ref IEdge edgeNotation, GEdge edge)
        {
            if (edgeNotation == null && input is IModelElement semanticElement)
            {
                edgeNotation = new Edge
                {
                    Id = edge.Id,
                    SemanticElement = semanticElement
                };
                notation = edgeNotation;
            }
        }

        private void UpdateRoutingPoints(IEdge edgeNotation, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                edgeNotation.BendPoints.Clear();
            }
            if (e.OldItems != null)
            {
                foreach (Point routingPoint in e.OldItems)
                {
                    if (e.OldStartingIndex != -1 && e.OldStartingIndex < edgeNotation.BendPoints.Count && PointEquals(edgeNotation.BendPoints[e.OldStartingIndex], routingPoint))
                    {
                        edgeNotation.BendPoints.RemoveAt(e.OldStartingIndex);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
            if (e.NewItems != null)
            {
                foreach(Point routingPoint in e.NewItems)
                {
                    edgeNotation.BendPoints.Add(new GPoint
                    {
                        X = routingPoint.X,
                        Y = routingPoint.Y,
                    });
                }
            }
        }

        private bool PointEquals(IGPoint gPoint, Point routingPoint)
        {
            return gPoint.X == routingPoint.X && gPoint.Y == routingPoint.Y;
        }

        public override string[] CalculateSourceTypeIds()
        {
            return Source.Skeleton.Closure(sk => sk.Refinements).Select(sk => sk.TypeName).ToArray();
        }

        public override string[] CalculateTargetTypeIds()
        {
            return Target.Skeleton.Closure(sk => sk.Refinements).Select(sk => sk.TypeName).ToArray();
        }
    }
}
