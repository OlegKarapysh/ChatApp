namespace Chat.Domain.DTOs.Conversations;

public class ConversationDto : ConversationBasicInfoDto
{
    public ConversationType Type { get; set; } = ConversationType.Dialog;
    public int Id { get; set; }
}