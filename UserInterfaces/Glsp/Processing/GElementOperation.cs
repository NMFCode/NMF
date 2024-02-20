using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Processing
{
    internal abstract class GElementOperation : ActionElement
    {
        public GElementOperation(string kind)
        {
            Kind = kind;
        }

        public string Kind { get; }

        public abstract Task Perform(GElement element, IDictionary<string, object> args, IGlspSession session);

        public abstract IEnumerable<LabeledAction> CreateActions(GElement element);
    }
}
