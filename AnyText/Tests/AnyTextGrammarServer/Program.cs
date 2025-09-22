using System.Diagnostics;
using AnyText.Tests.Synchronization;
using AnyText.Tests.Synchronization.Grammar;
using AnyText.Tests.Synchronization.Metamodel.PetriNet;
using AnyText.Tests.Synchronization.Metamodel.StateMachine;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.WebSockets;
using NMetaEditor.Language;
using NMF.AnyText;
using NMF.AnyText.AnyMeta;
using NMF.AnyText.Grammars;
using NMF.Synchronizations;
using NMF.Transformations;

#if DEBUG
if (args.Length == 1 && args[0] == "debug")
{
    Debugger.Launch();
}
#endif
/*var anyTextGrammar = new AnyTextGrammar();
var anyMetaGrammar = new AnyMetaGrammar();
var statemachineGrammar = new StateMachineGrammar();
var petrinetGrammar = new PetriNetGrammar();
Grammar[] grammars = [anyTextGrammar, anyMetaGrammar, statemachineGrammar, petrinetGrammar];
await Bootstrapper.RunLspServerOnStandardInStandardOutAsync(grammars, synchronizations);*/
var builder = WebApplication.CreateBuilder(args);


builder.WebHost.ConfigureKestrel(kestrel => kestrel.AllowSynchronousIO = true);

// Add services to the container.
builder.Services.AddWebSockets(opts => { });
builder.Services.AddLspServer();
builder.Services.AddLanguage<AnyTextGrammar>();
builder.Services.AddLanguage<AnyMetaGrammar>();
builder.Services.AddLanguage<StateMachineGrammar>();
builder.Services.AddLanguage<PetriNetGrammar>();

builder.Services.AddSingleton<ModelSynchronization>(sp =>
{
    var statemachineGrammar = sp.GetRequiredService<StateMachineGrammar>();
    var petrinetGrammar = sp.GetRequiredService<PetriNetGrammar>();

    return new ModelSynchronization<IStateMachine, IPetriNet, FSM2PN, FSM2PN.AutomataToNet>(
        statemachineGrammar,
        petrinetGrammar,
        SynchronizationDirection.LeftWins,
        ChangePropagationMode.TwoWay,
        (smContext, pnContext) =>
        {
            var smFileName = Path.GetFileNameWithoutExtension(smContext.FileUri.LocalPath);
            var pnFileName = Path.GetFileNameWithoutExtension(pnContext.FileUri.LocalPath);
            return smFileName.Equals(pnFileName, StringComparison.OrdinalIgnoreCase);
        }, false);
});

builder.Services.AddGlspServer();
builder.Services.AddLanguage<NMetaLanguage>();

var app = builder.Build();

app.UseWebSockets();
app.MapLspWebSocketServer("/lsp");
app.MapGlspWebSocketServer("/glsp");
await app.StartAsync();

var server = app.Services.GetRequiredService<IServer>();
var addressFeature = server.Features.Get<IServerAddressesFeature>();


Console.WriteLine($"[AnyText-Extension-Server]:Startup completed on {addressFeature!.Addresses.First()}");

await app.WaitForShutdownAsync();