using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MiddleR.Abstractions;

namespace MiddleR;

public class ServiceBus(IServiceProvider provider) : IServiceBus
{
    public Task<TResponse> Send<TResponse>(
        IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var method = typeof(ServiceBus)
            .GetMethod(nameof(DispatchRequest), BindingFlags.Instance | BindingFlags.NonPublic)!
            .MakeGenericMethod(requestType, typeof(TResponse));

        return (Task<TResponse>)method.Invoke(this, [request, cancellationToken])!;
    }

    public Task Send(
        IRequest request,
        CancellationToken cancellationToken = default)
    {
        var requestType = request.GetType();

        var method = typeof(ServiceBus)
            .GetMethod(nameof(DispatchVoid), BindingFlags.Instance | BindingFlags.NonPublic)!
            .MakeGenericMethod(requestType);

        return (Task)method.Invoke(this, new object[] { request, cancellationToken })!;
    }

    private Task<TResponse> DispatchRequest<TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : IRequest<TResponse>
    {
        var handler = provider
            .GetRequiredService<IRequestHandler<TRequest, TResponse>>();

        return handler.Handle(request, cancellationToken);
    }

    private Task DispatchVoid<TRequest>(
        TRequest request,
        CancellationToken cancellationToken)
        where TRequest : IRequest
    {
        var handler = provider
            .GetRequiredService<IRequestHandler<TRequest>>();

        return handler.Handle(request, cancellationToken);
    }
}
