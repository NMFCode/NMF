using Nerdbank.Streams;
using NMF.AnyText;
using NMF.AnyText.AnyMeta;
using NMF.AnyText.Grammars;
using System.Diagnostics;

if (args.Length == 1 && args[0] == "debug")
{
    //Debugger.Launch();
}

var anyTextGrammar = new AnyTextGrammar();
var anyMetaGrammar = new AnyMetaGrammar();
using (var rpc = AnyTextJsonRpcServerUtil.CreateServer(FullDuplexStream.Splice(Console.OpenStandardInput(), Console.OpenStandardOutput())))
{
    ILspServer lspServer = new LspServer(rpc, anyTextGrammar, anyMetaGrammar);
    
    AnyTextJsonRpcServerUtil.AddLocalRpcTarget(rpc, lspServer);

    rpc.StartListening();
    await rpc.Completion;
}