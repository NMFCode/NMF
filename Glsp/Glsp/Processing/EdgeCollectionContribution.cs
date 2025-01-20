using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Language;
using NMF.Glsp.Notation;
using NMF.Glsp.Protocol.Context;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Types;
using NMF.Utilities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace NMF.Glsp.Processing
{
    internal class EdgeCollectionContribution<T, TOther> : EdgeContributionBase<T>
    {
        protected readonly ConcurrentDictionary<TOther, GEdge> _recentlyCreated = new ConcurrentDictionary<TOther, GEdge>();

        public GElementSkeleton<TOther> Skeleton { get; init; }

        public EdgeDescriptor<TOther> EdgeDescriptor { get; init; }

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
                        if (_recentlyCreated.TryRemove(item, out var child))
                        {
                            Hook(element, collection, item, child);
                        }
                        else
                        {
                            AddElement(element, trace, collection, item);
                        }
                    }
                }
            };
        }

        private void AddElement(GElement element, ISkeletonTrace trace, INotifyCollection<TOther> collection, TOther item)
        {
            var childElement = Skeleton.Create(item, trace, element.NotationElement);
            Hook(element, collection, item, childElement);
        }

        private static void Hook(GElement element, INotifyCollection<TOther> collection, TOther item, GElement childElement)
        {
            element.Children.Add(childElement);
            childElement.Parent = element;
            childElement.Deleted += () => collection.Remove(item);
        }

        public override Type SourceType => typeof(T);

        public override Type TargetType => typeof(TOther);

        public override IEnumerable<LabeledAction> CreateActions(GElement item, List<GElement> selected, string contextId, EditorContext editorContext)
        {
            var args = new Dictionary<string, object>
            {
                ["contributionId"] = ContributionId,
            };
            if (item != null )
            {
                args["parentId"] = item.Id;
            }
            yield return new LabeledAction
            {
                Label = EdgeDescriptor.ToolLabel(null),
                SortString = EdgeDescriptor.ElementTypeId,
                Actions = new[] 
                {
                    new TriggerEdgeCreationAction
                    {
                        ElementTypeId = EdgeDescriptor.ElementTypeId,
                        Args = args
                    }
                }
            };
            foreach (var profile in Skeleton.Profiles)
            {
                var profileArgs = new Dictionary<string, object>(args)
                {
                    ["profile"] = profile
                };

                yield return new LabeledAction
                {
                    Label = EdgeDescriptor.ToolLabel(profile),
                    SortString = EdgeDescriptor.ElementTypeId,
                    Actions = new[] 
                    {
                        new TriggerEdgeCreationAction
                        {
                            ElementTypeId = EdgeDescriptor.ElementTypeId,
                            Args = profileArgs
                        }
                    }
                };
            }
        }

        public override void CreateEdge(GElement sourceElement, GElement targetElement, INotationElement parentNotation, CreateEdgeOperation createEdgeOperation, ISkeletonTrace trace)
        {
            var parent = createEdgeOperation.Args.TryGetValue("parentId", out var parentId) && parentId is string parentIdString ? sourceElement.Graph.Resolve(parentIdString) : sourceElement.Graph;
            if (parent.Collectibles.TryGetValue(this, out var disposable) && disposable is INotifyCollection<TOther> collection)
            {
                var transition = CreateTransition(sourceElement, targetElement, parentNotation, createEdgeOperation, trace);
                collection.Add(transition);
            }
        }

        protected virtual TOther CreateTransition(GElement source, GElement target, INotationElement notationElement, CreateEdgeOperation createEdgeOperation, ISkeletonTrace trace)
        {
            var skeleton = Skeleton as GEdgeSkeleton<TOther>;
            if (skeleton == null || !skeleton.Source.CanChange || !skeleton.Target.CanChange)
            {
                throw new InvalidOperationException("Cannot create edge");
            }
            object profile = null;
            createEdgeOperation.Args?.TryGetValue("profile", out profile);
            var transition = (TOther)skeleton.CreateInstance(profile?.ToString(), source.CreatedFrom);
            var edge = (GEdge)skeleton.Create(transition, trace, notationElement);
            _recentlyCreated.TryAdd(transition, edge);
            edge.SourceId = source.Id;
            edge.TargetId = target.Id;
            // edge is not yet registered, cannot resolve ids yet so we need to that manually
            skeleton.Source.SetElement(edge, source);
            skeleton.Target.SetElement(edge, target);
            return transition;
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

        protected override (TSource, TTarget) CreateTransition(GElement source, GElement target, INotationElement notationElement, CreateEdgeOperation createEdgeOperation, ISkeletonTrace trace)
        {
            return ((TSource)source.CreatedFrom, (TTarget)target.CreatedFrom);
        }
    }
}
