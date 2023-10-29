namespace OnlineShop.Infrastructure.Repositories;

public class CpanelWriteOnlyRepository : BaseWriteOnlyRepository<BaseEntity, ApplicationDbContext>, ICpanelWriteOnlyRepository
{
    public CpanelWriteOnlyRepository(
        ApplicationDbContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching
        ) : base(dbContext, currentUser, sequenceCaching)
    {
        
    }

    public async Task UpdateRoleAsync(object roleId, object actionId, bool value, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}