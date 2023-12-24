namespace Chat.WebUI.Services.SignalR;

public interface ISignallingConnectionService
{
    event Func<MessageWithSenderDto, Task>? ReceivedMessage;
    event Func<MessageWithSenderDto, Task>? UpdatedMessage;
    event Func<MessageDto, Task>? DeletedMessage;
    event Func<CallDto, Task>? ReceivedCallRequest;
    event Func<CallDto, Task>? ReceivedCallAnswer;
    event Func<WebRtcSignalDto, Task>? ReceivedWebRtcSignal;
    event Func<string, Task>? InterlocutorLeft;
    Task ConnectAsync();
    Task InvokeHubMethodAsync(Func<HubConnection?, Task?> methodCall);
}