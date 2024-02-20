using NMF.Glsp.Processing;
using NMF.Glsp.Protocol.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    internal class ChildSyntax : IChildSyntax
    {
        private readonly ActionElement _actionElement;

        public ChildSyntax(ActionElement actionElement)
        {
            _actionElement = actionElement;
        }

        public IChildSyntax HideIn(Func<string, bool> contextIdPredicate)
        {
            _actionElement.AddFilter(contextIdPredicate);
            return this;
        }
    }
}
