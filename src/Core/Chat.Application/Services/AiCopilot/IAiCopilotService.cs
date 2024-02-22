namespace Chat.Application.Services.AiCopilot;

public interface IAiCopilotService
{
    Task<SimpleMessageDto> SendMessageToChatAsync(SimpleMessageDto message);
}