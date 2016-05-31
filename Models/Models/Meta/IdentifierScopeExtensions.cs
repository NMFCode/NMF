using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Meta
{
    public static class IdentifierScopeExtensions
    {
        public static IdentifierScope GetActual(this IdentifierScope current, IdentifierScope inherited)
        {
            if (current == IdentifierScope.Inherit)
            {
                return inherited;
            }
            else
            {
                return current;
            }
        }
    }
}
