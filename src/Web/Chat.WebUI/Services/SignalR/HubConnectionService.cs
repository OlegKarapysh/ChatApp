using Chat.Domain.DTOs.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Services.SignalR;

public sealed class HubConnectionService
{
    public event Func<MessageWithSenderDto, Task>? ReceivedMessage;
    private readonly ITokenService _tokenService;
    private readonly string _hubUrl;
    private HubConnection? _connection;
    
    public HubConnectionService(ITokenService tokenService, IConfiguration configuration)
    {
        _tokenService = tokenService;
        _hubUrl = configuration["SignalR:HubUrl"]!;
    }

    public async Task ConnectAsync()
    {
        _connection = new HubConnectionBuilder()
                      .WithUrl(_hubUrl,
                          options =>
                          {
                              options.AccessTokenProvider = async () => (await _tokenService.GetTokens()).AccessToken;
                          })
                      .WithAutomaticReconnect()
                      .Build();
        _connection.On<MessageWithSenderDto>("ReceiveMessage", OnReceivedMessage);
        await _connection.StartAsync();
    }

    public async Task JoinConversationsAsync(string[] conversationIds)
    {
        await _connection?.InvokeAsync("JoinConversations", conversationIds);
    }

    public async Task SendMessageAsync(string conversationId, MessageWithSenderDto message)
    {
        await _connection?.InvokeAsync("SendMessage", conversationId, message);
    }

    private async Task OnReceivedMessage(MessageWithSenderDto message)
    {
        await ReceivedMessage?.Invoke(message);
    }
}