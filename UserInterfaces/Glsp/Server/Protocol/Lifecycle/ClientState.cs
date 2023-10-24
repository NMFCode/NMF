using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.Lifecycle
{
    public enum ClientState
    {
        /// <summary>
         ///  The client has been created.
         /// </summary>
        Initial,
        /// <summary>
         ///  `Start` has been called on the client and the start process is still on-going.
         /// </summary>
        Starting,
        /// <summary>
         ///  The client failed to complete the start process.
         /// </summary>
        StartFailed,
        /// <summary>
         ///  The client was successfully started and is now running.
         /// </summary>
        Running,
        /// <summary>
         ///  `Stop` has been called on the client and the stop process is still on-going.
         /// </summary>
        Stopping,
        /// <summary>
         ///  The client stopped and disposed the server connection. Thus, action messages can no longer be sent.
         /// </summary>
        Stopped,
        /// <summary>
         ///  An error was encountered while connecting to the server. No action messages can be sent.
         /// </summary>
        ServerError
    }
}
