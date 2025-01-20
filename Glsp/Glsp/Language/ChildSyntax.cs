using NMF.Glsp.Processing;
using System;

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
