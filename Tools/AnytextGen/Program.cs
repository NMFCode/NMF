using CommandLine;
using NMF.AnyTextGen.Verbs;

Environment.ExitCode = Parser.Default.ParseArguments(args,
    typeof(GenerateMetamodelVerb),
    typeof(GenerateParserVerb),
    typeof(GenerateCodeVerb))
    .MapResult((VerbBase verb) => verb.Execute(), _ => 2);