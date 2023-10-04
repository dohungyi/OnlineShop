using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;
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
            return nameof(username);
        }
        
        var phoneExists  = await _dbContext.ApplicationUsers.AnyAsync(u => u.PhoneNumber == phone, cancellationToken);
        if (phoneExists)
        {
            return nameof(phone);
        }
        
        var emailExists  = await _dbContext.ApplicationUsers.AnyAsync(u => u.Email == email, cancellationToken);
        if (emailExists)
        {
            return nameof(email);
        }

        return string.Empty;
    }

    public async Task<Avatar> GetAvatarAsync(CancellationToken cancellationToken)
    {
        var avatar = await _dbContext.Avatars.FirstOrDefaultAsync(a => a.UserId.ToString() == _currentUser.Context.UserId && !a.IsDeleted, cancellationToken);

        return avatar;
    }

    public async Task<User> GetUserInformationAsync(CancellationToken cancellationToken)
    {
        var user = await _dbContext.ApplicationUsers
            .Where(a => a.Id.ToString() == _currentUser.Context.UserId && !a.IsDeleted)
            .Include(a => a.Avatar)
            .Include(a => a.UserConfig)
            .Include(a => a.UserAddresses)
            .Include(a => a.UserPayments)
            .Include(a => a.Orders)
            .FirstOrDefaultAsync(cancellationToken);

        return user;
    }
}