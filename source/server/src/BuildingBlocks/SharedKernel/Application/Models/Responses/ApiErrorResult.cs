namespace SharedKernel.Application;

public class ApiErrorResult : ApiResult
{
    public ApiErrorResult()
    {
        
    }
    
    public ApiErrorResult(Error error)
    {
        Error = error;
    }

    public ApiErrorResult(string status, Error error)
    {
        Status = status;
        Error = error;
    }
}