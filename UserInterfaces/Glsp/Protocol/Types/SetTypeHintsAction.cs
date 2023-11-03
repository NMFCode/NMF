using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// Sent from the server to the client in order to provide hints certain modifications are allowed for a specific element type.
    /// </summary>
    public class SetTypeHintsAction : ResponseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SetTypeHintsActionKind = "setTypeHints";

        /// <inheritdoc/>
        public override string Kind => SetTypeHintsActionKind;

        /// <summary>
        ///  The hints for shape types.
        /// </summary>
        public ShapeTypeHint[] ShapeHints { get; init; }

        /// <summary>
        ///  The hints for edge types.
        /// </summary>
        public EdgeTypeHint[] EdgeHints { get; init; }
    }
}
