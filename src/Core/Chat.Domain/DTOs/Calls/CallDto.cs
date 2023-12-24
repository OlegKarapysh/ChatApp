namespace Chat.Domain.DTOs.Calls;

public class CallDto
{
    public string Id { get; set; } = string.Empty;
    public string ConversationId { get; set; } = string.Empty;
    public string ConversationTitle { get; set; } = string.Empty;
    public ConversationType ConversationType { get; set; }
    public string CallerUserName { get; set; } = string.Empty;
    public CallStatus Status { get; set; }
}