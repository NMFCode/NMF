using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Notation;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Types;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Glsp.Processing
{
    internal class GElementSkeleton<T> : GElementSkeletonBase
    {
        public string StaticType { get; set; }

        public ObservingFunc<T, string> DynamicType { get; set; }

        public List<(string key, string value)> StaticForwards { get; } = new List<(string key, string staticKey)>();

        public List<(string key, ObservingFunc<T, string> dynamicValue)> DynamicForwards { get; } = new List<(string key, ObservingFunc<T, string> dynamicValue)>();

        public List<string> StaticCssClasses { get; } = new List<string>();

        public List<ObservingFunc<T, string>> DynamicCssClasses { get; } = new List<ObservingFunc<T, string>>();

        public List<NodeContributionBase<T>> NodeContributions { get; } = new List<NodeContributionBase<T>>();

        public List<EdgeContributionBase<T>> EdgeContributions { get; } = new List<EdgeContributionBase<T>>();

        public override bool CanCreateInstance => InstantiationHelper.CanCreateInstance<T>();

        public override string TypeName => typeof(T).Name;

        protected virtual GElement CreateElement(T input, ISkeletonTrace trace, ref INotationElement notation) => new GElement();

        public GElement Create(T input, ISkeletonTrace trace, ref INotationElement notation)
        {
            var element = CreateElement(input, trace, ref notation);
            element.Skeleton = this;
            element.CreatedFrom = input;
            element.NotationElement = notation;
            Apply(input, trace, element);
            return element;
        }

        private void Apply(T input, ISkeletonTrace trace, GElement element)
        {
            trace.Trace(input, element);
            if (StaticType != null)
            {
                element.Type = StaticType;
            }
            if (DynamicType != null)
            {
                var dynamicType = DynamicType.Observe(input);
                element.Collectibles.Add(DynamicType, dynamicType);
                element.Type = dynamicType.Value;
                dynamicType.ValueChanged += element.UpdateType;
            }
            foreach (var forward in StaticForwards)
            {
                element.Details.Add(forward.key, forward.value);
            }
            foreach (var dynamicForward in DynamicForwards)
            {
                var dynamicValue = dynamicForward.dynamicValue.Observe(input);
                element.Collectibles.Add(dynamicForward.dynamicValue, dynamicValue);
                element.Details.Add(dynamicForward.key, dynamicValue.Value);
                dynamicValue.ValueChanged += (_, e) => element.Details[dynamicForward.key] = e.NewValue as string;
            }
            foreach (var staticCss in StaticCssClasses)
            {
                element.CssClasses.Add(staticCss);
            }
            foreach (var dynamicCss in DynamicCssClasses)
            {
                var dynamicClass = dynamicCss.Observe(input);
                if (dynamicClass != null)
                {
                    element.CssClasses.Add(dynamicClass.Value);
                }
                element.Collectibles.Add(dynamicCss, dynamicClass);
                dynamicClass.ValueChanged += element.UpdateClass;
            }
            foreach (var childContribution in NodeContributions)
            {
                childContribution.Contribute(input, element, trace);
            }
            foreach (var edgeContribution in EdgeContributions)
            {
                edgeContribution.Contribute(input, element, trace);
            }
            foreach (var refinement in Refinements)
            {
                refinement.TryApply(input, trace, element);
            }
        }

        public override IEnumerable<LabeledAction> SuggestActions(GElement item, List<GElement> selected, string contextId, EditorContext editorContext)
        {
            if (item.CreatedFrom is T element)
            {
                return (from cc in NodeContributions
                        from action in cc.SuggestActions(item, element, selected, contextId, editorContext)
                        group action by action.Kind into actions
                        select new LabeledAction
                        {
                           Label = actions.Key,
                           Actions = actions.ToArray(),
                        }).Concat(
                        from cc in EdgeContributions
                        from action in cc.SuggestActions(item, element, selected, contextId, editorContext)
                        group action by action.Kind into actions
                        select new LabeledAction
                        {
                            Label = actions.Key,
                            Actions = actions.ToArray(),
                        });
            }
            return Enumerable.Empty<LabeledAction>();
        }

        public override bool TryApply(object input, ISkeletonTrace trace, GElement element)
        {
            if (input is T castedInput && trace.ResolveElement(input, this) == null)
            {
                Apply(castedInput, trace, element);
                return true;
            }
            return false;
        }

        public override bool IsApplicable(object input)
        {
            return input is T;
        }

        public override void CreateNode(GElement container, CreateNodeOperation createNodeOperation)
        {
            var contributionId = createNodeOperation.Args["contributionId"];
            var contributor = NodeContributions.FirstOrDefault(c => c.ContributionId == contributionId);
            if (contributor != null)
            {
                contributor.CreateNode(container, createNodeOperation);
            }
        }

        public override void CreateEdge(GElement sourceElement, CreateEdgeOperation createEdgeOperation, GElement targetElement, ISkeletonTrace trace)
        {
            var contributionId = createEdgeOperation.Args["contributionId"];
            var contributor = EdgeContributions.FirstOrDefault(c => c.ContributionId == contributionId);
            if (contributor != null)
            {
                contributor.CreateEdge(sourceElement, targetElement, createEdgeOperation, trace);
            }
        }

        public override object CreateInstance() => InstantiationHelper.CreateInstance<T>();
    }
}
