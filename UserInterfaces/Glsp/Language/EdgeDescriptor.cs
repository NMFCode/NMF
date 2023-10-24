using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    public abstract class EdgeDescriptor<TTransition> : NodeDescriptor<TTransition>
    {
        private GEdgeSkeleton<TTransition> _skeleton;

        internal override GElementSkeleton<TTransition> CreateSkeleton()
        {
            return _skeleton = new GEdgeSkeleton<TTransition>();
        }

        protected void SourceNode(Expression<Func<TTransition, object>> selector, bool canChangeSource = false)
        {
            _skeleton.SourceSelector = selector;
            _skeleton.CanChangeSource = canChangeSource;
        }

        protected void TargetNode(Expression<Func<TTransition, object>> selector, bool canChangeTarget = false)
        {
            _skeleton.SourceSelector = selector;
            _skeleton.CanChangeTarget = canChangeTarget;
        }
    }

    public class EdgeDescriptor<TSource, TTarget> : EdgeDescriptor<(TSource, TTarget)>
    {
        protected internal override void DefineLayout()
        {
            SourceNode(pair => pair.Item1);
            TargetNode(pair => pair.Item2);
        }
    }
}
