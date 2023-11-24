using Chat.Domain.DTOs.Calls;
using Chat.Domain.DTOs.Messages;

namespace Chat.WebUI.Services.SignalR;

public interface IHubConnectionService
{
    event Func<MessageWithSenderDto, Task>? ReceivedMessage;
    event Func<MessageWithSenderDto, Task>? UpdatedMessage;
    event Func<MessageDto, Task>? DeletedMessage;
    event Func<CallDto, Task>? ReceivedCallRequest;
    event Func<CallDto, Task>? ReceivedCallAnswer;
    Task ConnectAsync();
    Task JoinConversationsAsync(string[] conversationIds);
    Task SendMessageAsync(string conversationId, MessageWithSenderDto message);
    Task UpdateMessageAsync(string conversationId, MessageWithSenderDto message);
    Task DeleteMessageAsync(string conversationId, MessageDto message);
    Task CallUserAsync(CallDto call);
    Task AnswerCallAsync(CallDto call);
}