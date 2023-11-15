namespace Chat.Domain.DTOs.Conversations;

public class NewGroupChatDto
{
    public int CreatorId { get; set; }
    public string Title { get; set; } = string.Empty;
}