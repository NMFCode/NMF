using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.WebSockets;
using NMetaEditor.Language;
using NMF.Models.Services.Forms;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(kestrel => kestrel.AllowSynchronousIO = true);

// Add services to the container.
builder.Services.AddWebSockets(opts => { });
builder.Services.AddGlspServer();
builder.Services.AddLanguage<NMetaLanguage>();
builder.Services.AddPropertyService();


var app = builder.Build();

app.UseWebSockets();
app.MapGlspWebSocketServer("/glsp");
app.MapPropertyServiceWebSocket("/prop");

var server = app.Services.GetRequiredService<IServer>();
var addressFeature = server.Features.Get<IServerAddressesFeature>();

await app.StartAsync();

Console.WriteLine($"[GLSP-Server]:Startup completed on {addressFeature!.Addresses.First()}");

await app.WaitForShutdownAsync();