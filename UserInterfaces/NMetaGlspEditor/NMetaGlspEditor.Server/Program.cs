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

builder.Services.AddSelectionController();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseWebSockets();
app.MapGlspWebSocketServer("/glsp");
app.MapPropertyServiceWebSocket("/prop");
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();

