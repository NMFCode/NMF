using LspTypes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using NMF.AnyText.Workspace;
using ChangeAnnotation = NMF.AnyText.Workspace.ChangeAnnotation;
using CreateFile = NMF.AnyText.Workspace.CreateFile;
using DeleteFile = NMF.AnyText.Workspace.DeleteFile;
using DeleteFileOptions = LspTypes.DeleteFileOptions;
using OptionalVersionedTextDocumentIdentifier = LspTypes.OptionalVersionedTextDocumentIdentifier;
using Range = LspTypes.Range;
using RenameFile = NMF.AnyText.Workspace.RenameFile;
using TextDocumentEdit = NMF.AnyText.Workspace.TextDocumentEdit;
using WorkspaceEdit = NMF.AnyText.Workspace.WorkspaceEdit;

namespace NMF.AnyText
{
    /// <summary>
    /// Class To Map Types used In Parser To LSP-Types
    /// </summary>
    public static class LspTypesMapper
    {
        /// <summary>
        /// Maps a collection of <see cref="TextEdit"/> objects to a collection of <see cref="LspTypes.TextEdit"/> objects.
        /// </summary>
        /// <param name="edits">The collection of <see cref="TextEdit"/> objects to be mapped.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> representing the mapped LSP text edits.</returns>
        public static IEnumerable<LspTypes.TextEdit> MapToLspTextEdits(IEnumerable<TextEdit> edits)
        {
            return edits.Select(e => new LspTypes.TextEdit
            {
                Range = new Range
                {
                    Start = new Position
                    {
                        Line = (uint)e.Start.Line,
                        Character = (uint)e.Start.Col
                    },
                    End = new Position
                    {
                        Line = (uint)e.End.Line,
                        Character = (uint)e.End.Col
                    }
                },
                NewText = string.Join("\n", e.NewText)
            });
        }

        /// <summary>
        /// Gets the mappings of symbol kinds to completion item kinds
        /// </summary>
        public static readonly ReadOnlyDictionary<SymbolKind, CompletionItemKind> SymbolKindMappings = (new Dictionary<SymbolKind, CompletionItemKind>()
        {
            { SymbolKind.File, CompletionItemKind.File },
            { SymbolKind.Module, CompletionItemKind.Module},
            { SymbolKind.Namespace, CompletionItemKind.Text },
            { SymbolKind.Package, CompletionItemKind.Folder },
            { SymbolKind.Class, CompletionItemKind.Class },
            { SymbolKind.Method, CompletionItemKind.Method },
            { SymbolKind.Property, CompletionItemKind.Property },
            { SymbolKind.Field, CompletionItemKind.Field },
            { SymbolKind.Constructor,CompletionItemKind.Constructor },
            { SymbolKind.Enum, CompletionItemKind.Enum },
            { SymbolKind.Interface, CompletionItemKind.Interface },
            { SymbolKind.Function, CompletionItemKind.Function },
            { SymbolKind.Variable, CompletionItemKind.Variable },
            { SymbolKind.Constant, CompletionItemKind.Constant },
            { SymbolKind.String, CompletionItemKind.Text },
            { SymbolKind.Number, CompletionItemKind.Value },
            { SymbolKind.Boolean, CompletionItemKind.Value },
            { SymbolKind.Array, CompletionItemKind.Unit },
            { SymbolKind.Object, CompletionItemKind.Struct },
            { SymbolKind.Key, CompletionItemKind.Keyword },
            { SymbolKind.Null, CompletionItemKind.Text },
            { SymbolKind.EnumMember, CompletionItemKind.EnumMember },
            { SymbolKind.Struct, CompletionItemKind.Struct },
            { SymbolKind.Event, CompletionItemKind.Event },
            { SymbolKind.Operator, CompletionItemKind.Operator },
            { SymbolKind.TypeParameter, CompletionItemKind.TypeParameter },
        }).AsReadOnly();
        
        /// <summary>
        /// Maps a  <see cref="WorkspaceEdit"/> objects to a <see cref="LspTypes.WorkspaceEdit"/>.
        /// </summary>
        /// <param name="workspaceEdit">The <see cref="WorkspaceEdit"/> object to be mapped.</param>
        /// <param name="workspaceUri">The workspace Uri for the edit.</param>
        /// <param name="useDocumentChanges">Option to use <see cref="DocumentChange"/> instead of <see cref="TextEdit"/> changes.</param>
        /// <returns>A <see cref="LspTypes.WorkspaceEdit"/> representing the mapped LSP workspace edit.</returns>
        public static LspTypes.WorkspaceEdit MapWorkspaceEdit(WorkspaceEdit workspaceEdit, string workspaceUri, bool useDocumentChanges = false)
        {
            var edit = new LspTypes.WorkspaceEdit
            {
                ChangeAnnotations = workspaceEdit.ChangeAnnotations != null
                    ? MapChangeAnnotations(workspaceEdit.ChangeAnnotations)
                    : null
            };
            
            if (useDocumentChanges && workspaceEdit.DocumentChanges != null)
            {
                edit.DocumentChanges = MapDocumentChanges(workspaceEdit.DocumentChanges, workspaceUri);
            }
            else
            {
                edit.Changes = MapChanges(workspaceEdit.Changes, workspaceUri);
            }
            
            return edit;
        }

        private static Dictionary<string, LspTypes.TextEdit[]> MapChanges(Dictionary<string, TextEdit[]> changes,
            string workspaceUri)
        {
            var lspChanges = new Dictionary<string, LspTypes.TextEdit[]>();
            foreach (var entry in changes)
            {
                string key = string.IsNullOrEmpty(workspaceUri) ? entry.Key : $"{workspaceUri}/{entry.Key}";
                lspChanges.Add(key, MapToLspTextEdits(entry.Value).ToArray());
            }
            
            return lspChanges;
        }
        
        private static SumType<LspTypes.TextDocumentEdit[], SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile,
            LspTypes.RenameFile, LspTypes.DeleteFile>[]> MapDocumentChanges(List<DocumentChange> documentChanges,
            string workspaceUri)
        {
            var lspDocumentChanges = documentChanges.Select(d => MapDocumentChange(d, workspaceUri)).ToList();
            return new SumType<LspTypes.TextDocumentEdit[], SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile,
                LspTypes.RenameFile, LspTypes.DeleteFile>[]>(lspDocumentChanges.ToArray());
        }


        private static SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>
            MapDocumentChange(DocumentChange docChange, string workspaceUri)
        {
            if (docChange.TextDocumentEdit != null)
                return MapTextDocumentEdit(docChange.TextDocumentEdit, workspaceUri);

            if (docChange.CreateFile != null) return MapCreateFile(docChange.CreateFile, workspaceUri);

            if (docChange.RenameFile != null) return MapRenameFile(docChange.RenameFile, workspaceUri);

            if (docChange.DeleteFile != null) return MapDeleteFile(docChange.DeleteFile, workspaceUri);

            throw new InvalidOperationException(
                "DocumentChange must contain one of TextDocumentEdit, CreateFile, RenameFile, or DeleteFile.");
        }

        private static SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>
            MapTextDocumentEdit(TextDocumentEdit textDocumentEdit, string workspaceUri)
        {
            
            var fileUri = string.IsNullOrEmpty(workspaceUri) ? textDocumentEdit.TextDocument.Uri : $"{workspaceUri}/{textDocumentEdit.TextDocument.Uri}";
            var edits = textDocumentEdit.Edits
                .Select(e => new SumType<LspTypes.TextEdit, AnnotatedTextEdit>(MapToLspTextEdits([e]).First())).ToArray();
            var lspTextDocumentEdit = new LspTypes.TextDocumentEdit
            {
                TextDocument = new OptionalVersionedTextDocumentIdentifier()
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

        private static SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>
            MapCreateFile(CreateFile createFile, string workspaceUri)
        {
            var fileUri = string.IsNullOrEmpty(workspaceUri) ? createFile.Uri : $"{workspaceUri}/{createFile.Uri}";
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

        private static SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>
            MapRenameFile(RenameFile renameFile, string workspaceUri)
        {
            

            var oldfileUri = string.IsNullOrEmpty(workspaceUri) ? renameFile.OldUri : $"{workspaceUri}/{renameFile.OldUri}";
            var newfileUri = string.IsNullOrEmpty(workspaceUri) ? renameFile.NewUri : $"{workspaceUri}/{renameFile.NewUri}";

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

        private static SumType<LspTypes.TextDocumentEdit, LspTypes.CreateFile, LspTypes.RenameFile, LspTypes.DeleteFile>
            MapDeleteFile(DeleteFile deleteFile, string workspaceUri)
        {
            var fileUri = string.IsNullOrEmpty(workspaceUri) ? deleteFile.Uri : $"{workspaceUri}/{deleteFile.Uri}";
            var lspDeleteFile = new LspTypes.DeleteFile
            {
                Kind = deleteFile.Kind,
                Uri = fileUri,
                Options = new DeleteFileOptions()
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

        private static Dictionary<string, LspTypes.ChangeAnnotation> MapChangeAnnotations(
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