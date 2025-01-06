using Nerdbank.Streams;
using NMF.AnyText;
using NMF.AnyText.Grammars;
using System.Diagnostics;

if (args.Length == 1 && args[0] == "debug")
{
    Debugger.Launch();
}

var grammar = new AnyTextGrammar();
using (var rpc = AnyTextJsonRpcServerUtil.CreateServer(FullDuplexStream.Splice(Console.OpenStandardInput(), Console.OpenStandardOutput())))
{
    ILspServer lspServer = new LspServer(rpc, grammar);
    
    AnyTextJsonRpcServerUtil.AddLocalRpcTarget(rpc, lspServer);

    rpc.StartListening();
    await rpc.Completion;
}