namespace Chat.WebUI.Services.SignalR;

public sealed class VideoCallSignallingService : SignallingServiceBase, IVideoCallSignallingService
{
    public VideoCallSignallingService(ISignallingConnectionService connectionService) : base(connectionService)
    {
    }
    
    public async Task CallUserAsync(CallDto call)
    {
        await ConnectionService.InvokeHubMethodAsync(connection => connection?.InvokeAsync(nameof(IChatHub.CallUser), call));
    }

    public async Task AnswerCallAsync(CallDto call)
    {
        await ConnectionService.InvokeHubMethodAsync(connection => connection?.InvokeAsync(nameof(IChatHub.AnswerCall), call));
    }
}