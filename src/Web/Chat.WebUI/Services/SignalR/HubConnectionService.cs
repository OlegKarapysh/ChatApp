namespace Chat.WebUI.Services.SignalR;

public sealed class HubConnectionService : IHubConnectionService
{
    public event Func<MessageWithSenderDto, Task>? ReceivedMessage;
    public event Func<MessageWithSenderDto, Task>? UpdatedMessage;
    public event Func<MessageDto, Task>? DeletedMessage;
    public event Func<CallDto, Task>? ReceivedCallRequest;
    public event Func<CallDto, Task>? ReceivedCallAnswer;
    private readonly ITokenStorageService _tokenService;
    private readonly IJwtAuthService _jwtAuthService;
    private readonly IToastService _toastService;
    private readonly NavigationManager _navigationManager;
    private readonly string _hubUrl;
    private HubConnection? _connection;
    
    public HubConnectionService(ITokenStorageService tokenService, IConfiguration configuration, IJwtAuthService jwtAuthService, NavigationManager navigationManager, IToastService toastService)
    {
        _tokenService = tokenService;
        _jwtAuthService = jwtAuthService;
        _navigationManager = navigationManager;
        _toastService = toastService;
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
        _connection.On<MessageDto>(nameof(IChatClient.DeleteMessage), OnDeletedMessageAsync);
        _connection.On<CallDto>(nameof(IChatClient.ReceiveCallRequest), OnReceivedCallRequest);
        _connection.On<CallDto>(nameof(IChatClient.ReceiveCallAnswer), OnReceivedCallAnswer);
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

    public async Task DeleteMessageAsync(string conversationId, MessageDto message)
    {
        await InvokeHubMethodAsync(() => _connection?.InvokeAsync(
            nameof(IChatHub.DeleteMessage), conversationId, message));
    }

    public async Task CallUserAsync(CallDto call)
    {
        await InvokeHubMethodAsync(() => _connection?.InvokeAsync(nameof(IChatHub.CallUser), call));
    }

    public async Task AnswerCallAsync(CallDto call)
    {
        await InvokeHubMethodAsync(() => _connection?.InvokeAsync(nameof(IChatHub.AnswerCall), call));
    }

    private async Task InvokeHubMethodAsync(Func<Task?> methodCall)
    {
        try
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            // TODO: refresh
            await _jwtAuthService.LogoutAsync();
            _navigationManager.NavigateTo("/login", true);
            _toastService.ShowInfo("Session expired");
        }
    }

    private async Task OnReceivedMessageAsync(MessageWithSenderDto message)
    {
        await InvokeEventAsync(ReceivedMessage, message);
    }
    
    private async Task OnUpdatedMessageAsync(MessageWithSenderDto message)
    {
        await InvokeEventAsync(UpdatedMessage, message);
    }

    private async Task OnDeletedMessageAsync(MessageDto message)
    {
        await InvokeEventAsync(DeletedMessage, message);
    }

    private async Task OnReceivedCallRequest(CallDto call)
    {
        await InvokeEventAsync(ReceivedCallRequest, call);
    }
    
    private async Task OnReceivedCallAnswer(CallDto call)
    {
        await InvokeEventAsync(ReceivedCallAnswer, call);
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