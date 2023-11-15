namespace Chat.Domain.DTOs.Messages;

public class MessageDto : MessageBasicInfoDto
{
    public int Id { get; set; }
    public bool IsRead { get; set; }
    public int SenderId { get; set; }
    public int ConversationId { get; set; }
}