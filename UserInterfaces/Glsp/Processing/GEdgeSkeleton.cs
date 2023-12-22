using NMF.Expressions;
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
        public GEdgeSkeleton(ElementDescriptor<T> elementDescriptor) : base(elementDescriptor)
        {
            Type = DefaultTypes.Edge;
        }

        public GElementSkeletonBase SourceSkeleton { get; set; }

        public ObservingFunc<T, object> SourceSelector { get; set; }

        public bool CanChangeSource { get; set; }

        public GElementSkeletonBase TargetSkeleton { get; set; }

        public ObservingFunc<T, object> TargetSelector { get; set; }

        public bool CanChangeTarget { get; set; }

        protected override GElement CreateElement(T input, ISkeletonTrace trace, ref INotationElement notation)
        {
            var traceResult = trace.ResolveElement(input, this);
            if (traceResult != null)
            {
                return traceResult;
            }

            var edgeNotation = notation as IEdge;

            var edge = new GEdge(edgeNotation?.Id);
            if (edgeNotation == null && input is IModelElement semanticElement)
            {
                edgeNotation = new Edge
                {
                    Id = edge.Id,
                    SemanticElement = semanticElement
                };
                notation = edgeNotation;
            }
            if (edgeNotation != null)
            {
                foreach (var routingPoint in edgeNotation.BendPoints)
                {
                    edge.RoutingPoints.Add(new Point(routingPoint.X, routingPoint.Y));
                }
                edge.RoutingPoints.AsNotifiable().CollectionChanged += (s, e) => UpdateRoutingPoints(edgeNotation, e);
            }

            if (CanChangeSource)
            {
                var sourceValue = SourceSelector.InvokeReversable(input);
                SetEdgeSource(trace, edgeNotation, edge, sourceValue);
                edge.SourceIdChanged += SourceIdChanged;
            }
            else
            {
                var sourceValue = SourceSelector.Observe(input);
                SetEdgeSource(trace, edgeNotation, edge, sourceValue);
            }

            if (CanChangeTarget)
            {
                var targetValue = TargetSelector.InvokeReversable(input);
                SetEdgeTarget(trace, edgeNotation, edge, targetValue);
                edge.TargetIdChanged += TargetIdChanged;
            }
            else
            {
                var targetValue = TargetSelector.Observe(input);
                SetEdgeTarget(trace, edgeNotation, edge, targetValue);
            }

            return edge;
        }

        private void SetEdgeTarget(ISkeletonTrace trace, IEdge edgeNotation, GEdge edge, INotifyValue<object> targetValue)
        {
            if (targetValue.Value != null)
            {
                var targetElement = trace.ResolveElement(targetValue.Value, TargetSkeleton);
                edge.TargetId = targetElement.Id;
                targetValue.ValueChanged += (o, e) => UpdateTarget(edge, e.NewValue, trace);
                edge.Collectibles.Add(TargetSelector, targetValue);
                if (edgeNotation != null) { edgeNotation.Target = targetElement.NotationElement; }
            }
        }

        private void SetEdgeSource(ISkeletonTrace trace, IEdge edgeNotation, GEdge edge, INotifyValue<object> sourceValue)
        {
            if (sourceValue.Value != null)
            {
                var sourceElement = trace.ResolveElement(sourceValue.Value, SourceSkeleton);
                edge.SourceId = sourceElement.Id;
                sourceValue.ValueChanged += (o, e) => UpdateSource(edge, e.NewValue, trace);
                edge.Collectibles.Add(SourceSelector, sourceValue);
                if (edgeNotation != null) edgeNotation.Source = sourceElement.NotationElement;
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

        private void TargetIdChanged(GEdge edge)
        {
            var element = edge.Graph.Resolve(edge.TargetId);
            if (element != null && edge.Collectibles.TryGetValue(TargetSelector, out var disposable) && disposable is INotifyReversableValue<object> targetSetter)
            {
                targetSetter.Value = element.CreatedFrom;
            }
        }

        private void UpdateTarget(GEdge edge, object newValue, ISkeletonTrace trace)
        {
            var newTargetElement = trace.ResolveElement(newValue, TargetSkeleton);
            if (edge.NotationElement is IEdge notationEdge)
            {
                notationEdge.Target = newTargetElement.NotationElement;
            }
            edge.SilentSetTarget(newTargetElement.Id);
        }

        private void SourceIdChanged(GEdge edge)
        {
            var element = edge.Graph.Resolve(edge.SourceId);
            if (element != null && edge.Collectibles.TryGetValue(SourceSelector, out var disposable) && disposable is INotifyReversableValue<object> sourceSetter)
            {
                sourceSetter.Value = element.CreatedFrom;
            }
        }

        private void UpdateSource(GEdge edge, object newValue, ISkeletonTrace trace)
        {
            var newSourceElement = trace.ResolveElement(newValue, SourceSkeleton);
            if (edge.NotationElement is IEdge notationEdge)
            {
                notationEdge.Source = newSourceElement.NotationElement;
            }
            edge.SilentSetSource(newSourceElement.Id);
        }

        public override string[] CalculateSourceTypeIds()
        {
            return SourceSkeleton.Closure(sk => sk.Refinements).Select(sk => sk.TypeName).ToArray();
        }

        public override string[] CalculateTargetTypeIds()
        {
            return TargetSkeleton.Closure(sk => sk.Refinements).Select(sk => sk.TypeName).ToArray();
        }
    }
}
