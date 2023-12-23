namespace Chat.WebUI.Services.WebRtc;

public sealed class WebRtcService
{
    public event EventHandler<IJSObjectReference>? OnRemoteStreamAcquired;
    public event Func<string, Task>? InterlocutorLeft;
  
    private readonly IJSRuntime _js;
    private readonly ITokenStorageService _tokenService;
    private IJSObjectReference? _jsModule;
    private DotNetObjectReference<WebRtcService>? _jsThis;
    private HubConnection? _hub;
    private string? _signalingChannel;
    private readonly string _hubUrl;

    public WebRtcService(IJSRuntime js, ITokenStorageService tokenService, IConfiguration config)
    {
        _js = js;
        _tokenService = tokenService;
        _hubUrl = config["SignalR:HubUrl"]!;
    }

    public async Task Join(string signalingChannel)
    {
        if (_signalingChannel != null) throw new InvalidOperationException();

        Console.WriteLine("Joining in webRTC service...");
        _signalingChannel = signalingChannel;
        var hub = await GetHub();
        await hub.SendAsync("join", signalingChannel);
        _jsModule = await _js.InvokeAsync<IJSObjectReference>(
            "import", "/js/WebRtcService.cs.js");
        _jsThis = DotNetObjectReference.Create(this);
        await _jsModule.InvokeVoidAsync("initialize", _jsThis);
    }
    
    public async Task<IJSObjectReference> StartLocalStream()
    {
        if (_jsModule == null) throw new InvalidOperationException();
        
        var stream = await _jsModule.InvokeAsync<IJSObjectReference>("startLocalStream");
        return stream;
    }
    
    public async Task Call()
    {
        if (_jsModule == null) throw new InvalidOperationException();
        
        var offerDescription = await _jsModule.InvokeAsync<string>("callAction");
        await SendOffer(offerDescription);
    }

    public async Task Hangup()
    {
        if (_jsModule == null) throw new InvalidOperationException();
        
        await _jsModule.InvokeVoidAsync("hangupAction");
        var hub = await GetHub();
        await hub.SendAsync("leave", _signalingChannel);

        _signalingChannel = null;
    }
    
    [JSInvokable]
    public async Task SendOffer(string offer)
    {
        var hub = await GetHub();
        await hub.SendAsync("SignalWebRtc", _signalingChannel, "offer", offer);
    }

    [JSInvokable]
    public async Task SendAnswer(string answer)
    {
        var hub = await GetHub();
        await hub.SendAsync("SignalWebRtc", _signalingChannel, "answer", answer);
    }

    [JSInvokable]
    public async Task SendCandidate(string candidate)
    {
        var hub = await GetHub();
        await hub.SendAsync("SignalWebRtc", _signalingChannel, "candidate", candidate);
    }

    [JSInvokable]
    public async Task SetRemoteStream()
    {
        if (_jsModule == null) throw new InvalidOperationException();

        var stream = await _jsModule.InvokeAsync<IJSObjectReference>("getRemoteStream");
        OnRemoteStreamAcquired?.Invoke(this, stream);
    }
    
    private async Task<HubConnection> GetHub()
    {
        if (_hub != null) return _hub;

        var hub = new HubConnectionBuilder()
                  .WithUrl(_hubUrl,
                      options =>
                      {
                          options.AccessTokenProvider = async () => (await _tokenService.GetTokensAsync()).AccessToken;
                      })
                  .Build();
        hub.On<string>(nameof(IChatClient.Leave), id => InterlocutorLeft?.Invoke(id));
        hub.On<string, string, string>("SignalWebRtc", async (signalingChannel, type, payload) =>
        {
            if (_jsModule == null) throw new InvalidOperationException();

            if (_signalingChannel != signalingChannel) return;
            
            switch (type)
            {
                case "offer":
                    await _jsModule.InvokeVoidAsync("processOffer", payload);
                    break;
                case "answer":
                    await _jsModule.InvokeVoidAsync("processAnswer", payload);
                    break;
                case "candidate":
                    await _jsModule.InvokeVoidAsync("processCandidate", payload);
                    break;
            }
        });

        await hub.StartAsync();
        _hub = hub;
        return _hub;
    }
}