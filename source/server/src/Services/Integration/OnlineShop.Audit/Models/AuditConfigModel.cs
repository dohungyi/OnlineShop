namespace OnlineShop.Audit.Models;

public class AuditConfigModel
{
    public string Module { get; set; }

    public AuditConfigModel()
    {
    }

    public AuditConfigModel(string module)
    {
        Module = module;
    }
}