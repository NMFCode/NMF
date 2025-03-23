﻿using LspTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NMF.AnyText.Grammars;
using NMF.Models.Services;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes the implementation of an LSP server
    /// </summary>
    public partial class LspServer : ILspServer
    {
        private readonly JsonRpc _rpc;
        private readonly Dictionary<string, Parser> _documents = new Dictionary<string, Parser>();
        private readonly Dictionary<string, Grammar> _languages;
        private ClientCapabilities _clientCapabilities;
        private WorkspaceFolder[] _workspaceFolders;
        
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rpc">the RPC handler</param>
        /// <param name="grammars">A collection of grammars</param>
        public LspServer(JsonRpc rpc, params Grammar[] grammars)
            : this(rpc, (IEnumerable<Grammar>)grammars)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="rpc">the RPC handler</param>
        /// <param name="grammars">A collection of grammars</param>
        public LspServer(JsonRpc rpc, IEnumerable<Grammar> grammars)
        {
            _rpc = rpc;
            _languages = grammars?.ToDictionary(sp => sp.LanguageId);

            foreach (Grammar grammar in grammars)
            {
                grammar.Initialize();
                foreach (var codeAction in grammar.ExecutableActions)
                {
                    _codeActions[codeAction.Key] = grammar;
                }
            }
        }

        /// <inheritdoc/>
        public InitializeResult Initialize(
            int? processId,
            _InitializeParams_ClientInfo clientInfo,
            string locale,
            string rootPath,
            Uri rootUri,
            ClientCapabilities capabilities,
            TraceValue trace,
            WorkspaceFolder[] workspaceFolders,
            object InitializationOptions = null)
        {
            _clientCapabilities = capabilities;
            _workspaceFolders = workspaceFolders;
            var serverCapabilities = new ServerCapabilities
            {
                TextDocumentSync = new TextDocumentSyncOptions
                {
                    OpenClose = true,
                    Change = TextDocumentSyncKind.Incremental,
                    Save = new SaveOptions
                    {
                        IncludeText = true,
                    }
                },
                ReferencesProvider = new ReferenceOptions
                {
                    WorkDoneProgress = false
                },
                DefinitionProvider = new DefinitionOptions
                {
                    WorkDoneProgress = false,
                    DocumentSelector = new DocumentFilter[] { new DocumentFilter() { Scheme = "file" } }
                },
                RenameProvider = new RenameOptions
                {
                    WorkDoneProgress = false
                },
                DocumentHighlightProvider = new DocumentHighlightOptions
                {
                    WorkDoneProgress = false
                },
                DocumentSymbolProvider = new DocumentSymbolOptions
                {
                    WorkDoneProgress = false
                },
                SelectionRangeProvider = new SelectionRangeOptions
                {
                    WorkDoneProgress = false
                },
                CodeActionProvider = new CodeActionOptions
                {
                    CodeActionKinds = new[]
                    {
                        CodeActionKind.RefactorExtract, CodeActionKind.Empty, CodeActionKind.Refactor,
                        CodeActionKind.Source, CodeActionKind.QuickFix, CodeActionKind.RefactorInline,
                        CodeActionKind.RefactorRewrite, CodeActionKind.SourceOrganizeImports
                    }
                },
                CodeLensProvider = new CodeLensOptions()
                {
                    ResolveProvider = true
                },
                FoldingRangeProvider = new FoldingRangeOptions
                {
                    WorkDoneProgress = false
                },
                DocumentFormattingProvider = new DocumentFormattingOptions(),
                DocumentRangeFormattingProvider = new DocumentRangeFormattingOptions(),
                CompletionProvider = new CompletionOptions { ResolveProvider = false, TriggerCharacters = _languages.Values.SelectMany(grammar => grammar.CompletionTriggerCharacters()).Distinct().ToArray() },
                HoverProvider = true,
            };
            UpdateTraceSource(trace);
            
            _ = SendLogMessage(MessageType.Info, "LSP Server initialization completed.");
            return new InitializeResult { Capabilities = serverCapabilities };
        }

        /// <inheritdoc/>
        public void Initialized() 
        {            
            RegisterCapabilitiesOnInitialized();
        }

        /// <inheritdoc/>
        public void Shutdown() { }

        /// <inheritdoc/>
        public void DidChange(JToken arg)
        {
            var changes = arg.ToObject<DidChangeTextDocumentParams>();

            if (changes.ContentChanges == null) return;
            if (_documents.TryGetValue(changes.TextDocument.Uri, out var document))
            {
                document.Update(changes.ContentChanges.Select(AsTextEdit));
                SendDiagnosticsAsync(changes.TextDocument.Uri, document.Context);
                _ = SendLogMessage(MessageType.Info, $"Document {changes.TextDocument.Uri} updated."); 
            }
        }

        /// <inheritdoc/>
        public void DidSave(TextDocumentIdentifier textDocument, string text)
        {
            _ = SendLogMessage(MessageType.Info, $"Document {textDocument.Uri} saved.");
        }

        private static ParsePosition AsParsePosition(Position position) => new ParsePosition((int)position.Line, (int)position.Character);

        private static Position AsPosition(ParsePosition parsePosition) => new Position((uint)parsePosition.Line, (uint)parsePosition.Col);

        private static LspTypes.Range AsRange(ParseRange parseRange) => new LspTypes.Range() { Start = AsPosition(parseRange.Start), End = AsPosition(parseRange.End) };

        private static TextEdit AsTextEdit(TextDocumentContentChangeEvent change)
        {
            var lines = change.Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            return new TextEdit(AsParsePosition(change.Range.Start), AsParsePosition(change.Range.End), lines);
        }

        /// <inheritdoc/>
        public void DidClose(JToken arg)
        {
            var closeParams = arg.ToObject<DidCloseTextDocumentParams>();
            if (_documents.Remove(closeParams.TextDocument.Uri))
            {
                _ = SendLogMessage(MessageType.Info, $"Document {closeParams.TextDocument.Uri} closed.");
            }
        }

        /// <inheritdoc/>
        public void DidOpen(JToken arg)
        {
            var openParams = arg.ToObject<DidOpenTextDocumentParams>();

            var uri = new Uri(Uri.UnescapeDataString(openParams.TextDocument.Uri), UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri && uri.IsFile)
            {
                if (_languages.TryGetValue(openParams.TextDocument.LanguageId, out var language))
                {
                    var parser = language.CreateParser();
                    parser.Initialize(File.ReadAllLines(uri.AbsolutePath));
                    _documents[openParams.TextDocument.Uri] = parser;
                    SendDiagnosticsAsync(openParams.TextDocument.Uri, parser.Context);
                    _ = SendLogMessage(MessageType.Info, $"Document {openParams.TextDocument.Uri} opened with language {openParams.TextDocument.LanguageId}.");
                }
                else
                {
                    var errorMessage = $"No grammar found for extension {openParams.TextDocument.LanguageId}";
                    _ = SendLogMessage(MessageType.Error, errorMessage);
                    throw new NotSupportedException(errorMessage);
                }
            }
            else
            {
                var errorMessage = $"Cannot open URI {openParams.TextDocument.Uri}";
                _ = SendLogMessage(MessageType.Error, errorMessage);
                throw new NotSupportedException(errorMessage);
            }
        }

        /// <inheritdoc/>
        public void Exit()
        {
            _ = SendLogMessage(MessageType.Info, "LSP Server exiting.");
        }
        
        /// <summary>
        /// Sends a log message to the client.
        /// </summary>
        /// <param name="type">The type of the message (Info, Warning, Error).</param>
        /// <param name="message">The message content.</param>
        protected internal Task SendLogMessage(MessageType type, string message)
        {
            var logMessageParams = new LogMessageParams
            {
                MessageType = ConvertMessageType(type),
                Message = message
            };
            
            return _rpc.NotifyWithParameterObjectAsync(Methods.WindowLogMessageName, logMessageParams);
        }

        private static LspTypes.MessageType ConvertMessageType(MessageType type)
        {
            switch (type)
            {
                case MessageType.Error: return LspTypes.MessageType.Error;
                case MessageType.Warning: return LspTypes.MessageType.Warning;
                case MessageType.Log: return LspTypes.MessageType.Log;
                default: return LspTypes.MessageType.Info;
            }
        }
    }
}