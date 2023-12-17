namespace Chat.Domain.DTOs.Conversations;

public class NewConversationMemberDto
{
    public int ConversationId { get; set; }
    public string MemberUserName { get; set; } = string.Empty;
}