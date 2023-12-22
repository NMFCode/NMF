using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    internal class AdHocEdgeDescriptor<TSource, TTarget> : EdgeDescriptor<TSource, TTarget>
    {
        public AdHocEdgeDescriptor(NodeDescriptor<TSource> sourceDescriptor, NodeDescriptor<TTarget> targetDescriptor)
        {
            SourceDescriptor = sourceDescriptor;
            TargetDescriptor = targetDescriptor;

            DefineLayout();
        }

        public override NodeDescriptor<TSource> SourceDescriptor { get; }

        public override NodeDescriptor<TTarget> TargetDescriptor { get; }
    }
}
