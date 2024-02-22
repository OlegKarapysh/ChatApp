namespace Chat.Application.Services.Groups;

public interface IGroupService
{
    Task<Group> GetGroupByIdAsync(int id);
    Task<IList<GroupInfoDto>> GetAllGroupsInfoAsync(int groupCreatorId);
    Task<GroupWithFilesDto> GetGroupWithFilesAsync(int groupId, int creatorId);
    Task<GroupWithMembersDto> GetGroupWithMembersAsync(int groupId, int creatorId);
    Task<GroupDto> CreateGroupAsync(NewGroupDto newGroupDto);
    Task<GroupDto> EditGroupAsync(int groupId, NewGroupDto groupDto);
    Task<GroupDto> AddGroupMemberAsync(NewGroupMemberDto newGroupMemberDto);
    Task<AssistantFileDto> AddFileToGroupAsync(int groupId, UploadedFileDto uploadedFileDto);
    Task<bool> DeleteFileFromGroupAsync(int fileId, int groupId);
    Task<bool> DeleteGroupMember(string memberUserName, int groupId);
    Task<bool> DeleteGroupAsync(int groupId);
}