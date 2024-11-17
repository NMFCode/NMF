using LspTypes;
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
    public partial class LspServer : ILspServer
    {
        private readonly JsonRpc _rpc;
        private readonly Dictionary<string, Parser> _documents = new Dictionary<string, Parser>();
        private readonly Dictionary<string, Grammar> _languages;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="grammars">A collection of grammars</param>
        public LspServer(JsonRpc rpc, params Grammar[] grammars)
            : this((IEnumerable<Grammar>)grammars)
        {
            _rpc = rpc;
        }
        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="grammars">A collection of grammars</param>
        public LspServer(IEnumerable<Grammar> grammars)
        {
            _languages = grammars?.ToDictionary(sp => sp.LanguageId);
        }

        [JsonRpcMethod(Methods.InitializeName)]
        public InitializeResult Initialize(
            int? processId
            , _InitializeParams_ClientInfo clientInfo
            , string locale
            , string rootPath
            , Uri rootUri
            , ClientCapabilities capabilities
            , TraceValue trace
            , WorkspaceFolder[] workspaceFolders
            , object InitializationOptions = null)
        {
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
                }
            };
            return new InitializeResult { Capabilities = serverCapabilities };
        }
        public void Initialized() { }

        public void Shutdown() { }

        public void DidChange(JToken arg)
        {
            var changes = arg.ToObject<DidChangeTextDocumentParams>();

            if (changes.ContentChanges == null) return;
            if (_documents.TryGetValue(changes.TextDocument.Uri, out var document))
            {
                document.Update(changes.ContentChanges.Select(AsTextEdit));
                SendDiagnostics(changes.TextDocument.Uri, document.Context);
            }
        }

        public void DidSave(TextDocumentIdentifier textDocument, string text)
        {
        }

        private static ParsePosition AsParsePosition(Position position) => new ParsePosition((int)position.Line, (int)position.Character);

        private static TextEdit AsTextEdit(TextDocumentContentChangeEvent change) => new TextEdit(AsParsePosition(change.Range.Start), AsParsePosition(change.Range.End), new[] { change.Text });

        public void DidClose(DidCloseTextDocumentParams closeParams)
        {
            if (_documents.TryGetValue(closeParams.TextDocument.Uri, out var document))
            {
                _documents.Remove(closeParams.TextDocument.Uri);
            }
        }

        public void DidOpen(JToken arg)
        {
            var openParams = arg.ToObject<DidOpenTextDocumentParams>();

            var uri = new Uri(openParams.TextDocument.Uri, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri && uri.IsFile)
            {
                if (_languages.TryGetValue(openParams.TextDocument.LanguageId, out var language))
                {
                    var parser = language.CreateParser();
                    parser.Initialize(File.ReadAllLines(uri.AbsolutePath));
                    _documents.Add(openParams.TextDocument.Uri, parser);

                    RegisterCapabilitiesOnOpen(openParams.TextDocument.LanguageId, parser);
                    SendDiagnostics(openParams.TextDocument.Uri, parser.Context);
                }
                else
                {
                    throw new NotSupportedException($"No grammar found for extension {openParams.TextDocument.LanguageId}");
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

        public void Exit()
        {
            throw new NotImplementedException();
        }
    }
}
