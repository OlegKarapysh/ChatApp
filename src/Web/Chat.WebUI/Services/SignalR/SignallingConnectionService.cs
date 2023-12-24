namespace Chat.WebUI.Services.SignalR;

public sealed class SignallingConnectionService : ISignallingConnectionService
{
    public event Func<MessageWithSenderDto, Task>? ReceivedMessage;
    public event Func<MessageWithSenderDto, Task>? UpdatedMessage;
    public event Func<MessageDto, Task>? DeletedMessage;
    public event Func<CallDto, Task>? ReceivedCallRequest;
    public event Func<CallDto, Task>? ReceivedCallAnswer;
    private HubConnection? _hubConnection;
    private readonly string _hubUrl;
    private readonly ITokenStorageService _tokenService;
    private readonly IJwtAuthService _jwtAuthService;
    private readonly IToastService _toastService;
    private readonly NavigationManager _navigationManager;
    
    public SignallingConnectionService(ITokenStorageService tokenService, IConfiguration configuration, IJwtAuthService jwtAuthService, NavigationManager navigationManager, IToastService toastService)
    {
        _tokenService = tokenService;
        _jwtAuthService = jwtAuthService;
        _navigationManager = navigationManager;
        _toastService = toastService;
        _hubUrl = configuration["SignalR:HubUrl"]!;
    }
    
    public async Task ConnectAsync()
    {
        _hubConnection = new HubConnectionBuilder()
                      .WithUrl(_hubUrl,
                          options =>
                          {
                              options.AccessTokenProvider = async () => (await _tokenService.GetTokensAsync()).AccessToken;
                          })
                      .WithAutomaticReconnect()
                      .Build();
        _hubConnection.On<MessageWithSenderDto>(nameof(IChatClient.ReceiveMessage), OnReceivedMessageAsync);
        _hubConnection.On<MessageWithSenderDto>(nameof(IChatClient.UpdateMessage), OnUpdatedMessageAsync);
        _hubConnection.On<MessageDto>(nameof(IChatClient.DeleteMessage), OnDeletedMessageAsync);
        _hubConnection.On<CallDto>(nameof(IChatClient.ReceiveCallRequest), OnReceivedCallRequest);
        _hubConnection.On<CallDto>(nameof(IChatClient.ReceiveCallAnswer), OnReceivedCallAnswer);
        await _hubConnection.StartAsync();
    }
    
    public async Task InvokeHubMethodAsync(Func<HubConnection?, Task?> methodCall)
    {
        try
        {
            if (_hubConnection is null)
            {
                await ConnectAsync();
            }

            var task = methodCall.Invoke(_hubConnection!);
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
}