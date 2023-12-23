namespace Chat.Domain.DTOs.Groups;

public class GroupDto : NewGroupDto
{
    public int Id { get; set; }
    public string AssistantId { get; set; } = string.Empty;
}