using Yarp.ReverseProxy.Transforms;

namespace Gateway.Extensions.YARP.Response;

public class XmlToJsonResponseTransform : ResponseTransform
{
    public override ValueTask ApplyAsync(ResponseTransformContext context)
    {
        throw new NotImplementedException();
    }
}
