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
        private readonly Dictionary<string, RuleApplication> _codeLensRuleApplications = new();
        
        /// <inheritdoc cref="ILspServer.CodeLens" />
        public CodeLens[] CodeLens(JToken arg)
        {
            var request = arg.ToObject<CodeLensParams>();
            
            if (!_documents.TryGetValue(request.TextDocument.Uri, out var document))
                return Array.Empty<CodeLens>();

            _codeLensRuleApplications.Clear();
            
            var codeLensInfos = new List<CodeLensApplication>();
            document.Context.RootRuleApplication.AddCodeLenses(codeLensInfos);
            
            var codeLenses = codeLensInfos.Select(r =>
            {
                var codeLens = r.CodeLens;
                var guid = Guid.NewGuid().ToString();
                _codeLensRuleApplications.TryAdd(guid, r.RuleApplication);
                var arguments = new object[] { request.TextDocument.Uri, guid };
                var end = r.RuleApplication.CurrentPosition + r.RuleApplication.Length;
                return new CodeLens
                {
                    Command = new Command()
                    {
                        
                        Title = codeLens.Title,
                        CommandIdentifier = codeLens.CommandIdentifier,
                        Arguments = codeLens.Arguments != null
                            ? arguments.Concat(codeLens.Arguments.Cast<object>()).ToArray()
                            : arguments.ToArray()
                    },
                    Data = codeLens.Data,
                    Range = new Range()
                    {
                        Start = new Position((uint)r.RuleApplication.CurrentPosition.Line, (uint) r.RuleApplication.CurrentPosition.Col),
                        End = new Position((uint) end.Line, (uint) end.Col)
                    }
                };
            });
            return codeLenses.ToArray();
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
