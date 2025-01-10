using LspTypes;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <inheritdoc cref="ILspServer.ExecuteCommand" />
        public void ExecuteCommand(JToken arg)
        {
            var request = arg.ToObject<ExecuteCommandParams>();
            if (!_languages.TryGetValue(_currentLanguageId, out var language))
            {
                SendLogMessage(MessageType.Error, $"{_currentLanguageId} Language not found");
                return;
            }

            if (!language.ExecutableCodeActions.TryGetValue(request.Command, out var executableCodeAction))
            {
                SendLogMessage(MessageType.Error, $"{request.Command} Command not supported");
                return;
            }

            executableCodeAction.Invoke(request.Arguments);
        }

        private Registration CreateExecuteCommandRegistration(string languageId)
        {
            _languages.TryGetValue(languageId, out var language);
            if (language == null) return null;

            var registrationOptions = new ExecuteCommandRegistrationOptions
            {
                Commands = language.ExecutableCodeActions.Keys.ToArray()
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