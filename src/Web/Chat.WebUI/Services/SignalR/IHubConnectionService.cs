using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Messages;

namespace Chat.WebUI.Services.SignalR;

public interface IHubConnectionService
{
    event Func<MessageWithSenderDto, Task>? ReceivedMessage;
    event Func<MessageWithSenderDto, Task>? UpdatedMessage;
    event Func<MessageDto, Task>? DeletedMessage;
    event Func<ConversationDto, Task>? ReceivedCallRequest;
    event Func<ConversationDto, Task>? ReceivedCallAnswer;
    Task ConnectAsync();
    Task JoinConversationsAsync(string[] conversationIds);
    Task SendMessageAsync(string conversationId, MessageWithSenderDto message);
    Task UpdateMessageAsync(string conversationId, MessageWithSenderDto message);
    Task DeleteMessageAsync(string conversationId, MessageDto message);
    Task CallUserAsync(ConversationDto conversation);
    Task AnswerCallAsync(ConversationDto conversation);
}