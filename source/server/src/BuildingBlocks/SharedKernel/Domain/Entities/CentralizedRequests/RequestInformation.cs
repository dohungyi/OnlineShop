using SharedKernel.Libraries;

namespace SharedKernel.Domain;

public class RequestInformation
{
    public Guid Id { get; set; }

    public string RequestId { get; set; }

    public string Ip { get; set; }

    public string Method { get; set; }

    public string ApiUrl { get; set; }

    public string Browser { get; set; }

    public string OS { get; set; }

    public string Device { get; set; }

    public string? UA { get; set; }

    public string? Origin { get; set; }

    public DateTime CreatedDate { get; set; }

    public RequestInformation()
    {
    }

    public RequestInformation(string requestId, string ip, string method, string apiUrl, string browser, string os, string device, string ua, string origin)
    {
        RequestId = requestId;
        Ip = ip;
        Method = method;
        ApiUrl = apiUrl;
        Browser = browser;
        OS = os;
        Device = device;
        UA = ua;
        Origin = origin;
        CreatedDate = DateHelper.Now;
    }
}