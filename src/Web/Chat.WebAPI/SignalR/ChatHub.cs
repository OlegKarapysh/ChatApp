using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Chat.Domain.DTOs.Messages;
using Chat.Application.SignalR;

namespace Chat.WebAPI.SignalR;

[Authorize]
public sealed class ChatHub : Hub<IChatClient>, IChatHub
{
    public override async Task OnConnectedAsync()
    {
        try
        {
            await base.OnConnectedAsync();
            Console.WriteLine("Connected!");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // TODO: create single Dto for all information
    public void SendMessage(string conversationId, MessageWithSenderDto message)
    {
        Clients.Group(conversationId).ReceiveMessage(message);
        Console.WriteLine($"Sent '{message.TextContent}' message");
    }

    public async Task JoinConversations(string[] conversationIds)
    {
        foreach (var conversationId in conversationIds.Where(x => !string.IsNullOrEmpty(x)))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
            Console.WriteLine($"user {Context.ConnectionId} joined {conversationId}");
        }
    }
}