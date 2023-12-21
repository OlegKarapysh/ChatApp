using Chat.Domain.DTOs.AssistantFiles;
using Chat.Domain.DTOs.Groups;
using Chat.Domain.Entities.Groups;

namespace Chat.Application.Services.Groups;

public interface IGroupService
{
    Task<Group> GetGroupByIdAsync(int id);
    Task<IList<GroupInfoDto>> GetAllGroupsInfoAsync(int groupCreatorId);
    Task<GroupWithFilesDto> GetGroupWithFilesAsync(int groupId);
    Task<GroupWithMembersDto> GetGroupWithMembersAsync(int groupId);
    Task<GroupDto> CreateGroupAsync(NewGroupDto newGroupDto);
    Task<GroupDto> EditGroupAsync(int groupId, NewGroupDto groupDto);
    Task<GroupDto> AddGroupMemberAsync(NewGroupMemberDto newGroupMemberDto);
    Task<AssistantFileDto> AddFileToGroupAsync(int groupId, UploadedFileDto uploadedFileDto);
    Task<bool> DeleteFileFromGroupAsync(int fileId, int groupId);
    Task<bool> DeleteGroupMember(string memberUserName, int groupId);
    Task<bool> DeleteGroupAsync(int groupId);
}