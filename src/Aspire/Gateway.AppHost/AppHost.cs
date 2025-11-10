using Domain.Shared.Constants;

const string DEFAULT_HEALTH_CHECK_ENDPOINT = "/health";

var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder
    .AddMongoDB(
        MongoDbConfiguration.DEPENDENCY_NAME,
        MongoDbConfiguration.DB_PORT)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent)
    .AddDatabase(MongoDbConfiguration.DB_NAME);

var redis = builder
    .AddRedis(RedisConfiguration.DEPENDENCY_NAME)
    .WithDataVolume(isReadOnly: false)
    .WithPersistence(
        interval: TimeSpan.FromMinutes(2),
        keysChangedThreshold: 100);

var gateway = builder
    .AddProject<Projects.Gateway>(ProjectNames.GATEWAY)
    .WithHttpHealthCheck(DEFAULT_HEALTH_CHECK_ENDPOINT)
    .WithReference(mongo)
    .WithReference(redis)
    .WaitFor(mongo)
    .WaitFor(redis)
    .WithOtlpExporter();

builder.Build().Run();