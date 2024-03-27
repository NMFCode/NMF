using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMF.Glsp.Processing
{
    internal class GElementOperation<T> : GElementOperation
    {
        private readonly Func<T, IGlspSession, Task> _execute;

        public GElementOperation(string kind, string description, Func<T, IGlspSession, Task> execute) : base(kind)
        {
            _execute = execute;
            Description = description;
        }

        public string Description { get; set; }

        public override IEnumerable<LabeledAction> CreateActions(GElement element)
        {
            yield return new LabeledAction
            {
                SortString = Kind,
                Label = Description,
                Actions = new[]
                {
                    new CustomOperation(Kind)
                    {
                        ElementId = element.Id
                    }
                }
            };
        }

        public override Task Perform(GElement element, IDictionary<string, object> args, IGlspSession session)
        {
            if (element.CreatedFrom is T semanticElement)
            {
                return _execute.Invoke(semanticElement, session) ?? Task.CompletedTask;
            }
            return Task.CompletedTask;
        }
    }
}
