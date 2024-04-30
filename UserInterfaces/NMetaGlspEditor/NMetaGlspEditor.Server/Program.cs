using Microsoft.AspNetCore.WebSockets;
using NMetaEditor.Language;
using NMF.Models.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebSockets(opts => { });
builder.Services.AddGlspServer();
builder.Services.AddLanguage<NMetaLanguage>();

builder.Services.AddControllers()
    .AddJsonOptions(json => json.JsonSerializerOptions.Converters.Add(new ShallowModelElementConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseStaticFiles();

app.UseWebSockets();
app.MapGlspWebSocketServer("/glsp");
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
