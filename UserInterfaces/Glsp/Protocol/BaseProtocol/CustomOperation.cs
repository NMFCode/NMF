using NMF.Glsp.Server.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.BaseProtocol
{
    /// <summary>
    /// Denotes a class for custom operations
    /// </summary>
    public class CustomOperation : Operation
    {
        /// <summary>
        /// Creates a new custom operation with the given kind
        /// </summary>
        /// <param name="kind"></param>
        public CustomOperation(string kind)
        {
            Kind = kind;
        }

        /// <inheritdoc />
        public override string Kind { get; }

        /// <summary>
        /// Gets or sets the element for which this operation was created
        /// </summary>
        public string ElementId { get; set; }

        /// <summary>
        /// Custom arguments.
        /// </summary>
        public IDictionary<string, object> Args { get; init; }

        /// <inheritdoc />
        public override Task Execute(IGlspSession session)
        {
            var element = session.Root.Resolve(ElementId);
            if (element != null && element.TryPerform(Kind, session, Args, out var operation))
            {
                return operation;
            }
            return Task.CompletedTask;
        }
    }
}
