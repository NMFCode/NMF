using Ionide.LanguageServerProtocol;
using Ionide.LanguageServerProtocol.Types;
using Microsoft.FSharp.Collections;
using Microsoft.FSharp.Control;
using Microsoft.FSharp.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    internal class AnyTextLspServerOld : Ionide.LanguageServerProtocol.ILspServer
    {
        public FSharpAsync<FSharpResult<FSharpOption<CallHierarchyIncomingCall[]>, JsonRpc.Error>> CallHierarchyIncomingCalls(CallHierarchyIncomingCallsParams value)
        {
            return ConvertOptional(CallHierarchyIncomingCallsAsync, value);
        }

        private async Task<CallHierarchyIncomingCall[]> CallHierarchyIncomingCallsAsync(CallHierarchyIncomingCallsParams value)
        {
            return null;
        }

        private static FSharpAsync<FSharpResult<TResult, JsonRpc.Error>> Convert<TIn, TResult>(Func<TIn, Task<TResult>> func, TIn input)
        {
            return FSharpAsync.AwaitTask(func(input).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    return FSharpResult<TResult, JsonRpc.Error>.NewError(JsonRpc.Error.InternalError);
                }
                return FSharpResult<TResult, JsonRpc.Error>.NewOk(t.Result);
            }, TaskScheduler.Default));
        }

        private static FSharpAsync<Unit> Convert<TIn>(Func<TIn, Task> func, TIn input)
        {
            return FSharpAsync.AwaitTask(func(input));
        }

        private static FSharpAsync<FSharpResult<FSharpOption<TResult>, JsonRpc.Error>> ConvertOptional<TIn, TResult>(Func<TIn, Task<TResult>> func, TIn input)
        {
            return FSharpAsync.AwaitTask(func(input).ContinueWith(t =>
            {
                if (t.IsFaulted)
                {
                    return FSharpResult<FSharpOption<TResult>, JsonRpc.Error>.NewError(JsonRpc.Error.InternalError);
                }
                if (EqualityComparer<TResult>.Default.Equals(t.Result, default))
                {
                    return FSharpResult<FSharpOption<TResult>, JsonRpc.Error>.NewOk(FSharpOption<TResult>.None);
                }
                return FSharpResult<FSharpOption<TResult>, JsonRpc.Error>.NewOk(FSharpOption<TResult>.Some(t.Result));
            }, TaskScheduler.Default));
        }

        public FSharpAsync<FSharpResult<FSharpOption<CallHierarchyOutgoingCall[]>, JsonRpc.Error>> CallHierarchyOutgoingCalls(CallHierarchyOutgoingCallsParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<CodeAction>, JsonRpc.Error>> CodeActionResolve(CodeAction value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<CodeLens, JsonRpc.Error>> CodeLensResolve(CodeLens value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<CompletionItem, JsonRpc.Error>> CompletionItemResolve(CompletionItem value)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public FSharpAsync<FSharpResult<DocumentLink, JsonRpc.Error>> DocumentLinkResolve(DocumentLink value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> Exit()
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<InitializeResult, JsonRpc.Error>> Initialize(InitializeParams value)
        {
            return FSharpAsync.AwaitTask(Task.FromResult(FSharpResult<InitializeResult, JsonRpc.Error>.NewOk(InitializeCore(value))));
        }

        private InitializeResult InitializeCore(InitializeParams value)
        {
            return new InitializeResult(new ServerCapabilities(
                    positionEncoding: FSharpOption<string>.None,
                    textDocumentSync: FSharpOption<U2<TextDocumentSyncOptions,TextDocumentSyncKind>>.Some(U2<TextDocumentSyncOptions, TextDocumentSyncKind>.NewC2(TextDocumentSyncKind.Incremental)),
                    notebookDocumentSync: FSharpOption<U2<NotebookDocumentSyncOptions, NotebookDocumentSyncRegistrationOptions>>.None,
                    completionProvider: FSharpOption<CompletionOptions>.None,
                    hoverProvider: FSharpOption<U2<bool, HoverOptions>>.None,
                    signatureHelpProvider: FSharpOption<SignatureHelpOptions>.None,
                    declarationProvider: FSharpOption<U3<bool, DeclarationOptions, DeclarationRegistrationOptions>>.None,
                    definitionProvider: FSharpOption<U2<bool, DefinitionOptions>>.None, 
                    typeDefinitionProvider: FSharpOption<U3<bool, TypeDefinitionOptions, TypeDefinitionRegistrationOptions>>.None,
                    implementationProvider: FSharpOption<U3<bool, ImplementationOptions, ImplementationRegistrationOptions>>.None,
                    referencesProvider: FSharpOption<U2<bool, ReferenceOptions>>.None,
                    documentHighlightProvider: FSharpOption<U2<bool, DocumentHighlightOptions>>.None,
                    documentSymbolProvider: FSharpOption<U2<bool, DocumentSymbolOptions>>.None,
                    codeActionProvider: FSharpOption<U2<bool, CodeActionOptions>>.None,
                    codeLensProvider: FSharpOption<CodeLensOptions>.None,
                    documentLinkProvider: FSharpOption<DocumentLinkOptions>.None,
                    colorProvider: FSharpOption<U3<bool, DocumentColorOptions, DocumentColorRegistrationOptions>>.None,
                    workspaceSymbolProvider: FSharpOption<U2<bool, WorkspaceSymbolOptions>>.None,
                    documentFormattingProvider: FSharpOption<U2<bool, DocumentFormattingOptions>>.None,
                    documentRangeFormattingProvider: FSharpOption<U2<bool, DocumentRangeFormattingOptions>>.None,
                    documentOnTypeFormattingProvider: FSharpOption<DocumentOnTypeFormattingOptions>.None,
                    renameProvider: FSharpOption<U2<bool, RenameOptions>>.None,
                    foldingRangeProvider: FSharpOption<U3<bool, FoldingRangeOptions, FoldingRangeRegistrationOptions>>.None,
                    selectionRangeProvider: FSharpOption<U3<bool, SelectionRangeOptions, SelectionRangeRegistrationOptions>>.None,
                    executeCommandProvider: FSharpOption<ExecuteCommandOptions>.None,
                    callHierarchyProvider: FSharpOption<U3<bool, CallHierarchyOptions, CallHierarchyRegistrationOptions>>.None,
                    linkedEditingRangeProvider: FSharpOption<U3<bool, LinkedEditingRangeOptions, LinkedEditingRangeRegistrationOptions>>.None,
                    semanticTokensProvider: FSharpOption< U2 < SemanticTokensOptions, SemanticTokensRegistrationOptions >>.None,
                    monikerProvider: FSharpOption<U3<bool, MonikerOptions, MonikerRegistrationOptions>>.None,
                    typeHierarchyProvider: FSharpOption<U3<bool, TypeHierarchyOptions, TypeHierarchyRegistrationOptions>>.None,
                    inlineValueProvider: FSharpOption<U3<bool, InlineValueOptions, InlineValueRegistrationOptions>>.None,
                    inlayHintProvider: FSharpOption<U3<bool, InlayHintOptions, InlayHintRegistrationOptions>>.None,
                    diagnosticProvider: FSharpOption<U2<DiagnosticOptions, DiagnosticRegistrationOptions>>.None,
                    workspace: FSharpOption<ServerCapabilitiesWorkspace>.None,
                    experimental: FSharpOption<JToken>.None
                ), FSharpOption<InitializeResultServerInfo>.Some(new InitializeResultServerInfo("AnyText", FSharpOption<string>.None)));
        }

        public FSharpAsync<Unit> Initialized()
        {
            return FSharpAsync.AwaitTask(Task.CompletedTask);
        }

        public FSharpAsync<FSharpResult<InlayHint, JsonRpc.Error>> InlayHintResolve(InlayHint value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> Shutdown()
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<U2<Command, CodeAction>[]>, JsonRpc.Error>> TextDocumentCodeAction(CodeActionParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<CodeLens[]>, JsonRpc.Error>> TextDocumentCodeLens(CodeLensParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<ColorPresentation[], JsonRpc.Error>> TextDocumentColorPresentation(ColorPresentationParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<CompletionList>, JsonRpc.Error>> TextDocumentCompletion(CompletionParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<U2<Location, Location[]>>, JsonRpc.Error>> TextDocumentDeclaration(TextDocumentPositionParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<U2<Location, Location[]>>, JsonRpc.Error>> TextDocumentDefinition(TextDocumentPositionParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<U2<RelatedFullDocumentDiagnosticReport, RelatedUnchangedDocumentDiagnosticReport>, JsonRpc.Error>> TextDocumentDiagnostic(DocumentDiagnosticParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> TextDocumentDidChange(DidChangeTextDocumentParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> TextDocumentDidClose(DidCloseTextDocumentParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> TextDocumentDidOpen(DidOpenTextDocumentParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> TextDocumentDidSave(DidSaveTextDocumentParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<ColorInformation[], JsonRpc.Error>> TextDocumentDocumentColor(DocumentColorParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<DocumentHighlight[]>, JsonRpc.Error>> TextDocumentDocumentHighlight(TextDocumentPositionParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<DocumentLink[]>, JsonRpc.Error>> TextDocumentDocumentLink(DocumentLinkParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<U2<SymbolInformation[], DocumentSymbol[]>>, JsonRpc.Error>> TextDocumentDocumentSymbol(DocumentSymbolParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<FSharpList<FoldingRange>>, JsonRpc.Error>> TextDocumentFoldingRange(FoldingRangeParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<Ionide.LanguageServerProtocol.Types.TextEdit[]>, JsonRpc.Error>> TextDocumentFormatting(DocumentFormattingParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<Hover>, JsonRpc.Error>> TextDocumentHover(TextDocumentPositionParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<U2<Location, Location[]>>, JsonRpc.Error>> TextDocumentImplementation(TextDocumentPositionParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<InlayHint[]>, JsonRpc.Error>> TextDocumentInlayHint(InlayHintParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<U3<InlineValueText, InlineValueVariableLookup, InlineValueEvaluatableExpression>[]>, JsonRpc.Error>> TextDocumentInlineValue(InlineValueParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<LinkedEditingRanges>, JsonRpc.Error>> TextDocumentLinkedEditingRange(TextDocumentPositionParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<Moniker[]>, JsonRpc.Error>> TextDocumentMoniker(TextDocumentPositionParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<Ionide.LanguageServerProtocol.Types.TextEdit[]>, JsonRpc.Error>> TextDocumentOnTypeFormatting(DocumentOnTypeFormattingParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<CallHierarchyItem[]>, JsonRpc.Error>> TextDocumentPrepareCallHierarchy(CallHierarchyPrepareParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<U3<Ionide.LanguageServerProtocol.Types.Range, PrepareRenameResultC2, PrepareRenameResultC3>>, JsonRpc.Error>> TextDocumentPrepareRename(PrepareRenameParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<TypeHierarchyItem[]>, JsonRpc.Error>> TextDocumentPrepareTypeHierarchy(TypeHierarchyPrepareParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<Ionide.LanguageServerProtocol.Types.TextEdit[]>, JsonRpc.Error>> TextDocumentRangeFormatting(DocumentRangeFormattingParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<Location[]>, JsonRpc.Error>> TextDocumentReferences(ReferenceParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<WorkspaceEdit>, JsonRpc.Error>> TextDocumentRename(RenameParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<FSharpList<SelectionRange>>, JsonRpc.Error>> TextDocumentSelectionRange(SelectionRangeParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<SemanticTokens>, JsonRpc.Error>> TextDocumentSemanticTokensFull(SemanticTokensParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<U2<SemanticTokens, SemanticTokensDelta>>, JsonRpc.Error>> TextDocumentSemanticTokensFullDelta(SemanticTokensDeltaParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<SemanticTokens>, JsonRpc.Error>> TextDocumentSemanticTokensRange(SemanticTokensRangeParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<SignatureHelp>, JsonRpc.Error>> TextDocumentSignatureHelp(SignatureHelpParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<U2<Location, Location[]>>, JsonRpc.Error>> TextDocumentTypeDefinition(TextDocumentPositionParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> TextDocumentWillSave(WillSaveTextDocumentParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<Ionide.LanguageServerProtocol.Types.TextEdit[]>, JsonRpc.Error>> TextDocumentWillSaveWaitUntil(WillSaveTextDocumentParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<TypeHierarchyItem[]>, JsonRpc.Error>> TypeHierarchySubtypes(TypeHierarchySubtypesParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<TypeHierarchyItem[]>, JsonRpc.Error>> TypeHierarchySupertypes(TypeHierarchySupertypesParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> WorkDoneProgressCancel(WorkDoneProgressCancelParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<WorkspaceDiagnosticReport, JsonRpc.Error>> WorkspaceDiagnostic(WorkspaceDiagnosticParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> WorkspaceDidChangeConfiguration(DidChangeConfigurationParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> WorkspaceDidChangeWatchedFiles(DidChangeWatchedFilesParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> WorkspaceDidChangeWorkspaceFolders(DidChangeWorkspaceFoldersParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> WorkspaceDidCreateFiles(CreateFilesParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> WorkspaceDidDeleteFiles(DeleteFilesParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<Unit> WorkspaceDidRenameFiles(RenameFilesParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<JToken, JsonRpc.Error>> WorkspaceExecuteCommand(ExecuteCommandParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<U2<SymbolInformation[], WorkspaceSymbol[]>>, JsonRpc.Error>> WorkspaceSymbol(WorkspaceSymbolParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<WorkspaceSymbol, JsonRpc.Error>> WorkspaceSymbolResolve(WorkspaceSymbol value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<WorkspaceEdit>, JsonRpc.Error>> WorkspaceWillCreateFiles(CreateFilesParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<WorkspaceEdit>, JsonRpc.Error>> WorkspaceWillDeleteFiles(DeleteFilesParams value)
        {
            throw new NotImplementedException();
        }

        public FSharpAsync<FSharpResult<FSharpOption<WorkspaceEdit>, JsonRpc.Error>> WorkspaceWillRenameFiles(RenameFilesParams value)
        {
            throw new NotImplementedException();
        }
    }
}
