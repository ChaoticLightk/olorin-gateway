using System;
using Microsoft.AspNetCore.Builder;
using Presentation.Endpoints;
using Presentation.Endpoints.Proxy;

namespace Presentation;

public static class DependencyInjection
{
    public static void AddPresentation(this WebApplication app)
    {
        app.MapEndpointsFromAssembly();
    }
}
