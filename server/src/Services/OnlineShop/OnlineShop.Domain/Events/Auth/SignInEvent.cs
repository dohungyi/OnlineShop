using SharedKernel.Auth;

namespace OnlineShop.Domain.Events.Auth;

public class SignInEvent : DomainEvent
{
    public SignInEvent(ICurrentUser token, Guid eventId, object body) : base(eventId, body, token)
    {
    }
}