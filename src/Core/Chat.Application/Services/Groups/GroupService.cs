using Chat.Application.Mappings;
using Chat.Application.RequestExceptions;
using Chat.Application.Services.OpenAI;
using Chat.Application.Services.Users;
using Chat.Domain.DTOs.Groups;
using Chat.Domain.Entities.Groups;
using Chat.DomainServices.Repositories;
using Chat.DomainServices.UnitsOfWork;

namespace Chat.Application.Services.Groups;

public sealed class GroupService : IGroupService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Group, int> _groupRepository;
    private readonly IUserService _userService;
    private readonly IOpenAiService _openAiService;

    public GroupService(IUserService userService, IUnitOfWork unitOfWork, IOpenAiService openAiService)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _openAiService = openAiService;
        _groupRepository = _unitOfWork.GetRepository<Group, int>();
    }

    public async Task<GroupDto> CreateGroupAsync(NewGroupDto newGroupDto)
    {
        var creator = await _userService.GetUserByIdAsync(newGroupDto.CreatorId);
        if (await _groupRepository.FindFirstAsync(
                x => x.CreatorId == creator.Id && x.Name == newGroupDto.Name) is not null)
        {
            throw new GroupAlreadyExistsException();
        }

        var assistantName = $"{creator.UserName}_{newGroupDto.Name}";
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
}