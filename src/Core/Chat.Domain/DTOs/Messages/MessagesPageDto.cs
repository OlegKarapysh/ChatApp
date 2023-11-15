namespace Chat.Domain.DTOs.Messages;

public class MessagesPageDto : PageDto
{
    public MessageBasicInfoDto[]? Messages { get; set; }
}