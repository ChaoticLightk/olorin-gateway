using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MiddleR.Abstractions;

namespace MiddleR;

public static class DependencyInjection
{
    public static IServiceCollection AddMiddleR(
        this IServiceCollection services,
        params Assembly[] assemblies)
    {
        if (assemblies is null || assemblies.Length == 0)
        {
            throw new ArgumentException("");
        }

        RegisterHandlers(services, assemblies);

        services.AddScoped<IServiceBus, ServiceBus>();

        return services;
    }

    private static void RegisterHandlers(IServiceCollection services, Assembly[] assemblies)
    {
        var a = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract && !t.IsGenericTypeDefinition)
            .SelectMany(t => t.GetInterfaces()
                .Where(i => i.IsGenericType &&
                    (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>) ||
                        i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)))
                .Select(i => (i, t)));

        foreach (var (service, implementation) in a)
        {
            services.AddTransient(service, implementation);
        }
    }
}
