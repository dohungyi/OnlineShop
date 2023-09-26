namespace SharedKernel.SignalR;

public interface IMessageHub
{
    Task ReceiveMessage(string message);

    Task GetMessage();
}