using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;

namespace Chat.Application.SignalR;

public interface IChatClient
{
    // TODO: create single Dto for all information
    Task ReceiveMessage(MessageWithSenderDto message);
}