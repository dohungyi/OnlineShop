using MediatR;
using SharedKernel.Auth;

namespace SharedKernel.Domain.Events.BaseEvents;

public class DomainEvent : INotification
{
    public Guid EventId { get; protected set; }
    public DateTime Timestamp { get; protected set; }
    public string EventType => GetNameWithoutGenericArity(GetType());
    public object Body { get; protected set; }
    public string EventQueue { get; protected set; }
    public IToken Token { get; protected set; }
    
    public DomainEvent(Guid eventId, object body, IToken token)
    {
        EventId = eventId;
        Body = body;
        Token = token;
    }

    public void SetBody(object body)
    {
        Body = body;
    }

    private string GetNameWithoutGenericArity(Type t)
    {
        string name = t.Name;
        int index = name.IndexOf('`');
        return index == -1 ? name : name.Substring(0, index);
    }
}