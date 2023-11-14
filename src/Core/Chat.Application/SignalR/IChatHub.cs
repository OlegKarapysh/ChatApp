using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Messages;

namespace Chat.Application.SignalR;

public interface IChatHub
{
    void SendMessage(string conversationId, MessageDto message);
    Task JoinConversations(string[] conversationIds);
}