using Extensions.Endpoints;
using Messaging.Domain;
using Messaging.Infrastructure;
using Messaging.Presentation;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

// builder.Host.UseWolverine();

builder.AddServiceDefaults();

builder.AddInfrastructure();

builder.AddDomain();

var app = builder.Build();

app.ConfigurePresentation();

app.MapOpenApi();

app.UseHttpsRedirection();

app.MapDefaultEndpoints();

app.ConfigureScalar();

app.Run();
