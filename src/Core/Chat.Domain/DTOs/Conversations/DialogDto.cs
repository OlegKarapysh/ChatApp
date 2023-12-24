namespace Chat.Domain.DTOs.Conversations;

public class DialogDto : ConversationBasicInfoDto
{
    public readonly ConversationType Type = ConversationType.Dialog;
    public int Id { get; set; }
}