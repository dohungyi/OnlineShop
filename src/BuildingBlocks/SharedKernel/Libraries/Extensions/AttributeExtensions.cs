using System.ComponentModel;
using System.Reflection;

namespace SharedKernel.Libraries;

public static class AttributeExtensions
{
    public static string GetDescription(this PropertyInfo property)
    {
        if (property == null)
            return "";

        if (!Attribute.IsDefined(property, typeof(DescriptionAttribute)))
            return "";

        return (Attribute.GetCustomAttribute(property, typeof(DescriptionAttribute)) as DescriptionAttribute)?.Description;
    }

    public static string GetDisplayText(this PropertyInfo property)
    {
        if (property == null)
            return "";

        if (!Attribute.IsDefined(property, typeof(DisplayTextAttribute)))
            return "";

        return (Attribute.GetCustomAttribute(property, typeof(DisplayTextAttribute)) as DisplayTextAttribute)?.Description;
    }
}