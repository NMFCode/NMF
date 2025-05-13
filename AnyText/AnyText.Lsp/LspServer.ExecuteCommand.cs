using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using LspTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NMF.AnyText.Grammars;
using NMF.AnyText.Rules;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        private readonly Dictionary<string, Grammar> _codeActions = new Dictionary<string, Grammar>();

        /// <inheritdoc cref="ILspServer.ExecuteCommand" />
        public void ExecuteCommand(JToken arg)
        {
            var request = arg.ToObject<ExecuteCommandParams>();
            ExecuteCommand(request.Command, request.Arguments);
        }

        private void ExecuteCommand(string commandIdentifier, object[] args)
        {
            if (!_codeActions.TryGetValue(commandIdentifier, out var language))
            {
                _ = SendLogMessageAsync(MessageType.Error, $"Command {commandIdentifier} not found");
                return;
            }

            var actions = language.ExecutableActions;

            if (!actions.TryGetValue(commandIdentifier, out var action))
            {
                _ = SendLogMessageAsync(MessageType.Error, $"{commandIdentifier} Command not supported");
                return;
            }

            var uri = args[0].ToString();
            if (!_documents.TryGetValue(uri!, out var document))
            {
                _ = SendLogMessageAsync(MessageType.Error, $"{commandIdentifier} no ParseContext found for URI {uri}");
                return;
            }

            Dictionary<string, object> dict = null;
            if (args.Length > 2 && args[2] != null)
            {
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(args[2].ToString()!);
                dict = new Dictionary<string, object>
                {
                    { jsonObject.Key.ToString(), jsonObject.Value }
                };
            }

            if (!FindRuleApplication(args, out var actionRuleApplication))
            {
                _ = SendLogMessageAsync(MessageType.Error, $"{commandIdentifier} no RuleApplication found for this Action");
                return;
            }

            var executeCommandArguments = new LspCommandArguments(this)
            {
                RuleApplication = actionRuleApplication,
                Context = document.Context,
                DocumentUri = uri,
                OtherOptions = dict
            };
            action.Invoke(executeCommandArguments);
        }

        private bool FindRuleApplication(object[] args, out RuleApplication actionRuleApplication)
        {
            var uid = args[1].ToString();
            return _codeActionRuleApplications.TryGetValue(uid, out actionRuleApplication)
                || _codeLensRuleApplications.TryGetValue(uid, out actionRuleApplication);
        }

        private Registration CreateExecuteCommandRegistration(Grammar language)
        {
            var registrationOptions = new ExecuteCommandRegistrationOptions
            {
                Commands = language.ExecutableActions.Keys.ToArray()
            };
            return new Registration
            {
                RegisterOptions = registrationOptions,
                Id = Guid.NewGuid().ToString(),
                Method = Methods.WorkspaceExecuteCommandName
            };
        }
    }
}