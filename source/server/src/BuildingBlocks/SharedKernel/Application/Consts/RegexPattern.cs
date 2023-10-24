namespace SharedKernel.Application.Consts;

public static class RegexPattern
{
    public const string PHONE_NUMBER_PATTERN = "(84|0[3|5|7|8|9])+([0-9]{8})";
    public const string ONLY_NUMBER_PATTERN = @"^[0-9]+$";
}