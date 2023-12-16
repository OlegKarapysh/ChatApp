namespace Chat.Domain.DTOs.Groups;

public class GroupDto : NewGroupDto
{
    public string AssistantId { get; set; } = string.Empty;
}