namespace SharedKernel.Libraries;

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