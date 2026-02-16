using LspTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Range = LspTypes.Range;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <summary>
        /// Updates diagnostics information for the given Uri
        /// </summary>
        /// <param name="context">the parse context with errors</param>
        /// <param name="uri">the uri of the document</param>
        /// <returns>a task that completes when sending diagnostics completed</returns>
        public async Task SendDiagnosticsAsync(ParseContext context, string uri)
        {
            var diagnostics = new List<Diagnostic>();
            var errors = context.Errors;
            foreach (var error in errors.Where(e => e.Message != null && e.Position.Line < context.Input.Length))
            {                
                var diagnostic = new Diagnostic()
                {
                    Severity = (LspTypes.DiagnosticSeverity)error.Severity,
                    Message = error.Message,
                    Source = error.Source,
                    Range = new Range()
                    {
                        Start = new Position((uint)error.Position.Line,(uint)error.Position.Col),
                        End = new Position((uint)(error.Position.Line+error.Length.Line),(uint)(error.Position.Col+error.Length.Col))
                    },
                };
                diagnostics.Add(diagnostic);                
            }
            var diagnosticsParams = new PublishDiagnosticParams()
            {
                Uri = uri ?? context.FileUri.ToString(),
                Diagnostics = diagnostics.ToArray(),
            };

            try
            {
                await _rpc.NotifyWithParameterObjectAsync(Methods.TextDocumentPublishDiagnosticsName, diagnosticsParams);
                await SendLogMessageAsync(MessageType.Info, $"Diagnostics published successfully for URI: {uri} with {diagnostics.Count} issue(s).");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error publishing diagnostics for URI: {uri}. Exception: {ex.Message}";
                await SendLogMessageAsync(MessageType.Error, errorMessage);
            }
        }
    }
}