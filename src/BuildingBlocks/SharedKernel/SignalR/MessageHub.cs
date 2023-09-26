using Microsoft.AspNetCore.SignalR;

namespace SharedKernel.SignalR;

public class MessageHub : Hub
{
    public async Task ReceiveMessage(string message)
    {
        throw new NotImplementedException();
    }

    public async Task GetMessage()
    {
        throw new NotImplementedException();
    }
}