namespace SharedKernel.Libraries;

/// <summary>
/// Filterable: Có thể lọc
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class FilterableAttribute : Attribute
{
    public readonly string displayName;

    public FilterableAttribute(string displayName)
    {
        this.displayName = displayName;
    }
}