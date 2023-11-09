using Chat.Application.Extensions;
using Chat.Application.Mappings;
using Chat.Domain.DTOs.Messages;
using Chat.Domain.DTOs.Users;
using Chat.Domain.Entities;
using Chat.Domain.Web;
using Chat.DomainServices.UnitsOfWork;

namespace Chat.Application.Services.Messages;

public sealed class MessageService : IMessageService
{
    private readonly IUnitOfWork _unitOfWork;

    public MessageService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<MessagesPageDto> SearchMessagesPagedAsync(PagedSearchDto searchData)
    {
        var repository = _unitOfWork.GetRepository<Message, int>();
        var foundMessages = repository.SearchWhere<MessageDto>(searchData.SearchFilter);
        var messagesCount = foundMessages.Count();
        var pageSize = PageInfo.DefaultPageSize;
        var pageInfo = new PageInfo(messagesCount, searchData.Page);
        var foundMessagesPage = foundMessages
                                     .ToSortedPage(searchData.SortingProperty, searchData.SortingOrder, searchData.Page, pageSize)
                                     .Select(x => x.MapToDto());
        
        return await Task.FromResult(new MessagesPageDto
        {
            PageInfo = pageInfo,
            Messages = foundMessagesPage.ToArray()
        });
    }
}