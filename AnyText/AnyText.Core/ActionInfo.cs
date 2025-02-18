using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes the abstract base class for a code action info
    /// </summary>
    public abstract class ActionInfo
    {
        /// <summary>
        /// Invokes the action with the given arguments
        /// </summary>
        /// <param name="arguments">A structure containing the rule application, context and position</param>
        public abstract void Invoke(ExecuteCommandArguments arguments);
    }
}
