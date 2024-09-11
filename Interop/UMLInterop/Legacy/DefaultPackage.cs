using NMF.Interop.Legacy.Cmof;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Interop.UML.Legacy
{
    public static class DefaultPackage
    {
        public static IPackage CreateDefaultPackage()
        {
            return new Package
            {
                Name = "Default",
                OwnedType =
                {
                    new PrimitiveType { Name = "String" },
                    new PrimitiveType { Name = "Integer" },
                    new PrimitiveType { Name = "Boolean" }
                }
            };
        }
    }
}
