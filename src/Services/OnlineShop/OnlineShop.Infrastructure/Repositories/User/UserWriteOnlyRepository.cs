using OnlineShop.Application.Repositories;
using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure.Persistence;
using SharedKernel.Auth;
using SharedKernel.Caching;
using SharedKernel.Infrastructures.Repositories;

namespace OnlineShop.Infrastructure.Repositories;

public class UserWriteOnlyRepository : BaseWriteOnlyRepository<ApplicationUser, ApplicationDbContext>, IUserWriteOnlyRepository
{
    public UserWriteOnlyRepository(
        ApplicationDbContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching) 
        : base(dbContext, currentUser, sequenceCaching)
    {
    }

    public Task<Guid> CreateUserAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SetAvatarAsync(string fileName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAvatarAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}