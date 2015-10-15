using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NMF.Transformations;
using NMF.Transformations.Core;

namespace NMF.Models.Meta
{
    internal static class Extensions
    {
        private static object RootClassesKey = new object();
        private static object ModelsKey = new object();

        public static ICollection<Model> GetModels(this ITransformationContext context, bool createIfNecessary)
        {
            Func<List<Model>> creator = null;
            if (createIfNecessary) creator = () => new List<Model>();
            return context.GetOrCreateUserItem<List<Model>>(ModelsKey, creator);
        }

        public static ICollection<IClass> GetRootClasses(this ITransformationContext context, bool createIfNeccessary)
        {
            Func<List<IClass>> creator = null;
            if (createIfNeccessary) creator = () => new List<IClass>();
            return context.GetOrCreateUserItem<List<IClass>>(RootClassesKey, creator);
        }
    }
}