using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Language;
using NMF.Glsp.Notation;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Selection;
using NMF.Glsp.Protocol.Types;
using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Glsp.Processing
{
    internal class GElementSkeleton<T> : GElementSkeletonBase
    {
        private readonly ElementDescriptor<T> _elementDescriptor;

        public GElementSkeleton(ElementDescriptor<T> elementDescriptor)
        {
            _elementDescriptor = elementDescriptor;
            Type = TypeName;
        }

        public virtual bool IsLabel => false;

        public string Type { get; set; }

        public override string ElementTypeId => _elementDescriptor.ElementTypeId;

        public Dimension Dimension { get; set; } = new Dimension(60, 30);

        public List<(string key, object value)> StaticForwards { get; } = new List<(string key, object staticKey)>();

        public List<(string key, ObservingFunc<T, object> dynamicValue)> DynamicForwards { get; } = new List<(string key, ObservingFunc<T, object> dynamicValue)>();

        public List<string> StaticCssClasses { get; } = new List<string>();

        public List<ObservingFunc<T, string>> DynamicCssClasses { get; } = new List<ObservingFunc<T, string>>();

        public List<NodeContributionBase<T>> NodeContributions { get; } = new List<NodeContributionBase<T>>();

        public IEnumerable<CompartmentContribution<T>> Compartments => NodeContributions.OfType<CompartmentContribution<T>>();

        public List<EdgeContributionBase<T>> EdgeContributions { get; } = new List<EdgeContributionBase<T>>();

        public override bool CanCreateInstance => _elementDescriptor.CanCreateElement;

        public override string TypeName => ModelHelper.ImplementationType<T>()?.Name;

        protected virtual GElement CreateElement(T input, ISkeletonTrace trace, ref INotationElement notation) => new GElement()
        {
            Type = DefaultTypes.Node
        };

        public GElement Create(T input, ISkeletonTrace trace, INotationElement parentNotation)
        {
            var notationElement = FindNotationElementFor(input, parentNotation);
            var needToRegister = notationElement == null;
            var element = CreateElement(input, trace, ref notationElement);
            if (needToRegister && notationElement != null)
            {
                RegisterNewNotation(parentNotation, notationElement);
            }
            element.Skeleton = this;
            element.CreatedFrom = input;
            element.NotationElement = notationElement;
            Apply(input, trace, element);
            return element;
        }

        private static void RegisterNewNotation(INotationElement parentNotation, INotationElement notationElement)
        {
            FindDiagram(parentNotation)?.Elements.Add(notationElement);
        }

        private static INotationElement FindNotationElementFor(object item, INotationElement parent)
        {
            if (parent != null && item is IModelElement semanticElement)
            {
                IDiagram diagram = FindDiagram(parent);
                if (diagram != null)
                {
                    return diagram.Elements.FirstOrDefault(el => el.SemanticElement == semanticElement);
                }
            }
            return null;
        }

        private static IDiagram FindDiagram(INotationElement parent)
        {
            if (parent is not IDiagram diagram)
            {
                diagram = parent?.Parent as IDiagram;
            }

            return diagram;
        }

        public GGraph CreateGraph(T input, ISkeletonTrace trace, IDiagram diagram)
        {
            var element = new GGraph(diagram?.Id);
            element.Skeleton = this;
            element.CreatedFrom = input;
            element.NotationElement = diagram;
            Apply(input, trace, element);
            element.Type = DefaultTypes.Graph;
            element.Size = new Dimension(0, 0);
            element.Position = new Point(0, 0);
            return element;
        }

        private void Apply(T input, ISkeletonTrace trace, GElement element)
        {
            trace.Trace(input, element);
            element.Size ??= Dimension;
            element.Type = Type;
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
            if (item == null || item.CreatedFrom is T)
            {
                var nodeActions = NodeContributions.SelectMany(c => c.SuggestActions(item, selected, contextId, editorContext));
                var edgeActions = EdgeContributions.Select(c => c.CreateAction(item, selected, contextId, editorContext));
                var allActions = nodeActions.Concat(edgeActions).ToArray();
                if (allActions.Length > 0)
                {
                    yield return new LabeledAction
                    {
                        Label = TypeName,
                        SortString = TypeName,
                        Actions = Array.Empty<BaseAction>(),
                        Children = allActions
                    };
                }
            }
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

        public override GElement CreateNode(GElement container, CreateNodeOperation createNodeOperation)
        {
            var contributionId = (string)createNodeOperation.Args["contributionId"];
            var contributor = NodeContributions.FirstOrDefault(c => c.ContributionId == contributionId);
            if (contributor != null)
            {
                return contributor.CreateNode(container, createNodeOperation);
            }
            return null;
        }

        public override GElement CreateEdge(GElement sourceElement, CreateEdgeOperation createEdgeOperation, GElement targetElement, ISkeletonTrace trace)
        {
            var contributionId = (string)createEdgeOperation.Args["contributionId"];
            var currentElement = sourceElement; 
            var contributor = EdgeContributions.FirstOrDefault(c => c.ContributionId == contributionId);
            if (contributor != null)
            {
                return contributor.CreateEdge(sourceElement, targetElement, sourceElement.NotationElement, createEdgeOperation, trace);
            }
            if (sourceElement.Parent != null)
            {
                return sourceElement.Parent.Skeleton.CreateEdge(sourceElement, createEdgeOperation, targetElement, trace);
            }
            return null;
        }

        public override object CreateInstance() => _elementDescriptor.CreateElement();

        public IEnumerable<string> ContainableTypeIds()
        {
            return NodeContributions.SelectMany(sk => sk.ContainableElementIds());
        }

        public override GGraph CreatePopup(RequestPopupModelAction popupRequest, GElement element)
        {
            if (element.CreatedFrom is T semanticElement)
            {
                var renderedPopup = _elementDescriptor.RenderPopup(semanticElement, popupRequest.Bounds);
                if (renderedPopup != null)
                {
                    return new GGraph("popup")
                    {
                        Type = "html",
                        Details =
                        {
                            ["canvasBounds"] = popupRequest.Bounds
                        },
                        Children =
                        {
                            new GElement("popup-title")
                            {
                                Type = DefaultTypes.PreRendered,
                                Details =
                                {
                                   ["code"] = $"<div class=\"sprotty-popup-title\">{_elementDescriptor.GetElementName(semanticElement)}</div>"
                                }
                            },
                            new GElement("popup-body")
                            {
                                Type = DefaultTypes.PreRendered,
                                Details =
                                {
                                    ["code"] = $"<div class=\"sprotty-popup-body\">{renderedPopup}</div>"
                                }
                            }
                        }
                    };
                }
            }
            return null;
        }
    }
}
