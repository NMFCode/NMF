using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using LspTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NMF.AnyText
{
    public partial class LspServer
    {
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
            if (args.Length > 3 && args[3] != null)
            {
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(args[3].ToString()!);
                dict = new Dictionary<string, object>
                {
                    { jsonObject.Key.ToString(), jsonObject.Value }
                };
            }


            var executeCommandArguments = new ExecuteCommandArguments

            {
                Context = document.Context,
                DocumentUri = uri,
                Start = ParsePositionFromJson(args[1].ToString()),
                End = ParsePositionFromJson(args[2].ToString()),
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

        private static ParsePosition ParsePositionFromJson(string jsonString)
        {
            var jsonDocument = JsonDocument.Parse(jsonString);

            var line = jsonDocument.RootElement.GetProperty("Line").GetInt32();
            var col = jsonDocument.RootElement.GetProperty("Col").GetInt32();

            return new ParsePosition(line, col);
        }
    }
}