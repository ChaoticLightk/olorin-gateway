using System.Text.Json.Serialization;

namespace Package.ResultPattern.ErrorResult;

public record ErrorResponse
{
    public ErrorResponse(Error error)
    {
        ArgumentNullException.ThrowIfNull(error);

        Message = error.Message;
        Type = error.Type;
        EventId = error.EventId;
    }

    public string Message { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public ErrorType Type { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ErrorType TypeDescription => Type;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? EventId { get; set; }

    public static explicit operator ErrorResponse(Error error)
        => new(error: error);
}
