using Chat.Domain.DTOs;
using Chat.Domain.DTOs.AssistantFiles;
using Chat.Domain.DTOs.Groups;
using Chat.Domain.Web;

namespace Chat.WebUI.Services.Groups;

public interface IGroupsWebApiService
{
    Task<WebApiResponse<GroupDto>> GetGroupAsync(int id);
    Task<WebApiResponse<IList<GroupInfoDto>>> GetAllGroupsInfoAsync();
    Task<WebApiResponse<GroupWithFilesDto>> GetGroupWithFilesAsync(int groupId);
    Task<WebApiResponse<GroupWithMembersDto>> GetGroupWithMembersAsync(int groupId);
    Task<WebApiResponse<GroupDto>> CreateGroupAsync(NewGroupDto newGroupDto);
    Task<WebApiResponse<GroupDto>> AddGroupMemberAsync(NewGroupMemberDto newGroupMemberDto);
    Task<WebApiResponse<AssistantFileDto>> AddFileToGroupAsync(int groupId, UploadedFileDto uploadedFileDto);
    Task<WebApiResponse<GroupDto>> EditGroupAsync(int groupId, NewGroupDto groupDto);
    Task<ErrorDetailsDto?> DeleteFileFromGroupAsync(int fileId, int groupId);
    Task<ErrorDetailsDto?> DeleteGroupAsync(int groupId);
}