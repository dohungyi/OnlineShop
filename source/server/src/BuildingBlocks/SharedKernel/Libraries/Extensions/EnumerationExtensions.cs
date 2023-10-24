using System.ComponentModel;
using System.Reflection;

namespace SharedKernel.Libraries;

public static class EnumerationExtensions
{
    /// <summary>
    /// Get description of enum
    /// </summary>
    public static string GetDescription(this Enum en)
    {
        // Get the Description attribute value for the enum value
        FieldInfo fi = en.GetType().GetField(en.ToString());
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

        if (attributes.Length > 0)
            return attributes[0].Description;
        else
            return en.ToString();
    }

    public static List<object> GetEnumerationValues(this Type type)
    {
        var result = new List<object>();
        var fieldInfos = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        foreach (var field in fieldInfos)
        {
            if (field.IsLiteral && !field.IsInitOnly)
            {
                result.Add(field.GetValue(type)!);
            }
        }

        return result;
    }
}