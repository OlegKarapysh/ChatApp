namespace Chat.Domain.DTOs.Conversations;

public class NewGroupMemberDto
{
    public int ConversationId { get; set; }
    public string MemberUserName { get; set; } = string.Empty;
}