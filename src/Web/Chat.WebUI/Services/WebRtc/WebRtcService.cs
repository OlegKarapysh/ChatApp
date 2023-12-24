namespace Chat.WebUI.Services.WebRtc;

public sealed class WebRtcService : IWebRtcService
{
    public event Func<IJSObjectReference, Task>? RemoteStreamAcquired;
    public event Func<string, Task>? InterlocutorLeft;
  
    private readonly IJSRuntime _js;
    private readonly ISignallingConnectionService _connectionService;
    private IJSObjectReference _jsModule;
    private DotNetObjectReference<WebRtcService>? _jsThis;
    private string _signalingChannel = string.Empty;

    public WebRtcService(IJSRuntime js, ISignallingConnectionService connectionService)
    {
        _js = js;
        _connectionService = connectionService;
        _connectionService.ReceivedWebRtcSignal += OnSignalReceived;
        _connectionService.InterlocutorLeft += OnInterlocutorLeft;
    }
    
    public void Dispose()
    {
        _connectionService.ReceivedWebRtcSignal -= OnSignalReceived;
        _connectionService.InterlocutorLeft -= OnInterlocutorLeft;
    }

    public async Task InitializeAsync(string signalingChannel)
    {
        Console.WriteLine("Joining in webRTC service...");
        _signalingChannel = signalingChannel;
        _jsModule = await _js.InvokeAsync<IJSObjectReference>("import", "/js/WebRtcService.cs.js");
        await _connectionService.InvokeHubMethodAsync(
            connection => connection?.SendAsync(nameof(IChatHub.Join), signalingChannel));
        _jsThis = DotNetObjectReference.Create(this);
        await _jsModule.InvokeVoidAsync("initialize", _jsThis);
    }
    
    public async Task<IJSObjectReference> StartLocalStreamAsync()
    {
        return await _jsModule.InvokeAsync<IJSObjectReference>("startLocalStream");
    }
    
    public async Task Call()
    {
        var offerDescription = await _jsModule.InvokeAsync<string>("callAction");
        await SendOffer(offerDescription);
    }

    public async Task Hangup()
    {
        await _jsModule.InvokeVoidAsync("hangupAction");
        await _connectionService.InvokeHubMethodAsync(
            connection => connection?.SendAsync(nameof(IChatHub.Leave), _signalingChannel));
        _signalingChannel = null;
    }
    
    [JSInvokable]
    public async Task SendOffer(string offer)
    {
        await _connectionService.InvokeHubMethodAsync(
            connection => connection?.SendAsync(nameof(IChatHub.SignalWebRtc), new WebRtcSignalDto
            {
                Channel = _signalingChannel,
                SignalType = WebRtcSignalType.Offer,
                PayloadJson = offer
            }));
    }

    [JSInvokable]
    public async Task SendAnswer(string answer)
    {
        await _connectionService.InvokeHubMethodAsync(
            connection => connection?.SendAsync(nameof(IChatHub.SignalWebRtc), new WebRtcSignalDto
            {
                Channel = _signalingChannel,
                SignalType = WebRtcSignalType.Answer,
                PayloadJson = answer
            }));
    }

    [JSInvokable]
    public async Task SendCandidate(string candidate)
    {
        await _connectionService.InvokeHubMethodAsync(
            connection => connection?.SendAsync(nameof(IChatHub.SignalWebRtc), new WebRtcSignalDto
            {
                Channel = _signalingChannel,
                SignalType = WebRtcSignalType.Candidate,
                PayloadJson = candidate
            }));
    }

    [JSInvokable]
    public async Task SetRemoteStream()
    {
        var stream = await _jsModule.InvokeAsync<IJSObjectReference>("getRemoteStream");
        var invocation = RemoteStreamAcquired?.Invoke(stream);
        if (invocation is not null)
        {
            await invocation;
        }
    }

    private async Task OnSignalReceived(WebRtcSignalDto signal)
    {
        if (_signalingChannel != signal.Channel)
        {
            return;
        }

        var invokeJsTask = signal.SignalType switch
        {
            WebRtcSignalType.Offer => _jsModule.InvokeVoidAsync("processOffer", signal.PayloadJson),
            WebRtcSignalType.Answer => _jsModule.InvokeVoidAsync("processAnswer", signal.PayloadJson),
            WebRtcSignalType.Candidate => _jsModule.InvokeVoidAsync("processCandidate", signal.PayloadJson),
            _ => throw new InvalidOperationException($"{signal.SignalType} web RTC signal is not supported!")
        };

        await invokeJsTask;
    }

    private async Task OnInterlocutorLeft(string id)
    {
        var task = InterlocutorLeft?.Invoke(id);
        if (task is not null)
        {
            await task;
        }
    }
}