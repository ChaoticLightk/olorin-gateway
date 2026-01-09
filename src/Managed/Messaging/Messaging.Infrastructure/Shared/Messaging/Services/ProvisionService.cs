using Messaging.Domain.Exceptions.Messaging;
using Messaging.Domain.Shared.Messaging.DTO;
using Messaging.Domain.Shared.Messaging.Services;
using RabbitMQ.Client;

namespace Messaging.Infrastructure.Shared.Messaging.Services;

public class ProvisionService(IConnection rabbitmq) : IProvisionService
{
    public async Task ProvisionAsync(ProvisionQueue provision, CancellationToken ct)
    {
        using var channel = await rabbitmq
            .CreateChannelAsync(cancellationToken: ct);

        var queueName = provision.RoutingKey;

        try
        {
            await channel.ExchangeDeclareAsync(
                $"{queueName}.exchange",
                ExchangeType.Topic,
                true, 
                cancellationToken: ct); 
            
            var arguments = new Dictionary<string, object?>();

            if (provision.EnabledDeadLetter)
            {
                await channel.ExchangeDeclareAsync(
                    exchange: $"{queueName}.dlx",
                    type: ExchangeType.Fanout,
                    durable: true,
                    cancellationToken: ct
                );

                arguments["x-dead-letter-exchange"] = $"{queueName}.dlx";

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
        }
        catch (Exception ex)
        {
            throw new ProvisionException(ex.Message, ex);
        }
    }
}
