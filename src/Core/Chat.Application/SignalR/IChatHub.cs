using Chat.Domain.DTOs.Messages;

namespace Chat.Application.SignalR;

public interface IChatHub
{
    Task SendMessage(string conversationId, MessageWithSenderDto message);
    Task UpdateMessage(string conversationId, MessageWithSenderDto message);
    Task DeleteMessage(string conversationId, MessageDto message);
    Task JoinConversations(string[] conversationIds);
}