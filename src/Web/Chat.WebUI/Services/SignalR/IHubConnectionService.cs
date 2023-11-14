using Chat.Domain.DTOs.Messages;

namespace Chat.WebUI.Services.SignalR;

public interface IHubConnectionService
{
    event Func<MessageWithSenderDto, Task>? ReceivedMessage;
    Task ConnectAsync();
    Task JoinConversationsAsync(string[] conversationIds);
    Task SendMessageAsync(string conversationId, MessageWithSenderDto message);
}