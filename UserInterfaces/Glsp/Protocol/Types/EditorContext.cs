﻿using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Types
{
    /// <summary>
    /// The EditorContext may be used to represent the current state of the editor for particular actions. 
    /// It encompasses the last recorded mouse position, the list of selected elements, and may contain 
    /// custom arguments to encode additional state information.
    /// </summary>
    public class EditorContext
    {

        /// <summary>
        ///  The list of selected element identifiers.
        /// </summary>
        public string[] SelectedElementIds { get; init; }

        /// <summary>
        ///  The last recorded mouse position.
        /// </summary>
        public Point? LastMousePosition { get; init; }

        /// <summary>
        ///  Custom arguments.
        /// </summary>
        public IDictionary<string, object> Args { get; init; }
    }
}
