using Chat.Domain.DTOs.Messages;

namespace Chat.Application.SignalR;

public interface IChatHub
{
    void SendMessage(string conversationId, MessageWithSenderDto message);
    Task JoinConversations(string[] conversationIds);
}