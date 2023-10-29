namespace OnlineShop.Application.Features.VersionOne;

[AuthorizationRequest(new ActionExponent[] { ActionExponent.SupperAdmin })]
public class CreateUserCommand : BaseInsertCommand<string>, IMapFrom<ApplicationUser>
{
    public string Username { get; init; }

    public string Password { get; init; }

    public string ConfirmPassword { get; init; }

    public string PhoneNumber { get; init; }
    
    public string Email  { get; init; }
    
    public string FirstName  { get; init; }
    
    public string LastName  { get; init; }

    public string FullName => $"{FirstName} {LastName}";
    
    public DateTime DateOfBirth { get; init; }
    
    public GenderType Gender { get; init; }
}