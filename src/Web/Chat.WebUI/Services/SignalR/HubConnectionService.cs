using Chat.Domain.DTOs.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Services.SignalR;

public sealed class HubConnectionService : IHubConnectionService
{
    public event Func<MessageWithSenderDto, Task>? ReceivedMessage;
    private readonly ITokenStorageService _tokenService;
    private readonly string _hubUrl;
    private HubConnection? _connection;
    
    public HubConnectionService(ITokenStorageService tokenService, IConfiguration configuration)
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
                              options.AccessTokenProvider = async () => (await _tokenService.GetTokensAsync()).AccessToken;
                          })
                      .WithAutomaticReconnect()
                      .Build();
        _connection.On<MessageWithSenderDto>("ReceiveMessage", OnReceivedMessage);
        await _connection.StartAsync();
    }

    public async Task JoinConversationsAsync(string[] conversationIds)
    {
        if (_connection is null)
        {
            await ConnectAsync();
        }
        var task = _connection?.InvokeAsync("JoinConversations", conversationIds);
        if (task is null)
        {
            return;
        }

        await task;
    }

    public async Task SendMessageAsync(string conversationId, MessageWithSenderDto message)
    {
        if (_connection is null)
        {
            await ConnectAsync();
        }
        var task = _connection?.InvokeAsync("SendMessage", conversationId, message);
        if (task is null)
        {
            return;
        }

        await task;
    }

    private async Task OnReceivedMessage(MessageWithSenderDto message)
    {
        var task = ReceivedMessage?.Invoke(message);
        if (task is null)
        {
            return;
        }

        await task;
    }
}