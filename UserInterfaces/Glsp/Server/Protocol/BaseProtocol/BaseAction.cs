using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.BaseProtocol
{
    /// <summary>
    /// An action is a declarative description of a behavior that shall be invoked by the receiver 
    /// upon receipt of the action. It is a plain data structure, and as such transferable between 
    /// server and client. Actions contained in action messages are identified by their kind 
    /// attribute. This attribute is required for all actions. Certain actions are meant to be sent 
    /// from the client to the server or vice versa, while other actions can be sent both ways, by 
    /// the client or the server. All actions must extend the default action interface.
    /// </summary>
    public abstract class BaseAction
    {
        /// <summary>
         ///  Unique identifier specifying the kind of action to process.
         /// </summary>
        public abstract string Kind { get; }
    }
}
