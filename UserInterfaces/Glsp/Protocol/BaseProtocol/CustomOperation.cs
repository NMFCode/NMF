using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.BaseProtocol
{
    public class CustomOperation : Operation
    {
        public CustomOperation(string kind)
        {
            Kind = kind;
        }

        public override string Kind { get; }

        public string ElementId { get; set; }

        /// <summary>
        ///  Custom arguments.
        /// </summary>
        public IDictionary<string, object> Args { get; init; }

        public override Task Execute(IGlspSession session)
        {
            var element = session.Root.Resolve(ElementId);
            return element.Perform(Kind, session, Args) ?? Task.CompletedTask;
        }
    }
}
