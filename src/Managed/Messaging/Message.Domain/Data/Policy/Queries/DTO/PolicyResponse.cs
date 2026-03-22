using Messaging.Domain.Entities.Queue.Mongo.Policy;

namespace Messaging.Domain.Data.Policy.Queries.DTO;

public record PolicyResponse(
    string QueueName,
    bool Enabled,
    bool EnabledDeadLetter,
    int Version,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    DateTime? DeletedAt
)
{
    public static PolicyResponse FromDocument(QueuePolicyDocument document)
        => new(
            document.QueueName,
            document.Enabled,
            document.EnabledDeadLetter,
            document.Version,
            document.CreatedAt,
            document.UpdatedAt,
            document.DeletedAt 
        );
}
