namespace Chat.Domain.DTOs.Messages;

public class MessageBasicInfoDto
{
    public string TextContent { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
    public string UpdatedAt { get; set; } = string.Empty;
}