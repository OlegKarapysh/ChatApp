using Microsoft.EntityFrameworkCore;
using Chat.Application.Extensions;
using Chat.Application.Mappings;
using Chat.Application.RequestExceptions;
using Chat.Application.Services.Users;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Entities;
using Chat.Domain.Web;
using Chat.DomainServices.Repositories;
using Chat.DomainServices.UnitsOfWork;

namespace Chat.Application.Services.Messages;

public sealed class MessageService : IMessageService
{
    private readonly IUserService _userService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Message, int> _messageRepository;

    public MessageService(IUserService userService, IUnitOfWork unitOfWork)
    {
        _userService = userService;
        _unitOfWork = unitOfWork;
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