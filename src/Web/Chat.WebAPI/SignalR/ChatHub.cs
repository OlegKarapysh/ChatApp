using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Chat.Domain.DTOs.Messages;
using Chat.Application.SignalR;

namespace Chat.WebAPI.SignalR;

[Authorize]
public sealed class ChatHub : Hub<IChatClient>, IChatHub
{
    public void SendMessage(string conversationId, MessageWithSenderDto message)
    {
        Clients.Group(conversationId).ReceiveMessage(message);
    }

    public async Task JoinConversations(string[] conversationIds)
    {
        foreach (var conversationId in conversationIds.Where(x => !string.IsNullOrEmpty(x)))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }
    }
}