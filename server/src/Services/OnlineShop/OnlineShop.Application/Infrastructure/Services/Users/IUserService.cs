namespace OnlineShop.Application.Services;

public interface IUserService
{
    Task<string> GetAvatarUrlByFileNameAsync(string fileName, object userId, CancellationToken cancellationToken);
}