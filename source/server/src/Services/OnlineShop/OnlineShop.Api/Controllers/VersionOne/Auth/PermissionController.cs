using Microsoft.AspNetCore.Mvc;
using SharedKernel.Libraries.Utility;
using SharedKernel.Runtime.Exceptions;

namespace OnlineShop.Api.Controllers.VersionOne;

public class PermissionController : BaseController
{
    [HttpGet("check")]
    public async Task<IActionResult> GetAsync(string p, string a)
    {
        var result = new ApiSimpleResult { Data = AuthUtility.ComparePermissionAsString(p, a) };

        return Ok(result);
    }
    
    [HttpGet("fetp")]
    public IActionResult ConvertToPermission(int exponent)
    {
        if (exponent < 0 || exponent > 100000)
        {
            throw new BadRequestException("The exponent is not valid");
        }
        
        return Ok(new ApiSimpleResult { Data = AuthUtility.FromExponentToPermission(exponent) });
    }
}