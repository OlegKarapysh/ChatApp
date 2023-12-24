namespace Chat.WebUI.Services.SignalR;

public interface IVideoCallSignallingService
{
    Task CallUserAsync(CallDto call);
    Task AnswerCallAsync(CallDto call);
}