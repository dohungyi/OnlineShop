using SharedKernel.Domain;

namespace SharedKernel.Application;

public class TokenUser : User
{
    public string Permission { get; set; }

    public List<string> RoleNames { get; set; } = new List<string>();
}