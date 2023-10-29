using OnlineShop.Application.Mappings;
using OnlineShop.Domain.Entities;
using SharedKernel.Application;

namespace OnlineShop.Application.Dtos;

public class UserDto : IMapFrom<ApplicationUser>
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string PhoneNumber { get; set; }

    public bool ConfirmedPhone { get; set; }

    public string Email { get; set; }

    public bool ConfirmedEmail { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string FullName => FirstName + " " + LastName;

    public string Address { get; set; }

    public DateTime DateOfBirth { get; set; }

    public GenderType Gender { get; set; }

    public DateTime CreatedDate { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ApplicationUser, UserDto>().ReverseMap();
        profile.CreateMap<IPagedList<ApplicationUser>, PagedList<UserDto>>().ReverseMap();
    }
}