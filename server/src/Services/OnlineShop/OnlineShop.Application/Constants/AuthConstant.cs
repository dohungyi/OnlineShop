namespace OnlineShop.Application.Constants;

public static class AuthConstant
{
    public const int MIN_TIME_TO_REQUIRED_OTP = 1; // Số phút tối thiểu để yêu cầu cấp OTP mới
    public const int MAX_TIME_OTP = 15; // OTP có giá trị trong 15p
    public const int OTP_LENGTH = 6; // Length OTP
    public const int PASSWORD_MIN_LENGTH = 8; // Min length của password
    public const int PASSWORD_MAX_LENGTH = 64; // Max length của password
    public const int TOKEN_TIME = 10 * 60; // Seconds
    public const int REFRESH_TOKEN_TIME = 30 * 24 * 60 * 60; // Seconds
}