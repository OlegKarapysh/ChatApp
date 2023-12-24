namespace Chat.WebUI.Services.WebRtc;

public interface IWebRtcService : IDisposable
{
    event EventHandler<IJSObjectReference>? OnRemoteStreamAcquired;
    event Func<string, Task>? InterlocutorLeft;
    
    Task Join(string signalingChannel);
    Task<IJSObjectReference> StartLocalStream();
    Task Call();
    Task Hangup();
    [JSInvokable]
    Task SendOffer(string offer);
    [JSInvokable]
    Task SendAnswer(string answer);
    [JSInvokable]
    Task SendCandidate(string candidate);
    [JSInvokable]
    Task SetRemoteStream();
}