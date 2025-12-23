using Shared.Constants;

const string DEFAULT_HEALTH_CHECK_ENDPOINT = "/health";

var builder = DistributedApplication
    .CreateBuilder(args);

var mongo = builder
    .AddMongoDB(
        MongoDbConfiguration.DEPENDENCY_NAME,
        MongoDbConfiguration.DB_PORT)
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var proxyDb = mongo
    .AddDatabase(MongoDbConfiguration.PROXY_DB);

var queuesDb = mongo
    .AddDatabase(MongoDbConfiguration.QUEUES_DB);

var redis = builder
    .AddRedis(RedisConfiguration.DEPENDENCY_NAME)
    .WithDataVolume(isReadOnly: false)
    .WithPersistence(
        interval: TimeSpan.FromMinutes(2),
        keysChangedThreshold: 100);

var userName = builder
    .AddParameter("rabbitmq-username", secret: true);

var password = builder
    .AddParameter("rabbitmq-password", secret: true);

var rabbitmq = builder
    .AddRabbitMQ(
        RabbitMQConfiguration.CONNECTION_NAME,
        userName, 
        password)
    .WithDataVolume(isReadOnly: false)
    .WithManagementPlugin();

var messaging = builder
    .AddProject<Projects.Messaging>(ProjectNames.MESSAGING)
    .WithHttpHealthCheck(DEFAULT_HEALTH_CHECK_ENDPOINT)
    .WithReference(rabbitmq)
    .WithReference(queuesDb)
    .WaitFor(queuesDb)
    .WaitFor(rabbitmq);

var gateway = builder
    .AddProject<Projects.Gateway>(ProjectNames.GATEWAY)
    .WithHttpHealthCheck(DEFAULT_HEALTH_CHECK_ENDPOINT)
    .WithReference(proxyDb)
    // .WithReference(redis)
    .WithOtlpExporter();

builder.Build().Run();