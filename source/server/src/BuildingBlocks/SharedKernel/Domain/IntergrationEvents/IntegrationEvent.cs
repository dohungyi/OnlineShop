namespace SharedKernel.Domain;

public class IntegrationEvent
{
    public string EventId { get; protected set; }

    public DateTime Timestamp { get; protected set; }

    public string EventType { get; protected set; }

    public object Body { get; protected set; }

    public IntegrationEvent(string eventId, DateTime timestamp, string eventType, object body)
    {
        EventId = eventId;
        Timestamp = timestamp;
        EventType = eventType;
        Body = body;
    }
}