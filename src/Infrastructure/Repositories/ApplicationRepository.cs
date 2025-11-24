using Domain.Entities.Mongo.Application;
using Domain.Repositories.Interfaces;
using Domain.Shared.Constants;
using MongoDB.Driver;

namespace Infrastructure.Repositories;

public class ApplicationRepository(IMongoClient mongo) : IApplicationRepository
{
    private const string APPLICATION_COLLECTION_NAME = "applications";

    private readonly IMongoCollection<ApplicationDocument> _appplicatonDocument = mongo 
        .GetDatabase(MongoDbConfiguration.DB_NAME)
        .GetCollection<ApplicationDocument>(APPLICATION_COLLECTION_NAME);

    public async Task<ApplicationDocument> GetApplicationDocumentAsync(string application)
    {
        return await _appplicatonDocument
            .Find(x => x.Name.Equals(application))
            .FirstOrDefaultAsync();
    }
}
