using Chat.Domain.DTOs.Messages;

namespace Chat.Application.SignalR;

public interface IChatClient
{
    Task ReceiveMessage(MessageWithSenderDto message);
}