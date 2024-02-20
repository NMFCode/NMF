using NMF.Glsp.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    internal class AdHocEdgeDescriptor<TSource, TTarget> : EdgeDescriptor<TSource, TTarget>
    {
        public Func<string> ToolLabelFunc { get; set; }

        public override string ToolLabel(string profile)
        {
            return ToolLabelFunc?.Invoke() ?? base.ToolLabel(profile);
        }

        public AdHocEdgeDescriptor(NodeDescriptor<TSource> sourceDescriptor, NodeDescriptor<TTarget> targetDescriptor)
        {
            SourceDescriptor = sourceDescriptor;
            TargetDescriptor = targetDescriptor;

            DefineLayout();
            Type(DefaultTypes.Edge);
        }

        public override NodeDescriptor<TSource> SourceDescriptor { get; }

        public override NodeDescriptor<TTarget> TargetDescriptor { get; }

        public IAdhocEdgeSyntax CreateSyntax(ActionElement element) => new Syntax(element, this);

        private class Syntax : IAdhocEdgeSyntax
        {
            private readonly AdHocEdgeDescriptor<TSource, TTarget> _descriptor;
            private readonly ActionElement _element;

            public Syntax(ActionElement actionElement, AdHocEdgeDescriptor<TSource, TTarget> descriptor)
            {
                _descriptor = descriptor;
                _element = actionElement;
            }

            public IAdhocEdgeSyntax HideIn(Func<string, bool> contextIdPredicate)
            {
                _element.AddFilter(contextIdPredicate);
                return this;
            }

            public IAdhocEdgeSyntax WithLabel(Func<string> label)
            {
                _descriptor.ToolLabelFunc = label;
                return this;
            }

            public IAdhocEdgeSyntax WithType(string type)
            {
                _descriptor.Type(type); 
                return this;
            }
        }
    }
}
