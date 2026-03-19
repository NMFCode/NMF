using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using LspTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NMF.AnyText.Rules;
using NMF.Utilities;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Range = LspTypes.Range;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        private readonly Dictionary<string, (Uri uri, RuleApplication ruleApplication)> _codeLensRuleApplications = new();
        
        /// <inheritdoc cref="ILspServer.CodeLens" />
        public CodeLens[] CodeLens(JToken arg)
        {
            var request = arg.ToObject<CodeLensParams>();

            if (!_documents.TryGetValue(request.TextDocument.Uri, out var document))
                return Array.Empty<CodeLens>();

            var uri = document.Context.FileUri;
            var codeLenses = new List<CodeLens>();
            lock (_codeLensRuleApplications)
            {
                var assigned = _codeLensRuleApplications.Where(kv => kv.Value.uri == uri).ToDictionary(kv => kv.Value.ruleApplication, kv => kv.Key);
                var used = new HashSet<string>();
                foreach (var r in GetCodeLenses(document))
                {
                    var codeLens = r.CodeLens;
                    if (!assigned.TryGetValue(r.RuleApplication, out var guid))
                    {
                        guid = Guid.NewGuid().ToString();
                        _codeLensRuleApplications.Add(guid, (uri, r.RuleApplication));
                        assigned.Add(r.RuleApplication, guid);
                    }
                    else
                    {
                        used.Add(guid);
                    }
                    var arguments = new object[] { request.TextDocument.Uri, guid };
                    var end = r.RuleApplication.CurrentPosition + r.RuleApplication.Length;
                    codeLenses.Add(new CodeLens
                    {
                        Command = new Command()
                        {
                            Title = codeLens.GetTitleForRuleApplication(r.RuleApplication, document.Context),
                            CommandIdentifier = codeLens.CommandIdentifier,
                            Arguments = r.Arguments != null
                                ? arguments.Concat(r.Arguments.Cast<object>()).ToArray()
                                : arguments.ToArray()
                        },
                        Data = codeLens.Data,
                        Range = new Range()
                        {
                            Start = new Position((uint)r.RuleApplication.CurrentPosition.Line, (uint)r.RuleApplication.CurrentPosition.Col),
                            End = new Position((uint)end.Line, (uint)end.Col)
                        }
                    });
                }
                foreach (var item in assigned.Values)
                {
                    if (!used.Contains(item))
                    {
                        _codeLensRuleApplications.Remove(item);
                    }
                }
            }
            return CodeLensesForDocument(document, codeLenses).ToArray();
        }

        private ICollection<CodeLensApplication> GetCodeLenses(Parser document)
        {
            _readWriteLock.EnterReadLock();
            try
            {
                return document.Context.RootRuleApplication.CodeLenses();
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        /// <summary>
        /// Calculates the code lenses for the given document
        /// </summary>
        /// <param name="codeLenses">the code lenses obtained from the document</param>
        /// <param name="document">the document for which to calculate code lenses</param>
        /// <returns>A collection of code lenses</returns>
        protected virtual IEnumerable<CodeLens> CodeLensesForDocument(Parser document, IEnumerable<CodeLens> codeLenses)
        {
            return codeLenses;
        }

        /// <inheritdoc cref="ILspServer.CodeLensResolve" />
        public CodeLens CodeLensResolve(JToken arg)
        {
            var request = arg.ToObject<CodeLens>();
            
            ExecuteCommand(request.Command.CommandIdentifier, request.Command.Arguments);
            
            return request;
        }
    }
}
