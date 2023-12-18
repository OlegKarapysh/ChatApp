using Chat.Domain.DTOs.AssistantFiles;
using Chat.Domain.DTOs.Groups;

namespace Chat.Application.Services.Groups;

public interface IGroupService
{
    Task<IList<GroupInfoDto>> GetAllGroupsInfoAsync(int groupCreatorId);
    Task<GroupWithFilesDto> GetGroupWithFilesAsync(int groupId);
    Task<GroupDto> CreateGroupAsync(NewGroupDto newGroupDto);
    Task<GroupDto> AddGroupMemberAsync(NewGroupMemberDto newGroupMemberDto);
    Task<AssistantFileDto> AddFileToGroupAsync(int groupId, UploadedFileDto uploadedFileDto);
    Task<bool> DeleteFileFromGroupAsync(int fileId, int groupId);
    Task<bool> DeleteGroupAsync(int groupId);
}