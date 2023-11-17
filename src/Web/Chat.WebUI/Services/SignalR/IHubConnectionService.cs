using Chat.Domain.DTOs.Messages;

namespace Chat.WebUI.Services.SignalR;

public interface IHubConnectionService
{
    event Func<MessageWithSenderDto, Task>? ReceivedMessage;
    event Func<MessageWithSenderDto, Task>? UpdatedMessage;
    Task ConnectAsync();
    Task JoinConversationsAsync(string[] conversationIds);
    Task SendMessageAsync(string conversationId, MessageWithSenderDto message);
    Task UpdateMessageAsync(string conversationId, MessageWithSenderDto message);
}