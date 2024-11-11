using Nerdbank.Streams;
using NMF.AnyText;
using NMF.AnyText.Grammars;
using System.Diagnostics;

if (args.Length == 1 && args[0] == "debug")
{
    Debugger.Launch();
}

var grammar = new AnyTextGrammar();
var lspServer = new LspServer(grammar);
using (var rpc = AnyTextJsonRpcServerUtil.CreateServer(FullDuplexStream.Splice(Console.OpenStandardInput(), Console.OpenStandardOutput()), lspServer))
{
    rpc.StartListening();
    await rpc.Completion;
}