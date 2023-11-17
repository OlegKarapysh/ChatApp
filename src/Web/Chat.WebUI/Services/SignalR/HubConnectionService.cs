using Microsoft.AspNetCore.SignalR.Client;
using Chat.Application.SignalR;
using Chat.Domain.DTOs.Messages;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Services.SignalR;

public sealed class HubConnectionService : IHubConnectionService
{
    public event Func<MessageWithSenderDto, Task>? ReceivedMessage;
    public event Func<MessageWithSenderDto, Task>? UpdatedMessage;
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
        _connection.On<MessageWithSenderDto>(nameof(IChatClient.ReceiveMessage), OnReceivedMessageAsync);
        _connection.On<MessageWithSenderDto>(nameof(IChatClient.UpdateMessage), OnUpdatedMessageAsync);
        await _connection.StartAsync();
    }

    public async Task JoinConversationsAsync(string[] conversationIds)
    {
        await InvokeHubMethodAsync(() => _connection?.InvokeAsync(
            nameof(IChatHub.JoinConversations), conversationIds));
    }

    public async Task SendMessageAsync(string conversationId, MessageWithSenderDto message)
    {
        await InvokeHubMethodAsync(() => _connection?.InvokeAsync(
            nameof(IChatHub.SendMessage), conversationId, message));
    }

    public async Task UpdateMessageAsync(string conversationId, MessageWithSenderDto message)
    {
        await InvokeHubMethodAsync(() => _connection?.InvokeAsync(
            nameof(IChatHub.UpdateMessage), conversationId, message));
    }

    private async Task InvokeHubMethodAsync(Func<Task?> methodCall)
    {
        if (_connection is null)
        {
            await ConnectAsync();
        }

        var task = methodCall.Invoke();
        if (task is null)
        {
            return;
        }

        await task;
    }
    
    private async Task OnUpdatedMessageAsync(MessageWithSenderDto message)
    {
        await InvokeEventAsync(UpdatedMessage, message);
    }

    private async Task OnReceivedMessageAsync(MessageWithSenderDto message)
    {
        await InvokeEventAsync(ReceivedMessage, message);
    }

    private async Task InvokeEventAsync<T>(Func<T, Task>? eventFunc, T parameter)
    {
        Func<T, Task>? eventHandler;
        lock (this)
        {
            eventHandler = eventFunc;
        }
        if (eventHandler is not null)
        {
            await eventHandler.Invoke(parameter);
        }
    }
}