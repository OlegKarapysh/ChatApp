namespace Chat.Domain.DTOs.Messages;

public class MessageDto
{
    public string TextContent { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}