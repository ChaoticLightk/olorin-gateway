using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Messaging.Domain.Entities.Queue.Mongo.Policy;
using Messaging.Domain.Exceptions.Messaging;
using Messaging.Domain.Shared.Messaging.DTO;
using Messaging.Domain.Shared.Messaging.Services;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Messaging.Infrastructure.Shared.Messaging.Services;

public class QueueService(
    IConnection rabbitmq, 
    ILogger<QueueService> logger)
    : IQueueService
{
    public async Task ProvisionAsync(
        QueuePolicyDocument policy,
        CancellationToken ct = default)
    {
        using var channel = await rabbitmq.CreateChannelAsync(cancellationToken: ct);

        var queueName = policy.QueueName;

        try
        {
            await channel.ExchangeDeclareAsync(
                $"{queueName}.exchange",
                ExchangeType.Topic,
                true, 
                cancellationToken: ct); 
            
            if (policy.EnabledDeadLetter)
            {
                await channel.ExchangeDeclareAsync(
                    exchange: $"{queueName}.dlx",
                    type: ExchangeType.Fanout,
                    durable: true,
                    cancellationToken: ct
                );
            }
    
            var arguments = new Dictionary<string, object?>();
    
            if (policy.EnabledDeadLetter)
            {
                arguments["x-dead-letter-exchange"] = $"{queueName}.dlx";
            }
    
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: arguments.Count > 0 ? arguments : null,
                cancellationToken: ct
            );
    
            await channel.QueueBindAsync(
                queue: queueName,
                exchange: $"{queueName}.exchange",
                routingKey: $"{queueName}.#",
                cancellationToken: ct
            );
    
            if (policy.EnabledDeadLetter)
            {
                var dlqName = $"{queueName}.dlq";
    
                await channel.QueueDeclareAsync(
                    queue: dlqName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    cancellationToken: ct
                );
    
                await channel.QueueBindAsync(
                    queue: dlqName,
                    exchange: $"{queueName}.dlx",
                    routingKey: string.Empty,
                    cancellationToken: ct);
            }
        }
        catch (Exception ex)
        {
            throw new ProvisionException(ex.Message, ex);
        }
    }

    public async Task PublishAsync(
        object? content,
        string routingKey,
        string exchange = "",
        MessagingMetadata? metadata = null,
        bool mandatory = false,
        CancellationToken ct = default)
    {
        string message = JsonSerializer.Serialize(content, JsonOptions);

        logger.LogPublishingRabbitMessage(message, routingKey, exchange);

        try
        {
            byte[] body = Encoding.UTF8.GetBytes(message);

            using var channel = await rabbitmq.CreateChannelAsync(cancellationToken: ct);
            
            var props = new BasicProperties();

            await channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: routingKey,
                mandatory: mandatory,
                basicProperties: props,
                body: body,
                cancellationToken: ct);
        }
        catch (Exception ex)
        {
            logger.LogErrorPublishingRabbitMessage(message, routingKey, exchange, ex);
            throw new PublishingException(ex.Message, ex);
        }
    }

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
    };
}
