using System.Net;

namespace SharedKernel.Application;

public class Error
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string Type { get; set; }

    public Error()
    {
    }

    public Error(int code, string message)
    {
        StatusCode = code;
        Message = message;
    }

    public Error(HttpStatusCode code, string message)
    {
        StatusCode = (int)code;
        Message = message;
    }

    public Error(int code, string message, string type)
    {
        StatusCode = code;
        Message = message;
        Type = type;
    }

    public Error(HttpStatusCode code, string message, string type)
    {
        StatusCode = (int)code;
        Message = message;
        Type = type;
    }
}