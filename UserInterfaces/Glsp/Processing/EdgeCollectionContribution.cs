using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Notation;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Context;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Types;
using NMF.Models;
using NMF.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Processing
{
    internal class EdgeCollectionContribution<T, TOther> : EdgeContributionBase<T>
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

        public override IEnumerable<BaseAction> SuggestActions(GElement item, T element, List<GElement> selected, string contextId, EditorContext editorContext)
        {
            if (item.Collectibles.TryGetValue(this, out var disposable) && disposable is INotifyCollection<TOther>)
            {
                foreach (var skeleton in Skeleton.Closure<GElementSkeletonBase>(sk => sk.Refinements)
                                                 .Where(sk => sk.CanCreateInstance))
                {
                    yield return new TriggerEdgeCreationAction
                    {
                        ElementTypeId = skeleton.TypeName,
                        Args =
                        {
                            ["contributionId"] = ContributionId,
                            ["parentId"] = item.Id
                        }
                    };
                }
            }
        }

        public override void CreateEdge(GElement sourceElement, GElement targetElement, CreateEdgeOperation createEdgeOperation, ISkeletonTrace trace)
        {
            var parent = sourceElement.Graph.Resolve(createEdgeOperation.Args["parentId"]);
            if (parent.Collectibles.TryGetValue(this, out var disposable) && disposable is INotifyCollection<TOther> collection)
            {
                var (edge, transition) = CreateTransition(sourceElement, targetElement, trace);
                collection.Add(transition);
            }
        }

        protected virtual (GElement,TOther) CreateTransition(GElement source, GElement target, ISkeletonTrace trace)
        {
            var skeleton = Skeleton as GEdgeSkeleton<TOther>;
            if (skeleton == null || !skeleton.CanChangeSource || !skeleton.CanChangeTarget)
            {
                throw new InvalidOperationException("Cannot create edge");
            }
            var transition = InstantiationHelper.CreateInstance<TOther>();
            var edge = (GEdge)skeleton.Create(transition, trace, source.NotationElement);
            edge.SourceId = source.Id;
            edge.TargetId = target.Id;
            return (edge, transition);
        }

        public override IEnumerable<EdgeTypeHint> CreateEdgeTypesHint()
        {
            return Skeleton.Closure<GElementSkeletonBase>(sk => sk.Refinements)
                           
                           .Where(sk => sk.CanCreateInstance)
                           .Select(skeleton => new EdgeTypeHint
                           {
                               Deletable = true,
                               Dynamic = false,
                               ElementTypeId = skeleton.TypeName,
                               Repositionable = true,
                               Routable = true,
                               SourceElementTypeIds = skeleton.CalculateSourceTypeIds(),
                               TargetElementTypeIds = skeleton.CalculateTargetTypeIds()
                           });
        }
    }

    internal class EdgeCollectionContribution<T, TSource, TTarget> : EdgeCollectionContribution<T, (TSource, TTarget)>
    {
        public override Type SourceType => typeof(TSource);

        public override Type TargetType => typeof(TTarget);

        protected override (GElement, (TSource, TTarget)) CreateTransition(GElement source, GElement target, ISkeletonTrace trace)
        {
            var tuple = ((TSource)source.CreatedFrom, (TTarget)target.CreatedFrom);
            var edge = Skeleton.Create(tuple, trace, source.NotationElement);
            return (edge, tuple);
        }
    }
}
