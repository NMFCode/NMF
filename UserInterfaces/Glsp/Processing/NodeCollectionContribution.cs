using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Context;
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
        private GElement _lastElementCreated;

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
            element.Size = CalculateUpdatedBounds(element);
            childElement.Deleted += () => collection.Remove(item);
            _lastElementCreated = childElement;
        }

        private Dimension? CalculateUpdatedBounds(GElement element)
        {
            if (element.Size is Dimension currentDimension)
            {
                var width = currentDimension.Width;
                var height = currentDimension.Height;
                foreach (var item in element.Children)
                {
                    if (item.Position is Point itemPosition && item.Size is Dimension itemSize)
                    {
                        width = Math.Max(width, itemPosition.X +  itemSize.Width);
                        height = Math.Max(height, itemPosition.Y + itemSize.Height);
                    }
                }
                return new Dimension(width, height);
            }
            if (element.Parent != null)
            {
                element.Parent.Size = CalculateUpdatedBounds(element.Parent);
            }
            return element.Size;
        }

        public override Type SourceType => typeof(T);

        public override Type TargetType => typeof(TOther);


        public override GElement CreateNode(GElement container, CreateNodeOperation operation)
        {
            var skeleton = Skeleton.Closure<GElementSkeletonBase>(sk => sk.Refinements)
                                   .FirstOrDefault(sk => sk.TypeName == operation.ElementTypeId);
            _lastElementCreated = null;
            if (container.Collectibles.TryGetValue(this, out var disposable) && disposable is INotifyCollection<TOther> collection)
            {
                object profile = null;
                operation.Args?.TryGetValue("profile", out profile);
                collection.Add((TOther)skeleton.CreateInstance(profile?.ToString()));
            }
            if (_lastElementCreated != null)
            {
                _lastElementCreated.Position = operation.Location ?? _lastElementCreated.Position;
            }
            return _lastElementCreated;
        }

        public override IEnumerable<LabeledAction> SuggestActions(GElement item, List<GElement> selected, string contextId, EditorContext editorContext)
        {
            if (item == null || item.Collectibles.TryGetValue(this, out var disposable) && disposable is INotifyCollection<TOther>)
            {
                foreach (var skeleton in Skeleton.Closure<GElementSkeletonBase>(sk => sk.Refinements))
                {
                    if (skeleton.CanCreateInstance)
                    {
                        yield return new LabeledAction
                        {
                            Label = $"Create new {skeleton.TypeName}",
                            SortString = skeleton.TypeName,
                            Actions = new[] {
                                new TriggerNodeCreationAction
                                {
                                    ElementTypeId = skeleton.TypeName,
                                    Args = new Dictionary<string, object>
                                    {
                                        ["contributionId"] = ContributionId
                                    }
                                }
                            }
                        };
                    }

                    foreach (var profile in skeleton.Profiles)
                    {
                        yield return new LabeledAction
                        {
                            Label = $"Create new {profile}",
                            SortString = skeleton.TypeName + profile,
                            Actions = new[] {
                                new TriggerNodeCreationAction
                                {
                                    ElementTypeId = skeleton.TypeName,
                                    Args = new Dictionary<string, object>
                                    {
                                        ["contributionId"] = ContributionId,
                                        ["profile"] = profile
                                    }
                                }
                            }
                        };
                    }
                }
            }
        }

        public override IEnumerable<string> ContainableElementIds()
        {
            return Skeleton.Closure<GElementSkeletonBase>(sk => sk.Refinements)
                .Select(sk => sk.ElementTypeId);
        }
    }
}
