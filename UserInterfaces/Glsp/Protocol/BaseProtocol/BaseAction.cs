using NMF.Glsp.Protocol.Clipboard;
using NMF.Glsp.Protocol.Context;
using NMF.Glsp.Protocol.Layout;
using NMF.Glsp.Protocol.ModelData;
using NMF.Glsp.Protocol.Modification;
using NMF.Glsp.Protocol.Navigation;
using NMF.Glsp.Protocol.Notification;
using NMF.Glsp.Protocol.Selection;
using NMF.Glsp.Protocol.Types;
using NMF.Glsp.Protocol.UndoRedo;
using NMF.Glsp.Protocol.Validation;
using System.Text.Json.Serialization;

namespace NMF.Glsp.Protocol.BaseProtocol
{
    /// <summary>
    /// An action is a declarative description of a behavior that shall be invoked by the receiver 
    /// upon receipt of the action. It is a plain data structure, and as such transferable between 
    /// server and client. Actions contained in action messages are identified by their kind 
    /// attribute. This attribute is required for all actions. Certain actions are meant to be sent 
    /// from the client to the server or vice versa, while other actions can be sent both ways, by 
    /// the client or the server. All actions must extend the default action interface.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = nameof(Kind))]
    [JsonDerivedType(typeof(CompoundOperation), CompoundOperation.CompoundOperationKind)]
    [JsonDerivedType(typeof(RejectAction), RejectAction.RejectActionKind)]
    [JsonDerivedType(typeof(CutOperation), CutOperation.CutOperationKind)]
    [JsonDerivedType(typeof(PasteOperation), PasteOperation.PasteOperationKind)]
    [JsonDerivedType(typeof(RequestClipboardDataAction), RequestClipboardDataAction.RequestClipboardDataActionKind)]
    [JsonDerivedType(typeof(SetClipboardDataAction), SetClipboardDataAction.SetClipboardDataActionKind)]
    [JsonDerivedType(typeof(RequestContextActions), RequestContextActions.RequestContextActionsKind)]
    [JsonDerivedType(typeof(SetContextActions), SetContextActions.SetContextActionsKind)]
    [JsonDerivedType(typeof(TriggerEdgeCreationAction), TriggerEdgeCreationAction.TriggerEdgeCreationActionKind)]
    [JsonDerivedType(typeof(TriggerNodeCreationAction), TriggerNodeCreationAction.TriggerNodeCreationActionKind)]
    [JsonDerivedType(typeof(CenterAction), CenterAction.CenterActionKind)]
    [JsonDerivedType(typeof(ComputedBoundsAction), ComputedBoundsAction.ComputedBoundsActionKind)]
    [JsonDerivedType(typeof(FitToScreenAction), FitToScreenAction.FitToScreenActionKind)]
    [JsonDerivedType(typeof(LayoutOperation), LayoutOperation.LayoutOperationKind)]
    [JsonDerivedType(typeof(RequestBoundsAction), RequestBoundsAction.RequestBoundsActionKind)]
    [JsonDerivedType(typeof(ExportSvgAction), ExportSvgAction.ExportSvgActionKind)]
    [JsonDerivedType(typeof(RequestExportSvgAction), RequestExportSvgAction.RequestExportSvgActionKind)]
    [JsonDerivedType(typeof(RequestModelAction), RequestModelAction.RequestModelActionKind)]
    [JsonDerivedType(typeof(SaveModelAction), SaveModelAction.SaveModelActionKind)]
    [JsonDerivedType(typeof(SetDirtyAction), SetDirtyAction.SetDirtyActionKind)]
    [JsonDerivedType(typeof(SetEditModeAction), SetEditModeAction.SetEditModeActionKind)]
    [JsonDerivedType(typeof(SetModelAction), SetModelAction.SetModelActionKind)]
    [JsonDerivedType(typeof(SourceModelChangedAction), SourceModelChangedAction.SourceModelChangedActionKind)]
    [JsonDerivedType(typeof(UpdateModelAction), UpdateModelAction.UpdateModelActionKind)]
    [JsonDerivedType(typeof(ApplyLabelEditOperation), ApplyLabelEditOperation.ApplyLabelEditOperationKind)]
    [JsonDerivedType(typeof(ChangeBoundsOperation), ChangeBoundsOperation.ChangeBoundsOperationKind)]
    [JsonDerivedType(typeof(ChangeContainerOperation), ChangeContainerOperation.ChangeContainerOperationKind)]
    [JsonDerivedType(typeof(ChangeRoutingPointsOperation), ChangeRoutingPointsOperation.ChangeRoutingPointsOperationKind)]
    [JsonDerivedType(typeof(CreateEdgeOperation), CreateEdgeOperation.CreateEdgeOperationKind)]
    [JsonDerivedType(typeof(CreateNodeOperation), CreateNodeOperation.CreateNodeOperationKind)]
    [JsonDerivedType(typeof(DeleteElementOperation), DeleteElementOperation.DeleteElementOperationKind)]
    [JsonDerivedType(typeof(ReconnectEdgeOperation), ReconnectEdgeOperation.ReconnectEdgeOperationKind)]
    [JsonDerivedType(typeof(NavigateToExternalTargetAction), NavigateToExternalTargetAction.NavigateToExternalTargetActionKind)]
    [JsonDerivedType(typeof(NavigateToTargetAction), NavigateToTargetAction.NavigateToTargetActionKind)]
    [JsonDerivedType(typeof(RequestNavigationTargetsAction), RequestNavigationTargetsAction.RequestNavigationTargetsActionKind)]
    [JsonDerivedType(typeof(ResolveNavigationTargetAction), ResolveNavigationTargetAction.ResolveNavigationTargetActionKind)]
    [JsonDerivedType(typeof(SetNavigationTargetsAction), SetNavigationTargetsAction.SetNavigationTargetsActionKind)]
    [JsonDerivedType(typeof(SetResolvedNavigationTargetAction), SetResolvedNavigationTargetAction.SetResolvedNavigationTargetActionKind)]
    [JsonDerivedType(typeof(EndProgressAction), EndProgressAction.EndProgressActionKind)]
    [JsonDerivedType(typeof(MessageAction), MessageAction.MessageActionKind)]
    [JsonDerivedType(typeof(StartProgressAction), StartProgressAction.StartProgressActionKind)]
    [JsonDerivedType(typeof(StatusAction), StatusAction.StatusActionKind)]
    [JsonDerivedType(typeof(UpdateProgressAction), UpdateProgressAction.UpdateProgressActionKind)]
    [JsonDerivedType(typeof(RequestPopupModelAction), RequestPopupModelAction.RequestPopupModelActionKind)]
    [JsonDerivedType(typeof(SelectAction), SelectAction.SelectActionKind)]
    [JsonDerivedType(typeof(SelectAllAction), SelectAllAction.SelectAllActionKind)]
    [JsonDerivedType(typeof(SetPopupModelAction), SetPopupModelAction.SetPopupModelActionKind)]
    [JsonDerivedType(typeof(CheckEdgeResultAction), CheckEdgeResultAction.CheckEdgeResultActionKind)]
    [JsonDerivedType(typeof(RequestCheckEdgeAction), RequestCheckEdgeAction.RequestCheckEdgeActionKind)]
    [JsonDerivedType(typeof(RequestTypeHintsAction), RequestTypeHintsAction.RequestTypeHintsActionKind)]
    [JsonDerivedType(typeof(SetTypeHintsAction), SetTypeHintsAction.SetTypeHintsActionKind)]
    [JsonDerivedType(typeof(RedoAction), RedoAction.RedoActionKind)]
    [JsonDerivedType(typeof(UndoAction), UndoAction.UndoActionKind)]
    [JsonDerivedType(typeof(DeleteMarkersAction), DeleteMarkersAction.DeleteMarkersActionKind)]
    [JsonDerivedType(typeof(RequestEditValidationAction), RequestEditValidationAction.RequestEditValidationActionKind)]
    [JsonDerivedType(typeof(RequestMarkersAction), RequestMarkersAction.RequestMarkersActionKind)]
    [JsonDerivedType(typeof(SetEditValidationResultAction), SetEditValidationResultAction.SetEditValidationResultActionKind)]
    [JsonDerivedType(typeof(SetMarkersAction), SetMarkersAction.SetMarkersActionKind)]
    public abstract class BaseAction
    {
        /// <summary>
        ///  Unique identifier specifying the kind of action to process.
        /// </summary>
        public abstract string Kind { get; }
    }
}
