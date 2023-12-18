using Chat.Domain.DTOs;
using Chat.Domain.DTOs.AssistantFiles;
using Chat.Domain.DTOs.Groups;
using Chat.Domain.Web;

namespace Chat.WebUI.Services.Groups;

public interface IGroupsWebApiService
{
    Task<WebApiResponse<IList<GroupInfoDto>>> GetAllGroupsInfoAsync();
    Task<WebApiResponse<GroupWithFilesDto>> GetGroupWithFilesAsync(int groupId);
    Task<WebApiResponse<GroupDto>> CreateGroupAsync(NewGroupDto newGroupDto);
    Task<WebApiResponse<AssistantFileDto>> AddFileToGroupAsync(int groupId, UploadedFileDto uploadedFileDto);
    Task<ErrorDetailsDto?> DeleteFileFromGroupAsync(int fileId, int groupId);
    Task<ErrorDetailsDto?> DeleteGroupAsync(int groupId);
}