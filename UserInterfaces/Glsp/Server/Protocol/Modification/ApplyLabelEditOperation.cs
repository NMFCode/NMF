using NMF.Glsp.Graph;
using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Modification
{
    /// <summary>
    /// A very common use case in domain models is the support of labels that display textual information to the user. 
    /// For instance, the GGraph model has support for labels that can be attached to a node, edge, or port, and that 
    /// contain some text that is rendered in the view. To apply new text to such a label element the client may send 
    /// an ApplyLabelEditOperation to the server.
    /// </summary>
    public class ApplyLabelEditOperation : Operation
    {
        /// <inheritdoc/>
        public override string Kind => "applyLabelEdit";

        /// <summary>
         ///  Identifier of the label model element.
         /// </summary>
        public string LabelId { get; init; }

        /// <summary>
         ///  Text that should be applied on the label.
         /// </summary>
        public string Text { get; init; }

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            var label = session.Root.Resolve(LabelId) as GLabel;
            if (label == null)
            {
                throw new InvalidOperationException("Label does not exist");
            }
            label.Text = Text;
        }
    }
}
