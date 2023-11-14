using Microsoft.AspNetCore.SignalR.Client;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Services.SignalR;

public sealed class HubConnectionService
{
    public event Func<string, Task>? ReceivedMessage;
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
        _connection.On<string>("ReceiveMessage", async message => await OnReceivedMessageAsync(message));
        await _connection.StartAsync();
    }

    public async Task JoinConversationGroupsAsync(string[] conversationIds)
    {
        await _connection?.InvokeAsync("JoinConversationGroups", conversationIds);
    }

    protected async Task OnReceivedMessageAsync(string e)
    {
        await ReceivedMessage?.Invoke(e);
    }
}