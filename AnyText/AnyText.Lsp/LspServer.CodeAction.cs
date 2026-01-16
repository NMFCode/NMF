using System;
using System.Collections.Generic;
using System.Linq;
using LspTypes;
using Newtonsoft.Json.Linq;
using NMF.AnyText.Rules;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        private readonly Dictionary<string, RuleApplication> _codeActionRuleApplications = new();
        /// <inheritdoc cref="ILspServer.CodeAction" />
        public CodeAction[] CodeAction(JToken arg)
        {
            var request = arg.ToObject<CodeActionParams>();

            if (!_documents.TryGetValue(request.TextDocument.Uri, out var document))
                return Array.Empty<CodeAction>();

            var codeActions = new List<CodeAction>();
            _codeActionRuleApplications.Clear();
            
            var codeActionCapabilities = _clientCapabilities?.TextDocument?.CodeAction;
            var supportsIsPreferred = codeActionCapabilities?.IsPreferredSupport ?? false;

            var documentUri = request.TextDocument.Uri;
            var diagnostics = request.Context.Diagnostics;
            var kindFilter = request.Context.Only;

            var startPosition = AsParsePosition(request.Range.Start);
            var endPosition = AsParsePosition(request.Range.End);

            var actions = document.GetCodeActionInfo(startPosition, endPosition);

            foreach (var actionApplication in actions)
            {
                var action = actionApplication.Action;
                var guid = Guid.NewGuid().ToString();
                _codeActionRuleApplications.TryAdd(guid, actionApplication.RuleApplication);
                
                var diagnosticIdentifier = action.DiagnosticIdentifier;
                var relevantDiagnostics = string.IsNullOrEmpty(diagnosticIdentifier) ? Array.Empty<Diagnostic>() : diagnostics
                    .Where(d => d.Message.Contains(diagnosticIdentifier))
                    .ToArray();
                
                if (!string.IsNullOrEmpty(diagnosticIdentifier) && relevantDiagnostics.Length == 0)
                    continue;
                
                var actionKind = !string.IsNullOrEmpty(action.Kind) ? ParseLspCodeActionKind(action.Kind) : null;
                if (kindFilter != null && kindFilter.Any() && actionKind != null &&
                    !kindFilter.Contains(actionKind.Value)) continue;
                
                var workspaceEdit = action.CreateWorkspaceEdit(new LspCommandArguments(this)
                {
                    RuleApplication = actionApplication.RuleApplication,
                    Context = document.Context,
                    DocumentUri = documentUri,
                    OtherOptions = action.Arguments
                });
                var workspaceFolder = _workspaceFolders.FirstOrDefault()?.Uri;

                var arguments = new object[] { documentUri, guid };
                codeActions.Add(new CodeAction
                {
                    Title = action.Title,
                    Kind = actionKind,
                    Diagnostics = relevantDiagnostics.Length == 0 ? null : relevantDiagnostics,
                    Edit = workspaceEdit != null ? LspTypesMapper.MapWorkspaceEdit(workspaceEdit, workspaceFolder) : null,
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
    }
}