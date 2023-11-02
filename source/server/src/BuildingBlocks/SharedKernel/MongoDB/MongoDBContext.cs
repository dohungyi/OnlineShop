using MongoDB.Driver;
using SharedKernel.Libraries;

namespace SharedKernel.MongoDB;

public class MongoDBContext
{
    #region [Declares + Constructor]

    private readonly IMongoDatabase _database;
    private readonly IMongoClient _mongoClient;
    
    public IMongoClient OriginClient => _mongoClient;

    public MongoDBContext(string connectionString, string databaseName)
    {
        _mongoClient = new MongoClient(connectionString);
        _database = _mongoClient.GetDatabase(databaseName);
    }

    #endregion

    #region [Methods]

    public IMongoCollection<TDocument> GetCollection<TDocument>()
    {
        string collectionName = typeof(TDocument).Name.ToSnakeCaseLower();
        return _database.GetCollection<TDocument>(collectionName);
    }

    #endregion
}