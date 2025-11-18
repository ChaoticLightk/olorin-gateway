using System.Dynamic;
using System.Net.Mime;
using System.Text;
using System.Xml.Linq;
using Newtonsoft.Json;
using Yarp.ReverseProxy.Transforms;

namespace Gateway.Extensions.YARP.Response;

public class XmlToJsonResponseTransform : ResponseTransform
{
    public static XmlToJsonResponseTransform CreateInstance()
        => new();

    public override async ValueTask ApplyAsync(ResponseTransformContext context)
    {
        var proxyResponse = context.ProxyResponse;

        if (proxyResponse?.Content is null)
            return;

        var contentType = proxyResponse.Content.Headers.ContentType?.MediaType;

        if (contentType is null || !contentType.Contains("xml", StringComparison.OrdinalIgnoreCase))
            return;

        var stream = await proxyResponse.Content.ReadAsStreamAsync();

        using var reader = new StreamReader(stream, Encoding.UTF8);

        var xmlString = await reader.ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(xmlString))
            return;

        var xml = XDocument.Parse(xmlString);

        if (xml.Root is null)
            return;

        var jsonText = JsonConvert.SerializeXNode(xml);

        object? dyn = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);

        var jsonString = System.Text.Json.JsonSerializer.Serialize(dyn);

        var bytes = Encoding.UTF8.GetBytes(jsonString);

        context.SuppressResponseBody = true;
        context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
        context.HttpContext.Response.ContentLength = bytes.Length;

        await context.HttpContext.Response.Body.WriteAsync(bytes);
    }
}
