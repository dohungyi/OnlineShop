using System.Net;

namespace SharedKernel.Application.Responses;

public class ApiResult
{
    public bool Status
    {
        get
        {
            if (Error is null)
            {
                return true;
            }
            return false;
        }
    }
    public int StatusCode { get; set; }
    public Error Error { get; set; }
    
    public ApiResult()
    {
        StatusCode = (int)HttpStatusCode.OK;
    }
    
    public ApiResult(HttpStatusCode statusCode)
    {
        StatusCode = (int)statusCode;
    }
}