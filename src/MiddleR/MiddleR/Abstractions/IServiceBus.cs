namespace MiddleR.Abstractions;

public interface IServiceBus 
{
    Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default); 

    Task Send(IRequest request, CancellationToken cancellationToken = default); 
}
