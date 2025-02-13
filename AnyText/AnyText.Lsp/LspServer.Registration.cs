using LspTypes;
using System;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <summary>
        ///     Registers Server Capabilities with the client after initialize.
        /// </summary>
        private void RegisterCapabilitiesOnInitialized()
        {
            foreach (var language in _languages.Values)
            {
                var semanticRegistration = CreateSemanticTokenRegistration(language);
                var executeRegistration = CreateExecuteCommandRegistration(language);
                RegisterCapabilities(new[] { semanticRegistration, executeRegistration });
            }
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