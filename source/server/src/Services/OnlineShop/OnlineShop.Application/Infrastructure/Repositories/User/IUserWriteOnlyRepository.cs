using OnlineShop.Domain.Entities;
using SharedKernel.Application;
using SharedKernel.Persistence;

namespace OnlineShop.Application.Infrastructure;

public interface IUserWriteOnlyRepository : IBaseWriteOnlyRepository<ApplicationUser, IApplicationDbContext>
{
    Task<ApplicationUser> CreateUserAsync(ApplicationUser user, CancellationToken cancellationToken = default);

    Task SetAvatarAsync(string fileName, CancellationToken cancellationToken = default);

    Task RemoveAvatarAsync(CancellationToken cancellationToken = default);
}