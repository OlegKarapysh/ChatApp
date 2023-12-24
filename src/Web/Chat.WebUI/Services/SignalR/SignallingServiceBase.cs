namespace Chat.WebUI.Services.SignalR;

public abstract class SignallingServiceBase
{
    private protected readonly ISignallingConnectionService ConnectionService;

    public SignallingServiceBase(ISignallingConnectionService connectionService)
    {
        ConnectionService = connectionService;
    }
}