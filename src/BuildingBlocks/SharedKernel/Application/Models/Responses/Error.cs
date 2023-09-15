using System.Net;

namespace SharedKernel.Application.Responses;

public class Error
{
    public int Code { get; set; }

    public string Message { get; set; }

    public string Type { get; set; }


    public Error()
    {
    }

    public Error(int code, string message)
    {
        Code = code;
        Message = message;
    }

    public Error(HttpStatusCode code, string message)
    {
        Code = (int)code;
        Message = message;
    }

    public Error(int code, string message, string type)
    {
        Code = code;
        Message = message;
        Type = type;
    }

    public Error(HttpStatusCode code, string message, string type)
    {
        Code = (int)code;
        Message = message;
        Type = type;
    }
}