﻿using NMF.Glsp.Protocol.BaseProtocol;

namespace NMF.Glsp.Protocol.Selection
{
    /// <summary>
    /// Triggered when the user changes the selection, e.g. by clicking on a selectable element. The action should trigger 
    /// a change in the selected state accordingly, so the elements can be rendered differently. The server can send such 
    /// an action to the client in order to change the selection remotely.
    /// </summary>
    public class SelectAction : BaseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SelectActionKind = "elementSelected";

        /// <inheritdoc/>
        public override string Kind => SelectActionKind;


        /// <summary>
        ///  The identifier of the elements to mark as selected.
        /// </summary>
        public string[] SelectedElementsIDs { get; set; }

        /// <summary>
        ///  The identifier of the elements to mark as not selected.
        /// </summary>
        public string[] DeselectedElementsIDs { get; set; }

        /// <summary>
        ///  Whether all currently selected elements should be deselected.
        /// </summary>
        public bool? DeselectAll { get; set; }
    }
}