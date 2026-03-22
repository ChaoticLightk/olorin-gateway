namespace Messaging.Domain.Data.Abstractions.Wolwerine;

public interface IHandler<in TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default);
}

public interface IHandler<in TRequest>
{
    Task Handle(TRequest request, CancellationToken cancellationToken = default);
}