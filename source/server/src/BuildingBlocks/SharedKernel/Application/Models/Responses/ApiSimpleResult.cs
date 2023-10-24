namespace SharedKernel.Application;

public class ApiSimpleResult : ApiResult
{
    public object Data { get; set; }

    public ApiSimpleResult()
    {
        
    }

    public ApiSimpleResult(object data)
    {
        Data = data;
    }
}