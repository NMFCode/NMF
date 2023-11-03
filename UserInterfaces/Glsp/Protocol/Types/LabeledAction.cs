using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// Labeled actions are used to denote a group of actions in a user-interface context, 
    /// e.g., to define an entry in the command palette or in the context menu.
    /// </summary>
    public class LabeledAction
    {
        /// <summary>
        ///  Group label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///  Actions in the group.
        /// </summary>
        public BaseAction[] Actions { get; init; }

        /// <summary>
        ///  Optional group icon.
        /// </summary>
        public string Icon { get; set; }
    }
}
