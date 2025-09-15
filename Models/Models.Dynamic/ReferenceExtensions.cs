using NMF.Models.Meta;
using System;
using System.Collections.Generic;
using System.Text;

namespace NMF.Models.Dynamic
{
    internal static class ReferenceExtensions
    {
        public static bool CanAdd(this IReference reference, IModelElement element)
        {
            if (element == null) return false;
            if (reference.ReferenceType is IClass referencedClass)
            {
                return referencedClass.IsAssignableFrom(element.GetClass());
            }
            return false;
        }
    }
}
