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

    public async Task<ApplicationUser> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await _dbSet.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
        return user;
    }

    public async Task<string> CheckDuplicateAsync(string username, string email, string phone, CancellationToken cancellationToken = default)
    {

        if (!string.IsNullOrEmpty(username))
        {
            var userNameExists  = await _dbSet.AnyAsync(u => u.Username == username && !u.IsDeleted, cancellationToken);
            if (userNameExists)
            {
                return nameof(username);
            }
        }

        if (!string.IsNullOrEmpty(phone))
        {
            var phoneExists  = await _dbSet.AnyAsync(u => u.PhoneNumber == phone && !u.IsDeleted, cancellationToken);
            if (phoneExists)
            {
                return nameof(phone);
            }
        }
        
        if (!string.IsNullOrEmpty(email))
        {
            var emailExists  = await _dbSet.AnyAsync(u => u.Email == email && !u.IsDeleted, cancellationToken);
            if (emailExists)
            {
                return nameof(email);
            }
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
        var user = await _dbSet
            .Where(a => a.Id.ToString() == _currentUser.Context.UserId && !a.IsDeleted)
            .Include(a => a.Avatar)
            .FirstOrDefaultAsync(cancellationToken);

        return user;
    }
}