using NMF.Glsp.Server.Protocol.BaseProtocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Types
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
        public List<BaseAction> Actions { get; } = new List<BaseAction>();

        /// <summary>
         ///  Optional group icon.
         /// </summary>
        public string Icon { get; set; }
    }
}
