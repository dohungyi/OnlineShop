using Microsoft.AspNetCore.Http;

namespace SharedKernel.Application;

public class OpenHttpRequest
{
    public string Method { get; set; }

    public string Scheme { get; set; }

    public HostString Host { get; set; }

    public PathString Path { get; set; }

    public QueryString QueryString { get; set; }

    public IHeaderDictionary Headers { get; }

    public string Ip { get; set; }

    public OpenHttpRequest(string method, string scheme, HostString host, PathString path, QueryString queryString, IHeaderDictionary headers, string ip = "")
    {
        Method = method;
        Scheme = scheme;
        Host = host;
        Path = path;
        QueryString = queryString;
        Headers = headers;
        Ip = ip;
    }
}