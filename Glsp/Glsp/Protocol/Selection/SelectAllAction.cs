using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Selection
{
    /// <summary>
    /// Used for selecting or deselecting all elements.
    /// </summary>
    public class SelectAllAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SelectAllActionKind = "allSelected";

        /// <inheritdoc/>
        public override string Kind => SelectAllActionKind;

        /// <summary>
        ///  If `select` is true, all elements are selected, otherwise they are deselected.
        /// </summary>
        public bool Select { get; set; }
    }
}
