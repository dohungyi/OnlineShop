using System.Net;

namespace SharedKernel.Application.Responses;

public class ApiErrorResult : ApiResult
{
    public ApiErrorResult(Error error)
    {
        StatusCode = (int)HttpStatusCode.BadRequest;
        Error = error;
    }

    public ApiErrorResult(HttpStatusCode statusCode, Error error)
    {
        StatusCode = (int)statusCode;
        Error = error;
    }
}