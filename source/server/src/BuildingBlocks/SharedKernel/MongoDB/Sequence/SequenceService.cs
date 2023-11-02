using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using SharedKernel.Application;
using SharedKernel.Domain;
using SharedKernel.Libraries;

namespace SharedKernel.MongoDB;

public class SequenceService<T> : ISequenceService<T> where T : BaseEntity
{
    
    private readonly IServiceProvider _provider;

    public SequenceService(IServiceProvider provider)
    {
        _provider = provider;
    }
    /// <summary>
    /// Lấy giá trị inc tiếp theo
    /// </summary>
    public async Task<long> Next()
    {
        var filter = Builders<Sequence>.Filter.Where(s => s.Table == typeof(T).Name.ToSnakeCaseLower());
        var mongoService = _provider.GetRequiredService<IMongoService<Sequence>>();
        var sequence = mongoService.Find(filter).ToList().FirstOrDefault();

        if (sequence == null)
        {
            var session = await mongoService.GetSessionAsync();

            session.StartTransaction();
            await mongoService.InsertOneAsync(session, new Sequence() { Table = typeof(T).Name.ToSnakeCaseLower(), SeqNo = 1 });
            await session.CommitTransactionAsync();
            return 1;
        }
        return sequence.SeqNo;
    }
}