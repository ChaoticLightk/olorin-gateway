using System.Reflection;
using Microsoft.AspNetCore.Routing;
using Presentation.Endpoints.Interfaces;

namespace Presentation.Endpoints;

public static class EndpointMapperExtensions
{
     public static void MapEndpointsFromAssembly(this IEndpointRouteBuilder app)
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a =>
                a.GetName().Name!.StartsWith(nameof(Presentation)));

        var mappers = assemblies
            .SelectMany(a => GetTypesSafely(a))
            .Where(t =>
                typeof(IEndpointMapper).IsAssignableFrom(t) &&
                !t.IsAbstract &&
                !t.IsInterface)
            .Select(Activator.CreateInstance)
            .Cast<IEndpointMapper>();

        foreach (var mapper in mappers)
        {
            mapper.Map(app);
        }
    }

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
}
