namespace SharedKernel.Libraries;

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
