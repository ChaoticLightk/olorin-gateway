using Microsoft.AspNetCore.Routing;

namespace Presentation.Endpoints.Interfaces;

public interface IEndpointMapper
{
    void Map(IEndpointRouteBuilder app);
}
