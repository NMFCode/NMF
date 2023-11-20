using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Types;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Processing
{
    internal class NodeCollectionContribution<T, TOther> : NodeContributionBase<T>
    {

        public GElementSkeleton<TOther> Skeleton { get; init; }

        public Func<T, ICollectionExpression<TOther>> Selector { get; init; }

        public override void Contribute(T input, GElement element, ISkeletonTrace trace)
        {
            var collection = Selector(input).AsNotifiable();
            element.Collectibles.Add(this, collection);
            foreach (var item in collection)
            {
                AddElement(element, trace, collection, item);
            }
            collection.CollectionChanged += (o, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    throw new NotImplementedException();
                }
                if (e.OldItems != null)
                {
                    foreach (var item in e.OldItems)
                    {
                        var childElement = trace.RemoveElement(item, Skeleton);
                        element.Children.Remove(childElement);
                        childElement.SilentDelete();
                    }
                }
                if (e.NewItems != null)
                {
                    foreach (TOther item in e.NewItems)
                    {
                        AddElement(element, trace, collection, item);
                    }
                }
            };
        }

        private void AddElement(GElement element, ISkeletonTrace trace, INotifyCollection<TOther> collection, TOther item)
        {
            var childElement = Skeleton.Create(item, trace, element.NotationElement);
            element.Children.Add(childElement);
            childElement.Parent = element;
            childElement.Deleted += () => collection.Remove(item);
        }

        public override Type SourceType => typeof(T);

        public override Type TargetType => typeof(TOther);


        public override void CreateNode(GElement container, CreateNodeOperation operation)
        {
            var skeleton = Skeleton.Closure<GElementSkeletonBase>(sk => sk.Refinements)
                                   .FirstOrDefault(sk => sk.TypeName == operation.ElementTypeId);

            if (container.Collectibles.TryGetValue(this, out var disposable) && disposable is INotifyCollection<TOther> collection)
            {
                collection.Add((TOther)skeleton.CreateInstance());
            }
        }

        public override IEnumerable<BaseAction> SuggestActions(GElement item, T element, List<GElement> selected, string contextId, EditorContext editorContext)
        {
            if (item.Collectibles.TryGetValue(this, out var disposable) && disposable is INotifyCollection<TOther>)
            {
                foreach (var skeleton in Skeleton.Closure<GElementSkeletonBase>(sk => sk.Refinements)
                                                 .Where(sk => sk.CanCreateInstance))
                {
                    yield return new CreateNodeOperation
                    {
                        ContainerId = item.Id,
                        ElementTypeId = skeleton.TypeName,
                        Args =
                        {
                            ["contributionId"] = ContributionId
                        }
                    };
                }
            }
        }
    }
}
