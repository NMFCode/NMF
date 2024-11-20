using NMF.Glsp.Contracts;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;

namespace NMF.Glsp.Protocol.Layout
{
    /// <summary>
    /// Sent from the client to the server to transmit the result of bounds computation as a response 
    /// to a RequestBoundsAction. If the server is responsible for parts of the layout, it can do so 
    /// after applying the computed bounds received with this action. Otherwise there is no need to 
    /// send the computed bounds to the server, so they can be processed locally by the client.
    /// </summary>
    public class ComputedBoundsAction : ResponseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string ComputedBoundsActionKind = "computedBounds";

        /// <inheritdoc/>
        public override string Kind => ComputedBoundsActionKind;


        /// <summary>
        ///  The new bounds of the model elements.
        /// </summary>
        public ElementAndBounds[] Bounds { get; set; }

        /// <summary>
        ///  The revision number.
        /// </summary>
        public int? Revision { get; set; }

        /// <summary>
        ///  The new alignment of the model elements.
        /// </summary>
        public ElementAndAlignment[] Alignments { get; set; }

        /// <summary>
        ///  The route of the model elements.
        /// </summary>
        public ElementAndRoutingPoints[] Routes { get; set; }

        /// <summary>
        /// Updates bounds of elements as sent by the client
        /// </summary>
        /// <param name="session"></param>
        public void UpdateBounds(IGlspSession session)
        {
            ApplyBounds(session);

            ApplyAlignments(session);

            if (Routes != null)
            {
                foreach (var route in Routes)
                {
                    var edge = session.Root.Resolve(route.ElementId);
                    if (edge != null)
                    {

                    }
                }
            }
        }

        private void ApplyAlignments(IGlspSession session)
        {
            if (Alignments != null)
            {
                foreach (var alignment in Alignments)
                {
                    var element = session.Root.Resolve(alignment.ElementId);
                    if (element != null)
                    {
                        element.Alignment = alignment.NewAlignment;
                    }
                }
            }
        }

        private void ApplyBounds(IGlspSession session)
        {
            if (Bounds != null)
            {
                foreach (var bounds in Bounds)
                {
                    var element = session.Root.Resolve(bounds.ElementId);
                    if (element != null)
                    {
                        if (bounds.NewPosition != null) element.Position = bounds.NewPosition;
                        if (bounds.NewSize != null) element.Size = bounds.NewSize;
                        element.Parent?.UpdateLayout();
                    }
                }
            }
        }

        private void SetPosition(GElement element, ElementAndAlignment alignment)
        {
            if (element.Parent != null)
            {
                element.Parent.Skeleton.LayoutStrategy.SetPosition(element, alignment.NewAlignment);
            }
        }
    }
}
