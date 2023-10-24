namespace SharedKernel.Libraries;

public static class StringHelper
{

    /// <summary>
    /// Loại bỏ khoảng trắng thừa
    /// </summary>
    public static string RemoveExtraWhitespace(string statement)
    {
        if (string.IsNullOrEmpty(statement))
        {
            return "";
        }

        statement = statement.Trim();

        var currentLength = statement.Length;
        while (true)
        {
            statement = statement.Replace("  ", " ");
            if (currentLength == statement.Length)
            {
                return statement;
            }
            currentLength = statement.Length;
        }
    }
}