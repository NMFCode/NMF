using Microsoft.AspNetCore.WebSockets;
using NMetaEditor.Language;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebSockets(opts => { });
builder.Services.AddGlspServer();
builder.Services.AddLanguage<NMetaLanguage>();


var app = builder.Build();

app.UseStaticFiles();

app.UseWebSockets();
app.MapGlspWebSocketServer("/glsp");

app.Run();
