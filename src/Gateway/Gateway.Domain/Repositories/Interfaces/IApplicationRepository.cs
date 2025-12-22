using Gateway.Domain.Entities.Mongo.Application;

namespace Domain.Repositories.Interfaces;

public interface IApplicationRepository
{
    Task<ApplicationDocument> GetApplicationDocumentAsync(string application);
}
