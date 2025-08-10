using Glsp.Test;
using Microsoft.AspNetCore.WebSockets;
using NMF.Glsp.Server.Websockets;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebSockets(configure =>
{
});
builder.Services.AddGlspServer();
builder.Services.AddLanguage<FsmLanguage>();
builder.Services.AddLanguage<PetriNetLanguage>();

var app = builder.Build();

app.UseWebSockets();
app.MapGlspWebSocketServer("/glsp");

await app.RunAsync();