﻿using Chat.Application.Services.OpenAI;

namespace Chat.Application.Services.Groups;

public sealed class GroupService : IGroupService
{
    private readonly IUserService _userService;
    private readonly IOpenAiService _openAiService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Group, int> _groupRepository;
    private readonly IRepository<GroupMember, int> _groupMembersRepository;
    private readonly IRepository<AssistantFile, int> _filesRepository;

    public GroupService(IUserService userService, IUnitOfWork unitOfWork, IOpenAiService openAiService)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _openAiService = openAiService;
        _groupRepository = _unitOfWork.GetRepository<Group, int>();
        _groupMembersRepository = _unitOfWork.GetRepository<GroupMember, int>();
        _filesRepository = _unitOfWork.GetRepository<AssistantFile, int>();
    }
    
    public async Task<IList<GroupInfoDto>> GetAllGroupsInfoAsync(int groupCreatorId)
    {
        var creator = await _userService.GetUserByIdAsync(groupCreatorId);
        var groupsInfo = await GetGroupsWithFilesAndMembers()
                               .Where(x => x.CreatorId == creator.Id)
                               .Select(x => x.MapToInfoDto())
                               .ToListAsync();
        return groupsInfo;
    }
    
    public async Task<GroupWithFilesDto> GetGroupWithFilesAsync(int groupId, int creatorId)
    {
        var group = await _groupRepository.AsQueryable()
                                          .Include(x => x.Files)
                                          .FirstOrDefaultAsync(x => x.Id == groupId)
                    ?? throw new EntityNotFoundException(nameof(Group));
        ThrowIfInvalidCreator(creatorId, group.CreatorId);
        
        return group.MapToWithFilesDto();
    }

    public async Task<GroupWithMembersDto> GetGroupWithMembersAsync(int groupId, int creatorId)
    {
        var group = await _groupRepository.AsQueryable()
                                          .Include(x => x.Members)
                                          .FirstOrDefaultAsync(x => x.Id == groupId)
                    ?? throw new EntityNotFoundException(nameof(Group));
        ThrowIfInvalidCreator(creatorId, group.CreatorId);
        
        return group.MapToWithMembersDto();
    }

    public async Task<GroupDto> CreateGroupAsync(NewGroupDto newGroupDto)
    {
        if (newGroupDto.CreatorId is null)
        {
            throw new EntityNotFoundException(nameof(User));
        }
        
        var creator = await _userService.GetUserByIdAsync((int)newGroupDto.CreatorId);
        if (await _groupRepository.FindFirstAsync(
                x => x.CreatorId == creator.Id && x.Name == newGroupDto.Name) is not null)
        {
            throw new GroupAlreadyExistsException();
        }

        var assistantName = GenerateAssistantName(creator.UserName ?? string.Empty, newGroupDto.Name);
        var assistant = await _openAiService.CreateAssistantWithRetrievalAsync(assistantName, newGroupDto.Instructions);
        var group = new Group
        {
            Name = newGroupDto.Name, Instructions = newGroupDto.Instructions,
            AssistantId = assistant.Id, CreatorId = creator.Id
        };
        var createdGroup = await _groupRepository.AddAsync(group);
        await _unitOfWork.SaveChangesAsync();

        return createdGroup.MapToDto();
    }

    public async Task<GroupDto> EditGroupAsync(int groupId, NewGroupDto groupDto)
    {
        var group = await GetGroupByIdAsync(groupId);
        var creator = await _userService.GetUserByIdAsync(group.CreatorId ?? default);
        var name = GenerateAssistantName(creator.UserName ?? string.Empty, groupDto.Name);
        var editedAssistant = await _openAiService.EditAssistantAsync(group.AssistantId, groupDto.Instructions, name);
        group.Name = groupDto.Name;
        group.Instructions = editedAssistant.Instructions;
        var editedGroup = _groupRepository.Update(group);
        await _unitOfWork.SaveChangesAsync();

        return editedGroup.MapToDto();
    }

    public async Task<GroupDto> AddGroupMemberAsync(NewGroupMemberDto newGroupMemberDto)
    {
        var group = await GetGroupByIdAsync(newGroupMemberDto.GroupId);
        var member = await _userService.GetUserByNameAsync(newGroupMemberDto.MemberUserName);
        var groups = await _groupRepository.AsQueryable()
                                           .Include(x => x.Members)
                                           .Where(x => x.CreatorId == group.CreatorId && x.Members.Contains(member))
                                           .ToListAsync();
        if (groups.Any())
        {
            throw new UserAlreadyAddedToGroupException();
        }

        var thread = await _openAiService.CreateThreadAsync();
        var groupMember = new GroupMember { Group = group, User = member, ThreadId = thread.Id };
        group.GroupMembers.Add(groupMember);
        _groupRepository.Update(group);
        await _unitOfWork.SaveChangesAsync();

        return group.MapToDto();
    }

    public async Task<AssistantFileDto> AddFileToGroupAsync(int groupId, UploadedFileDto uploadedFileDto)
    {
        var group = await GetGroupByIdAsync(groupId);
        await _openAiService.AddFileToAssistant(group.AssistantId, uploadedFileDto.FileId);
        var file = uploadedFileDto.MapToFile();
        file.Group = group;
        var addedFile = await _filesRepository.AddAsync(file);
        await _unitOfWork.SaveChangesAsync();

        return addedFile.MapToDto();
    }

    public async Task<bool> DeleteGroupMember(string memberUserName, int groupId)
    {
        var group = await GetGroupByIdAsync(groupId);
        var member = await _userService.GetUserByNameAsync(memberUserName);
        var groupMember = await _groupMembersRepository.FindFirstAsync(
            x => x.GroupId == group.Id && x.UserId == member.Id);
        if (groupMember is null)
        {
            throw new EntityNotFoundException(nameof(GroupMember));
        }

        var isThreadDeleted = await _openAiService.DeleteThreadAsync(groupMember.ThreadId);
        var isMemberDeleted = await _groupMembersRepository.RemoveAsync(groupMember.Id);
        await _unitOfWork.SaveChangesAsync();
        return isThreadDeleted && isMemberDeleted;
    }

    public async Task<bool> DeleteFileFromGroupAsync(int fileId, int groupId)
    {
        var group = await GetGroupByIdAsync(groupId);
        var file = await GetFileByIdAsync(fileId);
        var isFileDeletedFromOpenAi = await _openAiService.DeleteFileAsync(group.AssistantId, file.FileId);
        var isFileDeleted = await _filesRepository.RemoveAsync(fileId);
        await _unitOfWork.SaveChangesAsync();
        
        return isFileDeletedFromOpenAi && isFileDeleted;
    }
    
    public async Task<bool> DeleteGroupAsync(int groupId)
    {
        var group = await GetGroupsWithFilesAndMembers().FirstOrDefaultAsync(x => x.Id == groupId)
                    ?? throw new EntityNotFoundException(nameof(Group));
        var areFilesDeleted = await DeleteFilesFromStorageAsync(group);
        var isAssistantDeleted = await _openAiService.DeleteAssistantAsync(group.AssistantId);
        var areMembersDeleted = await _groupMembersRepository.RemoveRangeAsync(group.GroupMembers.Select(x => x.Id));
        var areFilesRemoved = await _filesRepository.RemoveRangeAsync(group.Files.Select(x => x.Id));
        var isGroupDeleted = await _groupRepository.RemoveAsync(group.Id);
        await _unitOfWork.SaveChangesAsync();
        
        return areFilesRemoved && isAssistantDeleted && areFilesDeleted && areMembersDeleted && isGroupDeleted;
    }
    
    public async Task<Group> GetGroupByIdAsync(int id)
    {
        return await _groupRepository.GetByIdAsync(id) ?? throw new EntityNotFoundException(nameof(Group));
    }

    public async Task<AssistantFile> GetFileByIdAsync(int id)
    {
        return await _filesRepository.GetByIdAsync(id) ?? throw new EntityNotFoundException(nameof(AssistantFile));
    }

    private async Task<bool> DeleteFilesFromStorageAsync(Group group)
    {
        var areAllDeleted = true;
        foreach (var file in group.Files)
        {
            if (!await _openAiService.DeleteFileAsync(group.AssistantId, file.FileId))
            {
                areAllDeleted = false;
            }
        }

        return areAllDeleted;
    }
    
    private IQueryable<Group> GetGroupsWithFilesAndMembers()
    {
        return _groupRepository.AsQueryable()
                               .Include(x => x.Files)
                               .Include(x => x.Members)
                               .Include(x => x.GroupMembers);
    }

    private void ThrowIfInvalidCreator(int claimedCreatorId, int? actualCreatorId)
    {
        if (claimedCreatorId != actualCreatorId)
        {
            throw new EntityNotFoundException(nameof(Group));
        }
    }

    private string GenerateAssistantName(string creatorName, string groupName) => $"{creatorName}_{groupName}";
}