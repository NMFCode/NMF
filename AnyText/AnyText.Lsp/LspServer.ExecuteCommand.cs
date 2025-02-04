using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using LspTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NMF.AnyText.Rules;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        private Dictionary<string, RuleApplication> ActionRuleApplications => _codeActionRuleApplications.Concat(_codeLensRuleApplications).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        /// <inheritdoc cref="ILspServer.ExecuteCommand" />
        public void ExecuteCommand(JToken arg)
        {
            var request = arg.ToObject<ExecuteCommandParams>();
            ExecuteCommand(request.Command, request.Arguments);
        }

        private void ExecuteCommand(string commandIdentifier, object[] args)
        {
            if (!_languages.TryGetValue(_currentLanguageId, out var language))
            {
                SendLogMessage(MessageType.Error, $"{_currentLanguageId} Language not found");
                return;
            }

            var actions = language.GetExecutableActions();

            if (!actions.TryGetValue(commandIdentifier, out var action))
            {
                SendLogMessage(MessageType.Error, $"{commandIdentifier} Command not supported");
                return;
            }

            var uri = args[0].ToString();
            if (!_documents.TryGetValue(uri!, out var document))
            {
                SendLogMessage(MessageType.Error, $"{commandIdentifier} no ParseContext found for URI {uri}");
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
            
            if (!ActionRuleApplications.TryGetValue(args[1].ToString()!, out var actionRuleApplication))
            {
                SendLogMessage(MessageType.Error, $"{commandIdentifier} no RuleApplication found for this Action");
                return;
            }

            var elem = actionRuleApplication.ContextElement.GetType();
            var executeCommandArguments = new ExecuteCommandArguments

            {
                RuleApplication = actionRuleApplication,
                Context = document.Context,
                DocumentUri = uri,
                OtherOptions = dict
            };
            action.Invoke(executeCommandArguments);
        }

        private Registration CreateExecuteCommandRegistration(string languageId)
        {
            _languages.TryGetValue(languageId, out var language);
            if (language == null) return null;


            var registrationOptions = new ExecuteCommandRegistrationOptions
            {
                Commands = language.GetExecutableActions().Keys.ToArray()
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