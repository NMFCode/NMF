using NMF.Glsp.Graph;
using NMF.Glsp.Server.Contracts;
using NMF.Glsp.Server.Protocol.BaseProtocol;
using NMF.Glsp.Server.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Modification
{
    /// <summary>
    /// Triggers the position or size change of elements. This action concerns only the element’s graphical size 
    /// and position. Whether an element can be resized or repositioned may be specified by the server with a 
    /// TypeHint to allow for immediate user feedback before resizing or repositioning.
    /// </summary>
    public class ChangeBoundsOperation : Operation
    {
        /// <inheritdoc/>
        public override string Kind => "changeBounds";

        /// <summary>
        ///  The new bounds of the respective elements.
        /// </summary>
        public ElementAndBounds[] NewBounds { get; init; }

        /// <inheritdoc/>
        public override void Execute(IClientSession session)
        {
            if (NewBounds != null)
            {
                foreach (var elementWithBounds in NewBounds)
                {
                    var element = session.Root.Resolve(elementWithBounds.ElementId);
                    if (element != null)
                    {
                        if (elementWithBounds.NewPosition.HasValue)
                        {
                            element.Position = elementWithBounds.NewPosition.Value;
                        }
                        if (elementWithBounds.NewSize.HasValue)
                        {
                            element.Size = elementWithBounds.NewSize.Value;
                        }
                    }
                }
            }
        }
    }
}
