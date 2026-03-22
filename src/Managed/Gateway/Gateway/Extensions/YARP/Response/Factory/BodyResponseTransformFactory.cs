using Gateway.Domain.Entities.Mongo.Endpoint;
using Yarp.ReverseProxy.Transforms;

namespace Gateway.Extensions.YARP.Response.Factory;

public static class BodyResponseTransformFactory
{
    public static ResponseTransform? CreateResponseTransform(
        BodyTransformType type, 
        IReadOnlyDictionary<string, string>? metadata = null)
    {
        return type switch
        {
            BodyTransformType.XmlToJson => XmlToJsonResponseTransform.CreateInstance(metadata),
                _ => throw new ArgumentException($"No response transform for type {BodyTransformType.None}")
        };
    }
}
