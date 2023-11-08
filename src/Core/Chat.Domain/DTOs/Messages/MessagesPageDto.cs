namespace Chat.Domain.DTOs.Messages;

public class MessagesPageDto : PageDto
{
    public MessageDto[]? Messages { get; set; }
}