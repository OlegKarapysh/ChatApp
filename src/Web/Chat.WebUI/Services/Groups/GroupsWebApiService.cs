using Chat.Domain.DTOs;
using Chat.Domain.DTOs.AssistantFiles;
using Chat.Domain.DTOs.Groups;
using Chat.Domain.Web;
using Chat.WebUI.Services.Auth;

namespace Chat.WebUI.Services.Groups;

public class GroupsWebApiService : WebApiServiceBase, IGroupsWebApiService
{
    private protected override string BaseRoute { get; init; } = "/groups";

    public GroupsWebApiService(IHttpClientFactory httpClientFactory, ITokenStorageService tokenService)
        : base(httpClientFactory, tokenService)
    {
    }

    public async Task<WebApiResponse<GroupDto>> GetGroupAsync(int id)
    {
        return await GetAsync<GroupDto>($"/{id}");
    }

    public async Task<WebApiResponse<IList<GroupInfoDto>>> GetAllGroupsInfoAsync()
    {
        return await GetAsync<IList<GroupInfoDto>>("/all");
    }

    public async Task<WebApiResponse<GroupWithMembersDto>> GetGroupWithMembersAsync(int groupId)
    {
        return await GetAsync<GroupWithMembersDto>($"/{groupId}/members");
    }

    public async Task<WebApiResponse<GroupWithFilesDto>> GetGroupWithFilesAsync(int groupId)
    {
        return await GetAsync<GroupWithFilesDto>($"/{groupId}/files");
    }

    public async Task<WebApiResponse<GroupDto>> CreateGroupAsync(NewGroupDto newGroupDto)
    {
        return await PostAsync<GroupDto, NewGroupDto>(newGroupDto);
    }

    public async Task<WebApiResponse<GroupDto>> AddGroupMemberAsync(NewGroupMemberDto newGroupMemberDto)
    {
        return await PostAsync<GroupDto, NewGroupMemberDto>(newGroupMemberDto, "/members");
    }

    public async Task<WebApiResponse<AssistantFileDto>> AddFileToGroupAsync(int groupId, UploadedFileDto uploadedFileDto)
    {
        return await PostAsync<AssistantFileDto, UploadedFileDto>(uploadedFileDto, $"/{groupId}/files");
    }

    public async Task<WebApiResponse<GroupDto>> EditGroupAsync(int groupId, NewGroupDto groupDto)
    {
        return await PutAsync<GroupDto, NewGroupDto>(groupDto, $"/{groupId}");
    }

    public async Task<ErrorDetailsDto?> DeleteFileFromGroupAsync(int fileId, int groupId)
    {
        return await DeleteAsync($"/{groupId}/files/{fileId}");
    }
    
    public async Task<ErrorDetailsDto?> DeleteGroupMemberAsync(int groupId, string memberUserName)
    {
        return await DeleteAsync($"/{groupId}/members/{memberUserName}");
    }

    public async Task<ErrorDetailsDto?> DeleteGroupAsync(int groupId)
    {
        return await DeleteAsync($"/{groupId}");
    }
}