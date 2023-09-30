namespace OnlineShop.Application.Constants;

public class OpenCacheKeys
{
    public static string GetCloudCodeKey(object tenantId, object ownerId, object directoryId) => $"{CloudConstant.SECRET_KEY_NAME}:{tenantId}:{ownerId}:{directoryId}";
    public static string GetCloudFileUrlKey(object tenantId, object ownerId, string fileName) => $"{tenantId}:{ownerId}:{fileName}";
    public static string GetAvatarUrlKey(object tenantId, object ownerId) => $"avatar:{tenantId}:{ownerId}";
}