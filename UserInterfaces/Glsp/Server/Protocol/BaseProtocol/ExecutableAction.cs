using NMF.Glsp.Graph;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Server.Protocol.BaseProtocol
{
    public abstract class ExecutableAction : BaseAction
    {
        /// <summary>
        /// Executes the action in the context of the given session
        /// </summary>
        /// <param name="session">The session in which to execute the action</param>
        public abstract void Execute(IClientSession session);

        /// <summary>
        /// Denotes whether the execution of the action requires a transaction
        /// </summary>
        public virtual bool RequireTransaction() => false;
    }
}
