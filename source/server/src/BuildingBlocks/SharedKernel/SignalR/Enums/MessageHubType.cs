namespace SharedKernel.SignalR;

public enum MessageHubType
{
    Message = 0,
    SignIn,
    SignOut,
    OnlineUser,
    UpdateRole,
    ReceivedFile,
    NewFeedback,
    SomeOneTyping,
}