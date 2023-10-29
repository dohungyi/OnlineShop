namespace OnlineShop.Infrastructure.Repositories;

public class UserWriteOnlyRepository : BaseWriteOnlyRepository<ApplicationUser, ApplicationDbContext>, IUserWriteOnlyRepository
{
    private readonly IAuthRepository _authRepository;
    private readonly IMapper _mapper;
    
    public UserWriteOnlyRepository(
        ApplicationDbContext dbContext, 
        ICurrentUser currentUser, 
        ISequenceCaching sequenceCaching,
        IAuthRepository authRepository,
        IMapper mapper) 
        : base(dbContext, currentUser, sequenceCaching)
    {
        _authRepository = authRepository;
        _mapper = mapper;
    }

    public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user, CancellationToken cancellationToken = default)
    {
        await InsertAsync(user, cancellationToken);
        
        var customerRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Code == RoleConstant.Customer, cancellationToken);
        
        await _authRepository.SetRoleForUserAsync(user.Id, new List<Guid> { customerRole.Id }, cancellationToken);
        
        return user;
    }

    public async Task SetAvatarAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var avatar = await _dbContext.Avatars
                .FirstOrDefaultAsync(a => a.UserId.ToString() == _currentUser.Context.UserId, cancellationToken);

        if (avatar is null)
        {
            var avatarNew = new Avatar
            {
                FileName = fileName,
                UserId = Guid.Parse(_currentUser.Context.UserId),
                CreatedBy = _currentUser.Context.Username
            };
            
            await _dbContext.AddAsync(avatarNew, cancellationToken);
        }
        else
        {
            avatar.FileName = fileName;
            avatar.LastModifiedBy = _currentUser.Context.Username;
            avatar.LastModifiedDate = DateHelper.Now;
            
            _dbContext.Update(avatar);
        }
    }

    public async Task RemoveAvatarAsync(CancellationToken cancellationToken = default)
    {
        var avatar = await _dbContext.Avatars
            .FirstOrDefaultAsync(a => a.UserId.ToString() == _currentUser.Context.UserId, cancellationToken);

        _dbContext.Remove(avatar);
        
    }
}