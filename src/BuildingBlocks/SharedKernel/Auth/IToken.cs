using Microsoft.AspNetCore.Http;

namespace SharedKernel.Auth;

public interface IToken
{
    string TokenId { get; }
    ExecutionContext Context { get; }
}

public class ExecutionContext
{
    public string AccessToken { get; set; }
    public long OwnerId { get; set; }
    public string Username { get; set; }
    public long TenantId { get; set; }
    public string Permission { get; set; }
    public HttpContext HttpContext { get; set; }
}

