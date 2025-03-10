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
        
        private async Task SendDiagnosticsAsync(string uri, ParseContext context)
        {
            var diagnostics = new List<Diagnostic>();
            var errors = context.Errors;
            foreach (var error in errors.Where(e => e.Message != null))
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
                Uri = uri,
                Diagnostics = diagnostics.ToArray(),
            };

            try
            {
                await _rpc.NotifyWithParameterObjectAsync(Methods.TextDocumentPublishDiagnosticsName, diagnosticsParams);
                await SendLogMessage(MessageType.Info, $"Diagnostics published successfully for URI: {uri} with {diagnostics.Count} issue(s).");
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error publishing diagnostics for URI: {uri}. Exception: {ex.Message}";
                await SendLogMessage(MessageType.Error, errorMessage);
            }
        }
    }
}