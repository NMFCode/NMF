using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Layout
{
    /// <summary>
    /// Request a layout of the diagram or selected elements from the server.
    /// </summary>
    public class LayoutOperation : Operation
    {
        /// <inheritdoc/>
        public override string Kind => "layout";

        /// <summary>
         ///  The identifiers of the elements that should be layouted, will default to the root element if not defined.
         /// </summary>
        public string[] ElementIds { get; set; }

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            throw new NotImplementedException();
        }
    }
}
