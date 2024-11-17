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

            RegisterCapabilities(new[] { semanticRegistration });
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
                await _rpc.InvokeWithParameterObjectAsync(Methods.ClientRegisterCapabilityName, registrationParams);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error register capabilities: {ex.Message}");
            }
        }

        private async void UnregisterCapabilities(Unregistration[] unregistrations)
        {
            throw new NotImplementedException();
        }
    }
}