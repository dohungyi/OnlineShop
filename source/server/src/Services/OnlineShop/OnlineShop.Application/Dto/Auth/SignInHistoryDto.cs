using OnlineShop.Application.Mappings;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Dto.Auth;

public class SignInHistoryDto : IMapFrom<SignInHistory>
{
    public string Id { get; set; }

    public DateTime SignInTime { get; set; }

    public string Ip { get; set; }

    public string Browser { get; set; }

    public string OS { get; set; }

    public string Device { get; set; }

    public string UA { get; set; }

    public string MAC { get; set; }

    public string City { get; set; }

    public string Country { get; set; }

    public string Lat { get; set; }

    public string Long { get; set; }

    public string Timezone { get; set; }

    public string Org { get; set; }

    public string Postal { get; set; }

    public string Origin { get; set; }
}