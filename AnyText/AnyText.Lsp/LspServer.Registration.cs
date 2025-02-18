using LspTypes;
using System;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <summary>
        ///     Registers Server Capabilities with the client when a document is opened.
        /// </summary>
        /// <param name="languageId">The language identifier for the document.</param>
        /// <param name="parser">The parser used to retrieve grammar details for the language.</param>
        private void RegisterCapabilitiesOnOpen(string languageId, Parser parser)
        {
            var semanticRegistration = CreateSemanticTokenRegistration(languageId, parser);
            var executeRegistration = CreateExecuteCommandRegistration(languageId);
            RegisterCapabilities(new[] { semanticRegistration, executeRegistration });
        }

        /// <summary>
        ///     Registers the given capabilities with the client.
        ///     This method is used to send registration requests to the client for various language server features.
        /// </summary>
        /// <param name="registrations">The capabilities (registrations) to be sent to the client.</param>
        private async void RegisterCapabilities(Registration[] registrations)
        {
            var registrationParams = new RegistrationParams
            {
                Registrations = registrations
            };

            try
            {
                SendLogMessage(MessageType.Info, "Sending capabilities registration request to client.");
                await _rpc.InvokeWithParameterObjectAsync(Methods.ClientRegisterCapabilityName, registrationParams);
                SendLogMessage(MessageType.Info, "Capabilities registration request completed successfully.");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error registering capabilities: {ex.Message}";
                SendLogMessage(MessageType.Error, errorMessage);
                Console.Error.WriteLine(errorMessage);
            }
        }

        private async void UnregisterCapabilities(Unregistration[] unregistrations)
        {
            throw new NotImplementedException();
        }
    }
}