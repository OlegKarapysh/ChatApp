using Microsoft.EntityFrameworkCore;
using Chat.Application.Extensions;
using Chat.Application.Mappings;
using Chat.Application.RequestExceptions;
using Chat.Application.Services.Groups;
using Chat.Application.Services.OpenAI;
using Chat.Application.Services.Users;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Entities.Groups;
using Chat.Domain.Web;
using Chat.DomainServices.Repositories;
using Chat.DomainServices.UnitsOfWork;
using OpenAI.Threads;
using Message = Chat.Domain.Entities.Message;

namespace Chat.Application.Services.Messages;

public sealed class MessageService : IMessageService
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOpenAiService _openAiService;
    private readonly IRepository<Message, int> _messageRepository;

    public MessageService(IUserService userService, IUnitOfWork unitOfWork, IOpenAiService openAiService)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
        _openAiService = openAiService;
        _messageRepository = _unitOfWork.GetRepository<Message, int>();
    }
    
    public async Task<MessagesPageDto> SearchMessagesPagedAsync(PagedSearchDto searchData)
    {
        var foundMessages = _messageRepository.SearchWhere<MessageBasicInfoDto>(searchData.SearchFilter);
        var messagesCount = foundMessages.Count();
        var pageSize = PageInfo.DefaultPageSize;
        var pageInfo = new PageInfo(messagesCount, searchData.Page);
        var foundMessagesPage = foundMessages
                                     .ToSortedPage(searchData.SortingProperty, searchData.SortingOrder, searchData.Page, pageSize)
                                     .Select(x => x.MapToBasicDto());
        
        return await Task.FromResult(new MessagesPageDto
        {
            PageInfo = pageInfo,
            Messages = foundMessagesPage.ToArray()
        });
    }

    public async Task<IList<MessageWithSenderDto>> GetAllConversationMessagesAsync(int conversationId)
    {
        return await _messageRepository.AsQueryable()
                                       .Include(x => x.Sender)
                                       .Where(x => x.ConversationId == conversationId)
                                       .Select(x => x.MapToDtoWithSender())
                                       .ToListAsync();
    }

    public async Task<MessageWithSenderDto> CreateMessageAsync(MessageDto messageData)
    {
        var message = new Message().MapFrom(messageData);
        var createdMessage = await _messageRepository.AddAsync(message);
        await _unitOfWork.SaveChangesAsync();
        createdMessage.Sender = await _userService.GetUserByIdAsync(messageData.SenderId);
        
        return createdMessage.MapToDtoWithSender();
    }

    public async Task<MessageResponse> AssistWithMessageAsync(MessageForAssistDto messageDto)
    {
        var message = await GetMessageByIdAsync(messageDto.MessageId);
        var sender = await _userService.GetUserByIdAsync((int)message.SenderId);
        var receiver = await _userService.GetUserByNameAsync(messageDto.ReceiverUserName);
        var groupRepository = _unitOfWork.GetRepository<Group, int>();
        var group = await groupRepository.AsQueryable()
                                         .Include(x => x.Members)
                                         .Include(x => x.GroupMembers)
                                         .FirstOrDefaultAsync(x =>
                                             x.CreatorId == receiver.Id && x.Members.Contains(sender));
        if (group is null)
        {
            throw new EntityNotFoundException(nameof(Group));
        }

        var threadId = group.GroupMembers.FirstOrDefault(x => x.UserId == sender.Id)?.ThreadId;
        return await _openAiService.SendMessageAsync(message.TextContent, group.AssistantId, threadId);
    }

    public async Task<MessageDto> UpdateMessageAsync(MessageDto messageData, int updaterId)
    {
        var message = await GetMessageByIdAsync(messageData.Id);
        if (message.SenderId != updaterId)
        {
            throw new InvalidMessageUpdaterException();
        }

        messageData.SenderId = updaterId;
        var updatedMessage = _messageRepository.Update(message.MapFrom(messageData));
        await _unitOfWork.SaveChangesAsync();
        
        return updatedMessage.MapToDto();
    }

    public async Task<bool> DeleteMessageAsync(int messageId)
    {
        var isDeletedSuccessfully = await _messageRepository.RemoveAsync(messageId);
        await _unitOfWork.SaveChangesAsync();

        return isDeletedSuccessfully;
    }

    public async Task<Message> GetMessageByIdAsync(int messageId)
    {
        return await _messageRepository.GetByIdAsync(messageId) ?? throw new EntityNotFoundException(nameof(Message));
    }
}