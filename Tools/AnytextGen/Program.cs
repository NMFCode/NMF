using CommandLine;
using NMF.AnyTextGen.Verbs;

Environment.ExitCode = Parser.Default.ParseArguments(args,
    typeof(GenerateMetamodelVerb),
    typeof(GenerateParserVerb))
    .MapResult((VerbBase verb) => verb.Execute(), _ => 2);