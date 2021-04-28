using NMF.Transformations.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Transformations.Trace
{
    /// <summary>
    /// Denotes a transformation context implementation that creates an image of the transformation using DGML
    /// </summary>
    public class TracingTransformationContext : TransformationContext
    {
        private int counter = 0;
        private readonly StringBuilder nodes = new StringBuilder();
        private readonly StringBuilder links = new StringBuilder();

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="transformation"></param>
        public TracingTransformationContext(Transformation transformation) : base(transformation) { }

        /// <inheritdoc />
        protected sealed override ComputationContext CreateComputationContext(object[] input, GeneralTransformationRule rule)
        {
            var context = new TracingComputationContext(CreateId(input, rule), this);

            nodes.AppendFormat("    <Node Id=\"{0}\" Label=\"{1}\"/>", context.Id, CreateLabel(input, rule));
            nodes.AppendLine();

            return context;
        }

        /// <summary>
        /// Creates the id for the input represented by the given inputs
        /// </summary>
        /// <param name="input">The inputs</param>
        /// <param name="rule">The transformation rule</param>
        /// <returns>An identifier for the node</returns>
        protected virtual string CreateId(object[] input, GeneralTransformationRule rule)
        {
            counter++;
            return "node" + counter.ToString();
        }

        /// <summary>
        /// Creates a label for the computation of the given inputs
        /// </summary>
        /// <param name="input">The inputs</param>
        /// <param name="rule">The transformation rule</param>
        /// <returns>A label for the node representing the computation of the inputs</returns>
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

        /// <summary>
        /// Writes the trace graph to the given text writer
        /// </summary>
        /// <param name="writer">The text writer</param>
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

        private class TracingComputationContext : ComputationContext
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
                    if (other.Context is TracingComputationContext tracingCompContext && TransformationContext is TracingTransformationContext tracingContext)
                    {
                        tracingContext.AppendDependency(Id, tracingCompContext.Id);
                    }
                }
            }
        }
    }
}
