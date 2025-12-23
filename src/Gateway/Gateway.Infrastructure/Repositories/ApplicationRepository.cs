using Gateway.Domain.Entities.Mongo.Application;
using Gateway.Domain.Repositories.Interfaces;
using MongoDB.Driver;
using Shared.Constants;

namespace Infrastructure.Repositories;

public class ApplicationRepository(IMongoClient mongo) : IApplicationRepository
{
    private const string APPLICATION_COLLECTION_NAME = "applications";

    private readonly IMongoCollection<ApplicationDocument> _appplicatonDocument = mongo 
        .GetDatabase(MongoDbConfiguration.PROXY_DB)
        .GetCollection<ApplicationDocument>(APPLICATION_COLLECTION_NAME);

    public async Task<ApplicationDocument> GetApplicationDocumentAsync(string application)
    {
        return await _appplicatonDocument
            .Find(x => x.Name.Equals(application))
            .FirstOrDefaultAsync();
    }
}
