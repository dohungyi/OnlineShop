using OnlineShop.Domain.Entities;
using SharedKernel.Application;
using SharedKernel.Domain;
using SharedKernel.Persistence;

namespace OnlineShop.Infrastructure.Repositories;

public interface IUserReadOnlyRepository : IBaseReadOnlyRepository<ApplicationUser, IAppDbContext>
{
    Task<string> CheckDuplicateAsync(string username, string email, string phone, CancellationToken cancellationToken = default);

    Task<object> GetAvatarAsync(CancellationToken cancellationToken);

    Task<User> GetUserInformationAsync(CancellationToken cancellationToken);
}