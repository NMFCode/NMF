using NMF.Expressions;
using NMF.Glsp.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language
{
    internal class ChildCollectionContribution<T, TOther> : ChildContributionBase<T>
    {
        public GElementSkeleton<TOther> Skeleton { get; init; }

        public Func<T, ICollectionExpression<TOther>> Selector { get; init; }

        public override void Contribute(T input, GElement element)
        {
            var collection = Selector(input).AsNotifiable();
            element.Collectibles.Add(collection);
            foreach (var item in collection)
            {
                var childElement = Skeleton.Create(item);
                element.Children.Add(childElement);
                childElement.Deleted += () => collection.Remove(item);
            }
        }
    }
}
