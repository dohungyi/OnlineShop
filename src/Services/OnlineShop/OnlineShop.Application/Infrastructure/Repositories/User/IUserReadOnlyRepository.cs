using OnlineShop.Domain.Entities;
using SharedKernel.Application;
using SharedKernel.Domain;
using SharedKernel.Persistence;

namespace OnlineShop.Infrastructure.Repositories;

public interface IUserReadOnlyRepository : IBaseReadOnlyRepository<ApplicationUser, IAppDbContext>
{
    Task<ApplicationUser> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<string> CheckDuplicateAsync(string username, string email, string phone, CancellationToken cancellationToken = default);

    Task<Avatar> GetAvatarAsync(CancellationToken cancellationToken = default);

    Task<User> GetUserInformationAsync(CancellationToken cancellationToken = default);
}