using Nerdbank.Streams;
using NMF.AnyText;
using NMF.AnyText.AnyMeta;
using NMF.AnyText.Grammars;
using System.Diagnostics;

#if DEBUG
if (args.Length == 1 && args[0] == "debug")
{
    Debugger.Launch();
}
#endif
var anyTextGrammar = new AnyTextGrammar();
var anyMetaGrammar = new AnyMetaGrammar();

Grammar[] grammars = [anyTextGrammar, anyMetaGrammar];

await Bootstrapper.RunLspServerOnStandardInStandardOutAsync(grammars);
