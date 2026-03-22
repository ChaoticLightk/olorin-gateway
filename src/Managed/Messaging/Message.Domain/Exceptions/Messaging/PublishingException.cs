namespace Messaging.Domain.Exceptions.Messaging;

public class PublishingException : Exception
{
    public PublishingException() : base("Ocorreu um erro ao publicar a mensagem no broker.")
    {
    }

    public PublishingException(string message) : base(message)
    {
    }

    public PublishingException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
