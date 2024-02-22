namespace Chat.WebUI.Services.AiCopilot;

public interface IAiCopilotWebApiService
{
    Task<WebApiResponse<SimpleMessageDto>> SendMessageToCopilotAsync(SimpleMessageDto message);
}