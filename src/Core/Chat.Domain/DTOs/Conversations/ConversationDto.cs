namespace Chat.Domain.DTOs.Conversations;

public class ConversationDto
{
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}