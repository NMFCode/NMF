using Glsp.Test;
using Microsoft.AspNetCore.WebSockets;
using NMF.Glsp.Language;
using NMF.Glsp.Server;
using NMF.Glsp.Server.Contracts;
using NMF.Models;
using NMF.Models.Repository;
using System.Globalization;
using System.Net;
using System.Net.Sockets;

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