using System.Net;

namespace SharedKernel.Application;

public class ApiSuccessResult : ApiResult
{
    public ApiSuccessResult()
    {
        StatusCode = (int)HttpStatusCode.OK;
    }
}

public class ApiSuccessResult<T> : ApiResult
{
    public T Data { get; set; }

    public ApiSuccessResult()
    {
        Data = default;
    }
    

    public ApiSuccessResult(T data)
    {
        Data = data;
        StatusCode = (int)HttpStatusCode.OK;
    }
}