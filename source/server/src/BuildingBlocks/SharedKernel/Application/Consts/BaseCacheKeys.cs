namespace SharedKernel.Application.Consts;

public class BaseCacheKeys
{
    public static string GetConfigKey(object userId) => $"config:{userId}";
    public static string GetAccessTokenKey(object userId) => $"access-token:{userId}";
    public static string GetRevokeAccessTokenKey(string accessToken) => $"revoked-token:{accessToken}";
    public static string GetClientInformationKey(string ip) => $"client-information:{ip}";
    public static string GetSystemFullRecordsKey(string tableName) => $"system-full-records:{tableName}";
    public static string GetSystemRecordByIdKey(string tableName, object recordId) => $"system-record-by-id:{tableName}:{recordId}";
    public static string GetSystemRecordByForeignIdKey(string tableName, object foreignKeyId) => $"system-record-by-foreignkey-id:{tableName}:{foreignKeyId}";
}