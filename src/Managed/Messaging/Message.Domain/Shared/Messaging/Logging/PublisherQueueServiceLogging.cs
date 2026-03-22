using Package.ResultPattern.ErrorResult;

namespace Microsoft.Extensions.Logging;

public static partial class PublisherQueueServiceLogging
{
    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Consumindo mensagem recebida do sistema RabbitMQ na fila {queueName}.")]
    public static partial void LogConsumingRabbitMessage(
        this ILogger logger,
        string queueName);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Erro ao consumir a mensagem recebida do sistema RabbitMQ na fila {queueName}.")]
    public static partial void LogErrorConsumingRabbitMessage(
        this ILogger logger,
        string queueName,
        Exception exception);
    
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Erro ao consumir a mensagem recebida do sistema RabbitMQ na fila {queueName}. {error}")]
    public static partial void LogResultErrorConsumingRabbitMessage(
        this ILogger logger,
        string queueName,
        Error error);

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "Publicando mensagem ({message}) na fila {queueName} e exchange {exchange}.")]
    public static partial void LogPublishingRabbitMessage(
        this ILogger logger,
        string message,
        string queueName,
        string exchange);

    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Erro ao publicar a mensagem ({message}) para a fila {queueName} e exchange {exchange}.")]
    public static partial void LogErrorPublishingRabbitMessage(
        this ILogger logger,
        string message,
        string queueName,
        string exchange,
        Exception exception);
}
