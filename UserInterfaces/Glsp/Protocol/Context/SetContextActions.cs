using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;
using System.Collections.Generic;

namespace NMF.Glsp.Protocol.Context
{
    /// <summary>
    /// The SetContextActions is the response to a RequestContextActions containing all actions for the queried context.
    /// </summary>
    public class SetContextActions : ResponseAction
    {
        /// <summary>
        /// The kind value used for this kind of action
        /// </summary>
        public const string SetContextActionsKind = "setContextActions";

        /// <inheritdoc/>
        public override string Kind => SetContextActionsKind;

        /// <summary>
        ///  The actions available in the queried context.
        /// </summary>
        public LabeledAction[] Actions { get; init; }

        /// <summary>
        ///  Custom arguments.
        /// </summary>
        public IDictionary<string, string> Args { get; } = new Dictionary<string, string>();
    }
}
