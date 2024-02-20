using Microsoft.AspNetCore.WebSockets;
using NMetaEditor.Language;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebSockets(configure =>
{
});
builder.Services.AddGlspServer();
builder.Services.AddLanguage<NMetaLanguage>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.UseWebSockets();
app.MapGlspWebSocketServer("/glsp");

app.Run();
