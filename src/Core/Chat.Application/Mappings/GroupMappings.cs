using Chat.Domain.DTOs.Groups;
using Chat.Domain.Entities.Groups;

namespace Chat.Application.Mappings;

public static class GroupMappings
{
    public static GroupDto MapToDto(this Group group)
    {
        return new GroupDto
        {
            Id = group.Id,
            Name = group.Name,
            Instructions = group.Instructions,
            AssistantId = group.AssistantId,
            CreatorId = group.CreatorId
        };
    }

    public static GroupInfoDto MapToInfoDto(this Group group)
    {
        return new GroupInfoDto
        {
            Id = group.Id,
            Name = group.Name,
            Instructions = group.Instructions,
            FilesCount = group.Files.Count,
            MembersCount = group.Members.Count
        };
    }

    public static GroupWithFilesDto MapToWithFilesDto(this Group group)
    {
        return new GroupWithFilesDto
        {
            Id = group.Id,
            Name = group.Name,
            Instructions = group.Instructions,
            AssistantId = group.AssistantId,
            CreatorId = group.CreatorId,
            Files = group.Files.Select(x => x.MapToDto()).ToList()
        };
    }

    public static GroupWithMembersDto MapToWithMembersDto(this Group group)
    {
        return new GroupWithMembersDto
        {
            Id = group.Id,
            Name = group.Name,
            Instructions = group.Instructions,
            AssistantId = group.AssistantId,
            CreatorId = group.CreatorId,
            Members = group.Members.Select(x => x.MapToDto()).ToList()
        };
    }
}