using System.Dynamic;
using System.IO.Compression;
using System.Net.Mime;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Gateway.Domain.Entities.Mongo.Endpoint;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Yarp.ReverseProxy.Transforms;

namespace Gateway.Extensions.YARP.Response;

public class XmlToJsonResponseTransform(IReadOnlyDictionary<string, string>? metadata)
    : ResponseTransform
{
    public IReadOnlyDictionary<string, string>? Metadata { get; init; } = metadata;

    public static XmlToJsonResponseTransform CreateInstance(
        IReadOnlyDictionary<string, string>? routeMetadata)
        => new(routeMetadata);

    public override async ValueTask ApplyAsync(ResponseTransformContext context)
    {
        var proxyResponse = context.ProxyResponse;

        if (proxyResponse?.Content is null)
            return;

        var contentType = proxyResponse.Content.Headers.ContentType?.MediaType;

        if (contentType is null || !contentType.Contains("xml", StringComparison.OrdinalIgnoreCase))
            return;

        var stream = await proxyResponse.DecompressStream();

        using var reader = new StreamReader(stream, Encoding.UTF8);

        var xmlString = await reader.ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(xmlString))
            return;

        var xml = new XmlDocument();

        xml.LoadXml(xmlString);

        if (xml is null)
            return;

        if (Metadata is not null &&
            Metadata.TryGetValue(nameof(RouteDocument.BodyTransformRoot), out var bodyTransformRoot) &&
            !string.IsNullOrWhiteSpace(bodyTransformRoot))
        {
            var target = FindElementRecursive(xml.DocumentElement!, bodyTransformRoot);

            if (target is not null)
            {
               // cria novo documento contendo apenas o nó desejado
                var cleanRoot = StripAllNamespaces(target);

                var newDoc = new XmlDocument();
                var imported = newDoc.ImportNode(cleanRoot, true);
                newDoc.AppendChild(imported);

                xml = newDoc;
            }
        }

        var jsonText = JsonConvert.SerializeXmlNode(xml);

        object? dyn = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);

        var jsonString = System.Text.Json.JsonSerializer.Serialize(dyn);

        var bytes = Encoding.UTF8.GetBytes(jsonString);

        context.SuppressResponseBody = true;
        context.HttpContext.Response.ContentType = MediaTypeNames.Application.Json;
        context.HttpContext.Response.ContentLength = bytes.Length;

        context.HttpContext.Response.Headers.Remove(HeaderNames.ContentEncoding);

        await context.HttpContext.Response.Body.WriteAsync(bytes);
    }

    public static XmlNode? FindElementRecursive(XmlNode node, string name)
    {
        if (node.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ||
            node.LocalName.Equals(name, StringComparison.OrdinalIgnoreCase))
        {
            return node;
        }

        foreach (XmlNode child in node.ChildNodes)
        {
            var found = FindElementRecursive(child, name);
            if (found != null)
                return found;
        }

        return null;
    }

   public static XmlElement StripAllNamespaces(XmlNode node)
    {
        XmlDocument doc = new XmlDocument();

        XmlElement Clean(XmlNode n)
        {
            var element = doc.CreateElement(n.LocalName);

            foreach (XmlNode child in n.ChildNodes)
            {
                if (child.NodeType == XmlNodeType.Element)
                    element.AppendChild(Clean(child));
                else if (child.NodeType == XmlNodeType.Text)
                    element.InnerText = child.Value ?? "";
            }

            return element;
        }

        return Clean(node);
    }
}

public static class HttpResponseMessageExtensions
{
    public static async Task<Stream> DecompressStream(this HttpResponseMessage response)
    {
        var stream = await response.Content.ReadAsStreamAsync();

        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
            return new GZipStream(stream, CompressionMode.Decompress);

        if (response.Content.Headers.ContentEncoding.Contains("deflate"))
            return new DeflateStream(stream, CompressionMode.Decompress);

        return stream;
    }
}
