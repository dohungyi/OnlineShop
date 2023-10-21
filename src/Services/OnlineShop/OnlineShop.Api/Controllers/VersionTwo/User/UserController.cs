using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Api.Controllers.VersionTwo;

public class UserController : BaseController
{
    [AllowAnonymous]
    [HttpGet("shop-owner-information")]
    public async Task<IActionResult> GetAsync(string name)
    {
        var result = new ApiSimpleResult()
        {
            Data = new
            {
                FullName = "Đỗ Chí Hùng",
                DateOfBirth = new DateTime(2002, 09, 06),
                Phone = "0976580418",
                Email = "dohung.csharp@gmail.com",
                Facebook = "https://www.facebook.com/dohungiy",
                MostBeautifulDay = "Ngày em đẹp nhất là ngày anh chưa có gì trong tay!"
            }
        };
        
        return Ok(result);
    }
}