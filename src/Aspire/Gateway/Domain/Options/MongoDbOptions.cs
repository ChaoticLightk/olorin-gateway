using System;

namespace Gateway.Domain.Options;

public class MongoDbOptions
{
    public const string SECTION_NAME = "MongoDb";
    public string? ConnectionString { get; init; } = string.Empty;
    public string? DatabaseName { get; init; } = "Proxy";
}
