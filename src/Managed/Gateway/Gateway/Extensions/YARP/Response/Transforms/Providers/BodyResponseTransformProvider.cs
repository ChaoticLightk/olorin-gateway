using Gateway.Domain.Entities.Mongo.Endpoint;
using Gateway.Extensions.YARP.Response.Factory;
using Yarp.ReverseProxy.Transforms;
using Yarp.ReverseProxy.Transforms.Builder;

namespace Gateway.Extensions.YARP.Response.Transforms.Providers;

public class BodyResponseTransformProvider : ITransformProvider
{
    public void Apply(TransformBuilderContext context)
    {
        if (context.Route.Metadata == null)
            return;

        if (context.Route.Metadata?.TryGetValue(nameof(RouteDocument.BodyTransform), out var _) == true
            && context.Route.Metadata?.TryGetValue(nameof(RouteDocument.BodyTransformType), out var type) == true
            && Enum.TryParse<BodyTransformType>(type, out var enumType))
        {
            var metadata = context.Route.Metadata; 

            context.AddResponseTransform(async context =>
            {
                var transform = BodyResponseTransformFactory
                    .CreateResponseTransform(enumType, metadata);

                if(transform is not null)
                    await transform.ApplyAsync(context);
            });
        }
    }

    public void ValidateRoute(TransformRouteValidationContext context)
    {
        if (context.Route.Metadata?.TryGetValue(nameof(RouteDocument.BodyTransform), out var _) == true
            && context.Route.Metadata?.TryGetValue(nameof(RouteDocument.BodyTransformType), out var type) == true
            && type.Equals(BodyTransformType.None.ToString()))
        {
            context.Errors.Add(new ArgumentException(
             $"A non-empty {nameof(RouteDocument.BodyTransformType)} value is required"));
        }
    }

    public void ValidateCluster(TransformClusterValidationContext context)
    {
    }
}
