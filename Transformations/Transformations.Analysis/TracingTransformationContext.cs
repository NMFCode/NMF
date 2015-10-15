using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Transformations.Trace
{
    public class TracingTransformationContext : TransformationContext
    {
        private int counter = 0;
        private StringBuilder nodes = new StringBuilder();
        private StringBuilder links = new StringBuilder();

        public TracingTransformationContext(Transformation transformation) : base(transformation) { }

        protected sealed override ComputationContext CreateComputationContext(object[] input, GeneralTransformationRule rule)
        {
            var context = new TracingComputationContext(CreateId(input, rule), this);

            nodes.AppendFormat("    <Node Id=\"{0}\" Label=\"{1}\"/>", context.Id, CreateLabel(input, rule));
            nodes.AppendLine();

            return context;
        }

        protected virtual string CreateId(object[] input, GeneralTransformationRule rule)
        {
            counter++;
            return "node" + counter.ToString();
        }

        protected virtual string CreateLabel(object[] input, GeneralTransformationRule rule)
        {
            var sb = new StringBuilder();
            sb.Append("[");
            if (input[0] != null)
            {
                sb.Append(input[0].ToString());
            }
            else
            {
                sb.Append("(null)");
            }
            for (int i = 1; i < input.Length; i++)
            {
                sb.Append(",");
                if (input[i] != null)
                {
                    sb.Append(input[i].ToString());
                }
                else
                {
                    sb.Append("(null)");
                }
            }
            sb.Append("], ");
            sb.Append(rule.ToString());
            return sb.ToString();
        }

        private void AppendDependency(string fromId, string toId)
        {
            links.AppendFormat("    <Link Source=\"{0}\" Target=\"{1}\"/>", fromId, toId);
            links.AppendLine();
        }

        public void WriteTraceGraph(TextWriter writer)
        {
            writer.WriteLine("<DirectedGraph xmlns=\"http://schemas.microsoft.com/vs/2009/dgml\">");
            writer.WriteLine("  <Nodes>");
            writer.Write(nodes.ToString());
            writer.WriteLine("  </Nodes>");
            writer.WriteLine("  <Links>");
            writer.Write(links.ToString());
            writer.WriteLine("  </Links>");
            writer.WriteLine("</DirectedGraph>");
        }

        public class TracingComputationContext : ComputationContext
        {
            public string Id
            {
                get;
                private set;
            }

            public TracingComputationContext(string id, TracingTransformationContext context)
                : base(context)
            {
                Id = id;
            }

            public override void MarkRequire(Computation other, bool isRequired)
            {
                base.MarkRequire(other, isRequired);
                if (isRequired)
                {
                    var tracingCompContext = other.Context as TracingComputationContext;
                    var tracingContext = TransformationContext as TracingTransformationContext;
                    if (tracingCompContext != null && tracingContext != null)
                    {
                        tracingContext.AppendDependency(Id, tracingCompContext.Id);
                    }
                }
            }
        }
    }
}
