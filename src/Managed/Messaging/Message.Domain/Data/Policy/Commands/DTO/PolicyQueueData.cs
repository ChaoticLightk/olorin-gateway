using Messaging.Domain.Entities.Queue.Mongo.Policy;

namespace Messaging.Domain.Data.Policy.Commands.DTO;

public record PolicyQueueData(
    string Name, 
    bool Enabled = false, 
    bool EnabledDeadLetter = false,
    PolicyRetryData? Retry = null,
    PolicyQueueMetadata? Metadata = null)
{
    public QueuePolicyDocument MapDocument()
    {
        RetryPolicy? retryPolicy = null; 
        QueueMetadata? queueMetadata = null; 

        if (Retry is PolicyRetryData retryData)
        {
            retryPolicy = RetryPolicy.CreateInstance(
                retryData.MaxRetries,
                retryData.DelaySeconds,
                retryData.PenalyFactor,
                retryData.MaxDelaySeconds);
        } 

        if (Metadata is PolicyQueueMetadata metadata)
        {
            queueMetadata = QueueMetadata.CreateInstance(
                metadata.Description,
                metadata.Owner);
        }

        var document = QueuePolicyDocument
            .CreateInstance(
                Name,
                retryPolicy,
                queueMetadata,
                Enabled,
                EnabledDeadLetter);

        return document;
    }
}
