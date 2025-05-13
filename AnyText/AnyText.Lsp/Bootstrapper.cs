using Nerdbank.Streams;
using NMF.AnyText.Grammars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.AnyText
{
    /// <summary>
    /// Denotes a helper class to simplify common operations
    /// </summary>
    public static class Bootstrapper
    {
        /// <summary>
        /// Runs an LSP server serving the provided grammars
        /// </summary>
        /// <param name="grammars">A collection of grammars to serve</param>
        /// <returns>A task representing the running server</returns>
        public static async Task RunLspServerOnStandardInStandardOutAsync(params Grammar[] grammars)
        {
            using (var rpc = AnyTextJsonRpcServerUtil.CreateServer(FullDuplexStream.Splice(Console.OpenStandardInput(), Console.OpenStandardOutput())))
            {
                ILspServer lspServer = new LspServer(rpc, grammars);

                AnyTextJsonRpcServerUtil.AddLocalRpcTarget(rpc, lspServer);

                rpc.StartListening();
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
                await rpc.Completion;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
            }
        }
    }
}
