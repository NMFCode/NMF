using System;
using System.Linq.Expressions;
using NMF.Glsp.Processing;

namespace NMF.Glsp.Language
{
    public abstract class EdgeDescriptor<TTransition> : NodeDescriptor<TTransition>
    {
        private GEdgeSkeleton<TTransition> _skeleton;

        internal override GElementSkeleton<TTransition> CreateSkeleton()
        {
            return _skeleton = new GEdgeSkeleton<TTransition>();
        }

        protected void SourceNode<TSource>(NodeDescriptor<TSource> descriptor, Expression<Func<TTransition, object>> selector, bool canChangeSource = true)
        {
            _skeleton.SourceSkeleton = descriptor.CurrentSkeleton;
            _skeleton.SourceSelector = selector;
            _skeleton.CanChangeSource = canChangeSource;
        }

        protected void TargetNode<TTarget>(NodeDescriptor<TTarget> descriptor, Expression<Func<TTransition, object>> selector, bool canChangeTarget = true)
        {
            _skeleton.TargetSkeleton = descriptor.CurrentSkeleton;
            _skeleton.SourceSelector = selector;
            _skeleton.CanChangeTarget = canChangeTarget;
        }
    }

    public abstract class EdgeDescriptor<TSource, TTarget> : EdgeDescriptor<(TSource, TTarget)>
    {
        public abstract NodeDescriptor<TSource> SourceDescriptor { get; }

        public abstract NodeDescriptor<TTarget> TargetDescriptor { get; }

        protected internal override void DefineLayout()
        {
            SourceNode(SourceDescriptor, pair => pair.Item1, false);
            TargetNode(TargetDescriptor, pair => pair.Item2, false);
        }
    }
}
