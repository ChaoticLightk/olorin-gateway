using Extensions.Endpoints;
using Microsoft.AspNetCore.Builder;

namespace Gateway.Presentation;

public static class DependencyInjection
{
    public static void AddPresentation(this WebApplication app)
    {
        app.MapEndpointsFromAssembly();
    }
}
