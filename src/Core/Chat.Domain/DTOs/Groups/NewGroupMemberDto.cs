namespace Chat.Domain.DTOs.Groups;

public class NewGroupMemberDto
{
    public int GroupId { get; set; }
    public string MemberUserName { get; set; } = string.Empty;
}