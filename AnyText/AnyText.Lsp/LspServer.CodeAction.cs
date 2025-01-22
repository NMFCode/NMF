using System;
using System.Collections.Generic;
using System.Linq;
using LspTypes;
using Newtonsoft.Json.Linq;
using NMF.AnyText.Workspace;
using Range = LspTypes.Range;
using ChangeAnnotation = NMF.AnyText.Workspace.ChangeAnnotation;
using CreateFile = NMF.AnyText.Workspace.CreateFile;
using DeleteFile = NMF.AnyText.Workspace.DeleteFile;
using DeleteFileOptions = LspTypes.DeleteFileOptions;
using OptionalVersionedTextDocumentIdentifier = LspTypes.OptionalVersionedTextDocumentIdentifier;
using RenameFile = NMF.AnyText.Workspace.RenameFile;
using TextDocumentEdit = NMF.AnyText.Workspace.TextDocumentEdit;
using WorkspaceEdit = NMF.AnyText.Workspace.WorkspaceEdit;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <inheritdoc cref="ILspServer.CodeAction" />
        public CodeAction[] CodeAction(JToken arg)
        {
            var request = arg.ToObject<CodeActionParams>();

            var codeActions = new List<CodeAction>();
            if (!_documents.TryGetValue(request.TextDocument.Uri, out var document))
                return codeActions.ToArray();

            var codeActionCapabilities = _clientCapabilities?.TextDocument?.CodeAction;
            var supportsIsPreferred = codeActionCapabilities?.IsPreferredSupport == true;

            var documentUri = request.TextDocument.Uri;
            var diagnostics = request.Context.Diagnostics;
            var kindFilter = request.Context.Only;

            var startPosition = AsParsePosition(request.Range.Start);
            var endPosition = AsParsePosition(request.Range.End);

            var actions = document.GetCodeActionInfo(startPosition, endPosition);
            
           

            foreach (var action in actions)
            {
                var diagnosticIdentifier = action.DiagnosticIdentifier;
                var relevantDiagnostics = diagnostics
                    .Where(d => d.Message.Contains(diagnosticIdentifier))
                    .ToArray();
                
                if (!string.IsNullOrEmpty(diagnosticIdentifier) && relevantDiagnostics.Length == 0)
                    continue;
                
                var actionKind = !string.IsNullOrEmpty(action.Kind) ? ParseLspCodeActionKind(action.Kind) : null;
                if (kindFilter != null && kindFilter.Any() && actionKind != null &&
                    !kindFilter.Contains(actionKind.Value)) continue;
                
                var workspaceEdit = action.WorkspaceEdit?.Invoke(new ExecuteCommandArguments()
                {
                    Context = document.Context,
                    DocumentUri = documentUri,
                    Start = action.Start,
                    End = action.End,
                    OtherOptions = action.Arguments
                });
                
                var arguments = new object[]
                    { documentUri, action.Start, action.End };
                codeActions.Add(new CodeAction
                {
                    Title = action.Title,
                    Kind = actionKind,
                    Diagnostics = relevantDiagnostics.Length == 0 ? null : relevantDiagnostics,
                    Edit = action.WorkspaceEdit != null ? MapWorkspaceEdit(workspaceEdit) : null,
                    IsPreferred = supportsIsPreferred && action.IsPreferred ? true : null,
                    Command = action.CommandIdentifier != null
                        ? new Command
                        {
                            Title = action.CommandTitle,
                            CommandIdentifier = action.CommandIdentifier,
                            Arguments = action.Arguments != null
                                ? arguments.Concat(action.Arguments.Cast<object>()).ToArray()
                                : arguments.ToArray()
                        }
                        : null
                });
            }

            return codeActions.ToArray();
        }

        private static readonly Dictionary<string, CodeActionKind> KindMapping = new(StringComparer.OrdinalIgnoreCase)
        {
            { "", CodeActionKind.Empty },
            { "quickfix", CodeActionKind.QuickFix },
            { "refactor", CodeActionKind.Refactor },
            { "refactor.extract", CodeActionKind.RefactorExtract },
            { "refactor.inline", CodeActionKind.RefactorInline },
            { "refactor.rewrite", CodeActionKind.RefactorRewrite },
            { "source", CodeActionKind.Source },
            { "source.organizeImports", CodeActionKind.SourceOrganizeImports }
        };

        private static CodeActionKind? ParseLspCodeActionKind(string kind)
        {
            if (KindMapping.TryGetValue(kind, out var result)) return result;

            return null;
        }

        private LspTypes.WorkspaceEdit MapWorkspaceEdit(WorkspaceEdit workspaceEdit)
        {
            return new LspTypes.WorkspaceEdit
            {
                Changes = workspaceEdit.Changes != null ? MapChanges(workspaceEdit.Changes) : null,
                DocumentChanges = MapDocumentChanges(workspaceEdit.DocumentChanges),
                ChangeAnnotations = workspaceEdit.ChangeAnnotations != null
                    ? MapChangeAnnotations(workspaceEdit.ChangeAnnotations)
                    : null
            };
        }

        private Dictionary<string, LspTypes.TextEdit[]> MapChanges(Dictionary<string, TextEdit[]> changes)
        {
            var lspChanges = new Dictionary<string, LspTypes.TextEdit[]>();
            var workspaceFolder = _workspaceFolders.FirstOrDefault()?.Uri;
            foreach (var entry in changes)
                lspChanges.Add($"{workspaceFolder}/{entry.Key}", MapTextEditsArray(entry.Value));

            return lspChanges;
        }

        private LspTypes.TextEdit[] MapTextEditsArray(TextEdit[] textEdits)
        {
            return textEdits.Select(e => MapTextEdit(e)).ToArray();
        }

        private LspTypes.TextEdit MapTextEdit(TextEdit textEdit)
        {
            return new LspTypes.TextEdit
            {
                Range = new Range
                {
                    Start = new Position
                    {
                        Line = (uint)textEdit.Start.Line,
                        Character = (uint)textEdit.Start.Col
                    },
                    End = new Position
                    {
                        Line = (uint)textEdit.End.Line,
                        Character = (uint)textEdit.End.Col
                    }
                },
                NewText = string.Concat(textEdit.NewText)
            };
        }

        private SumType<LspTypes.TextDocumentEdit[], SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile,
            LspTypes.RenameFile, LspTypes.DeleteFile>[]> MapDocumentChanges(List<DocumentChange> documentChanges)
        {
            var lspDocumentChanges = documentChanges.Select(MapDocumentChange).ToList();
            return new SumType<LspTypes.TextDocumentEdit[], SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile,
                LspTypes.RenameFile, LspTypes.DeleteFile>[]>(lspDocumentChanges.ToArray());
        }


        private SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>
            MapDocumentChange(DocumentChange docChange)
        {
            if (docChange.TextDocumentEdit != null) return MapTextDocumentEdit(docChange.TextDocumentEdit);

            if (docChange.CreateFile != null) return MapCreateFile(docChange.CreateFile);

            if (docChange.RenameFile != null) return MapRenameFile(docChange.RenameFile);

            if (docChange.DeleteFile != null) return MapDeleteFile(docChange.DeleteFile);

            throw new InvalidOperationException(
                "DocumentChange must contain one of TextDocumentEdit, CreateFile, RenameFile, or DeleteFile.");
        }

        private SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>
            MapTextDocumentEdit(TextDocumentEdit textDocumentEdit)
        {
            var workspaceFolder = _workspaceFolders.FirstOrDefault()?.Uri;
            var fileUri = $"{workspaceFolder}/{textDocumentEdit.TextDocument.Uri}";
            var edits = textDocumentEdit.Edits
                .Select(e => new SumType<LspTypes.TextEdit, AnnotatedTextEdit>(MapTextEdit(e))).ToArray();
            var lspTextDocumentEdit = new LspTypes.TextDocumentEdit
            {
                TextDocument = new OptionalVersionedTextDocumentIdentifier
                {
                    Uri = fileUri,
                    Version = textDocumentEdit.TextDocument.Version
                },
                Edits = edits
            };

            return new
                SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>(
                    lspTextDocumentEdit);
        }

        private SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>
            MapCreateFile(CreateFile createFile)
        {
            var workspaceFolder = _workspaceFolders.FirstOrDefault()?.Uri;
            var fileUri = $"{workspaceFolder}/{createFile.Uri}";
            var lspCreateFile = new LspTypes.CreateFile
            {
                Kind = createFile.Kind,
                Uri = fileUri,
                Options = new CreateFileOptions
                {
                    Overwrite = createFile.Options?.Overwrite,
                    IgnoreIfExists = createFile.Options?.IgnoreIfExists
                },
                AnnotationId = createFile.AnnotationId
            };

            return new
                SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>(
                    lspCreateFile);
        }

        private SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>
            MapRenameFile(RenameFile renameFile)
        {
            var workspaceFolder = _workspaceFolders.FirstOrDefault()?.Uri;
            var oldfileUri = $"{workspaceFolder}/{renameFile.OldUri}";
            var newfileUri = $"{workspaceFolder}/{renameFile.NewUri}";
            var lspRenameFile = new LspTypes.RenameFile
            {
                Kind = renameFile.Kind,
                OldUri = oldfileUri,
                NewUri = newfileUri,
                Options = new RenameFileOptions
                {
                    Overwrite = renameFile.Options?.Overwrite,
                    IgnoreIfExists = renameFile.Options?.IgnoreIfExists
                },
                AnnotationId = renameFile.AnnotationId
            };

            return new
                SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>(
                    lspRenameFile);
        }

        private SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>
            MapDeleteFile(DeleteFile deleteFile)
        {
            var workspaceFolder = _workspaceFolders.FirstOrDefault()?.Uri;
            var fileUri = $"{workspaceFolder}/{deleteFile.Uri}";
            var lspDeleteFile = new LspTypes.DeleteFile
            {
                Kind = deleteFile.Kind,
                Uri = fileUri,
                Options = new DeleteFileOptions
                {
                    Recursive = deleteFile.Options?.Recursive,
                    IgnoreIfNotExists = deleteFile.Options?.IgnoreIfNotExists
                },
                AnnotationId = deleteFile.AnnotationId
            };

            return new
                SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>(
                    lspDeleteFile);
        }

        private Dictionary<string, LspTypes.ChangeAnnotation> MapChangeAnnotations(
            Dictionary<string, ChangeAnnotation> changeAnnotations)
        {
            var lspChangeAnnotations = changeAnnotations.ToDictionary(
                entry => entry.Key,
                entry => new LspTypes.ChangeAnnotation
                {
                    Label = entry.Value.Label,
                    NeedsConfirmation = entry.Value.NeedsConfirmation,
                    Description = entry.Value.Description
                }
            );

            return lspChangeAnnotations;
        }
    }
}