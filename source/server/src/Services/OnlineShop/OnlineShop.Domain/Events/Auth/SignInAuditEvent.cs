using SharedKernel.Auth;

namespace OnlineShop.Domain.Events.Auth;

public class SignInAuditEvent : AuditEvent
{
    public SignInAuditEvent(ICurrentUser token, Guid eventId = default) : base("SignIn", AuditAction.SignIn, token, eventId)
    {
        
    }
}