using LspTypes;
using NMF.AnyText.Grammars;
using StreamJsonRpc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    internal class LspServer : IDisposable
    {
        private bool _disposedValue;
        private ManualResetEventSlim _disposedEvent;
        private JsonRpc _rpc;

        private readonly Dictionary<string, Parser> _documents = new Dictionary<string, Parser>();
        private readonly Dictionary<string, ReflectiveGrammar> _languages = new Dictionary<string, ReflectiveGrammar>();

        public LspServer(Stream sender, Stream receiver)
        {
            _rpc = new JsonRpc(sender, receiver, this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                _disposedValue = true;

                if (disposing)
                {
                    _rpc.Dispose();
                    _disposedEvent.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void WaitForExit(CancellationToken cancellationToken)
        {
            _disposedEvent.Wait(cancellationToken);
        }

        [JsonRpcMethod(Methods.ExitName)]
        public void Exit()
        {
            _disposedEvent.Set();
        }

        [JsonRpcMethod(Methods.InitializeName)]
        public InitializeResult Initialize(InitializeParams @params)
        {
            var capabilities = new ServerCapabilities
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
                SemanticTokensProvider = new SemanticTokensOptions
                {
                    Full = true,
                    Range = false,
                    Legend = new SemanticTokensLegend
                    {
                        tokenTypes = new[]
                        {
                            "keyword"
                        }
                    }
                },
                ReferencesProvider = new ReferenceOptions
                {
                    WorkDoneProgress = false
                }
            };
            return new InitializeResult { Capabilities = capabilities };
        }

        [JsonRpcMethod(Methods.TextDocumentSemanticTokensFull)]
        public SemanticTokens QuerySemanticTokens(SemanticTokensParams tokenParams)
        {
            return null;
        }

        [JsonRpcMethod(Methods.InitializedName)]
        public void Initialized() { }

        [JsonRpcMethod(Methods.ShutdownName)]
        public void Shutdown() { }

        [JsonRpcMethod(Methods.TextDocumentDidChangeName)]
        public void DidChange(DidChangeTextDocumentParams changes)
        {
            if (changes.ContentChanges == null) return;
            if (_documents.TryGetValue(changes.TextDocument.Uri, out var document))
            {
                document.Update(changes.ContentChanges.Select(AsTextEdit));
                SendDiagnostics(changes.TextDocument.Uri, document.Context);
            }
        }

        [JsonRpcMethod(Methods.TextDocumentDidSaveName)]
        public void DidSave(DidSaveTextDocumentParams saveParams)
        {
        }

        private static ParsePosition AsParsePosition(Position position) => new ParsePosition((int)position.Line, (int)position.Character);

        private static TextEdit AsTextEdit(TextDocumentContentChangeEvent change) => new TextEdit(AsParsePosition(change.Range.Start), AsParsePosition(change.Range.End), new[] { change.Text } );

        [JsonRpcMethod(Methods.TextDocumentDidCloseName)]
        public void DidClose(DidCloseTextDocumentParams closeParams)
        {
            if (_documents.TryGetValue(closeParams.TextDocument.Uri, out var document))
            {
                _documents.Remove(closeParams.TextDocument.Uri);
            }
        }

        [JsonRpcMethod(Methods.TextDocumentDidOpenName)]
        public void DidOpen(DidOpenTextDocumentParams openParams)
        {
            var uri = new Uri(openParams.TextDocument.Uri, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri && uri.IsFile)
            {
                var extension = Path.GetExtension(uri.AbsolutePath);
                if (_languages.TryGetValue(extension, out var language))
                {
                    var parser = language.CreateParser();
                    parser.Initialize(File.ReadAllLines(uri.AbsolutePath));
                    _documents.Add(openParams.TextDocument.Uri, parser);
                    SendDiagnostics(openParams.TextDocument.Uri, parser.Context);
                }
                else
                {
                    throw new NotSupportedException($"No grammar found for extension {extension}");
                }
            }
            else
            {
                throw new NotSupportedException($"Cannot open URI {openParams.TextDocument.Uri}");
            }
        }

        private void SendDiagnostics(string uri, ParseContext context)
        {

        }
    }
}
