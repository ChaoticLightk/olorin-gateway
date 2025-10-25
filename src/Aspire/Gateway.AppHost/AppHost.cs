using Gateway.Shared.Constants;
using Gateway.Shared.Constants.Database;
using Projects;

const string DEFAULT_HEALTH_CHECK_ENDPOINT = "/health";

var builder = DistributedApplication.CreateBuilder(args);

var optimisticApi = builder
    .AddProject<OptimisticApi>(ProjectNames.OPTIMISTICAPI)
    .WithHttpHealthCheck(DEFAULT_HEALTH_CHECK_ENDPOINT); 

var pessimisticApi = builder
    .AddProject<PessimisticApi>(ProjectNames.PESSIMISTICAPI)
    .WithHttpHealthCheck(DEFAULT_HEALTH_CHECK_ENDPOINT); 

builder.AddProject<BlazorApplication>(ProjectNames.WEBBLAZOR)
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck(DEFAULT_HEALTH_CHECK_ENDPOINT)
    .WithReference(optimisticApi)
    .WaitFor(pessimisticApi);

var mongo = builder
    .AddMongoDB(MongoDbConfiguration.DEPENDENCY_NAME, 27017)
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