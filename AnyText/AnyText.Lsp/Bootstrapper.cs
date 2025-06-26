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
        /// <param name="synchronizations">An optional array of model synchronizations to support model transformations.</param>
        /// <returns>A task representing the running server</returns>
        public static async Task RunLspServerOnStandardInStandardOutAsync(Grammar[] grammars, ModelSynchronization[] synchronizations = null)
        {
            using var rpc = AnyTextJsonRpcServerUtil.CreateServer(FullDuplexStream.Splice(Console.OpenStandardInput(), Console.OpenStandardOutput()));
            ILspServer lspServer = new LspServer(rpc, grammars, synchronizations);

            AnyTextJsonRpcServerUtil.AddLocalRpcTarget(rpc, lspServer);

            rpc.StartListening();
            await rpc.Completion;
        }
    }
}
