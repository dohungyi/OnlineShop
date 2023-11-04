namespace OnlineShop.Application.Features.VersionOne;

public class CreateUserCommandHandler : BaseCommandHandler, IRequestHandler<CreateUserCommand, string>
{
    
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _userWriteOnlyRepository;
    private readonly IStringLocalizer<Resources> _localizer;
    private readonly IMapper _mapper;
    
    public CreateUserCommandHandler(
        IEventDispatcher eventDispatcher, 
        IAuthService authService,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUserWriteOnlyRepository userWriteOnlyRepository,
        IStringLocalizer<Resources> localizer,
        IMapper mapper
        ) : base(eventDispatcher, authService)
    {
        _userReadOnlyRepository = userReadOnlyRepository;
        _userWriteOnlyRepository = userWriteOnlyRepository;
        _localizer = localizer;
        _mapper = mapper;
    }

    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        await ValidateAndThrowAsync(request, cancellationToken);

        var user = _mapper.Map<ApplicationUser>(request);
        
        await _userWriteOnlyRepository.CreateUserAsync(user, cancellationToken);
        await _userWriteOnlyRepository.UnitOfWork.CommitAsync(cancellationToken: cancellationToken);
        
        return user.Id.ToString();
    }

    private async Task ValidateAndThrowAsync(CreateUserCommand request, CancellationToken cancellationToken = default)
    {
        var type = await _userReadOnlyRepository.CheckDuplicateAsync(request.Username, request.Email, request.PhoneNumber, cancellationToken);
        if (!string.IsNullOrEmpty(type))
        {
            switch (type)
            {
                case "username":
                    throw new BadRequestException(_localizer["auth_user_already_exist"].Value);
                case "email":
                    throw new BadRequestException(_localizer["auth_email_already_exist"].Value);
                case "phone":
                    throw new BadRequestException(_localizer["auth_phone_already_exist"].Value);
                default:
                    break;
            }
        }
    }
}