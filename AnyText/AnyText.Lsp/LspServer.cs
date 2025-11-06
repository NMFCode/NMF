﻿using LspTypes;
using Newtonsoft.Json.Linq;
using NMF.AnyText.Grammars;
using NMF.AnyText.InlayClasses;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes the implementation of an LSP server
    /// </summary>
    public partial class LspServer : ILspServer
    {
        private JsonRpc _rpc;
        private readonly Dictionary<string, Parser> _documents = new Dictionary<string, Parser>();
        private readonly Dictionary<string, Grammar> _languages;
        private ClientCapabilities _clientCapabilities;
        private WorkspaceFolder[] _workspaceFolders;

        private Channel<DidChangeTextDocumentParams> _changesChannel;
        private Task _processTask;
        private CancellationTokenSource _cancellationSource;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="grammars">A collection of grammars</param>
        public LspServer(params Grammar[] grammars) : this((IEnumerable<Grammar>)grammars) { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="grammars">A collection of grammars</param>
        /// <param name="asyncChanges">true, if changes should be processed asynchronously, otherwise false</param>
        public LspServer(IEnumerable<Grammar> grammars, bool asyncChanges = false)
        {
            _languages = grammars?.ToDictionary(sp => sp.LanguageId);

            foreach (Grammar grammar in grammars)
            {
                grammar.Initialize();
                foreach (var codeAction in grammar.ExecutableActions)
                {
                    _codeActions[codeAction.Key] = grammar;
                }
            }

            if (asyncChanges)
            {
                _changesChannel = Channel.CreateUnbounded<DidChangeTextDocumentParams>(new UnboundedChannelOptions
                {
                    SingleReader = true,
                    SingleWriter = false
                });
                _cancellationSource = new CancellationTokenSource();
                _processTask = ProcessDidChangesAsync();
            }
        }

        private async Task ProcessDidChangesAsync()
        {
            while (!_cancellationSource.IsCancellationRequested)
            {
                try
                {
                    await foreach (var change in _changesChannel.Reader.ReadAllAsync(_cancellationSource.Token))
                    {
                        ProcessDidChange(change);
                    }
                }
                catch (OperationCanceledException)
                {
                    // intentionally left blank
                }
                catch (Exception ex)
                {
                    await Console.Error.WriteAsync(ex?.ToString());
                }
            }
        }

        /// <summary>
        /// Gets a collection of currently opened documents
        /// </summary>
        public ICollection<Parser> OpenDocuments => _documents.Values;

        /// <summary>
        /// Tries to fetch the given open document
        /// </summary>
        /// <param name="documentUri">the URI of the document</param>
        /// <param name="document">the document or null, if it was not found</param>
        /// <returns>true, if the document was found, otherwise false</returns>
        public bool TryGetOpenDocument(string documentUri, out Parser document)
        {
            return _documents.TryGetValue(documentUri, out document);
        }

        /// <summary>
        /// Gets the raw dictionary of open documents
        /// </summary>
        protected IDictionary<string, Parser> Documents => _documents;

        /// <summary>
        /// Gets the raw dictionary of grammars
        /// </summary>
        protected IDictionary<string, Grammar> Grammars => _languages;
        
        /// <inheritdoc/>
        public void SetRpc(JsonRpc rpc)
        {
            _rpc = rpc;
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
            var serverCapabilities = new ExtendedServerCapabilities
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
                InlayHintProvider = new InlayHintOptions { ResolveProvider = false }
            };
            UpdateTraceSource(trace);

            _ = SendLogMessageAsync(MessageType.Info, "LSP Server initialization completed.", true);
            return new InitializeResult { Capabilities = serverCapabilities };
        }

        /// <inheritdoc/>
        public void Initialized() 
        {            
            RegisterCapabilitiesOnInitialized();
        }

        /// <inheritdoc/>
        public void Shutdown()
        {
            foreach (var document in _documents.Values)
            {
                document.Context.Dispose();
            }
        }

        /// <inheritdoc/>
        public void DidChange(JToken arg)
        {
            var changes = arg.ToObject<DidChangeTextDocumentParams>();

            if (changes.ContentChanges == null) return;

            if (_changesChannel != null)
            {
                _changesChannel.Writer.TryWrite(changes);
            }
            else
            {
                ProcessDidChange(changes);
            }
        }

        private void ProcessDidChange(DidChangeTextDocumentParams changes)
        {
            if (_documents.TryGetValue(changes.TextDocument.Uri, out var document))
            {
                OnDocumentUpdate(document, changes.ContentChanges.Select(AsTextEdit), changes.TextDocument.Uri);
                _ = SendLogMessageAsync(MessageType.Log, string.Join(", ", changes.ContentChanges.Select(c => c.Text)));
            }
        }

        /// <summary>
        /// Gets called when a document should be updated
        /// </summary>
        /// <param name="document">the parsed document</param>
        /// <param name="edits">the edits that should be performed</param>
        /// <param name="uri">the URI of the document</param>
        protected virtual void OnDocumentUpdate(Parser document, IEnumerable<TextEdit> edits, string uri)
        {
            document.Update(edits);
            _ = SendDiagnosticsAsync(uri, document.Context);
            _ = SendLogMessageAsync(MessageType.Info, $"Document {uri} updated.");
        }

        /// <inheritdoc/>
        public void DidSave(TextDocumentIdentifier textDocument, string text)
        {
            _ = SendLogMessageAsync(MessageType.Info, $"Document {textDocument.Uri} saved.");
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
            if (_documents.TryGetValue(closeParams.TextDocument.Uri, out var document))
            { 
                _documents.Remove(closeParams.TextDocument.Uri);
                _ = SendLogMessageAsync(MessageType.Info, $"Document {closeParams.TextDocument.Uri} closed.");
            }
        }

        /// <summary>
        /// Gets called when a document is closed
        /// </summary>
        /// <param name="parser">the closed document</param>
        protected virtual void CloseDocument(Parser parser) { }

        /// <summary>
        /// Gets called when a new document is opened
        /// </summary>
        /// <param name="parser">the opened document</param>
        protected virtual void OpenNewDocument(Parser parser) { }

        /// <inheritdoc/>
        public void DidOpen(JToken arg)
        {
            var openParams = arg.ToObject<DidOpenTextDocumentParams>();

            var uri = new Uri(Uri.UnescapeDataString(openParams.TextDocument.Uri), UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri && uri.IsFile)
            {
                if (_languages.TryGetValue(openParams.TextDocument.LanguageId, out var language))
                {
                    if (!_documents.ContainsKey(openParams.TextDocument.Uri))
                    {
                        var parser = language.CreateParser();
                        parser.Initialize(uri);
                        _documents[openParams.TextDocument.Uri] = parser;
                        
                        OpenNewDocument(parser);
                        
                        _ = SendDiagnosticsAsync(openParams.TextDocument.Uri, parser.Context);
                        _ = SendLogMessageAsync(MessageType.Info,
                            $"Document {openParams.TextDocument.Uri} opened with language {openParams.TextDocument.LanguageId}.");
                        
                    }
                }
                else
                {
                    var errorMessage = $"No grammar found for extension {openParams.TextDocument.LanguageId}";
                    _ = SendLogMessageAsync(MessageType.Error, errorMessage);
                    throw new NotSupportedException(errorMessage);
                }
            }
            else
            {
                var errorMessage = $"Cannot open URI {openParams.TextDocument.Uri}";
                _ = SendLogMessageAsync(MessageType.Error, errorMessage);
                throw new NotSupportedException(errorMessage);
            }
        }

        /// <inheritdoc/>
        public void Exit()
        {
            _ = SendLogMessageAsync(MessageType.Info, "LSP Server exiting.");
        }
        
        /// <summary>
        /// Sends a log message to the client.
        /// </summary>
        /// <param name="type">The type of the message (Info, Warning, Error).</param>
        /// <param name="message">The message content.</param>
        /// <param name="always">Whether to always log this message even when not debugging. Warnings and errors are always logged regardless.</param>
        protected internal Task SendLogMessageAsync(MessageType type, string message, bool always = false)
        {
            if (!(Debugger.IsAttached || type is MessageType.Warning or MessageType.Error || always))
            {
                return Task.CompletedTask;
            }

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