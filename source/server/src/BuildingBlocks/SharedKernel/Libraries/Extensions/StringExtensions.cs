using System.Text;
using System.Text.RegularExpressions;

namespace SharedKernel.Libraries;

public static class StringExtensions
{
    // CamelCase: camelCase
    // SnakeCase: snake_case
    // KebabCase: kebab-case

    /// </summary>
    /// camel case sang snake case
    /// </summary>
    public static string ToSnakeCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()));
    }
    
    /// </summary>
    /// camel case sang snake case lower
    /// </summary>
    public static string ToSnakeCaseLower(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }
        
        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLower();
    }

    /// </summary>
    /// camel case sang kebab case
    /// </summary>
    public static string ToKebabCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString()));
    }
    
    /// </summary>
    /// camel case sang kebab case lower
    /// </summary>
    public static string ToKebabCaseLower(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException(nameof(input));
        }

        return string.Concat(input.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString())).ToLower();
    }
    
    /// <summary>
    /// Convert string to MD5
    /// </summary>
    public static string ToMD5(this string input)
    {
        // Use input string to calculate MD5 hash
        using (var md5 = System.Security.Cryptography.MD5.Create())
        {
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
    
    /// <summary>
    /// To Base64 Encode
    /// </summary>
    
    public static string ToBase64Encode(this string plainText)
    {
        if (plainText == null)
        {
            throw new ArgumentNullException();
        }

        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }
    
    /// <summary>
    /// To Base64 Decode
    /// </summary>
    public static string ToBase64Decode(this string base64EncodedData)
    {
        if (base64EncodedData == null)
        {
            throw new ArgumentNullException("");
        }
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }
    
    /// <summary>
    /// Replace Regex - Thay thế
    /// </summary>
    public static string ReplaceRegex(this string value, string pattern, string replacement)
    {
        try
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            value = value.Trim();
            return Regex.Replace(value, pattern, replacement);
        }
        catch
        {
            return value;
        }
    }

    /// <summary>
    /// Normalize String : Chuẩn hóa chuỗi, loại bỏ các khoảng trắng
    /// </summary>
    public static string NormalizeString(this string value)
    {
        try
        {
            return value.ReplaceRegex("\\s+", " ");
        }
        catch
        {
            return value;
        }
    }

    /// <summary>
    /// Vi to En
    /// </summary>
    /// <param name="unicodeString"></param>
    /// <param name="special"></param>
    /// <returns></returns>
    public static string ViToEn(this string unicodeString, bool special = false)
    {
        if (string.IsNullOrEmpty(unicodeString))
        {
            return string.Empty;
        }

        try
        {
            unicodeString = unicodeString.NormalizeString();
            unicodeString = unicodeString.Trim();
            unicodeString = Regex.Replace(unicodeString, "[áàảãạâấầẩẫậăắằẳẵặ]", "a");
            unicodeString = Regex.Replace(unicodeString, "[éèẻẽẹêếềểễệ]", "e");
            unicodeString = Regex.Replace(unicodeString, "[iíìỉĩị]", "i");
            unicodeString = Regex.Replace(unicodeString, "[óòỏõọơớờởỡợôốồổỗộ]", "o");
            unicodeString = Regex.Replace(unicodeString, "[úùủũụưứừửữự]", "u");
            unicodeString = Regex.Replace(unicodeString, "[yýỳỷỹỵ]", "y");
            unicodeString = Regex.Replace(unicodeString, "[đ]", "d");
            if (special)
            {
                unicodeString = Regex.Replace(unicodeString, "[\"`~!@#$%^&*()-+=?/>.<,{}[]|]\\]", "");
            }
            
            // "\u0300": dấu huyền (grave accent)
            // "\u0323": dấu hỏi (hook above)
            // "\u0309": dấu sắc (acute accent)
            // "\u0303": dấu ngã (tilde)
            // "\u0301": dấu móc (hook)

            unicodeString = unicodeString.Replace("\u0300", "").Replace("\u0323", "").Replace("\u0309", "").Replace("\u0303", "").Replace("\u0301", "");
            return unicodeString;
        }
        catch
        {
            return "";
        }
    }

    /// <summary>
    /// string has unicode
    /// </summary>
    public static bool HasUnicode(this string source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return false;
        }
        
        var length = source.Length;

        source = Regex.Replace(source, "[áàảãạâấầẩẫậăắằẳẵặ]", "");
        source = Regex.Replace(source, "[éèẻẽẹêếềểễệ]", "");
        source = Regex.Replace(source, "[iíìỉĩị]", "");
        source = Regex.Replace(source, "[óòỏõọơớờởỡợôốồổỗộ]", "");
        source = Regex.Replace(source, "[úùủũụưứừửữự]", "");
        source = Regex.Replace(source, "[yýỳỷỹỵ]", "");
        source = Regex.Replace(source, "[đ]", "");

        return source.Length != length;
    }

    /// <summary>
    /// char is number
    /// </summary>
    public static bool IsNumber(this char c)
    {
        switch (c)
        {
            case '0':
            case '1':
            case '2':
            case '3':
            case '4':
            case '5':
            case '6':
            case '7':
            case '8':
            case '9':
                return true;

            default:
                return false;
        }
    }

    /// <summary>
    /// Strip Html (Strip - Loại bỏ)
    /// </summary>
    public static string StripHtml(this string input)
    {
        return Regex.Replace(input, "<.*?>", string.Empty);
    }
    
}