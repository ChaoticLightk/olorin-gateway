using System.Reflection;
using Extensions.Endpoints.Abstractions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;

namespace Extensions.Endpoints;

public static class EndpointRouteBuilderExtensions
{
    public static void ConfigureScalar(this IEndpointRouteBuilder app)
    {
        app.MapScalarApiReference();
    }

    public static void MapEndpointsFromAssembly(this IEndpointRouteBuilder app)
    {
        var mappers = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(GetTypesSafely)
            .Where(t =>
                typeof(IEndpointMapper).IsAssignableFrom(t) &&
                !t.IsAbstract &&
                !t.IsInterface)
            .Select(t => (IEndpointMapper) ActivatorUtilities.CreateInstance(
                app.ServiceProvider, t))
            .ToList();

        mappers.ForEach(map => map.Map(app));
    }

    #region Private
    private static IEnumerable<Type> GetTypesSafely(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException ex)
        {
            return ex.Types.Where(t => t != null)!;
        }
    }
    #endregion
}
