using NMF.Expressions;
using NMF.Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models
{
    public static class ModelExtensions
    {
        public static IEnumerableExpression<IModelElement> Descendants(this IModelElement element)
        {
            return new DescendantsCollection(element);
        }
    }
}
