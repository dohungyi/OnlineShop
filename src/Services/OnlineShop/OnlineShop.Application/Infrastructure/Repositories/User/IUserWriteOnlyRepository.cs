using OnlineShop.Domain.Entities;
using SharedKernel.Application;
using SharedKernel.Persistence;

namespace OnlineShop.Application.Repositories;

public interface IUserWriteOnlyRepository : IBaseWriteOnlyRepository<ApplicationUser, IAppDbContext>
{
    Task<ApplicationUser> CreateUserAsync(ApplicationUser user, CancellationToken cancellationToken = default);

    Task SetAvatarAsync(string fileName, CancellationToken cancellationToken = default);

    Task RemoveAvatarAsync(CancellationToken cancellationToken = default);
}