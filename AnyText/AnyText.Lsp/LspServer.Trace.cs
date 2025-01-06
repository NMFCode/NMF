using LspTypes;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace NMF.AnyText
{
    public partial class LspServer
    {
        /// <inheritdoc cref="ILspServer.SetTrace"/>
        public void SetTrace(JToken arg)
        {
            var setTraceParams = arg.ToObject<SetTraceParams>();

            UpdateTraceSource(setTraceParams.Value);
            SendLogMessage(MessageType.Info, $"Trace level updated to: {setTraceParams.Value}");
        }

        private void UpdateTraceSource(TraceValue traceValue)
        {
            _rpc.TraceSource =
                AnyTextJsonRpcServerUtil.CreateTraceSource(MapTraceValueToSourceLevels(traceValue));
        }
        private static SourceLevels MapTraceValueToSourceLevels(TraceValue traceValue)
        {
            return traceValue switch
            {
                TraceValue.Off => SourceLevels.Off,
                TraceValue.Messages => SourceLevels.Information,
                TraceValue.Verbose => SourceLevels.Verbose,
                _ => SourceLevels.All
            };
        }
    }
}