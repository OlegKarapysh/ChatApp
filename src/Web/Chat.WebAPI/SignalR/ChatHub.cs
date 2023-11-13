using Chat.Application.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Chat.Domain.DTOs.Conversations;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.Entities;

namespace Chat.WebAPI.SignalR;

[Authorize]
public sealed class ChatHub : Hub<IChatClient>, IChatHub
{
    public override Task OnConnectedAsync()
    {
        return Task.CompletedTask;
    }

    // TODO: create single Dto for all information
    public void SendMessage(ConversationDto conversation, MessageDto message)
    {
        //Clients.Group(conversation.Id).ReceiveMessage(message);
    }

    public async Task JoinConversation(string conversationId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
    }
}