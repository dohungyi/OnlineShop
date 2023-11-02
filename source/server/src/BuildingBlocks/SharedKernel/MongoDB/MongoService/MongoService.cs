using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace SharedKernel.MongoDB;

public class MongoService<TDocument> : IMongoService<TDocument>
{

    #region [Declares + Constructor]

    private readonly string connectionString;
    private readonly string databaseName;

    /// <summary>
    /// Đối tượng truy cập mongo
    /// </summary>
    private MongoDBContext _mongoDbContext;
    public MongoDBContext mongoDbContext
    {
        get
        {
            if (_mongoDbContext is null)
            {
                _mongoDbContext = new MongoDBContext(connectionString, databaseName);
            }
            return _mongoDbContext;
        }
    }
    
    /// <summary>
    /// Đối tượng mongo collection
    /// </summary>
    private IMongoCollection<TDocument> _collection;
    public IMongoCollection<TDocument> collection
    {
        get
        {
            if (_collection is null)
            {
                _collection = mongoDbContext.GetCollection<TDocument>();
            }
            return _collection;

        }
    }
    
    /// <summary>
    /// Đối tượng client
    /// </summary>
    private IMongoClient _client;
    public virtual IMongoClient client { 
        get
        {
            if (_client == null)
            {
                _client = mongoDbContext.OriginClient;
            }
            return _client;
        } 
    }
    
    public MongoService(IServiceProvider provider, string dbName)
    {
        var config = provider.GetRequiredService<IConfiguration>();

        connectionString = config.GetRequiredSection("Mongo:Cluster").Value;
        databaseName = string.IsNullOrEmpty(dbName) ? config.GetRequiredSection("Mongo:Database").Value : dbName;
    }

    public MongoService(IServiceProvider provider) : this(provider, string.Empty)
    {

    }
    
    #endregion
    
    #region [Methods]

    #region [Get session]
    
    public IClientSessionHandle GetSession()
    {
        return client.StartSession();
    }

    public Task<IClientSessionHandle> GetSessionAsync()
    {
        return client.StartSessionAsync();
    }
    
    #endregion

    #region [Insert]

    public void InsertMany(IClientSessionHandle session, IEnumerable<TDocument> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        InsertManyAsync(session, documents, options, cancellationToken).GetAwaiter().GetResult();
    }
    
    public Task InsertManyAsync(IClientSessionHandle session, IEnumerable<TDocument> documents, InsertManyOptions options = null,
        CancellationToken cancellationToken = default)
    {
        if (session is null)
        {
            throw new Exception("session required!");
        }
        
        return collection.InsertManyAsync(session, documents, options, cancellationToken);
    }

    public void InsertOne(IClientSessionHandle session, TDocument document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        collection.InsertOneAsync(session, document, options, cancellationToken).GetAwaiter().GetResult();
    }

    public Task InsertOneAsync(IClientSessionHandle session, TDocument document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return collection.InsertOneAsync(session, document, options, cancellationToken);
    }

    #endregion

    #region [Update]

    public UpdateResult UpdateMany(IClientSessionHandle session, FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return UpdateManyAsync(session, filter, update, options, cancellationToken).GetAwaiter().GetResult();
    }

    public Task<UpdateResult> UpdateManyAsync(IClientSessionHandle session, FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return UpdateManyAsync(session, filter, update, options, cancellationToken);
    }

    public UpdateResult UpdateOne(IClientSessionHandle session, FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return UpdateOneAsync(session, filter, update, options, cancellationToken).GetAwaiter().GetResult();
    }

    public Task<UpdateResult> UpdateOneAsync(IClientSessionHandle session, FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return collection.UpdateOneAsync(session, filter, update, options, cancellationToken);
    }

    #endregion

    #region [Replace]

    public ReplaceOneResult ReplaceOne(IClientSessionHandle session, FilterDefinition<TDocument> filter, TDocument replacement, ReplaceOptions options, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return ReplaceOneAsync(session, filter, replacement, options, cancellationToken).GetAwaiter().GetResult();
    }

    public Task<ReplaceOneResult> ReplaceOneAsync(IClientSessionHandle session, FilterDefinition<TDocument> filter, TDocument replacement, ReplaceOptions options, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return collection.ReplaceOneAsync(session, filter, replacement, options, cancellationToken);
    }
    
    #endregion

    #region [Delete]

    public DeleteResult DeleteMany(IClientSessionHandle session, FilterDefinition<TDocument> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return DeleteManyAsync(session, filter, options, cancellationToken).GetAwaiter().GetResult();
    }

    public Task<DeleteResult> DeleteManyAsync(IClientSessionHandle session, FilterDefinition<TDocument> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return collection.DeleteManyAsync(session, filter, options, cancellationToken);
    }

    public DeleteResult DeleteOne(IClientSessionHandle session, FilterDefinition<TDocument> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return DeleteOneAsync(session, filter, options, cancellationToken).GetAwaiter().GetResult();
    }

    public Task<DeleteResult> DeleteOneAsync(IClientSessionHandle session, FilterDefinition<TDocument> filter, DeleteOptions options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return collection.DeleteOneAsync(session, filter, options, cancellationToken);
    }

    #endregion

    #region [Find]

    public IFindFluent<TDocument, TDocument> Find(FilterDefinition<TDocument> filter, FindOptions options = null)
    {
        return collection.Find(filter, options);
    }

    public Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(FilterDefinition<TDocument> filter, FindOptions<TDocument, TProjection> options = null, CancellationToken cancellationToken = default)
    {
        return collection.FindAsync(filter, options, cancellationToken);
    }

    public TProjection FindOneAndDelete<TProjection>(IClientSessionHandle session, FilterDefinition<TDocument> filter, FindOneAndDeleteOptions<TDocument, TProjection> options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return FindOneAndDeleteAsync(session, filter, options, cancellationToken).GetAwaiter().GetResult();
    }

    public Task<TProjection> FindOneAndDeleteAsync<TProjection>(IClientSessionHandle session, FilterDefinition<TDocument> filter, FindOneAndDeleteOptions<TDocument, TProjection> options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return collection.FindOneAndDeleteAsync(session, filter, options, cancellationToken);
    }

    public TProjection FindOneAndReplace<TProjection>(IClientSessionHandle session, FilterDefinition<TDocument> filter, TDocument replacement, FindOneAndReplaceOptions<TDocument, TProjection> options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return FindOneAndReplaceAsync(session, filter, replacement, options, cancellationToken).GetAwaiter().GetResult();
    }

    public Task<TProjection> FindOneAndReplaceAsync<TProjection>(IClientSessionHandle session, FilterDefinition<TDocument> filter, TDocument replacement, FindOneAndReplaceOptions<TDocument, TProjection> options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return collection.FindOneAndReplaceAsync(session, filter, replacement, options, cancellationToken);
    }

    public TProjection FindOneAndUpdate<TProjection>(IClientSessionHandle session, FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument, TProjection> options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return FindOneAndUpdateAsync(session, filter, update, options, cancellationToken).GetAwaiter().GetResult();
    }

    public Task<TProjection> FindOneAndUpdateAsync<TProjection>(IClientSessionHandle session, FilterDefinition<TDocument> filter, UpdateDefinition<TDocument> update, FindOneAndUpdateOptions<TDocument, TProjection> options = null, CancellationToken cancellationToken = default)
    {
        if (session == null)
        {
            throw new Exception("session required!");
        }
        return collection.FindOneAndUpdateAsync(session, filter, update, options, cancellationToken);
    }

    #endregion

    #region [Count]

    public long CountDocuments(FilterDefinition<TDocument> filter, CountOptions options = null, CancellationToken cancellationToken = default)
    {
        return CountDocumentsAsync(filter, options, cancellationToken).GetAwaiter().GetResult();
    }

    public Task<long> CountDocumentsAsync(FilterDefinition<TDocument> filter, CountOptions options = null, CancellationToken cancellationToken = default)
    {
        return collection.CountDocumentsAsync(filter, options, cancellationToken);
    }
    
    #endregion

    #endregion
}