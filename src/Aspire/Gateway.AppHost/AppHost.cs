using Domain.Shared.Constants;

const string DEFAULT_HEALTH_CHECK_ENDPOINT = "/health";

var builder = DistributedApplication.CreateBuilder(args);

var mongo = builder
    .AddMongoDB(
        MongoDbConfiguration.DEPENDENCY_NAME,
        MongoDbConfiguration.DB_PORT)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var proxyDb = mongo
    .AddDatabase(MongoDbConfiguration.PROXY_DB);
var queues = mongo
    .AddDatabase(MongoDbConfiguration.QUEUES_DB);

var redis = builder
    .AddRedis(RedisConfiguration.DEPENDENCY_NAME)
    .WithDataVolume(isReadOnly: false)
    .WithPersistence(
        interval: TimeSpan.FromMinutes(2),
        keysChangedThreshold: 100);

var rabbitmq = builder
    .AddRabbitMQ(RabbitMQConfiguration.CONNECTION_NAME)
    .WithDataVolume(isReadOnly: false)
    .WithManagementPlugin();

var gateway = builder
    .AddProject<Projects.Gateway>(ProjectNames.GATEWAY)
    .WithHttpHealthCheck(DEFAULT_HEALTH_CHECK_ENDPOINT)
    .WithReference(proxyDb)
    .WithReference(queues)
    .WithReference(redis)
    .WaitFor(proxyDb)
    .WaitFor(queues)
    .WaitFor(redis)
    .WithOtlpExporter();

builder.Build().Run();