using System.Numerics;

namespace Console.Utility;

public static class AuthUtility
{
    public static string FromExponentToPermission(int exponent)
    {
        var result = new BigInteger(1);
        var two = new BigInteger(2);

        for (int i = 1; i <= exponent; i++)
            result *= two;

        return result.ToString();
    }

    public static string CalculateToTalPermission(IEnumerable<int> exponents)
    {
        var result = new BigInteger(0);
        foreach (var exponent in exponents)
            result += BigInteger.Parse(FromExponentToPermission(exponent));
        return result.ToString();
    }

    public static string ConvertToBinary(string input)
    {
        var result = "";
        var parse = BigInteger.Parse(input);
        var two = new BigInteger(2);

        while (true)
        {
            var b = parse % two;
            parse = parse / two;
            result += b;
            if (parse.IsZero)
            {
                break;
            }
        }
        return string.Join("", result.Reverse());
    }

    public static bool ComparePermissionAsString(string permission, string actionPermission)
    {
        if (string.IsNullOrEmpty(permission) || string.IsNullOrEmpty(actionPermission))
        {
            return false;
        }
        var left = ConvertToBinary(permission);
        var right = ConvertToBinary(actionPermission);
        var offset = "";
        var andResult = "";

        // Ensure Left always greater than Right
        if (right.Length > left.Length)
        {
            var tmp = left;
            left = right;
            right = tmp;
        }

        // Compensate for the number on the right
        for (int i = 1; i <= left.Length - right.Length; i++)
        {
            offset += "0";
        }
        right = offset + right;

        // Execute bitwise &
        for (int i = 0; i < left.Length; i++)
        {
            if (left[i] == right[i] && left[i] == '1')
                andResult += "1";
            else
                andResult += "0";
        }

        return andResult.Equals(right);
    }
}