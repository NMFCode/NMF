using LspTypes;
using System;
using System.Threading.Tasks;

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
                _ = RegisterCapabilities(new[] { semanticRegistration, executeRegistration });
            }
        }

        /// <summary>
        ///     Registers the given capabilities with the client.
        ///     This method is used to send registration requests to the client for various language server features.
        /// </summary>
        /// <param name="registrations">The capabilities (registrations) to be sent to the client.</param>
        private async Task RegisterCapabilities(Registration[] registrations)
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
                var errorMessage = $"Error registering capabilities: {ex.Message}";
                await SendLogMessage(MessageType.Error, errorMessage);
            }
        }
    }
}