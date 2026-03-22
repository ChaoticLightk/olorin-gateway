using Microsoft.AspNetCore.Routing;

namespace Extensions.Endpoints.Abstractions;

public interface IEndpointMapper
{
    void Map(IEndpointRouteBuilder app);
}

