using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Notation;

namespace NMF.Glsp.Processing
{
    internal abstract class EdgeHelper<T>
    {
        public GElementSkeletonBase Skeleton { get; set; }

        public bool CanChange { get; set; }

        public abstract void SetTargetId(T input, ISkeletonTrace trace, IEdge edgeNotation, GEdge edge);

        public abstract void SetSourceId(T input, ISkeletonTrace trace, IEdge edgeNotation, GEdge edge);

        public abstract void SetElement(GEdge edge, GElement element);
    }

    internal class EdgeHelper<T, TValue> : EdgeHelper<T>
    {
        public ObservingFunc<T, TValue> Selector { get; set; }

        private void SetEdgeTarget(ISkeletonTrace trace, IEdge edgeNotation, GEdge edge, INotifyValue<TValue> targetValue)
        {
            if (targetValue.Value != null)
            {
                var targetElement = trace.ResolveElement(targetValue.Value, Skeleton);
                edge.TargetId = targetElement.Id;
                if (edgeNotation != null) { edgeNotation.Target = targetElement.NotationElement; }
            }
            targetValue.ValueChanged += (o, e) => UpdateTarget(edge, e.NewValue, trace);
            edge.Collectibles.Add(Selector, targetValue);
        }

        private void SetEdgeSource(ISkeletonTrace trace, IEdge edgeNotation, GEdge edge, INotifyValue<TValue> sourceValue)
        {
            if (sourceValue.Value != null)
            {
                var sourceElement = trace.ResolveElement(sourceValue.Value, Skeleton);
                edge.SourceId = sourceElement.Id;
                if (edgeNotation != null) edgeNotation.Source = sourceElement.NotationElement;
            }
            sourceValue.ValueChanged += (o, e) => UpdateSource(edge, e.NewValue, trace);
            edge.Collectibles.Add(Selector, sourceValue);
        }

        public override void SetTargetId(T input, ISkeletonTrace trace, IEdge edgeNotation, GEdge edge)
        {
            if (CanChange)
            {
                var targetValue = Selector.InvokeReversable(input);
                SetEdgeTarget(trace, edgeNotation, edge, targetValue);
                edge.TargetIdChanged += TargetIdChanged;
            }
            else
            {
                var targetValue = Selector.Observe(input);
                targetValue.Successors.SetDummy();
                SetEdgeTarget(trace, edgeNotation, edge, targetValue);
            }
        }

        public override void SetSourceId(T input, ISkeletonTrace trace, IEdge edgeNotation, GEdge edge)
        {
            if (CanChange)
            {
                var sourceValue = Selector.InvokeReversable(input);
                SetEdgeSource(trace, edgeNotation, edge, sourceValue);
                edge.SourceIdChanged += SourceIdChanged;
            }
            else
            {
                var sourceValue = Selector.Observe(input);
                sourceValue.Successors.SetDummy();
                SetEdgeSource(trace, edgeNotation, edge, sourceValue);
            }
        }


        private void TargetIdChanged(GEdge edge)
        {
            var graph = edge.Graph;
            if (graph == null) return;
            var element = graph.Resolve(edge.TargetId);
            SetElement(edge, element);
        }

        private void UpdateTarget(GEdge edge, object newValue, ISkeletonTrace trace)
        {
            var newTargetElement = trace.ResolveElement(newValue, Skeleton);
            if (edge.NotationElement is IEdge notationEdge)
            {
                notationEdge.Target = newTargetElement.NotationElement;
            }
            edge.SilentSetTarget(newTargetElement.Id);
        }

        private void SourceIdChanged(GEdge edge)
        {
            var graph = edge.Graph;
            if (graph == null) return;
            var element = graph.Resolve(edge.SourceId);
            SetElement(edge, element);
        }

        private void UpdateSource(GEdge edge, object newValue, ISkeletonTrace trace)
        {
            var newSourceElement = trace.ResolveElement(newValue, Skeleton);
            if (edge.NotationElement is IEdge notationEdge)
            {
                notationEdge.Source = newSourceElement.NotationElement;
            }
            edge.SilentSetSource(newSourceElement.Id);
        }

        public override void SetElement(GEdge edge, GElement element)
        {
            if (element != null && edge.Collectibles.TryGetValue(Selector, out var disposable) && disposable is INotifyReversableExpression<TValue> setter)
            {
                setter.Value = (TValue)element.CreatedFrom;
            }
        }
    }
}
