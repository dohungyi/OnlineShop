using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure.Persistence;
using SharedKernel.Auth;
using SharedKernel.Caching;
using SharedKernel.Infrastructures.Repositories;

namespace OnlineShop.Infrastructure.Repositories;

public class UserReadOnlyRepository : BaseReadOnlyRepository<ApplicationUser, ApplicationDbContext>, IUserReadOnlyRepository
{
    public UserReadOnlyRepository(
        ApplicationDbContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching, 
        IServiceProvider provider
        ) : base(dbContext, currentUser, sequenceCaching, provider)
    {
        
    }

    public async Task<string> CheckDuplicateAsync(string username, string email, string phone, CancellationToken cancellationToken = default)
    {
        
        var userNameExists  = await _dbContext.ApplicationUsers.AnyAsync(u => u.Username == username, cancellationToken);
        if (userNameExists)
        {
            return "username";
        }
        
        var phoneExists  = await _dbContext.ApplicationUsers.AnyAsync(u => u.PhoneNumber == phone, cancellationToken);
        if (phoneExists)
        {
            return "phone";
        }
        
        var emailExists  = await _dbContext.ApplicationUsers.AnyAsync(u => u.Email == email, cancellationToken);
        if (emailExists)
        {
            return "email";
        }

        return string.Empty;
    }

    public Task<object> GetAvatarAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetUserInformationAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}