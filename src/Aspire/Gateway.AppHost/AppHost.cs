using Gateway.Shared.Constants;
using Gateway.Shared.Constants.Database;

const string DEFAULT_HEALTH_CHECK_ENDPOINT = "/health";

var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder
    .AddMongoDB(
        MongoDbConfiguration.DEPENDENCY_NAME,
        MongoDbConfiguration.DB_PORT)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase(MongoDbConfiguration.DB_NAME);

var gateway = builder
    .AddProject<Projects.Gateway>(ProjectNames.GATEWAY)
    .WithHttpHealthCheck(DEFAULT_HEALTH_CHECK_ENDPOINT)
    .WithReference(mongo)
    .WaitFor(mongo)
    .WithOtlpExporter();

builder.Build().Run();