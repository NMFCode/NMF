using NMF.Glsp.Protocol.BaseProtocol;
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
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NMF.Glsp.Server
{
    /// <summary>
    /// Denotes a class that can convert actions to JSON
    /// </summary>
    public class BaseActionConverter : JsonConverter<BaseAction>
    {
        /// <inheritdoc />
        public override BaseAction Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            Utf8JsonReader readerClone = reader;
            if (readerClone.TokenType != JsonTokenType.StartObject) { Throw(); }
            readerClone.Read();
            if (readerClone.TokenType != JsonTokenType.PropertyName) { Throw(); }
            if (readerClone.GetString() != "kind") { Throw(); }
            readerClone.Read();
            var kind = readerClone.GetString();
            return kind switch
            {
                CompoundOperation.CompoundOperationKind => JsonSerializer.Deserialize<CompoundOperation>(ref reader, options),
                RejectAction.RejectActionKind => JsonSerializer.Deserialize<RejectAction>(ref reader, options),
                CutOperation.CutOperationKind => JsonSerializer.Deserialize<CutOperation>(ref reader, options),
                PasteOperation.PasteOperationKind => JsonSerializer.Deserialize<PasteOperation>(ref reader, options),
                RequestClipboardDataAction.RequestClipboardDataActionKind => JsonSerializer.Deserialize<RequestClipboardDataAction>(ref reader, options),
                SetClipboardDataAction.SetClipboardDataActionKind => JsonSerializer.Deserialize<SetClipboardDataAction>(ref reader, options),
                RequestContextActions.RequestContextActionsKind => JsonSerializer.Deserialize<RequestContextActions>(ref reader, options),
                SetContextActions.SetContextActionsKind => JsonSerializer.Deserialize<SetContextActions>(ref reader, options),
                TriggerEdgeCreationAction.TriggerEdgeCreationActionKind => JsonSerializer.Deserialize<TriggerEdgeCreationAction>(ref reader, options),
                TriggerNodeCreationAction.TriggerNodeCreationActionKind => JsonSerializer.Deserialize<TriggerNodeCreationAction>(ref reader, options),
                CenterAction.CenterActionKind => JsonSerializer.Deserialize<CenterAction>(ref reader, options),
                ComputedBoundsAction.ComputedBoundsActionKind => JsonSerializer.Deserialize<ComputedBoundsAction>(ref reader, options),
                FitToScreenAction.FitToScreenActionKind => JsonSerializer.Deserialize<FitToScreenAction>(ref reader, options),
                LayoutOperation.LayoutOperationKind => JsonSerializer.Deserialize<LayoutOperation>(ref reader, options),
                RequestBoundsAction.RequestBoundsActionKind => JsonSerializer.Deserialize<RequestBoundsAction>(ref reader, options),
                ExportSvgAction.ExportSvgActionKind => JsonSerializer.Deserialize<ExportSvgAction>(ref reader, options),
                RequestExportSvgAction.RequestExportSvgActionKind => JsonSerializer.Deserialize<RequestExportSvgAction>(ref reader, options),
                RequestModelAction.RequestModelActionKind => JsonSerializer.Deserialize<RequestModelAction>(ref reader, options),
                SaveModelAction.SaveModelActionKind => JsonSerializer.Deserialize<SaveModelAction>(ref reader, options),
                SetDirtyAction.SetDirtyActionKind => JsonSerializer.Deserialize<SetDirtyAction>(ref reader, options),
                SetEditModeAction.SetEditModeActionKind => JsonSerializer.Deserialize<SetEditModeAction>(ref reader, options),
                SetModelAction.SetModelActionKind => JsonSerializer.Deserialize<SetModelAction>(ref reader, options),
                SourceModelChangedAction.SourceModelChangedActionKind => JsonSerializer.Deserialize<SourceModelChangedAction>(ref reader, options),
                UpdateModelAction.UpdateModelActionKind => JsonSerializer.Deserialize<UpdateModelAction>(ref reader, options),
                ApplyLabelEditOperation.ApplyLabelEditOperationKind => JsonSerializer.Deserialize<ApplyLabelEditOperation>(ref reader, options),
                ChangeBoundsOperation.ChangeBoundsOperationKind => JsonSerializer.Deserialize<ChangeBoundsOperation>(ref reader, options),
                ChangeContainerOperation.ChangeContainerOperationKind => JsonSerializer.Deserialize<ChangeContainerOperation>(ref reader, options),
                ChangeRoutingPointsOperation.ChangeRoutingPointsOperationKind => JsonSerializer.Deserialize<ChangeRoutingPointsOperation>(ref reader, options),
                CreateEdgeOperation.CreateEdgeOperationKind => JsonSerializer.Deserialize<CreateEdgeOperation>(ref reader, options),
                CreateNodeOperation.CreateNodeOperationKind => JsonSerializer.Deserialize<CreateNodeOperation>(ref reader, options),
                DeleteElementOperation.DeleteElementOperationKind => JsonSerializer.Deserialize<DeleteElementOperation>(ref reader, options),
                ReconnectEdgeOperation.ReconnectEdgeOperationKind => JsonSerializer.Deserialize<ReconnectEdgeOperation>(ref reader, options),
                NavigateToExternalTargetAction.NavigateToExternalTargetActionKind => JsonSerializer.Deserialize<NavigateToExternalTargetAction>(ref reader, options),
                NavigateToTargetAction.NavigateToTargetActionKind => JsonSerializer.Deserialize<NavigateToTargetAction>(ref reader, options),
                RequestNavigationTargetsAction.RequestNavigationTargetsActionKind => JsonSerializer.Deserialize<RequestNavigationTargetsAction>(ref reader, options),
                ResolveNavigationTargetAction.ResolveNavigationTargetActionKind => JsonSerializer.Deserialize<ResolveNavigationTargetAction>(ref reader, options),
                SetNavigationTargetsAction.SetNavigationTargetsActionKind => JsonSerializer.Deserialize<SetNavigationTargetsAction>(ref reader, options),
                SetResolvedNavigationTargetAction.SetResolvedNavigationTargetActionKind => JsonSerializer.Deserialize<SetResolvedNavigationTargetAction>(ref reader, options),
                EndProgressAction.EndProgressActionKind => JsonSerializer.Deserialize<EndProgressAction>(ref reader, options),
                MessageAction.MessageActionKind => JsonSerializer.Deserialize<MessageAction>(ref reader, options),
                StartProgressAction.StartProgressActionKind => JsonSerializer.Deserialize<StartProgressAction>(ref reader, options),
                StatusAction.StatusActionKind => JsonSerializer.Deserialize<StatusAction>(ref reader, options),
                UpdateProgressAction.UpdateProgressActionKind => JsonSerializer.Deserialize<UpdateProgressAction>(ref reader, options),
                RequestPopupModelAction.RequestPopupModelActionKind => JsonSerializer.Deserialize<RequestPopupModelAction>(ref reader, options),
                SelectAction.SelectActionKind => JsonSerializer.Deserialize<SelectAction>(ref reader, options),
                SelectAllAction.SelectAllActionKind => JsonSerializer.Deserialize<SelectAllAction>(ref reader, options),
                SetPopupModelAction.SetPopupModelActionKind => JsonSerializer.Deserialize<SetPopupModelAction>(ref reader, options),
                CheckEdgeResultAction.CheckEdgeResultActionKind => JsonSerializer.Deserialize<CheckEdgeResultAction>(ref reader, options),
                RequestCheckEdgeAction.RequestCheckEdgeActionKind => JsonSerializer.Deserialize<RequestCheckEdgeAction>(ref reader, options),
                RequestTypeHintsAction.RequestTypeHintsActionKind => JsonSerializer.Deserialize<RequestTypeHintsAction>(ref reader, options),
                SetTypeHintsAction.SetTypeHintsActionKind => JsonSerializer.Deserialize<SetTypeHintsAction>(ref reader, options),
                RedoAction.RedoActionKind => JsonSerializer.Deserialize<RedoAction>(ref reader, options),
                UndoAction.UndoActionKind => JsonSerializer.Deserialize<UndoAction>(ref reader, options),
                DeleteMarkersAction.DeleteMarkersActionKind => JsonSerializer.Deserialize<DeleteMarkersAction>(ref reader, options),
                RequestEditValidationAction.RequestEditValidationActionKind => JsonSerializer.Deserialize<RequestEditValidationAction>(ref reader, options),
                RequestMarkersAction.RequestMarkersActionKind => JsonSerializer.Deserialize<RequestMarkersAction>(ref reader, options),
                SetEditValidationResultAction.SetEditValidationResultActionKind => JsonSerializer.Deserialize<SetEditValidationResultAction>(ref reader, options),
                SetMarkersAction.SetMarkersActionKind => JsonSerializer.Deserialize<SetMarkersAction>(ref reader, options),
                _ => DeserializeCustom(kind, ref reader, options)
            };
        }

        private BaseAction DeserializeCustom(string kind, ref Utf8JsonReader reader, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<CustomOperation>(ref reader, options);
        }

        private void Throw()
        {
            throw new JsonException("Input Json has invalid format");
        }

        /// <inheritdoc />
        public override void Write(Utf8JsonWriter writer, BaseAction value, JsonSerializerOptions options)
        {
            switch (value)
            {
                case CompoundOperation CompoundOperation: JsonSerializer.Serialize(writer, CompoundOperation, options); break;
                case RejectAction RejectAction: JsonSerializer.Serialize(writer, RejectAction, options); break;
                case CutOperation CutOperation: JsonSerializer.Serialize(writer, CutOperation, options); break;
                case PasteOperation PasteOperation: JsonSerializer.Serialize(writer, PasteOperation, options); break;
                case RequestClipboardDataAction RequestClipboardDataAction: JsonSerializer.Serialize(writer, RequestClipboardDataAction, options); break;
                case SetClipboardDataAction SetClipboardDataAction: JsonSerializer.Serialize(writer, SetClipboardDataAction, options); break;
                case RequestContextActions RequestContextActions: JsonSerializer.Serialize(writer, RequestContextActions, options); break;
                case SetContextActions SetContextActions: JsonSerializer.Serialize(writer, SetContextActions, options); break;
                case TriggerEdgeCreationAction TriggerEdgeCreationAction: JsonSerializer.Serialize(writer, TriggerEdgeCreationAction, options); break;
                case TriggerNodeCreationAction TriggerNodeCreationAction: JsonSerializer.Serialize(writer, TriggerNodeCreationAction, options); break;
                case CenterAction CenterAction: JsonSerializer.Serialize(writer, CenterAction, options); break;
                case ComputedBoundsAction ComputedBoundsAction: JsonSerializer.Serialize(writer, ComputedBoundsAction, options); break;
                case FitToScreenAction FitToScreenAction: JsonSerializer.Serialize(writer, FitToScreenAction, options); break;
                case LayoutOperation LayoutOperation: JsonSerializer.Serialize(writer, LayoutOperation, options); break;
                case RequestBoundsAction RequestBoundsAction: JsonSerializer.Serialize(writer, RequestBoundsAction, options); break;
                case ExportSvgAction ExportSvgAction: JsonSerializer.Serialize(writer, ExportSvgAction, options); break;
                case RequestExportSvgAction RequestExportSvgAction: JsonSerializer.Serialize(writer, RequestExportSvgAction, options); break;
                case RequestModelAction RequestModelAction: JsonSerializer.Serialize(writer, RequestModelAction, options); break;
                case SaveModelAction SaveModelAction: JsonSerializer.Serialize(writer, SaveModelAction, options); break;
                case SetDirtyAction SetDirtyAction: JsonSerializer.Serialize(writer, SetDirtyAction, options); break;
                case SetEditModeAction SetEditModeAction: JsonSerializer.Serialize(writer, SetEditModeAction, options); break;
                case SetModelAction SetModelAction: JsonSerializer.Serialize(writer, SetModelAction, options); break;
                case SourceModelChangedAction SourceModelChangedAction: JsonSerializer.Serialize(writer, SourceModelChangedAction, options); break;
                case UpdateModelAction UpdateModelAction: JsonSerializer.Serialize(writer, UpdateModelAction, options); break;
                case ApplyLabelEditOperation ApplyLabelEditOperation: JsonSerializer.Serialize(writer, ApplyLabelEditOperation, options); break;
                case ChangeBoundsOperation ChangeBoundsOperation: JsonSerializer.Serialize(writer, ChangeBoundsOperation, options); break;
                case ChangeContainerOperation ChangeContainerOperation: JsonSerializer.Serialize(writer, ChangeContainerOperation, options); break;
                case ChangeRoutingPointsOperation ChangeRoutingPointsOperation: JsonSerializer.Serialize(writer, ChangeRoutingPointsOperation, options); break;
                case CreateEdgeOperation CreateEdgeOperation: JsonSerializer.Serialize(writer, CreateEdgeOperation, options); break;
                case CreateNodeOperation CreateNodeOperation: JsonSerializer.Serialize(writer, CreateNodeOperation, options); break;
                case DeleteElementOperation DeleteElementOperation: JsonSerializer.Serialize(writer, DeleteElementOperation, options); break;
                case ReconnectEdgeOperation ReconnectEdgeOperation: JsonSerializer.Serialize(writer, ReconnectEdgeOperation, options); break;
                case NavigateToExternalTargetAction NavigateToExternalTargetAction: JsonSerializer.Serialize(writer, NavigateToExternalTargetAction, options); break;
                case NavigateToTargetAction NavigateToTargetAction: JsonSerializer.Serialize(writer, NavigateToTargetAction, options); break;
                case RequestNavigationTargetsAction RequestNavigationTargetsAction: JsonSerializer.Serialize(writer, RequestNavigationTargetsAction, options); break;
                case ResolveNavigationTargetAction ResolveNavigationTargetAction: JsonSerializer.Serialize(writer, ResolveNavigationTargetAction, options); break;
                case SetNavigationTargetsAction SetNavigationTargetsAction: JsonSerializer.Serialize(writer, SetNavigationTargetsAction, options); break;
                case SetResolvedNavigationTargetAction SetResolvedNavigationTargetAction: JsonSerializer.Serialize(writer, SetResolvedNavigationTargetAction, options); break;
                case EndProgressAction EndProgressAction: JsonSerializer.Serialize(writer, EndProgressAction, options); break;
                case MessageAction MessageAction: JsonSerializer.Serialize(writer, MessageAction, options); break;
                case StartProgressAction StartProgressAction: JsonSerializer.Serialize(writer, StartProgressAction, options); break;
                case StatusAction StatusAction: JsonSerializer.Serialize(writer, StatusAction, options); break;
                case UpdateProgressAction UpdateProgressAction: JsonSerializer.Serialize(writer, UpdateProgressAction, options); break;
                case RequestPopupModelAction RequestPopupModelAction: JsonSerializer.Serialize(writer, RequestPopupModelAction, options); break;
                case SelectAction SelectAction: JsonSerializer.Serialize(writer, SelectAction, options); break;
                case SelectAllAction SelectAllAction: JsonSerializer.Serialize(writer, SelectAllAction, options); break;
                case SetPopupModelAction SetPopupModelAction: JsonSerializer.Serialize(writer, SetPopupModelAction, options); break;
                case CheckEdgeResultAction CheckEdgeResultAction: JsonSerializer.Serialize(writer, CheckEdgeResultAction, options); break;
                case RequestCheckEdgeAction RequestCheckEdgeAction: JsonSerializer.Serialize(writer, RequestCheckEdgeAction, options); break;
                case RequestTypeHintsAction RequestTypeHintsAction: JsonSerializer.Serialize(writer, RequestTypeHintsAction, options); break;
                case SetTypeHintsAction SetTypeHintsAction: JsonSerializer.Serialize(writer, SetTypeHintsAction, options); break;
                case RedoAction RedoAction: JsonSerializer.Serialize(writer, RedoAction, options); break;
                case UndoAction UndoAction: JsonSerializer.Serialize(writer, UndoAction, options); break;
                case DeleteMarkersAction DeleteMarkersAction: JsonSerializer.Serialize(writer, DeleteMarkersAction, options); break;
                case RequestEditValidationAction RequestEditValidationAction: JsonSerializer.Serialize(writer, RequestEditValidationAction, options); break;
                case RequestMarkersAction RequestMarkersAction: JsonSerializer.Serialize(writer, RequestMarkersAction, options); break;
                case SetEditValidationResultAction SetEditValidationResultAction: JsonSerializer.Serialize(writer, SetEditValidationResultAction, options); break;
                case SetMarkersAction SetMarkersAction: JsonSerializer.Serialize(writer, SetMarkersAction, options); break;
                default: SerializeCustomType(writer, value, options); break;
            }
        }

        private void SerializeCustomType(Utf8JsonWriter writer, BaseAction value, JsonSerializerOptions options)
        {
            if (value == null) { return; }
            JsonSerializer.Serialize(writer, value as CustomOperation, options);
        }
    }
}
