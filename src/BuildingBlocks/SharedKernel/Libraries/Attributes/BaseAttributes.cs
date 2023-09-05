using System.ComponentModel;
using Enum = SharedKernel.Application.Enums.Enum;

namespace SharedKernel.Libraries.Attributes;

public class BaseAttributes
{
    public static List<Type> GetCommonIgnoreAttribute()
    {
        return new List<Type>()
        {
            typeof(IgnoreAttribute)
        };
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class IgnoreAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public class AutoIncAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public class DisplayTextAttribute : DescriptionAttribute
{
    public DisplayTextAttribute(string description) : base(description)
    {
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class FilterableAttribute : Attribute
{
    public readonly string displayName;

    public FilterableAttribute(string displayName)
    {
        this.displayName = displayName;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class AuditableAttribute : Attribute
{
    public bool UseInsert { get; }

    public bool UseDelete { get; }

    public AuditableAttribute(bool useInsert = false, bool useDelete = false)
    {
        UseInsert = useInsert;
        UseDelete = useDelete;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class AuthorizationRequestAttribute : Attribute
{
    public Enum.ActionExponent[] Exponents { get; } = new Enum.ActionExponent[] { Enum.ActionExponent.View };

    public AuthorizationRequestAttribute(Enum.ActionExponent[] exponents)
    {
        Exponents = Exponents.Concat(exponents).ToArray();
    }

    public AuthorizationRequestAttribute()
    {
    }
}