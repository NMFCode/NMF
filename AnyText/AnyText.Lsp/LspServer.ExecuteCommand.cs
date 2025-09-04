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
        private const string CreateModelCommand = "anytext.createModelSync";
        /// <summary>
        /// VS Code Extension Command to Synchronize Existing  Models
        /// </summary>
        public const string SyncModelCommand = "anytext.syncModel";
        
        private readonly Dictionary<string, Grammar> _codeActions = new Dictionary<string, Grammar>();

        /// <inheritdoc cref="ILspServer.ExecuteCommand" />
        public void ExecuteCommand(JToken arg)
        {
            var request = arg.ToObject<ExecuteCommandParams>();
            ExecuteCommand(request.Command, request.Arguments);
        }
      
        private void HandleExtensionCommand(string commandIdentifier, object[] args)
        {
            switch (commandIdentifier)
            {
                case CreateModelCommand:

                    var uri = args[0].ToString();
                    var uri2 = args[1].ToString();
                    var lang = args[2].ToString();
                    if (string.IsNullOrEmpty(uri) || string.IsNullOrEmpty(uri2) || string.IsNullOrEmpty(lang))
                    {
                        Console.WriteLine("Invalid arguments received.");
                        return;
                    }
                    _synchronizationService.ProcessModelGeneration(uri, uri2, _documents,_languages);
                    break;

                case SyncModelCommand:
                    uri = args[0].ToString();
                    if (_documents.TryGetValue(uri, out var document))
                    {
                        _synchronizationService.ProcessSync(document, _documents.Values, true);
                    }
                    break;

                default:
                    Console.WriteLine($"Unknown command: {commandIdentifier}");
                    break;
            }
        }

        private void ExecuteCommand(string commandIdentifier, object[] args)
        {
            if (commandIdentifier.StartsWith("anytext."))
            {
                HandleExtensionCommand(commandIdentifier, args);
                return;
            }
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