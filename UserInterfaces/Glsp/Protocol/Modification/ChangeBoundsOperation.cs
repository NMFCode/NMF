using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.Modification
{
    /// <summary>
    /// Triggers the position or size change of elements. This action concerns only the element’s graphical size 
    /// and position. Whether an element can be resized or repositioned may be specified by the server with a 
    /// TypeHint to allow for immediate user feedback before resizing or repositioning.
    /// </summary>
    public class ChangeBoundsOperation : Operation
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string ChangeBoundsOperationKind = "changeBounds";

        /// <inheritdoc/>
        public override string Kind => ChangeBoundsOperationKind;

        /// <summary>
        ///  The new bounds of the respective elements.
        /// </summary>
        public ElementAndBounds[] NewBounds { get; init; }

        /// <inheritdoc/>
        public override Task ExecuteAsync(IGlspSession session)
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
                        element.Parent?.UpdateLayout();
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
