using Chat.Domain.DTOs.Groups;

namespace Chat.Application.Services.Groups;

public interface IGroupService
{
    Task<IList<GroupInfoDto>> GetAllGroupsInfoAsync(int groupCreatorId);
    Task<GroupDto> CreateGroupAsync(NewGroupDto newGroupDto);
    Task<bool> DeleteGroupAsync(int groupId);
    Task<GroupDto> AddGroupMemberAsync(NewGroupMemberDto newGroupMemberDto);
}