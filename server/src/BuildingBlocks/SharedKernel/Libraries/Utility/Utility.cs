using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using SharedKernel.Application.Consts;

namespace SharedKernel.Libraries.Utility;

public static class Utility
{
    #region Fields
    public static string NETCORE_ENVIRONMENT = "ASPNETCORE_ENVIRONMENT";
    public static string NETCORE_PROJECTNAME = "PROJECT_NAME";
    #endregion
    
    public static string GetEnvironment()
    {
        return Environment.GetEnvironmentVariable(NETCORE_ENVIRONMENT) ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
    }

    public static string GetEnvironmentLower()
    {
        return GetEnvironment().ToLower();
    }
    
    public static string RandomNumber(int length)
    {
        StringBuilder sb = new StringBuilder();
        Random random = new Random();

        while (sb.Length < length)
        {
            int num = random.Next(0, 9);
            sb.Append(num + "");
        }

        return sb.ToString();
    }
    
    public static string RandomString(int length, bool hasNumber = true)
    {
        var random = new Random();
        var mix = Enumerable.Range(65, 26).Concat(Enumerable.Range(97, 26)).ToList();
        if (hasNumber)
        {
            mix.Concat(Enumerable.Range(48, 10));
        }

        var result = new List<char>();
        var mixCount = mix.Count;
        if (length <= mixCount)
        {
            return string.Join("", mix.OrderBy(x => random.Next()).Take(length).Select(x => (char)x));
        }

        while (length > 0)
        {
            result.AddRange(mix.OrderBy(x => random.Next()).Take(length).Select(x => (char)x));
            length -= mixCount;
        }

        return string.Join("", result);
    }
    
    public static bool IsEmail(string input)
    {
        try
        {
            new MailAddress(input);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
    
    public static bool IsPhoneNumber(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }
        return Regex.IsMatch(input, RegexPattern.PHONE_NUMBER_PATTERN);
    }
}