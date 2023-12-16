using Chat.Domain.DTOs.Groups;
using Chat.Domain.Entities.Groups;

namespace Chat.Application.Mappings;

public static class GroupMappings
{
    public static GroupDto MapToDto(this Group group)
    {
        return new GroupDto
        {
            Name = group.Name,
            Instructions = group.Instructions,
            AssistantId = group.AssistantId,
            CreatorId = group.CreatorId
        };
    }
}