using Microsoft.AspNetCore.WebSockets;
using NMetaEditor.Language;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebSockets(configure =>
{
});
builder.Services.AddGlspServer();
builder.Services.AddLanguage<NMetaLanguage>();

var app = builder.Build();

app.UseWebSockets();
app.MapGlspWebSocketServer("/glsp");

await app.RunAsync();
