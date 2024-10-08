﻿using NMF.Glsp.Contracts;
using NMF.Glsp.Graph;
using NMF.Glsp.Notation;
using NMF.Glsp.Processing;
using NMF.Glsp.Processing.Layouting;
using NMF.Glsp.Protocol.BaseProtocol;
using NMF.Glsp.Protocol.Clipboard;
using NMF.Glsp.Protocol.Context;
using NMF.Glsp.Protocol.Layout;
using NMF.Glsp.Protocol.ModelData;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Navigation;
using NMF.Glsp.Protocol.Selection;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Protocol.UndoRedo;
using NMF.Glsp.Protocol.Validation;
using NMF.Glsp.Server;
using NMF.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NMF.Glsp.Language
{
    /// <summary>
    /// Denotes a graphical language
    /// </summary>
    public abstract class GraphicalLanguage : IClientSessionProvider
    {
        /// <summary>
        /// Gets the diagram type supported by this graphical language
        /// </summary>
        public virtual string DiagramType => GetType().Name;

        /// <summary>
        /// Gets the default layout engine for this language
        /// </summary>
        public virtual ILayoutEngine DefaultLayoutEngine => LayeredLayoutService.Instance;

        /// <summary>
        /// Gets the rule to start with
        /// </summary>
        public abstract DescriptorBase StartRule { get; }

        /// <inheritdoc />
        public virtual IEnumerable<string> SupportedActions
        {
            get
            {
                Initialize();
                return ActionKindsSupportedByDefault().Concat(CustomOperations);
            }
        }

        /// <summary>
        /// Gets a collection of action kinds supported by default
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<string> ActionKindsSupportedByDefault()
        {
            yield return CompoundOperation.CompoundOperationKind;
            yield return CutOperation.CutOperationKind;
            yield return PasteOperation.PasteOperationKind;
            yield return RequestClipboardDataAction.RequestClipboardDataActionKind;

            yield return RequestContextActions.RequestContextActionsKind;
            yield return ComputedBoundsAction.ComputedBoundsActionKind;
            yield return LayoutOperation.LayoutOperationKind;

            yield return RequestExportSvgAction.RequestExportSvgActionKind;
            yield return RequestModelAction.RequestModelActionKind;
            yield return SaveModelAction.SaveModelActionKind;
            yield return SetDirtyAction.SetDirtyActionKind;
            yield return SetEditModeAction.SetEditModeActionKind;

            yield return ApplyLabelEditOperation.ApplyLabelEditOperationKind;
            yield return ChangeBoundsOperation.ChangeBoundsOperationKind;
            yield return ChangeContainerOperation.ChangeContainerOperationKind;
            yield return ChangeRoutingPointsOperation.ChangeRoutingPointsOperationKind;
            yield return CreateEdgeOperation.CreateEdgeOperationKind;
            yield return CreateNodeOperation.CreateNodeOperationKind;
            yield return DeleteElementOperation.DeleteElementOperationKind;
            yield return ReconnectEdgeOperation.ReconnectEdgeOperationKind;

            yield return RequestNavigationTargetsAction.RequestNavigationTargetsActionKind;
            yield return ResolveNavigationTargetAction.ResolveNavigationTargetActionKind;

            yield return RequestPopupModelAction.RequestPopupModelActionKind;

            yield return RedoAction.RedoActionKind;
            yield return UndoAction.UndoActionKind;

            yield return DeleteMarkersAction.DeleteMarkersActionKind;
            yield return RequestEditValidationAction.RequestEditValidationActionKind;
            yield return RequestMarkersAction.RequestMarkersActionKind;
            yield return RequestCheckEdgeAction.RequestCheckEdgeActionKind;
            yield return RequestTypeHintsAction.RequestTypeHintsActionKind;

            yield return SelectAction.SelectActionKind;
        }

        /// <summary>
        /// Gets a collection of all rules
        /// </summary>
        public IEnumerable<DescriptorBase> AllRules => _rules.Values.Concat(_adHocRules);

        internal ICollection<string> CustomOperations { get; } = new HashSet<string>();

        private readonly Dictionary<Type, DescriptorBase> _rules = new Dictionary<Type, DescriptorBase>();
        private readonly List<DescriptorBase> _adHocRules = new List<DescriptorBase>();
        private bool _isInitialized;

        /// <summary>
        /// Initializes the graphical language
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized) { return; }
            _isInitialized = true;
            foreach (var ruleType in GetType().GetNestedTypes())
            {
                if (!ruleType.IsAbstract && !ruleType.IsInterface && typeof(DescriptorBase).IsAssignableFrom(ruleType))
                {
                    var ruleInstance = Activator.CreateInstance(ruleType) as DescriptorBase;
                    ruleInstance.Language = this;
                    _rules.Add(ruleType, ruleInstance);
                }
            }
            foreach (var rule in _rules.Values)
            {
                rule.DefineLayout();
            }
            StartRule.GetRootSkeleton().Type = DefaultTypes.Graph;
        }

        /// <summary>
        /// Resolves the given descriptor type to a descriptor
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <returns>The descriptor instance, if any</returns>
        public T Descriptor<T>() where T : DescriptorBase
        {
            if (_rules.TryGetValue(typeof(T), out var ruleInstance))
            {
                return ruleInstance as T;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Calculates shape hints for this language
        /// </summary>
        /// <returns>A collection of shape hints</returns>
        public IEnumerable<TypeHint> CalculateTypeHints()
        {
            return _rules.Values.Concat(_adHocRules).SelectMany(r => r.CalculateTypeHints());
        }

        /// <summary>
        /// Adds the given rule to the list of supported descriptors
        /// </summary>
        /// <param name="rule"></param>
        protected internal void AddRule(DescriptorBase rule)
        {
            _adHocRules.Add(rule);
        }

        /// <summary>
        /// Creates a graph element for the given semantic root element
        /// </summary>
        /// <param name="semanticRoot">The semantic root element</param>
        /// <param name="diagram">The notational instance</param>
        /// <param name="trace">The trace object</param>
        /// <returns>A graph notation</returns>
        public GGraph Create(object semanticRoot, IDiagram diagram, ISkeletonTrace trace)
        {
            return StartRule.CreateGraph(semanticRoot, diagram, trace);
        }

        /// <inheritdoc />
        public virtual IGlspClientSession CreateSession(IDictionary<string, object> args, IModelServer modelServer)
        {
            return new ClientSession(this, modelServer);
        }

        /// <summary>
        /// Obtains a collection of root actions
        /// </summary>
        /// <param name="contextId"></param>
        /// <returns></returns>
        public virtual IEnumerable<LabeledAction> RetrieveRootActions(string contextId)
        {
            return StartRule.GetRootSkeleton().SuggestActions(null, null, contextId, null);
        }
    }
}
