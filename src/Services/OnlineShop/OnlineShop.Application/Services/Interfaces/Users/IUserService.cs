namespace OnlineShop.Application.Services.Interfaces.Users;

public interface IUserService
{
    Task<string> GetAvatarUrlByFileNameAsync(string fileName, object userId, CancellationToken cancellationToken);
}