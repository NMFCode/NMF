using LspTypes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <summary>
        /// Registers Server Capabilities with the client after initialize.
        /// </summary>
        private void RegisterCapabilitiesOnInitialized()
        {
            var registrations = new List<Registration>();
            foreach (var language in _languages.Values)
            {
                var semanticRegistration = CreateSemanticTokenRegistration(language);
                if (semanticRegistration != null)
                {
                    registrations.Add(semanticRegistration);
                }
                var executeRegistration = CreateExecuteCommandRegistration(language);
                if (executeRegistration != null)
                {
                    registrations.Add(executeRegistration);
                }
            }
            var systemCommands = CreateSystemCommandRegistration();
            if (systemCommands != null)
            {
                registrations.Add(systemCommands);
            }
            _ = RegisterCapabilitiesAsync(registrations.ToArray());
        }

        /// <summary>
        ///     Registers the given capabilities with the client.
        ///     This method is used to send registration requests to the client for various language server features.
        /// </summary>
        /// <param name="registrations">The capabilities (registrations) to be sent to the client.</param>
        private async Task RegisterCapabilitiesAsync(Registration[] registrations)
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
                await SendLogMessageAsync(MessageType.Error, errorMessage);
            }
        }
    }
}