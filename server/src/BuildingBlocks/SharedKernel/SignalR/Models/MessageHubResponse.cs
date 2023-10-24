namespace SharedKernel.SignalR;

public class MessageHubResponse
{
    public MessageHubType Type { get; set; } = MessageHubType.Message;

    public object Message { get; set; }
}