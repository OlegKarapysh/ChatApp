using Chat.Application.Extensions;
using Chat.Application.Mappings;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Entities;
using Chat.Domain.Web;
using Chat.DomainServices.Repositories;
using Chat.DomainServices.UnitsOfWork;

namespace Chat.Application.Services.Messages;

public sealed class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Message, int> _messageRepository;

    public MessageService(IUnitOfWork unitOfWork)
    {
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

    public async Task<IList<MessageDto>> GetAllConversationMessagesAsync(int conversationId)
    {
        return (await _messageRepository.FindAllAsync(message => message.ConversationId == conversationId))
               .Select(message => message.MapToDto())
               .ToArray();
    }

    public async Task<MessageDto> CreateMessageAsync(MessageDto messageData)
    {
        var message = new Message().MapFrom(messageData);
        var createdMessage = await _messageRepository.AddAsync(message);
        await _unitOfWork.SaveChangesAsync();
        
        return createdMessage.MapToDto();
    }
}