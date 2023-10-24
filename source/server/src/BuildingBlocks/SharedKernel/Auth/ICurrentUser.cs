using Microsoft.AspNetCore.Http;

namespace SharedKernel.Auth;

public interface ICurrentUser
{
    string Id { get; }
    ExecutionContext Context { get; }
}

public class ExecutionContext
{
    public string AccessToken { get; set; }
    public string UserId { get; set; }
    public string Username { get; set; }
    public string Roles { get; set; }
    public string Permission { get; set; }
    public HttpContext HttpContext { get; set; }
}

