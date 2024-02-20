using NMF.Expressions;
using NMF.Glsp.Graph;
using NMF.Glsp.Language;
using NMF.Glsp.Notation;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Selection;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Server.Contracts;
using NMF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace NMF.Glsp.Processing
{
    internal class GElementSkeleton<T> : GElementSkeletonBase
    {
        protected readonly ElementDescriptor<T> _elementDescriptor;

        public GElementSkeleton(ElementDescriptor<T> elementDescriptor)
        {
            _elementDescriptor = elementDescriptor;
            Type = TypeName;
        }

        public virtual bool IsLabel => false;

        public string Type { get; set; }

        public override string ElementTypeId => _elementDescriptor.ElementTypeId;

        public Dimension? Dimension { get; set; }

        public List<(string key, object value)> StaticForwards { get; } = new List<(string key, object staticKey)>();

        public List<(string key, ObservingFunc<T, object> dynamicValue)> DynamicForwards { get; } = new List<(string key, ObservingFunc<T, object> dynamicValue)>();

        public List<string> StaticCssClasses { get; } = new List<string>();

        public List<ObservingFunc<T, string>> DynamicCssClasses { get; } = new List<ObservingFunc<T, string>>();

        public List<NodeContributionBase<T>> NodeContributions { get; } = new List<NodeContributionBase<T>>();

        public Dictionary<string, GElementOperation> Operations { get; } = new Dictionary<string, GElementOperation>();

        public IEnumerable<CompartmentContribution<T>> Compartments => NodeContributions.OfType<CompartmentContribution<T>>();

        public List<EdgeContributionBase<T>> EdgeContributions { get; } = new List<EdgeContributionBase<T>>();

        public override bool CanCreateInstance => _elementDescriptor.CanCreateElement;

        public override string TypeName => ModelHelper.ImplementationType<T>()?.Name;

        public override IEnumerable<string> Profiles => _elementDescriptor.Profiles.Keys;

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
            LayoutStrategy.Apply(element);
            foreach (var forward in StaticForwards)
            {
                element.Details[forward.key] = forward.value;
            }
            foreach (var dynamicForward in DynamicForwards)
            {
                var dynamicValue = dynamicForward.dynamicValue.Observe(input);
                dynamicValue.Successors.SetDummy();
                element.Collectibles.Add(dynamicForward.dynamicValue, dynamicValue);
                element.Details[dynamicForward.key] = dynamicValue.Value;
                dynamicValue.ValueChanged += (_, e) =>
                {
                    element.Details[dynamicForward.key] = e.NewValue; 
                };
            }
            foreach (var staticCss in StaticCssClasses)
            {
                element.CssClasses.Add(staticCss);
            }
            foreach (var dynamicCss in DynamicCssClasses)
            {
                var dynamicClass = dynamicCss.Observe(input);
                dynamicClass.Successors.SetDummy();
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
                var nodeActions = NodeContributions
                    .SelectMany(c => c.SuggestActions(item, selected, contextId, editorContext));
                var edgeActions = EdgeContributions
                    .Where(n => n.ShowInContext(contextId))
                    .SelectMany(c => c.CreateActions(item, selected, contextId, editorContext));
                var operations = item == null ? Enumerable.Empty<LabeledAction>()
                    : Operations.Values
                        .Where(n => n.ShowInContext(contextId))
                        .SelectMany(op => op.CreateActions(item));

                var allActions = nodeActions.Concat(edgeActions).Concat(operations).ToArray();
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
                element.Skeleton = this;
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
            var contributionPair = FindContribution(container, contributionId, createNodeOperation.ElementTypeId);
            if (contributionPair.Contributor != null)
            {
                return contributionPair.Contributor.CreateNode(contributionPair.Element, createNodeOperation);
            }
            return null;
        }

        private (GElement Element, NodeContributionBase<T> Contributor) FindContribution(GElement container, string contributionId, string elementTypeId)
        {
            var toSearch = new Queue<(GElement Element, NodeContributionBase<T> Contributor)>();
            var alternative = default((GElement, NodeContributionBase<T>));
            foreach (var contrib in NodeContributions)
            {
                toSearch.Enqueue((container, contrib));
            }
            while (toSearch.Count > 0)
            {
                var nodeContribution = toSearch.Dequeue();
                if (nodeContribution.Contributor.ContributionId == contributionId)
                {
                    return nodeContribution;
                }
                else if (nodeContribution.Contributor.ContainableElementIds().Contains(contributionId))
                {
                    alternative = nodeContribution;
                }
                if (nodeContribution.Contributor is CompartmentContribution<T> compartmentContribution)
                {
                    var compartment = nodeContribution.Element.Children.FirstOrDefault(c => c.Skeleton == compartmentContribution.CompartmentSkeleton);
                    if (compartment != null)
                    {
                        foreach (var child in compartmentContribution.CompartmentSkeleton.NodeContributions)
                        {
                            toSearch.Enqueue((compartment, child));
                        }
                    }
                }
            }
            return alternative;
        }

        public override void CreateEdge(GElement sourceElement, CreateEdgeOperation createEdgeOperation, GElement targetElement, ISkeletonTrace trace)
        {
            var contributionId = (string)createEdgeOperation.Args["contributionId"];
            var currentElement = sourceElement; 
            var contributor = EdgeContributions.FirstOrDefault(c => c.ContributionId == contributionId);
            if (contributor != null)
            {
                contributor.CreateEdge(sourceElement, targetElement, sourceElement.NotationElement, createEdgeOperation, trace);
                return;
            }
            if (sourceElement.Parent != null)
            {
                sourceElement.Parent.Skeleton.CreateEdge(sourceElement, createEdgeOperation, targetElement, trace);
                return;
            };
        }

        public override object CreateInstance(string profile, object parent) => _elementDescriptor.CreateElement(profile, parent);

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
                        Position = new Point(popupRequest.Bounds.X, popupRequest.Bounds.Y),
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
                                   ["code"] = $"<div class=\"sprotty-popup-title\"><span class=\"fa fa-info-circle\"/> { HtmlEncoder.Default.Encode( _elementDescriptor.GetElementName(semanticElement)) }</div>"
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "VSTHRD114:Avoid returning a null Task", Justification = "internal operation")]
        public override Task Perform(string kind, GElement gElement, IGlspSession session, IDictionary<string, object> args)
        {
            if (Operations.TryGetValue(kind, out var operation))
            {
                return operation.Perform(gElement, args, session);
            }
            return null;
        }

        public override string GetToolLabel(string profile)
        {
            return _elementDescriptor.ToolLabel(profile);
        }
    }
}
