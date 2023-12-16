using Chat.Domain.DTOs.Groups;

namespace Chat.Application.Services.Groups;

public interface IGroupService
{
    Task<GroupDto> CreateGroupAsync(NewGroupDto newGroupDto);
}