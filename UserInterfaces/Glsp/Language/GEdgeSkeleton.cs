using NMF.Expressions;
using NMF.Glsp.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    internal class GEdgeSkeleton<T> : GElementSkeleton<T>
    {
        public ObservingFunc<T, object> SourceSelector { get; set; }

        public bool CanChangeSource { get; set; }

        public ObservingFunc<T, object> TargetSelector { get; set; }

        public bool CanChangeTarget { get; set; }

        protected override GElement CreateElement(T input)
        {
            var edge = new GEdge();

            return edge;
        }
    }
}
