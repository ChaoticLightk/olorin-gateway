using Extensions.Endpoints;
using Messaging.Presentation;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Host.UseWolverine();

builder.AddServiceDefaults();

var app = builder.Build();

app.ConfigurePresentation();

app.MapOpenApi();

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

app.ConfigureScalar();

app.Run();
