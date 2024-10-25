using LspTypes;
using NMF.AnyText.Grammars;
using NMF.Models.Services;
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
    public class LspServer : ILspServer
    {
        private readonly Dictionary<string, Parser> _documents = new Dictionary<string, Parser>();
        private readonly Dictionary<string, Grammar> _languages;

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="grammars">A collection of grammars</param>
        public LspServer(params Grammar[] grammars)
            : this((IEnumerable<Grammar>)grammars)
        {
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="grammars">A collection of grammars</param>
        public LspServer(IEnumerable<Grammar> grammars)
        {
            _languages = grammars?.ToDictionary(sp => sp.LanguageId);
        }

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

        public SemanticTokens QuerySemanticTokens(SemanticTokensParams tokenParams)
        {
            return null;
        }

        public void Initialized() { }

        public void Shutdown() { }

        public void DidChange(DidChangeTextDocumentParams changes)
        {
            if (changes.ContentChanges == null) return;
            if (_documents.TryGetValue(changes.TextDocument.Uri, out var document))
            {
                document.Update(changes.ContentChanges.Select(AsTextEdit));
                SendDiagnostics(changes.TextDocument.Uri, document.Context);
            }
        }

        public void DidSave(DidSaveTextDocumentParams saveParams)
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

        public void DidOpen(DidOpenTextDocumentParams openParams)
        {
            var uri = new Uri(openParams.TextDocument.Uri, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri && uri.IsFile)
            {
                if (_languages.TryGetValue(openParams.TextDocument.LanguageId, out var language))
                {
                    var parser = language.CreateParser();
                    parser.Initialize(File.ReadAllLines(uri.AbsolutePath));
                    _documents.Add(openParams.TextDocument.Uri, parser);
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
