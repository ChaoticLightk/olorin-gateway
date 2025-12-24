using Extensions.Endpoints;
using Microsoft.AspNetCore.Builder;

namespace Messaging.Presentation;

public static class DependencyInjection
{
    public static void ConfigurePresentation(this WebApplication app)
    {
        app.MapEndpointsFromAssembly();
    }
}
