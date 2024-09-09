using NMF.Glsp.Contracts;
using System.Threading.Tasks;

namespace NMF.Glsp.Protocol.BaseProtocol
{
    /// <summary>
    /// Denotes an action that can be executed
    /// </summary>
    public abstract class ExecutableAction : BaseAction
    {
        /// <summary>
        /// Executes the action in the context of the given session
        /// </summary>
        /// <param name="session">The session in which to execute the action</param>
        public abstract Task ExecuteAsync(IGlspSession session);

        /// <summary>
        /// Denotes whether the execution of the action requires a transaction
        /// </summary>
        public virtual bool RequireTransaction() => false;
    }
}
