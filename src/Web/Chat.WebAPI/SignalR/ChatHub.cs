﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Chat.Domain.DTOs.Messages;
using Chat.Application.SignalR;

namespace Chat.WebAPI.SignalR;

[Authorize]
public sealed class ChatHub : Hub<IChatClient>, IChatHub
{
    public async Task SendMessage(string conversationId, MessageWithSenderDto message)
    {
        await Clients.Group(conversationId).ReceiveMessage(message);
    }

    public async Task UpdateMessage(string conversationId, MessageWithSenderDto message)
    {
        await Clients.Group(conversationId).UpdateMessage(message);
    }

    public async Task JoinConversations(string[] conversationIds)
    {
        foreach (var conversationId in conversationIds.Where(x => !string.IsNullOrEmpty(x)))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
        }
    }
    
    public async Task Join(string channel)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, channel);
        await Clients.OthersInGroup(channel).Join(Context.ConnectionId);
    }
    
    public async Task Leave(string channel)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, channel);
        await Clients.OthersInGroup(channel).Leave(Context.ConnectionId);
    }

    public async Task SignalWebRtc(string channel, string type, string payload)
    {
        await Clients.OthersInGroup(channel).SignalWebRtc(channel, type, payload);
    }

    public async Task Offer(string channel, string offer)
    {
        await Clients.OthersInGroup(channel).ReceiveOffer(offer);
    }
    public async Task Answer(string channel, string answer)
    {
        await Clients.OthersInGroup(channel).ReceiveAnswer(answer);
    }
    public async Task Candidate(string channel, string candidate)
    {
        await Clients.OthersInGroup(channel).ReceiveCandidate(candidate);
    }
}