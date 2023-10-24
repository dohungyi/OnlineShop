using System.Net;

namespace SharedKernel.Application;

public class ApiResult
{
    protected string _status = "success";

    public string Status
    {
        get
        {
            if (_status == "success" && Error != null)
            {
                _status = "error";
            }
            return _status;
        }
        set { _status = value; }
    }

    public Error Error { get; set; }
    
    public ApiResult()
    {
    }
    
    
}