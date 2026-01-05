using System;
using System.Runtime.Serialization;

namespace Messaging.Domain.Exceptions.Messaging;

public class ProvisionException : Exception
{
    public ProvisionException() : base("Ocorreu um erro ao provisionar a fila.")
    {
    }

    public ProvisionException(string? message) : base(message)
    {
    }

    public ProvisionException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
