using NMF.Glsp.Contracts;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Types;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMF.Glsp.Processing
{
    internal abstract class GElementOperation : ActionElement
    {
        protected GElementOperation(string kind)
        {
            Kind = kind;
        }

        public string Kind { get; }

        public abstract Task PerformAsync(GElement element, IDictionary<string, object> args, IGlspSession session);

        public abstract IEnumerable<LabeledAction> CreateActions(GElement element);
    }
}
