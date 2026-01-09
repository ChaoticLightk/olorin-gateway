using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
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
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
        WriteIndented = true
    };

    public async Task PublishAsync(
        Message message,
        string exchange = "",
        MessagingMetadata? metadata = null,
        bool mandatory = false,
        CancellationToken ct = default)
    {
        await PublishAsync(
            message,
            message.Queue,
            exchange,
            metadata,
            mandatory,
            ct);
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
        await PublishAsync(message, routingKey, exchange, metadata, mandatory, ct);
    }

    public async Task PublishAsync(
        string message,
        string routingKey,
        string exchange = "",
        MessagingMetadata? metadata = null,
        bool mandatory = false,
        CancellationToken ct = default)
    {
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
}
